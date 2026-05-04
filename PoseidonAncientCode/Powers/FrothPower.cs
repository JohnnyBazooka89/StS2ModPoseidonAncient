using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace PoseidonAncient.PoseidonAncientCode.Powers;

public class FrothPower : PoseidonAncientPower
{
    private const string StacksToTakeDamageKey = "StacksToTakeDamage";
    private const string DamageToTakeKey = "DamageToTake";
    private const string StrengthToLoseKey = "StrengthToLose";

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => GetInternalData<Data>().StacksApplied;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(StacksToTakeDamageKey, 3M),
        new(DamageToTakeKey, 10M),
        new(StrengthToLoseKey, 1M)
    ];

    protected override object InitInternalData() => new Data();

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (amount <= 0M || power != this)
            return;

        Data data = GetInternalData<Data>();
        data.StacksApplied += (int)amount;
        while (data.StacksApplied >= DynamicVars[StacksToTakeDamageKey].IntValue)
        {
            Flash();
            await CreatureCmd.Damage(choiceContext, Owner, DynamicVars[DamageToTakeKey].BaseValue, ValueProp.Unpowered,
                Owner);
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, -DynamicVars[StrengthToLoseKey].BaseValue, Owner, null);            

            data.StacksApplied -= DynamicVars[StacksToTakeDamageKey].IntValue;
            if (data.StacksApplied == 0)
            {
                await PowerCmd.Remove(this);
            }
        }

        InvokeDisplayAmountChanged();
    }

    private class Data
    {
        public int StacksApplied;
    }
}