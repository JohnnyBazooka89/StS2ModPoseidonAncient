using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using PoseidonAncient.PoseidonAncientCode.Relics;

namespace PoseidonAncient.PoseidonAncientCode.Powers;

public class HighSurfPower : PoseidonAncientTemporaryPower<HighSurf, DexterityPower>
{
    protected override Func<PlayerChoiceContext, Creature, decimal, Creature?, CardModel?, bool, Task> ApplyPowerFunc
        => (playerChoiceContext, creature, amount, applier, cardSource, _)
            => PowerCmd.Apply<DexterityPower>(playerChoiceContext, creature,
                amount, applier, cardSource);
    public override PowerModel InternallyAppliedPower => ModelDb.Power<DexterityPower>();

    public override AbstractModel OriginModel =>
        ModelDb.Relic<HighSurf>();

    public override LocString Description => new("powers", "TEMPORARY_DEXTERITY_POWER.description");

    protected override string SmartDescriptionLocKey => "TEMPORARY_DEXTERITY_POWER.smartDescription";
}