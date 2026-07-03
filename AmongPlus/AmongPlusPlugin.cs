using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Reactor;
using UnityEngine;

namespace AmongPlus;

[BepInPlugin(AmongPlusPluginInfo.Id, AmongPlusPluginInfo.Name, AmongPlusPluginInfo.Version)]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
public class AmongPlusPlugin : BasePlugin
{
    private Harmony Harmony { get; } = new(AmongPlusPluginInfo.Id);

    public ConfigEntry<string> Name { get; private set; }
    public ConfigEntry<string> StartColour { get; private set; }
    public ConfigEntry<string> MidColour { get; private set; }
    public ConfigEntry<string> EndColour { get; private set; }

    public static ManualLogSource Logger;

    public override void Load()
    {
        Name = Config.Bind("Name", "Name Text", "Change This");
        StartColour = Config.Bind("Name", "Start Colour", "#ffffff");
        MidColour = Config.Bind("Name", "Middle Colour", "#ffffff");
        EndColour = Config.Bind("Name", "End Colour", "#ffffff");
        
        Logger = Log;

        Harmony.PatchAll();
    }
    
    public static string GetGradient(string text, Color32 start, Color32 mid, Color32 end)
    {
        string result = "";
        for (int i = 0; i < text.Length; i++)
        {
            float t = text.Length <= 1 ? 0 : i / (float)(text.Length - 1);
            Color32 c = t < 0.5f ? Color32.Lerp(start, mid, t * 2f) : Color32.Lerp(mid, end, t * 2f - 1f);
            result += $"<color=#{c.r:X2}{c.g:X2}{c.b:X2}>{text[i]}</color>";
            Logger.LogError(result);
        }
        return result;
    }
}