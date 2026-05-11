using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace PoseidonAncient.PoseidonAncientCode.Powers;

public class SlipperySlopePower : PoseidonAncientTemporaryPower<FrothPower, StrengthPower>
{
    protected override Func<PlayerChoiceContext, Creature, decimal, Creature?, CardModel?, bool, Task> ApplyPowerFunc
        => (playerChoiceContext, creature, amount, applier, cardSource, _)
            => PowerCmd.Apply<StrengthPower>(playerChoiceContext, creature,
                amount, applier, cardSource);
    public override PowerModel InternallyAppliedPower => ModelDb.Power<StrengthPower>();

    public override AbstractModel OriginModel =>
        ModelDb.Power<FrothPower>();

    protected override bool InvertInternalPowerAmount => true;

    public override PowerType Type => PowerType.Debuff;

    public override LocString Description => new("powers", "TEMPORARY_STRENGTH_DOWN.description");

    protected override string SmartDescriptionLocKey => "TEMPORARY_STRENGTH_DOWN.smartDescription";
}