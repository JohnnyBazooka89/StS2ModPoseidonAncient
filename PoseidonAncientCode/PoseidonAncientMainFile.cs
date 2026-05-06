using BaseLib.Config;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace PoseidonAncient.PoseidonAncientCode;

[ModInitializer(nameof(Initialize))]
public partial class PoseidonAncientMainFile : Node
{
    public const string ModId = "PoseidonAncient"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static Logger Logger { get; } =
        new(ModId, LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);

        harmony.PatchAll();

        ModConfigRegistry.Register(ModId, new PoseidonModConfig());
    }
}