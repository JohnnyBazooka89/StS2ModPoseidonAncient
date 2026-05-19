using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Events;

namespace PoseidonAncient.PoseidonAncientCode.Patches;

[HarmonyPatch(typeof(NAncientEventLayout), "SetDialogueLineAndAnimate")]
public static class PoseidonAncient_AdjustLayout_Patch
{
    private const float XOffset = 185f;
    private const float YOffset = -5f;
    private const float ScaleAmount = 1.0f;
    private const float OriginalSpacing = 10f;

    private static readonly ConditionalWeakTable<NAncientEventLayout, Box> State = new();

    private static bool IsTargetEvent(NAncientEventLayout layout)
    {
        var ancientEvent = Traverse.Create(layout)
            .Field("_ancientEvent")
            .GetValue<AncientEventModel>();

        return ancientEvent.Id == ModelDb.GetId<Ancients.PoseidonAncient>();
    }

    static void Prefix(NAncientEventLayout __instance)
    {
        if (!IsTargetEvent(__instance))
            return;

        var t = Traverse.Create(__instance);

        var content = t.Field("_content").GetValue<VBoxContainer>();
        var contentContainer = t.Field("_contentContainer").GetValue<Control>();

        if (content == null || contentContainer == null)
            return;

        if (!State.TryGetValue(__instance, out var state))
        {
            state = new Box(content.Position.X, contentContainer.Size.X);
            State.Add(__instance, state);
        }

        content.Position = new Vector2(
            state.BaseContentX + XOffset,
            content.Position.Y
        );

        content.Scale = new Vector2(ScaleAmount, ScaleAmount);

        contentContainer.ClipContents = false;

        float extraWidth = Mathf.Abs(XOffset) * 2f;

        contentContainer.Size = new Vector2(
            state.BaseContainerWidth + extraWidth,
            contentContainer.Size.Y
        );
    }

    private static float GetSpacingForEvent(NAncientEventLayout layout, float originalSpacing)
    {
        if (!IsTargetEvent(layout))
            return originalSpacing;

        return originalSpacing - YOffset;
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var code in instructions)
        {
            if (code.opcode == OpCodes.Ldc_R4 &&
                code.operand is float f &&
                f == OriginalSpacing)
            {
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return new CodeInstruction(OpCodes.Ldc_R4, OriginalSpacing);
                yield return CodeInstruction.Call(
                    typeof(PoseidonAncient_AdjustLayout_Patch),
                    nameof(GetSpacingForEvent)
                );
            }
            else
            {
                yield return code;
            }
        }
    }

    private sealed class Box
    {
        public readonly float BaseContainerWidth;
        public readonly float BaseContentX;

        public Box(float x, float width)
        {
            BaseContentX = x;
            BaseContainerWidth = width;
        }
    }
}