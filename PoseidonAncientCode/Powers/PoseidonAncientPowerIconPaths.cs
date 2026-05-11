using BaseLib.Extensions;
using PoseidonAncient.PoseidonAncientCode.Extensions;

namespace PoseidonAncient.PoseidonAncientCode.Powers;

public static class PoseidonAncientPowerIconPaths
{
    public static string PackedIconPath(string entry) => $"{entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();

    public static string BigIconPath(string entry) => $"{entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}