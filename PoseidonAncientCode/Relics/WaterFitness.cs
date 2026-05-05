using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using PoseidonAncient.PoseidonAncientCode.DynamicVars;
using PoseidonAncient.PoseidonAncientCode.Hooks;

namespace PoseidonAncient.PoseidonAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class WaterFitness : PoseidonAncientRelic, IAfterAnyRelicObtained
{
    private const string TotalHpToGainKey = "TotalHpToGain";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new MaxHpVar(2M),
        new CalculationBaseVar(0M),
        new CalculationExtraVar(1M),
        new CalculatedRelicVar(TotalHpToGainKey).WithMultiplier(static (relic, _) =>
            (relic.Owner.Relics.Count + 1) * relic.DynamicVars.MaxHp.BaseValue)
    ];


    public override RelicRarity Rarity => RelicRarity.Ancient;

    public async Task AfterAnyRelicObtained(Player player, RelicModel relic)
    {
        Flash();
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
    }

    public override async Task AfterObtained()
    {
        Flash();
        await CreatureCmd.GainMaxHp(Owner.Creature,
            ((CalculatedRelicVar)DynamicVars[TotalHpToGainKey]).Calculate(null) - DynamicVars.MaxHp.BaseValue);
    }
}