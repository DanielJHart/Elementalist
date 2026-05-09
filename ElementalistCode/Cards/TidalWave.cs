using Elementalist.ElementalistCode.Cards;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Elementalist.ElementalistCode.Cards;


public class TidalWave() : ElementalistCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override bool HasEnergyCostX => true;

    private bool _activated = false;
    private decimal _energyUsed = 0;
    
    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (_activated)
        {
            return card.Owner != this.Owner ? playCount : (int)_energyUsed;
        }

        return playCount;
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        decimal energy = ResolveEnergyXValue();
        if (energy > 0)
        {
            _energyUsed = energy;
            
            CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, 1);
            CardModel? selection = (await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, (Func<CardModel, bool>) (c =>
            {
                return true;
            }), (AbstractModel) this)).FirstOrDefault<CardModel>();

            if (selection != null)
            {
                _activated = true;
                await CardCmd.AutoPlay(choiceContext, selection, null);
                _activated = false;
            }
        }
    }

    protected override void OnUpgrade()
    {

    }
}