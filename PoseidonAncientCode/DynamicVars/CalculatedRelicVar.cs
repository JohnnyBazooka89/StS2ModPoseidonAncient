using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace PoseidonAncient.PoseidonAncientCode.DynamicVars;

public class CalculatedRelicVar(string name) : DynamicVar(name, 0M)
{
    private Func<RelicModel, Creature?, Decimal>? _multiplierCalc;

    public override void SetOwner(AbstractModel owner)
    {
        base.SetOwner(owner);
        UpdateValues();
    }

    public CalculatedRelicVar WithMultiplier(Func<RelicModel, Creature?, Decimal> multiplierCalc)
    {
        if (_multiplierCalc != null)
            throw new InvalidOperationException($"Tried to set extra multiplier calc on {this} twice!");
        _multiplierCalc = !(multiplierCalc.Target is AbstractModel)
            ? multiplierCalc
            : throw new InvalidOperationException("Multiplier calc must be static!");
        return this;
    }

    public Decimal Calculate(Creature? target)
    {
        if (_multiplierCalc == null)
            throw new InvalidOperationException("Extra multiplier calc must be specified!");
        RelicModel owner = (RelicModel)_owner;
        Decimal num = _multiplierCalc(owner, target);
        return GetBaseVar().BaseValue + GetExtraVar().BaseValue * num;
    }

    public override void UpdateCardPreview(
        CardModel card,
        CardPreviewMode previewMode,
        Creature? target,
        bool runGlobalHooks)
    {
        PreviewValue = Calculate(target);
    }

    protected virtual DynamicVar GetBaseVar()
    {
        return ((RelicModel)_owner).DynamicVars.CalculationBase;
    }

    protected virtual DynamicVar GetExtraVar()
    {
        return ((RelicModel)_owner).DynamicVars.CalculationExtra;
    }

    protected override Decimal GetBaseValueForIConvertible() => Calculate(null);

    public override string ToString() => Calculate(null).ToString();

    private void UpdateValues()
    {
        if (_owner == null)
            return;
        BaseValue = GetBaseVar().BaseValue;
    }
}