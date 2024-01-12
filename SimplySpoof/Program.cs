using static System.ConsoleColor;
using static SimplySpoof.Models;
using static SimplySpoof.Utils;
using static SimplySpoof.Handlers;

namespace SimplySpoof
{
    internal class Program
    {
        internal static List<SpoofPointersStruct> PointerList = new List<SpoofPointersStruct>();

        static async Task Main(string[] args)
        {
            Print("Welcome to SimplySpoof, created by Founder (github.com/Founderroni)", Magenta);
            Print("Downloading pointers", Yellow);
            try
            {
                string spoofListContent = await GetString("https://raw.githubusercontent.com/Founderroni/Assets/main/Other/SpoofPtrs.txt");
                string[] spoofListLines = spoofListContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in spoofListLines)
                {
                    string[] parts = line.Split(':');
                    string version = parts[0];
                    string didPtr = parts[1];
                    string mcidPtr = parts[2];
                    PointerList.Add(new SpoofPointersStruct(version, didPtr, mcidPtr));
                }
                Print("Pointers downloaded", Yellow);
            } catch (Exception ex)
            {
                Print($"Could not setup pointer list\n{ex.Message}", Red);
            }

            Print("Randomizing data", Yellow);
            RandomizeData();

            Print("Attempting to spoof IDs", Yellow);
            try
            {
                MMR.CheckInject();
            } catch (Exception)
            {
                Print("Could not find Minecraft, make sure the game is open. Press enter to close.", Red);
                Console.ReadKey();
                Environment.Exit(0);
            }
            SpoofIds(MMR.GetVersion());

            Print("Press enter to exit", Cyan);
            Console.ReadKey();
        }
    }
}
