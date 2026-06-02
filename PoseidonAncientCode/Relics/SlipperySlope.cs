using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;
using PoseidonAncient.PoseidonAncientCode.Powers;

namespace PoseidonAncient.PoseidonAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class SlipperySlope : PoseidonAncientRelic
{
    private const string FrothToApplyKey = "FrothToApply";

    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(FrothToApplyKey, 1M)
    ];

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<FrothPower>(),
        HoverTipFactory.FromPower<StrengthPower>()
    ];

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource)
    {
        if ((dealer != Owner.Creature && dealer != Owner.Osty) || !props.IsPoweredAttack())
            return;
        Flash();
        await PowerCmd.Apply<FrothPower>(choiceContext, target, DynamicVars[FrothToApplyKey].BaseValue,
            Owner.Creature, null);
    }
}