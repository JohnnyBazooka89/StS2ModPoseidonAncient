using BaseLib.Abstracts;

namespace PoseidonAncient.PoseidonAncientCode.Powers;

public abstract class PoseidonAncientPower : CustomPowerModel
{
    public override string CustomPackedIconPath => PoseidonAncientPowerIconPaths.PackedIconPath(Id.Entry);

    public override string CustomBigIconPath => PoseidonAncientPowerIconPaths.BigIconPath(Id.Entry);
}