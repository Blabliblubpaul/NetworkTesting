namespace NetworkingTests;

public static class LobbyNetwork {
    public static async Task StartHost() {
        Console.WriteLine("Starting Server...");
        ServerCapability.StartServer();

        while (true) {
            string message = Console.ReadLine();
            Console.WriteLine("Broadcasting...");
            await ServerCapability.BroadcastMessage(message);
            Console.WriteLine("Done.");
            
            ConsoleKey key = Console.ReadKey(true).Key;
            
            if (key is ConsoleKey.Escape) {
                Console.WriteLine("Shutting down server...");
                break;
            }
        }
    }
    
    public static async Task StartClient() {
        Console.Write("Enter server ip address: ");
        string address = Console.ReadLine();
        
        Console.WriteLine("Starting Client...");
        ClientCapability.StartClient(address);

        int receivedMessages = 0;

        ClientCapability.BroadcastReceived += (message) => {
            Console.WriteLine($"[Broadcast received]: {message}");
            receivedMessages++;
        };

        await Task.Delay(500000);
        
        Console.WriteLine(receivedMessages);

        Console.ReadKey(true);
    }
}