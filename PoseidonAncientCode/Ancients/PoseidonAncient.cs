using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;
using PoseidonAncient.PoseidonAncientCode.Extensions;
using PoseidonAncient.PoseidonAncientCode.Relics;

namespace PoseidonAncient.PoseidonAncientCode.Ancients;

[Pool(typeof(AncientEventModel))]
public class PoseidonAncient : CustomAncientModel
{
    public override string CustomScenePath => "poseidon.tscn".AncientImagePath();
    public override string CustomMapIconPath => "map_icon.png".AncientImagePath();
    public override string CustomMapIconOutlinePath => "map_icon_outline.png".AncientImagePath();
    public override string CustomRunHistoryIconPath => "run_history_icon.png".AncientImagePath();
    public override string CustomRunHistoryIconOutlinePath => "run_history_icon_outline.png".AncientImagePath();

    protected override OptionPools MakeOptionPools
    {
        get
        {
            List<AncientOption> manyAttacksRelicsPool =
            [
                AncientOption<HydraulicMight>(),
                AncientOption<KingTide>(),
                AncientOption<RazorShoals>(),
                AncientOption<SlipperySlope>(),
            ];

            List<AncientOption> otherRelicsPool =
            [
                AncientOption<BuriedTreasure>(),
                AncientOption<FloodGain>(),
                AncientOption<HighSurf>(),
                AncientOption<SeaStar>(),
                AncientOption<SecondWave>(),
                AncientOption<WaterFitness>(),
                AncientOption<WaveStrike>(),
                AncientOption<WaveFlourish>()
            ];

            return new OptionPools(MakePool(otherRelicsPool.ToArray()), MakePool(manyAttacksRelicsPool.ToArray()));
        }
    }

    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 2;
    }
}