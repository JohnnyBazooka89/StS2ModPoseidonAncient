using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;
using PoseidonAncient.PoseidonAncientCode.Powers;

namespace PoseidonAncient.PoseidonAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class RazorShoals : PoseidonAncientRelic
{
    private const string RuptureToApplyKey = "RuptureToApply";

    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(RuptureToApplyKey, 1M)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<RupturePower>()
    ];

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource)
    {
        if (dealer != Owner.Creature || !props.IsPoweredAttack())
            return;
        Flash();
        await PowerCmd.Apply<RupturePower>(choiceContext, target, DynamicVars[RuptureToApplyKey].BaseValue,
            Owner.Creature, null);
    }
}