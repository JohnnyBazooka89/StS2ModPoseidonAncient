using System.Runtime.CompilerServices;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Events;

namespace PoseidonAncient.PoseidonAncientCode.Patches;

[HarmonyPatch(typeof(NAncientEventLayout), "SetDialogueLineAndAnimate")]
public static class AphroditeAncientLayoutPatch
{
    private const float XOffset = 185f;

    private static readonly ConditionalWeakTable<NAncientEventLayout, Box> State = new();

    static void Prefix(NAncientEventLayout __instance)
    {
        var t = Traverse.Create(__instance);

        var ancientEvent = t.Field("_ancientEvent").GetValue<AncientEventModel>();
        if (ancientEvent.Id != ModelDb.GetId<Ancients.PoseidonAncient>())
        {
            return;
        }

        var content = t.Field("_content").GetValue<VBoxContainer>();
        var contentContainer = t.Field("_contentContainer").GetValue<Control>();

        if (content == null || contentContainer == null)
            return;

        if (!State.TryGetValue(__instance, out var state))
        {
            state = new Box(
                content.Position.X,
                contentContainer.Size.X,
                contentContainer.CustomMinimumSize.X,
                content.CustomMinimumSize.X
            );

            State.Add(__instance, state);
        }

        float extraWidth = Mathf.Abs(XOffset) * 2f;

        contentContainer.ClipContents = false;

        content.Position = new Vector2(
            state.BaseContentX + XOffset,
            content.Position.Y
        );

        contentContainer.Size = new Vector2(
            state.BaseContainerWidth + extraWidth,
            contentContainer.Size.Y
        );

        contentContainer.CustomMinimumSize = new Vector2(
            state.BaseContainerMinWidth + extraWidth,
            contentContainer.CustomMinimumSize.Y
        );

        content.CustomMinimumSize = new Vector2(
            state.BaseContentMinWidth + extraWidth,
            content.CustomMinimumSize.Y
        );
    }

    private sealed class Box
    {
        public readonly float BaseContainerMinWidth;
        public readonly float BaseContainerWidth;
        public readonly float BaseContentMinWidth;
        public readonly float BaseContentX;

        public Box(float contentX, float containerWidth, float containerMinWidth, float contentMinWidth)
        {
            BaseContentX = contentX;
            BaseContainerWidth = containerWidth;
            BaseContainerMinWidth = containerMinWidth;
            BaseContentMinWidth = contentMinWidth;
        }
    }
}