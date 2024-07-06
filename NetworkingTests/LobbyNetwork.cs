namespace NetworkingTests;

public static class LobbyNetwork {
    private static string input = string.Empty;
    
    
    public static async Task StartHost() {
        Console.WriteLine("Starting Server...");
        ServerCapability.StartServer();
        
        ServerCapability.ClientMessageReceived += (message) => {
            Write($"[Client message received]: {message}");
        }; 

        Task writer = Task.Run(async () => {
            while (true) {
                string message = Read();

                await ServerCapability.BroadcastMessage(message);
            }
        });

        await Task.Delay(50000000);

        Console.ReadKey(true);
    }
    
    public static async Task StartClient() {
        Console.Write("Enter server ip address: ");
        string address = Console.ReadLine();
        
        Console.WriteLine("Starting Client...");
        ClientCapability.StartClient(address);
        
        ClientCapability.BroadcastReceived += (message) => {
            Write($"[Broadcast received]: {message}");
        }; 

        Task writer = Task.Run(async () => {
            while (true) {
                string message = Read();

                await ClientCapability.SendMessageToServer(message);
            }
        });

        await Task.Delay(500000);

        Console.ReadKey(true);
    }

    private static void Write(string message) {
        Console.CursorLeft = 0;

        for (int i = 0; i < input.Length; i++) {
            Console.Write(" ");
        }
        
        Console.CursorLeft = 0;
        
        Console.WriteLine(message);
        Console.Write(input);
    }

    private static string Read() {
        do {
            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key) {
                case ConsoleKey.Enter:
                    Console.WriteLine();
                    string ret = input;
                    input = string.Empty;
                    return ret;

                case ConsoleKey.Backspace:
                    if (input.Length > 0) {
                        Console.CursorLeft--;
                        Console.Write(" ");
                        Console.CursorLeft--;
                        input = input.Remove(input.Length - 1);
                    }

                    break;

                default:
                    input += key.KeyChar;
                    Console.Write(key.KeyChar);
                    break;
            }
        } while (true);

        return string.Empty;
    }
}