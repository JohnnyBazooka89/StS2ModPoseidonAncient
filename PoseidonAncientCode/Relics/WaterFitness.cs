using BaseLib.Cards.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using PoseidonAncient.PoseidonAncientCode.Hooks;

namespace PoseidonAncient.PoseidonAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class WaterFitness : PoseidonAncientRelic, IAfterAnyRelicObtained
{
    private const string TotalHpToGainKey = "TotalHpToGain";

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new MaxHpVar(2M),
        new(TotalHpToGainKey + "Base", 0M),
        new(TotalHpToGainKey + "Extra", 1M),
        new CustomCalculatedVar(TotalHpToGainKey).WithMultiplier(static (relic, _) =>
            (relic.Owner.Relics.Count + 1) * relic.DynamicVars.MaxHp.BaseValue)
    ];

    public override RelicRarity Rarity => RelicRarity.Ancient;

    public async Task AfterAnyRelicObtained(Player player, RelicModel relic)
    {
        if (relic == this)
        {
            return;
        }

        Flash();
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
    }

    public override async Task AfterObtained()
    {
        Flash();
        await CreatureCmd.GainMaxHp(Owner.Creature,
            ((CustomCalculatedVar)DynamicVars[TotalHpToGainKey]).CalculateCustom(null) -
            DynamicVars.MaxHp.BaseValue);
    }
}