using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkingTests;

public static class BasicNetworking {
    public static async Task StartHost() {
        IPEndPoint endpoint = new(IPAddress.Any, 5000);
        
        using Socket listener = new(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        
        listener.Bind(endpoint);
        listener.Listen(100);

        Socket handler = await listener.AcceptAsync();

        while (true) {
            // Receive message
            byte[] buffer = new byte[1024];
            int received = await handler.ReceiveAsync(buffer, SocketFlags.None);
            string response = Encoding.UTF8.GetString(buffer, 0, received);

            if (response == "Client handshake") {
                Console.WriteLine("Received client handshake. Sending ACK");

                string message = "Server handshake";
                byte[] echoBytes = Encoding.UTF8.GetBytes(message);
                await handler.SendAsync(echoBytes, 0);
            }
            
            ConsoleKey key = Console.ReadKey(true).Key;
            
            if (key is ConsoleKey.Escape or ConsoleKey.E) {
                Console.WriteLine("Shutting down server...");
                break;
            }
        }
        
        listener.Shutdown(SocketShutdown.Both);
    }
    
    public static async Task StartClient() {
        IPEndPoint endpoint = new(IPAddress.Parse("127.0.0.1"), 5000);

        using Socket client = new(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        await client.ConnectAsync(endpoint);

        while (true) {
            // Send message
            string message = "Client handshake";
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            _ = await client.SendAsync(messageBytes, SocketFlags.None);
            Console.WriteLine($"Sent client handshake to server, awaiting ACK.");
            
            // Receive acck
            byte[] buffer = new byte[1024];
            int received = await client.ReceiveAsync(buffer, SocketFlags.None);
            string response = Encoding.UTF8.GetString(buffer, 0, received);

            if (response == "Server handshake") {
                Console.WriteLine("Received server ACK.");
            }

            ConsoleKey key = Console.ReadKey(true).Key;

            if (key is ConsoleKey.Escape or ConsoleKey.E) {
                Console.WriteLine("Shutting down client...");
                break;
            }
        }
        
        client.Shutdown(SocketShutdown.Both);
    }
}