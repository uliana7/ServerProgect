using System.Net;
using System.Net.Sockets;

namespace ChatServer;

internal static class Program
{
    private const int Port = 13000;

    public static async Task Main()
    {
        var listener = new TcpListener(IPAddress.Any, Port);
        listener.Start();

        Console.WriteLine($"ChatServer started on port {Port}");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            Console.WriteLine("Client connected: " + client.Client.RemoteEndPoint);

            client.Close();
        }
    }
}