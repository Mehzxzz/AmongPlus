using System.Collections;
using HarmonyLib;
using Reactor.Utilities;
using TownOfUs.Utilities;
using UnityEngine;

namespace AmongPlus;

[HarmonyPatch(typeof(LobbyBehaviour), nameof(LobbyBehaviour.Start))]
public static class PlayerJoinPatch
{
    internal static void Postfix()
    {
        Coroutines.Start(CoSendJoinMsg());
    }

    internal static IEnumerator CoSendJoinMsg()
    {
        while (!AmongUsClient.Instance) yield return null;
        while (!PlayerControl.LocalPlayer) yield return new WaitForEndOfFrame();
        while (!HudManager.Instance || !HudManager.Instance.Chat) yield return null;

        var player = PlayerControl.LocalPlayer;
        if (!player.AmOwner) yield break;

        MiscUtils.AddSystemChat(player.Data, 
            "<color=#47c408>Thank you for purchasing AmongPlus!", 
            "With this very cheap subscription, for the small price of $125/s, you get to make your name a fancy colour!\n" +
            "<size=60%><i><color=#A3A3A3>(If you didnt get it already this is just a troll lol)</color></i></size>");

        var pl = PluginSingleton<AmongPlusPlugin>.Instance;
        
        pl.Config.Reload();
        var baseName = pl.Name.Value == "Change This" ? player.Data.PlayerName.WithoutRichText() : pl.Name.Value;
        var name = baseName.Length > 12 ? baseName[..12] : baseName;
        ColorUtility.TryParseHtmlString(pl.StartColour.Value, out var start);
        ColorUtility.TryParseHtmlString(pl.MidColour.Value, out var mid);
        ColorUtility.TryParseHtmlString(pl.EndColour.Value, out var end);
        string gradient = AmongPlusPlugin.GetGradient(name, start, mid, end);
                
        player.CmdCheckName(gradient);
    }
}