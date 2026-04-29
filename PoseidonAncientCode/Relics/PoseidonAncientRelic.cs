using BaseLib.Abstracts;
using BaseLib.Extensions;
using PoseidonAncient.PoseidonAncientCode.Extensions;
using Godot;

namespace PoseidonAncient.PoseidonAncientCode.Relics;

public abstract class PoseidonAncientRelic : CustomRelicModel
{
    //PoseidonAncient/images/relics
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
    protected override string PackedIconOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicOutlineImagePath();
    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}