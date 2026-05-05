using BaseLib.Hooks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace PoseidonAncient.PoseidonAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class BuriedTreasure : PoseidonAncientRelic, IHealAmountModifier
{
    private const string MorePercentGoldKey = "MorePercentGold";
    private const string MorePercentHealKey = "MorePercentHeal";
    private bool _isApplyingBonus;
    private Decimal _pendingBonusGold;

    private Decimal PendingBonusGold
    {
        get => _pendingBonusGold;
        set
        {
            AssertMutable();
            _pendingBonusGold = value;
        }
    }

    private bool IsApplyingBonus
    {
        get => _isApplyingBonus;
        set
        {
            AssertMutable();
            _isApplyingBonus = value;
        }
    }

    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(MorePercentGoldKey, 50M),
        new(MorePercentHealKey, 50M),
        new GoldVar(150),
        new HealVar(15),
    ];

    public Decimal ModifyHealMultiplicative(Creature creature, Decimal amount)
    {
        if (creature.Player != Owner)
        {
            return 1;
        }

        Flash();
        return 1 + DynamicVars[MorePercentHealKey].BaseValue / 100M;
    }

    public override bool ShouldGainGold(Decimal amount, Player player)
    {
        if (IsApplyingBonus || player != Owner)
            return true;
        PendingBonusGold = Math.Floor(amount * (DynamicVars[MorePercentGoldKey].BaseValue / 100M));
        return true;
    }

    public override async Task AfterGoldGained(Player player)
    {
        if (player != Owner || IsApplyingBonus || PendingBonusGold <= 0M)
            return;
        Decimal pendingBonusGold = PendingBonusGold;
        PendingBonusGold = 0M;
        IsApplyingBonus = true;
        Flash();
        await PlayerCmd.GainGold(pendingBonusGold, Owner);
        IsApplyingBonus = false;
    }

    public override async Task AfterObtained()
    {
        await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue * 2M / 3M, Owner);
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue * 2M / 3M);
    }
}