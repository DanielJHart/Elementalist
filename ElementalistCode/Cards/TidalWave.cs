using Elementalist.ElementalistCode.Cards;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Elementalist.ElementalistCode.Cards;


public class TidalWave() : ElementalistCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        decimal energy = ResolveEnergyXValue();
        if (energy > 0)
        {
            CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, 1);
            CardModel? selection = (await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, (Func<CardModel, bool>) (c =>
            {
                return true;
            }), (AbstractModel) this)).FirstOrDefault<CardModel>();

            if (selection != null)
            {
                
            }
        }
    }

    protected override void OnUpgrade()
    {

    }
}