using System;
using AudioSwitcher.AudioApi.CoreAudio;
using System.Runtime.Versioning;
using System.Text;


[SupportedOSPlatform("windows")]
class WindowsHelperMonkey
{
    private Action[] actions;
    private SoundManager soundManager;

    public WindowsHelperMonkey()
    {
        actions = new Action[]
        {
            OpenApps,
            ModifyVolume,
            DesktopScramble,
            AnnoyingPaperclip,
            BananaRequest
        };

        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string relativePath = @"Resources\Sounds"; // Relative path from the output directory
        string fullPathToSounds = Path.Combine(baseDirectory, relativePath);

        soundManager = new(fullPathToSounds);
    }

    public static void SetSystemVolume(int volumeLevel)
    {
        var device = new CoreAudioController().DefaultPlaybackDevice;
        if (device != null && volumeLevel >= 0 && volumeLevel <= 100)
        {
            device.Volume = volumeLevel;
        }
    }

    public void PlaySound(string soundName)
    {
        soundManager.PlaySound(soundName);
    }

    public static void OpenApps()
    {
        Console.WriteLine("Opening and closing apps - Monkey is busy!");
    }

    public static void ModifyVolume()
    {
        Console.WriteLine("Modifying system volume - Monkey is in control!");
    }

    public static void DesktopScramble()
    {
        Console.WriteLine("Scrambling the desktop - Monkey is mischievous!");
    }

    public static void AnnoyingPaperclip()
    {
        Console.WriteLine("Annoying paperclip pal - Monkey is playful!");
    }

    public static void BananaRequest()
    {
        Console.WriteLine("Banana request - Monkey wants a banana!");
    }

    public void Run()
    {
        Console.WriteLine("Enter commands:");
        while (true)
        {
            string? inputLine = Console.ReadLine();
            if (string.IsNullOrEmpty(inputLine)) continue;

            string[] parts = inputLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            string command = parts[0].ToLower();
            string[] args = parts.Skip(1).ToArray();

            switch (command)
            {
                case "playsound":
                    if (args.Length > 0) PlaySound(args[0]);
                    else Console.WriteLine("Missing argument for playsound.");
                    break;
                case "setvolume":
                    if (args.Length > 0 && int.TryParse(args[0], out int volume)) SetSystemVolume(volume);
                    else Console.WriteLine("Invalid or missing argument for setvolume.");
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }

    public void RunWithAutoComplete()
    {
        List<string> commands = new List<string> { "playsound", "setvolume", "exit" };
        Console.WriteLine("Enter commands (with basic autocomplete):");

        while (true)
        {
            Console.Write("> ");
            string input = ReadLineWithAutoComplete(commands);
            if (string.IsNullOrEmpty(input)) continue;

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            string command = parts[0].ToLower();
            string[] args = parts.Skip(1).ToArray();

            switch (command)
            {
                case "playsound":
                    if (args.Length > 0) PlaySound(args[0]);
                    else Console.WriteLine("Missing argument for playsound.");
                    break;
                case "setvolume":
                    if (args.Length > 0 && int.TryParse(args[0], out int volume)) SetSystemVolume(volume);
                    else Console.WriteLine("Invalid or missing argument for setvolume.");
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                // Add more cases for other commands
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }

    string ReadLineWithAutoComplete(List<string> suggestions)
    {
        StringBuilder input = new StringBuilder();
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                if (key.Key == ConsoleKey.W)
                {
                    Console.Write(key.Key + " " + ConsoleKey.Backspace);
                }
                Console.Write(key.Key + " " + ConsoleKey.Backspace);
                Console.WriteLine();
            }

            if (key.Key == ConsoleKey.Tab)
            {
                string prefix = input.ToString();
                var match = suggestions.FirstOrDefault(s => s.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
                if (match != null)
                {
                    Console.Write(match[prefix.Length..]); // Complete the suggestion
                    input.Append(match[prefix.Length..]); // Update input buffer
                }
            }
            else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                Console.Write("\b \b"); // Remove last character
                input.Remove(input.Length - 1, 1);
            }
            else if (!char.IsControl(key.KeyChar))
            {
                Console.Write(key.KeyChar);
                input.Append(key.KeyChar);
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine(); // Move to next line after Enter
        return input.ToString();
    }

    static void Main()
    {
        // SetSystemVolume(50);
        WindowsHelperMonkey monkeyBackend = new WindowsHelperMonkey();
        monkeyBackend.Run();
    }
}
