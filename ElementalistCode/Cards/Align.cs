using Elementalist.ElementalistCode.Cards;
using Elementalist.ElementalistCode.Character;
using Elementalist.ElementalistCode.Relics;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Elementalist.ElementalistCode.Cards;

public class Align() : ElementalistCard(1,
    CardType.Skill, CardRarity.Event,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    protected override Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (this.Owner.Character is Character.Elementalist elementalist)
        {
            var relic = this.Owner.GetRelic<ElementalMastery>();
            relic?.SetElement(elementalist.GetCurrentElement(CycleType.Secondary));
        }
        
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}