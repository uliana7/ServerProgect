using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient;

public partial class ChatForm : Form
{
    private readonly string serverIp;
    private readonly int serverPort;
    private readonly string login;

    private TcpClient? tcpClient;
    private StreamReader? reader;
    private StreamWriter? writer;
    private readonly SemaphoreSlim sendLock = new(1, 1);
    private CancellationTokenSource? cts;

    private static readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web);

    public ChatForm(string ip, int port, string login)
    {
        InitializeComponent();

        serverIp = ip;
        serverPort = port;
        this.login = login;

        Text = $"Чат - {login}";
    }

    protected override async void OnShown(EventArgs e)
    {
        base.OnShown(e);
        await ConnectAsync();
    }

    private async Task ConnectAsync()
    {
        try
        {
            tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(serverIp, serverPort);

            NetworkStream stream = tcpClient.GetStream();
            reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
            writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true)
            {
                AutoFlush = true
            };

            cts = new CancellationTokenSource();

            await SendPacketAsync(new Packet
            {
                Type = "hello",
                Login = login
            });

            string? line = await reader.ReadLineAsync();
            if (line is null)
            {
                throw new Exception("Сервер закрыл соединение.");
            }

            Packet? response = JsonSerializer.Deserialize<Packet>(line, jsonOptions);

            if (response?.Type == "hello_error")
            {
                throw new Exception(response.Error ?? "Логин отклонён.");
            }

            if (response?.Type != "hello_ok")
            {
                throw new Exception("Неожиданный ответ сервера.");
            }

            AppendSystem("Подключено к серверу.");

            _ = Task.Run(() => ReceiveLoopAsync(cts.Token));
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка подключения: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }
    }

    private async Task ReceiveLoopAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested && reader != null)
            {
                string? line = await reader.ReadLineAsync();
                if (line is null)
                {
                    break;
                }

                Packet? packet;
                try
                {
                    packet = JsonSerializer.Deserialize<Packet>(line, jsonOptions);
                }
                catch
                {
                    continue;
                }

                if (packet == null)
                {
                    continue;
                }

                if (IsHandleCreated)
                {
                    BeginInvoke(new Action(() => HandlePacket(packet)));
                }
            }
        }
        catch
        {
        }
        finally
        {
            if (IsHandleCreated)
            {
                BeginInvoke(new Action(() =>
                {
                    AppendSystem("Соединение закрыто.");
                }));
            }
        }
    }

    private void HandlePacket(Packet packet)
    {
        switch (packet.Type)
        {
            case "history":
                if (packet.Messages != null)
                {
                    foreach (ChatMessage message in packet.Messages)
                    {
                        AppendChat(message.From, message.Text, message.Timestamp);
                    }
                }
                break;

            case "chat":
                AppendChat(packet.From ?? "?", packet.Text ?? "", packet.Timestamp ?? DateTime.UtcNow);
                break;

            case "system":
                AppendSystem(packet.Text ?? "");
                break;

            case "users":
                listBoxUsers.Items.Clear();

                if (packet.Users != null)
                {
                    foreach (string user in packet.Users)
                    {
                        listBoxUsers.Items.Add(user);
                    }
                }
                break;
        }
    }

    private async void buttonSend_Click(object sender, EventArgs e)
    {
        await SendChatAsync();
    }

    private async void textBoxMessage_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            await SendChatAsync();
        }
    }

    private async Task SendChatAsync()
    {
        string text = textBoxMessage.Text.Trim();

        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        if (writer == null)
        {
            return;
        }

        textBoxMessage.Clear();

        await SendPacketAsync(new Packet
        {
            Type = "chat",
            Text = text
        });
    }

    private async Task SendPacketAsync(Packet packet)
    {
        if (writer == null)
        {
            return;
        }

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

    private void AppendChat(string from, string text, DateTime timestamp)
    {
        string time = timestamp.ToLocalTime().ToString("HH:mm:ss");
        richTextBoxChat.AppendText($"[{time}] {from}: {text}{Environment.NewLine}");
    }

    private void AppendSystem(string text)
    {
        richTextBoxChat.AppendText($"* {text}{Environment.NewLine}");
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        try
        {
            cts?.Cancel();
        }
        catch
        {
        }

        try
        {
            tcpClient?.Close();
        }
        catch
        {
        }

        base.OnFormClosing(e);
    }

    private class Packet
    {
        public string Type { get; set; } = "";
        public string? Login { get; set; }
        public string? From { get; set; }
        public string? Text { get; set; }
        public DateTime? Timestamp { get; set; }
        public List<string>? Users { get; set; }
        public List<ChatMessage>? Messages { get; set; }
        public string? Error { get; set; }
    }

    private class ChatMessage
    {
        public string From { get; set; } = "";
        public string Text { get; set; } = "";
        public DateTime Timestamp { get; set; }
    }
}