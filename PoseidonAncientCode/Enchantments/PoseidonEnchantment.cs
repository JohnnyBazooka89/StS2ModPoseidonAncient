using BaseLib.Abstracts;
using BaseLib.Extensions;
using PoseidonAncient.PoseidonAncientCode.Extensions;

namespace PoseidonAncient.PoseidonAncientCode.Enchantments;

public abstract class PoseidonEnchantment : CustomEnchantmentModel
{
    protected override string CustomIconPath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".EnchantmentImagePath();
}