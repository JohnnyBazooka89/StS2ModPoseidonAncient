using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;

namespace PoseidonAncient.PoseidonAncientCode.Powers;

public abstract class PoseidonAncientTemporaryPower<TModel, TPower> : CustomTemporaryPowerModelWrapper<TModel, TPower>
    where TModel : AbstractModel
    where TPower : PowerModel
{
    public override string CustomPackedIconPath => PoseidonAncientPowerIconPaths.PackedIconPath(Id.Entry);

    public override string CustomBigIconPath => PoseidonAncientPowerIconPaths.BigIconPath(Id.Entry);
}