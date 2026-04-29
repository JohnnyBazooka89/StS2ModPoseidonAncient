using BaseLib.Abstracts;
using BaseLib.Extensions;
using PoseidonAncient.PoseidonAncientCode.Extensions;
using Godot;

namespace PoseidonAncient.PoseidonAncientCode.Powers;

public abstract class PoseidonAncientPower : CustomPowerModel
{
    //Loads from PoseidonAncient/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}