using System.Collections;
using System.Text.RegularExpressions;
using HarmonyLib;
using MiraAPI.GameOptions;
using TownOfUs.Utilities;
using UnityEngine;

namespace AmongPlus;

// Original system by https://github.com/idkimneil - https://github.com/AU-Avengers/TOU-Mira/commit/1a8c52ff88a69c1f042fb11d4713eb22e6d8e6d9

[HarmonyPatch]
public static class AntiRichTextNamePatch
{
    private static readonly Regex RichTextPattern = new(
        "<[^>]+>",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CheckName))]
    [HarmonyPostfix]
    public static void CheckNamePostfix(PlayerControl __instance)
    {
        string name = __instance.Data.PlayerName;

        if (!RichTextPattern.IsMatch(name)) return;
        if (!AmongUsClient.Instance.AmHost) return;
        if (!OptionGroupSingleton<HostOptions>.Instance.ClearRichTextTags) return;

        MiscUtils.AddFakeChat(PlayerControl.LocalPlayer.Data, $"<color=#D53F42>Anticheat Warning</color>", $"{name}'s name contains Unity Rich Text Tags! Their name has been reset to default text.", true, altColors:true);
        __instance.CmdCheckName(name.WithoutRichText());
    }
}