using static SimplySpoof.Models;
using System.Security.Cryptography;
using System;
using static SimplySpoof.Utils;
using static System.ConsoleColor;

namespace SimplySpoof
{
    internal class Handlers
    {
        internal static void RandomizeData()
        {
            string McpeDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Packages\Microsoft.MinecraftUWP_8wekyb3d8bbwe\LocalState\games\com.mojang\minecraftpe\");
            string OptionsFile = Path.Combine(McpeDirectory, "options.txt");

            if (!Directory.Exists(McpeDirectory))
                Print("Minecraft Directory not found, are you using a custom launcher or not have the game installed?", Red);
            else
            {
                if (File.Exists(McpeDirectory + "clientId.txt"))
                    File.WriteAllText(McpeDirectory + "clientId.txt", RandomString(19, false, true));

                if (File.Exists(McpeDirectory + "telemetry_info.json"))
                    File.Delete(McpeDirectory + "telemetry_info.json");

                string[] lines = File.ReadAllLines(OptionsFile);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("mp_username"))
                    {
                        string[] parts = lines[i].Split(':');
                        parts[1] = RandomString(RandomInt(4, 10));
                        lines[i] = string.Join(":", parts);
                    }
                    if (lines[i].StartsWith("last_xuid"))
                    {
                        string[] parts = lines[i].Split(':');
                        parts[1] = RandomString(16, false, true);
                        lines[i] = string.Join(":", parts);
                    }
                    if (lines[i].StartsWith("game_skintypefull"))
                    {
                        string[] parts = lines[i].Split(':');
                        parts[1] = RandomString(RandomInt(7, 50)).ToLower();
                        lines[i] = string.Join(":", parts);
                    }
                    if (lines[i].StartsWith("game_lastcustomskinnew"))
                    {
                        string[] parts = lines[i].Split(':');
                        parts[1] = RandomString(RandomInt(20, 100)).ToLower();
                        lines[i] = string.Join(":", parts);
                    }
                    if (lines[i].StartsWith("last_minecraft_id"))
                    {
                        string[] parts = lines[i].Split(':');
                        parts[1] = RandomString();
                        lines[i] = string.Join(":", parts);
                        break;
                    }
                }
                File.WriteAllLines(OptionsFile, lines);
                Print("Data randomized", Green);
            }
        }

        internal static void SpoofIds(string version)
        {
            bool hasVersionEverMatched = false;
            foreach (SpoofPointersStruct Instance in Program.PointerList)
            {
                if (MMR.GetVersion().StartsWith(Instance.version))
                {
                    hasVersionEverMatched = true;
                    Print($@"Version matches ({Instance.version})", Yellow);
                    if (Instance.didPtr != "null")
                    {
                        long addr = MMR.GetMultiLevelPtr(MMR.GetOffset(Instance.didPtr), MMR.GetSubOffsets(Instance.didPtr));
                        string DidString = MMR.ReadMemory_str(addr, false);
                        Print($"Old DID: {DidString}", DarkYellow);
                        string uuid = Guid.NewGuid().ToString();
                        MMR.WriteMemory_str(addr, uuid, false);
                        Print($"New DID: {uuid}", DarkYellow);
                    }
                    if (Instance.mcidPtr != "null")
                    {
                        long addr = MMR.GetMultiLevelPtr(MMR.GetOffset(Instance.mcidPtr), MMR.GetSubOffsets(Instance.mcidPtr));
                        string McidString = MMR.ReadMemory_str(addr, false);
                        Print($"Old MCID: {McidString}", DarkYellow);
                        string newmcid = RandomString(16, true, true).ToUpper();
                        MMR.WriteMemory_str(addr, newmcid, false);
                        Print($"New MCID: {newmcid}", DarkYellow);
                    }
                    break;
                }
            }

            if (!hasVersionEverMatched)
                Print("Version does not match any in pointer list, if this is a new game update then you will need to wait for the pointer list to be updated.", Red);
            else
                Print("IDs have been spoofed", Green);
        }
    }
}
