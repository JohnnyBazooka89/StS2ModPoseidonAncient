using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace PoseidonAncient.PoseidonAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class HydraulicMight : PoseidonAncientRelic
{
    private const string TurnsKey = "Turns";

    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(TurnsKey, 2M)
    ];

    public override Decimal ModifyDamageMultiplicative(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (!props.IsPoweredAttack() || cardSource == null || dealer != Owner.Creature)
            return 1M;

        return dealer.CombatState.RoundNumber <= DynamicVars[TurnsKey].BaseValue ? 2M : 1M;
    }
}