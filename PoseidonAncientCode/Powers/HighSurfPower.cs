using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using PoseidonAncient.PoseidonAncientCode.Relics;

namespace PoseidonAncient.PoseidonAncientCode.Powers;

public class HighSurfPower : PoseidonAncientPower
{
    private bool _shouldIgnoreNextInstance;

    public override PowerType Type => !IsPositive ? PowerType.Debuff : PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public AbstractModel OriginModel => ModelDb.Relic<HighSurf>();

    public PowerModel InternallyAppliedPower => ModelDb.Power<DexterityPower>();

    protected virtual bool IsPositive => true;

    private int Sign => !IsPositive ? -1 : 1;

    public override LocString Title
    {
        get
        {
            switch (OriginModel)
            {
                case CardModel cardModel:
                    return cardModel.TitleLocString;
                case PotionModel potionModel:
                    return potionModel.Title;
                case RelicModel relicModel:
                    return relicModel.Title;
                default:
                    throw new InvalidOperationException();
            }
        }
    }

    public override LocString Description => new LocString("powers",
        IsPositive ? "TEMPORARY_DEXTERITY_POWER.description" : "TEMPORARY_DEXTERITY_DOWN.description");

    protected override string SmartDescriptionLocKey => !IsPositive
        ? "TEMPORARY_DEXTERITY_DOWN.smartDescription"
        : "TEMPORARY_DEXTERITY_POWER.smartDescription";

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> items = new List<IHoverTip>();
            List<IHoverTip> hoverTipList = items;
            IEnumerable<IHoverTip> collection;
            switch (OriginModel)
            {
                case CardModel card:
                    collection = [HoverTipFactory.FromCard(card)];
                    break;
                case PotionModel model:
                    collection = [HoverTipFactory.FromPotion(model)];
                    break;
                case RelicModel relic:
                    collection = HoverTipFactory.FromRelic(relic);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            hoverTipList.AddRange(collection);
            items.Add(HoverTipFactory.FromPower<DexterityPower>());
            return items;
        }
    }

    public void IgnoreNextInstance() => _shouldIgnoreNextInstance = true;

    public override async Task BeforeApplied(
        Creature target,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (_shouldIgnoreNextInstance)
        {
            _shouldIgnoreNextInstance = false;
        }
        else
        {
            await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), target, Sign * amount, applier,
                cardSource, true);
        }
    }

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (amount == Amount || power != this)
            return;
        if (_shouldIgnoreNextInstance)
        {
            _shouldIgnoreNextInstance = false;
        }
        else
        {
            await PowerCmd.Apply<DexterityPower>(choiceContext, Owner, Sign * amount, applier, cardSource, true);
        }
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side)
            return;
        Flash();
        await PowerCmd.Remove(this);
        await PowerCmd.Apply<DexterityPower>(choiceContext, Owner, -Sign * Amount, Owner, null);
    }
}