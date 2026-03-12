using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ChatServer;

internal static class Program
{
    private const int Port = 13000;

    private static readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web);
    private static readonly ConcurrentDictionary<string, ClientConnection> clients = new();

    private static SqliteStore store = null!;

    public static async Task Main()
    {
        store = new SqliteStore(Path.Combine(AppContext.BaseDirectory, "data", "chat.db"));
        await store.InitAsync();

        var listener = new TcpListener(IPAddress.Any, Port);
        listener.Start();

        Console.WriteLine($"ChatServer started on port {Port}");

        while (true)
        {
            TcpClient tcpClient = await listener.AcceptTcpClientAsync();
            _ = Task.Run(() => HandleClientAsync(tcpClient));
        }
    }

    private static async Task HandleClientAsync(TcpClient tcpClient)
    {
        string? login = null;

        try
        {
            NetworkStream networkStream = tcpClient.GetStream();
            using var reader = new StreamReader(networkStream, Encoding.UTF8, leaveOpen: true);
            using var writer = new StreamWriter(networkStream, Encoding.UTF8, leaveOpen: true)
            {
                AutoFlush = true
            };

            var sendLock = new SemaphoreSlim(1, 1);

            Console.WriteLine("Client connected: " + tcpClient.Client.RemoteEndPoint);

            string? helloLine = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(helloLine))
            {
                return;
            }

            Packet? helloPacket = DeserializePacket(helloLine);
            if (helloPacket == null || helloPacket.Type != "hello" || string.IsNullOrWhiteSpace(helloPacket.Login))
            {
                await SendPacketAsync(writer, sendLock, new Packet
                {
                    Type = "hello_error",
                    Error = "Некорректный запрос подключения."
                });
                return;
            }

            login = helloPacket.Login.Trim();

            if (clients.ContainsKey(login))
            {
                await SendPacketAsync(writer, sendLock, new Packet
                {
                    Type = "hello_error",
                    Error = "Этот логин уже занят."
                });
                return;
            }

            var clientConnection = new ClientConnection(login, writer, sendLock);

            if (!clients.TryAdd(login, clientConnection))
            {
                await SendPacketAsync(writer, sendLock, new Packet
                {
                    Type = "hello_error",
                    Error = "Не удалось подключить пользователя."
                });
                return;
            }

            await SendPacketAsync(writer, sendLock, new Packet
            {
                Type = "hello_ok"
            });

            Console.WriteLine($"User connected: {login}");

            List<ChatMessage> history = await store.GetLastMessagesAsync(50);
            await SendPacketAsync(writer, sendLock, new Packet
            {
                Type = "history",
                Messages = history
            });

            await BroadcastUsersAsync();

            await BroadcastAsync(new Packet
            {
                Type = "system",
                Text = $"Пользователь {login} вошёл в чат.",
                Timestamp = DateTime.UtcNow
            });

            while (true)
            {
                string? line = await reader.ReadLineAsync();
                if (line == null)
                {
                    break;
                }

                Packet? packet = DeserializePacket(line);
                if (packet == null)
                {
                    continue;
                }

                if (packet.Type == "chat" && !string.IsNullOrWhiteSpace(packet.Text))
                {
                    var message = new ChatMessage
                    {
                        From = login,
                        Text = packet.Text.Trim(),
                        Timestamp = DateTime.UtcNow
                    };

                    await store.AddMessageAsync(message);

                    await BroadcastAsync(new Packet
                    {
                        Type = "chat",
                        From = message.From,
                        Text = message.Text,
                        Timestamp = message.Timestamp
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Server error: " + ex);
        }
        finally
        {
            if (login != null)
            {
                clients.TryRemove(login, out _);

                Console.WriteLine($"User disconnected: {login}");

                await BroadcastUsersAsync();

                await BroadcastAsync(new Packet
                {
                    Type = "system",
                    Text = $"Пользователь {login} покинул чат.",
                    Timestamp = DateTime.UtcNow
                });
            }

            try
            {
                tcpClient.Close();
            }
            catch
            {
            }
        }
    }

    private static Packet? DeserializePacket(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<Packet>(json, jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    private static async Task BroadcastUsersAsync()
    {
        var users = clients.Keys.OrderBy(name => name).ToList();

        await BroadcastAsync(new Packet
        {
            Type = "users",
            Users = users
        });
    }

    private static async Task BroadcastAsync(Packet packet)
    {
        string json = JsonSerializer.Serialize(packet, jsonOptions);

        foreach (ClientConnection client in clients.Values.ToList())
        {
            await client.SendAsync(json);
        }
    }

    private static async Task SendPacketAsync(StreamWriter writer, SemaphoreSlim sendLock, Packet packet)
    {
        string json = JsonSerializer.Serialize(packet, jsonOptions);

        await sendLock.WaitAsync();
        try
        {
            await writer.WriteLineAsync(json);
        }
        finally
        {
            sendLock.Release();
        }
    }

    private sealed class ClientConnection
    {
        public string Login { get; }

        private StreamWriter Writer { get; }
        private SemaphoreSlim SendLock { get; }

        public ClientConnection(string login, StreamWriter writer, SemaphoreSlim sendLock)
        {
            Login = login;
            Writer = writer;
            SendLock = sendLock;
        }

        public async Task SendAsync(string json)
        {
            await SendLock.WaitAsync();
            try
            {
                await Writer.WriteLineAsync(json);
            }
            finally
            {
                SendLock.Release();
            }
        }
    }
}