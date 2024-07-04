using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkingTests;

public static class ServerCapability {
    private static List<Socket> clientHandlers;
    private static IPEndPoint listenerEndpoint = new(IPAddress.Any, 5000);
    private static Socket listener;

    public static void StartServer() {
        clientHandlers = [];
        
        listener = new Socket(listenerEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        
        listener.Bind(listenerEndpoint);
        listener.Listen(100);

        Task connectionHandler = Task.Run(ManageIncomingConnections);
    }

    public static void StopServer() {
        listener.Shutdown(SocketShutdown.Both);
    }

    public static async Task BroadcastMessage(string message) {
        byte[] payload = Encoding.UTF8.GetBytes(message);

        List<Task> messageTasks = new(clientHandlers.Count);
        
        Console.WriteLine(clientHandlers[0].Connected);
        Console.WriteLine(clientHandlers[0].IsBound);
        foreach (Socket handler in clientHandlers) {
            messageTasks.Add(handler.SendAsync(payload, SocketFlags.None));
        }

        foreach (Task messageTask in messageTasks) {
            messageTask.Wait();
        }
    }

    private static async Task ManageIncomingConnections() {
        while (true) {
            clientHandlers.Add(await listener.AcceptAsync());
            Console.WriteLine("Remote client connection accepted");
        }
    }
}