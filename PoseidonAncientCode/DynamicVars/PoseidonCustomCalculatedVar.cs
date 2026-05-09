using BaseLib.Cards.Variables;
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace PoseidonAncient.PoseidonAncientCode.DynamicVars;

public class PoseidonCustomCalculatedVar(string name) : CustomCalculatedVar(name)
{
    public Decimal InvokeProtectedCalculateCustom(Creature? target)
    {
        return CalculateCustom(target);
    }
}