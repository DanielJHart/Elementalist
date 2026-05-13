using Elementalist.ElementalistCode.Cards;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Elementalist.ElementalistCode.Cards;

public class SecondWind() : ElementalistCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromSimpleGrid(choiceContext, PileType.Exhaust.GetPile(this.Owner).Cards, this.Owner, prefs)).FirstOrDefault<CardModel>();
        if (card == null)
            return;
        CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Hand);

        if (this.IsUpgraded && cardPileAddResult.success)
        {
            card.SetToFreeThisTurn();
        }
    }
}