using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace PoseidonAncient.PoseidonAncientCode.Hooks;

public interface PoseidonHooks
{
    private static async Task Dispatch<T>(IRunState? runState, ICombatState? combatState, Func<T, Task> action)
        where T : class
    {
        foreach (var model in runState?.IterateHookListeners(combatState).OfType<T>() ?? [])
        {
            await action(model);
        }
    }

    public static Task AfterAnyRelicObtained(IRunState? rs, ICombatState? cs, Player player, RelicModel relic)
    {
        return Dispatch<IAfterAnyRelicObtained>(rs, cs, m => m.AfterAnyRelicObtained(player, relic));
    }
}