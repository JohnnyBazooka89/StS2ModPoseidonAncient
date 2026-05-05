using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace PoseidonAncient.PoseidonAncientCode.Hooks;

public interface IAfterAnyRelicObtained
{
    Task AfterAnyRelicObtained(Player player, RelicModel relic);
}