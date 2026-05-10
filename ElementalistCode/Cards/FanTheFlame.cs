using Elementalist.ElementalistCode.Cards;
using Elementalist.ElementalistCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Elementalist.ElementalistCode.Cards;

public class FanTheFlame() : ElementalistCard(2, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BurnPower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        FanTheFlame fanTheFlame = this;

        if (fanTheFlame.CombatState == null)
            return;
        
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target!, "cardPlay.Target");
        List<PowerModel> originalDebuffs = cardPlay.Target.Powers.Where<PowerModel>((Func<PowerModel, bool>) (p => p is BurnPower)).Select<PowerModel, PowerModel>((Func<PowerModel, PowerModel>) (p => (PowerModel) p.ClonePreservingMutability())).ToList<PowerModel>();
        foreach (Creature hittableEnemy in (IEnumerable<Creature>) fanTheFlame.CombatState.HittableEnemies)
        {
            Creature enemy = hittableEnemy;
            if (enemy != cardPlay.Target)
            {
                foreach (PowerModel powerModel in originalDebuffs)
                {
                    PowerModel? powerById = enemy.GetPowerById(powerModel.Id);
                    if (powerById != null && !powerById.IsInstanced)
                    {
                        FanTheFlame.DoHackyThingsForSpecificPowers(powerById);
                        int num = await PowerCmd.ModifyAmount(powerById, (Decimal) powerModel.Amount, fanTheFlame.Owner.Creature, (CardModel) fanTheFlame);
                    }
                    else
                    {
                        PowerModel power = (PowerModel) powerModel.ClonePreservingMutability();
                        FanTheFlame.DoHackyThingsForSpecificPowers(power);
                        await PowerCmd.Apply(power, enemy, (Decimal) powerModel.Amount, fanTheFlame.Owner.Creature, (CardModel) fanTheFlame);
                    }
                }
            }
        }
    }
    
    private static void DoHackyThingsForSpecificPowers(PowerModel power)
    {
        if (!(power is ITemporaryPower temporaryPower))
            return;
        temporaryPower.IgnoreNextInstance();
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}