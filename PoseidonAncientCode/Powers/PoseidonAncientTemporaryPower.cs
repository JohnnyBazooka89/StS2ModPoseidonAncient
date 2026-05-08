using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using PoseidonAncient.PoseidonAncientCode.Extensions;

namespace PoseidonAncient.PoseidonAncientCode.Powers;

public abstract class PoseidonAncientTemporaryPower : CustomTemporaryPowerModel
{
    //Loads from PoseidonAncient/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();

    protected abstract IHoverTip InternallyAppliedPowerHoverTip { get; }

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
                case PowerModel power:
                    collection = [HoverTipFactory.FromPower(power)];
                    break;
                default:
                    throw new InvalidOperationException();
            }

            hoverTipList.AddRange(collection);
            items.Add(InternallyAppliedPowerHoverTip);
            return items;
        }
    }
}