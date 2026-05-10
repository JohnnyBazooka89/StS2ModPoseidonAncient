using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using PoseidonAncient.PoseidonAncientCode.Hooks;

namespace PoseidonAncient.PoseidonAncientCode.Patches;

[HarmonyPatch(typeof(RelicCmd), nameof(RelicCmd.Obtain), typeof(RelicModel), typeof(Player), typeof(int))]
public static class RelicCmd_Obtain_Patch
{
    public static async Task<RelicModel> Postfix(Task<RelicModel> __result, Player player)
    {
        RelicModel relic = await __result;

        await PoseidonHooks.AfterAnyRelicObtained(player.RunState, player.Creature.CombatState, player, relic);

        return relic;
    }
}