using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace PoseidonAncient.PoseidonAncientCode.Powers;

public class RupturePower : PoseidonAncientPower
{
    private static readonly Color Color = new(110/255f, 0, 0);

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (side != Owner.Side)
        {
            return;
        }

        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner, Amount, ValueProp.Unpowered, Owner);
    }

    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        return [new HealthBarForecastSegment(Amount, Color, HealthBarForecastDirection.FromRight)];
    }
}