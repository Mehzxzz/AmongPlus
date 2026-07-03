using System;
using HarmonyLib;
using Reactor.Utilities;
using TownOfUs.Utilities;
using UnityEngine;

namespace AmongPlus;

[HarmonyPatch]
public static class ChatCommandPatches
{
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
    public static bool FirstPrefix(ChatController __instance)
    {
        var textRegular = __instance.freeChatField.Text.WithoutRichText();

        if (textRegular.Length < 1)
        {
            return true;
        }

        var systemName = "<color=#47c408>AmongPlus</color>";

        if (textRegular.StartsWith("/lc", StringComparison.OrdinalIgnoreCase) || textRegular.StartsWith("/loadcolour", StringComparison.OrdinalIgnoreCase) || textRegular.StartsWith("/loadcolor", StringComparison.OrdinalIgnoreCase))
        {
            var msg = "You can only update your name colour in the lobby.";
            if (LobbyBehaviour.Instance)
            {
                var pl = PluginSingleton<AmongPlusPlugin>.Instance;
                var player = PlayerControl.LocalPlayer;

                pl.Config.Reload();
                var baseName = pl.Name.Value == "Change This" ? player.Data.PlayerName.WithoutRichText() : pl.Name.Value;
                var name = baseName.Length > 12 ? baseName[..12] : baseName;
                ColorUtility.TryParseHtmlString(pl.StartColour.Value, out var start);
                ColorUtility.TryParseHtmlString(pl.MidColour.Value, out var mid);
                ColorUtility.TryParseHtmlString(pl.EndColour.Value, out var end);
                string gradient = AmongPlusPlugin.GetGradient(name, start, mid, end);
                
                player.CmdCheckName(gradient);
                
                msg = $"Your name colour has been updated! Your name is now {player.Data.PlayerName}.";
            }

            MiscUtils.AddSystemChat(PlayerControl.LocalPlayer.Data, systemName, msg);

            ClearChat(__instance);
            
            return false;
        }

        return true;
    }

    private static void ClearChat(ChatController chat)
    {
        chat.freeChatField.Clear();
        chat.quickChatMenu.Clear();
        chat.quickChatField.Clear();
        chat.UpdateChatMode();
    }
}