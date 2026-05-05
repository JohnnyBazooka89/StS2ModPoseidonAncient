using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace PoseidonAncient.PoseidonAncientCode.Hooks;

public interface PoseidonHooks
{
    private static async Task Dispatch<T>(ICombatState? combatState, Func<T, Task> action)
        where T : class
    {
        foreach (var model in combatState?.IterateHookListeners().OfType<T>() ?? [])
        {
            await action(model);
        }
    }

    public static Task AfterAnyRelicObtained(ICombatState? cs, Player player, RelicModel relic)
    {
        return Dispatch<IAfterAnyRelicObtained>(cs, m => m.AfterAnyRelicObtained(player, relic));
    }
}