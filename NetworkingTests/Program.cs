using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkingTests;

class Program {
    static async Task Main(string[] args) {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        
        Console.WriteLine("Basic Networking <1>, Lobby Networking <2>");
        string input = Console.ReadLine();

        if (input == "1") {
            await BasicNetworkingTest();
        }
        else {
            await LobbyNetworkingTest();
        }
    }

    private static async Task BasicNetworkingTest() {
        Console.Clear();
        
        Console.WriteLine("Host <1>, Client <2>");
        string input = Console.ReadLine();

        if (input == "1") {
            await BasicNetworking.StartHost();
        }
        else {
            await BasicNetworking.StartClient();
        }
    }
    
    private static async Task LobbyNetworkingTest() {
        Console.Clear();
        
        Console.WriteLine("Host <1>, Client <2>");
        string input = Console.ReadLine();

        if (input == "1") {
            await LobbyNetwork.StartHost();
        }
        else {
            await LobbyNetwork.StartClient();
        }
    }
}
