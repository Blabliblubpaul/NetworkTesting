using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkingTests;

public static class ClientCapability {
    private static IPEndPoint endpoint;
    private static Socket client;

    public delegate void MessageReceived(string message);
    public static event MessageReceived BroadcastReceived;

    public static async void StartClient(string address) {
        endpoint = new IPEndPoint(IPAddress.Parse(address), 5000);
        client = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        await client.ConnectAsync(endpoint);
        
        Task broadcastListener = Task.Run(BroadcastListener);
    }

    public static void StopClient() {
        client.Shutdown(SocketShutdown.Both);
    }

    private static async Task BroadcastListener() {
        while (true) {
            byte[] buffer = new byte[1024];
            int received = await client.ReceiveAsync(buffer, SocketFlags.None);
            string response = Encoding.UTF8.GetString(buffer, 0, received);

            BroadcastReceived(response);
        }
    }
}