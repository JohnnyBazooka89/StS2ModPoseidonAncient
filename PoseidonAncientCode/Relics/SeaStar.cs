using System.Reflection;
using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rewards;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using PoseidonAncient.PoseidonAncientCode.Extensions;
using PoseidonAncient.PoseidonAncientCode.SpireFields;

namespace PoseidonAncient.PoseidonAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class SeaStar : PoseidonAncientRelic
{
    private const string RewardCopyPercentChangeKey = "RewardCopyPercentChange";

    private static readonly MethodInfo UpdateScreenStateMethod =
        AccessTools.Method(typeof(NRewardsScreen), "UpdateScreenState");

    private static readonly MethodInfo TryEnableProceedButtonMethod =
        AccessTools.Method(typeof(NRewardsScreen), "TryEnableProceedButton");

    private static readonly List<String> SoundPaths =
    [
        "sea_star/Poseidon [247].ogg".SoundPath(),
        "sea_star/Poseidon [248].ogg".SoundPath(),
        "sea_star/Poseidon [249].ogg".SoundPath(),
        "sea_star/Poseidon [328].ogg".SoundPath(),
        "sea_star/Poseidon [328].ogg".SoundPath(),
        "sea_star/Poseidon [328].ogg".SoundPath(),
        "sea_star/Poseidon [330].ogg".SoundPath(),
        "sea_star/Poseidon [331].ogg".SoundPath(),
        "sea_star/Poseidon [334].ogg".SoundPath(),
        "sea_star/Poseidon [338].ogg".SoundPath(),
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(RewardCopyPercentChangeKey, 30M),
    ];

    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override async Task AfterRewardTaken(Player player, Reward reward)
    {
        if (player != Owner)
        {
            return;
        }

        int random = Owner.RunState.Rng.Niche.NextInt(0, 100);
        if (random >= DynamicVars[RewardCopyPercentChangeKey].BaseValue)
        {
            return;
        }

        Reward? newReward = GetSameTypeReward(reward, Owner);
        if (newReward == null)
        {
            return;
        }

        PlaySound();
        await AddRewardToCurrentScreen(newReward);
    }

    private static Reward? GetSameTypeReward(Reward reward, Player player)
    {
        if (reward is CardReward cardReward)
        {
            var optionsGetter =
                AccessTools.PropertyGetter(typeof(CardReward), "Options");

            var rerollOptionsGetter =
                AccessTools.PropertyGetter(typeof(CardReward), "RerollOptions");

            var optionCountGetter =
                AccessTools.PropertyGetter(typeof(CardReward), "OptionCount");

            var cardsWereManuallySet =
                AccessTools.FieldRefAccess<CardReward, bool>("_cardsWereManuallySet")(cardReward);

            if (cardsWereManuallySet)
            {
                var options = (CardCreationOptions)optionsGetter.Invoke(cardReward, null)!;
                var rerollOptions = (CardCreationOptions)rerollOptionsGetter.Invoke(cardReward, null)!;

                var originalCards = PoseidonSpireFields.SeaStarOriginalCards.Get(cardReward) ?? [];
                var cardsToOffer = originalCards
                    .Select<CardModel, CardModel>(card =>
                        cardReward.Player.RunState.CloneCard(card)
                    )
                    .ToList();

                return new CardReward(
                    cardsToOffer,
                    options.Source,
                    cardReward.Player,
                    rerollOptions
                );
            }
            else
            {
                var options = (CardCreationOptions)optionsGetter.Invoke(cardReward, null)!;
                var optionCount = (int)optionCountGetter.Invoke(cardReward, null)!;

                return new CardReward(
                    options,
                    optionCount,
                    cardReward.Player
                );
            }
        }

        if (reward is CardRemovalReward cardRemovalReward)
        {
            return new CardRemovalReward(cardRemovalReward.Player);
        }

        if (reward is GoldReward goldReward)
        {
            return new GoldReward(
                goldReward.Amount,
                goldReward.Player,
                AccessTools.FieldRefAccess<GoldReward, bool>("_wasGoldStolenBack")(goldReward)
            );
        }

        if (reward is PotionReward potionReward)
        {
            return new PotionReward(potionReward.Player);
        }

        if (reward is RelicReward relicReward)
        {
            return new RelicReward(
                relicReward.Rarity,
                relicReward.Player
            );
        }

        if (reward is SpecialCardReward specialCardReward)
        {
            CardModel specialCard =
                AccessTools.FieldRefAccess<SpecialCardReward, CardModel>("_card")(specialCardReward);
            if (specialCard is LanternKey) // There's no need to duplicate Lantern Key
            {
                return null;
            }

            return new SpecialCardReward(
                specialCard,
                specialCardReward.Player
            );
        }

        return null;
    }

    private void PlaySound()
    {
        String soundPath = SoundPaths[Owner.RunState.Rng.Niche.NextInt(0, SoundPaths.Count)];

        if (PoseidonModConfig.DisableSeaStarSoundEffects)
        {
            return;
        }

        float master = SaveManager.Instance.SettingsSave.VolumeMaster;
        float sfx = SaveManager.Instance.SettingsSave.VolumeSfx;

        float finalLinear = master * sfx;

        AudioStream sound = GD.Load<AudioStream>(soundPath);
        AudioStreamPlayer player = new()
        {
            Stream = sound,
            VolumeLinear = finalLinear
        };
        NGame.Instance.AddChild(player);
        player.Play();
        player.Finished += player.QueueFree;
    }

    private static async Task AddRewardToCurrentScreen(Reward newReward)
    {
        if (!newReward.IsPopulated)
        {
            await newReward.Populate();
        }

        newReward.MarkContentAsSeen();

        var screen = NOverlayStack.Instance
            .GetChildren()
            .OfType<NRewardsScreen>()
            .LastOrDefault();

        if (screen == null)
            return;

        var rewardButtonsRef =
            AccessTools.FieldRefAccess<NRewardsScreen, List<Control>>("_rewardButtons");

        var rewardsContainerRef =
            AccessTools.FieldRefAccess<NRewardsScreen, Control>("_rewardsContainer");

        var rewardButtons = rewardButtonsRef(screen);
        var rewardsContainer = rewardsContainerRef(screen);

        Control option;

        if (newReward is LinkedRewardSet linkedReward)
        {
            option = NLinkedRewardSet.Create(linkedReward, screen);
            option.Connect(
                NLinkedRewardSet.SignalName.RewardClaimed,
                Callable.From<NLinkedRewardSet>(screen.RewardCollectedFrom)
            );
        }
        else
        {
            var button = NRewardButton.Create(newReward, screen);

            button.Connect(
                NRewardButton.SignalName.RewardClaimed,
                Callable.From<NRewardButton>(screen.RewardCollectedFrom)
            );

            button.Connect(
                NRewardButton.SignalName.RewardSkipped,
                Callable.From<NRewardButton>(screen.RewardSkippedFrom)
            );

            option = button;
        }

        rewardButtons.Add(option);
        rewardsContainer.AddChildSafely(option);

        UpdateScreenStateMethod.Invoke(screen, null);
        TryEnableProceedButtonMethod.Invoke(screen, null);
    }
}