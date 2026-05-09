using Elementalist.ElementalistCode.Character;
using Elementalist.ElementalistCode.Relics;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Elementalist.ElementalistCode.Cards;

public abstract class ChangeElementBase() : ElementalistCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected abstract ElementType  ElementToChangeTo { get; }

    protected override bool IsPlayable
    {
        get
        {
            if (this.Owner.Character is Character.Elementalist elementalist)
            {
                return elementalist.GetCurrentElement(0) !=  ElementToChangeTo;
            }
            
            return true;
        }
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        // TODO: Make sure this works for the upgraded version too...
        var relic = this.Owner.GetRelic<ElementalAttunement>();
        relic?.SetElement(ElementToChangeTo);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}