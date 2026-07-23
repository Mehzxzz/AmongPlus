using MiraAPI.GameOptions;
using MiraAPI.GameOptions.OptionTypes;

namespace AmongPlus;

public sealed class HostOptions : AbstractOptionGroup
{
    public override string GroupName => "Host Options";
    public override uint GroupPriority => 0;

    public ModdedToggleOption ClearRichTextTags { get; set; } =
        new("Clear names using rich text tags (coloured names)", true, false);
}