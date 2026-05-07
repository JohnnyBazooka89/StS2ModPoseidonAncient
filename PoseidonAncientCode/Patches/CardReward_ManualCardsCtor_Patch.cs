using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using PoseidonAncient.PoseidonAncientCode.SpireFields;

namespace PoseidonAncient.PoseidonAncientCode.Patches;

[HarmonyPatch(typeof(CardReward))]
[HarmonyPatch(
    MethodType.Constructor,
    typeof(IEnumerable<CardModel>),
    typeof(CardCreationSource),
    typeof(Player),
    typeof(CardCreationOptions)
)]
public static class CardReward_ManualCardsCtor_Patch
{
    private static void Postfix(CardReward __instance, Player player)
    {
        var cards =
            AccessTools.FieldRefAccess<CardReward, List<CardCreationResult>>("_cards")(__instance);

        var cardsToOffer = cards
            .Select<CardCreationResult, CardModel>(result =>
                player.RunState.CloneCard(result.Card)
            )
            .ToList();

        PoseidonSpireFields.SeaStarOriginalCards.Set(__instance, cardsToOffer);
    }
}