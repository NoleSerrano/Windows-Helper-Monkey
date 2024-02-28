using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Runtime.Versioning;

[SupportedOSPlatform("windows")]
class SoundManager
{
    private Dictionary<string, string> soundFilePaths = new Dictionary<string, string>();
    private SoundPlayer player = new SoundPlayer();

    public SoundManager(string soundsDirectory)
    {
        LoadSounds(soundsDirectory);
    }

    private void LoadSounds(string directoryPath)
    {
        foreach (var filePath in Directory.GetFiles(directoryPath))
        {
            // This includes the extension in the key name
            var fileNameWithExtension = Path.GetFileName(filePath);
            soundFilePaths[fileNameWithExtension] = filePath;
        }
    }

    public void PlaySound(string soundName)
    {
        if (soundFilePaths.TryGetValue(soundName, out var filePath))
        {
            player.SoundLocation = filePath;
            player.Play();
        }
        else
        {
            // Handle the case where the sound name is not found
            Console.WriteLine($"Sound not found: {soundName}");
        }
    }
}