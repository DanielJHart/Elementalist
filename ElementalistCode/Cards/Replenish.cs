using Elementalist.ElementalistCode.Cards;
using Elementalist.ElementalistCode.Character;
using Elementalist.ElementalistCode.Powers;
using Elementalist.ElementalistCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Elementalist.ElementalistCode.Cards;


public class Replenish() : ElementalistCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [this.EnergyHoverTip, HoverTipFactory.FromPower<WaterPower>()];

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    public override TargetType TargetType
    {
        get
        {
            // Doing a try catch as this fails if looking into the compendium.
            try
            {
                bool isAligned = ElementalistUtility.IsAligned(this.Owner, ElementType.Water);
                return isAligned ? TargetType.AllAllies : TargetType.AnyAlly;
            }
            catch (Exception)
            {
                return TargetType.AnyAlly;
            }
            
        }
    }
        

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (TargetType == TargetType.AnyAlly && play.Target is { Player: not null })
        {
            await PlayerCmd.GainEnergy(this.DynamicVars.Energy.BaseValue, play.Target.Player);
        }
        else if (TargetType == TargetType.AllAllies)
        {
            var creatureCombatState = this.Owner.Creature.CombatState;
            if (creatureCombatState != null)
            {
                foreach (var ally in creatureCombatState.Allies)
                {
                    if (ally.Player != null)
                    {
                        await PlayerCmd.GainEnergy(this.DynamicVars.Energy.BaseValue, ally.Player);
                    }
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Energy.UpgradeValueBy(1);
    }
}