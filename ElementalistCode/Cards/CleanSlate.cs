using Elementalist.ElementalistCode.Cards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Elementalist.ElementalistCode.Cards;

public class CleanSlate() : ElementalistCard(1, CardType.Skill, CardRarity.Rare, TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CombatState? state = this.Owner.Creature.CombatState;
        if (state == null)
            return;
        
        foreach (CardModel card in PileType.Hand.GetPile(this.Owner).Cards.ToList<CardModel>())
        {
            CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Draw);
        }

        foreach (CardModel card in PileType.Discard.GetPile(this.Owner).Cards.ToList<CardModel>())
        {
            CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Draw);
        }

        await CardPileCmd.Shuffle(choiceContext, this.Owner);
        
        // This is taken straight from the begin player turn code.
        decimal handDraw = Hook.ModifyHandDraw(state, this.Owner, 5m, out IEnumerable<AbstractModel> modifiers);
        await Hook.AfterModifyingHandDraw(state, modifiers);
        if (state.RoundNumber == 1)
        {
            CardPile pile = PileType.Draw.GetPile(this.Owner);
            List<CardModel> list = pile.Cards.Where((CardModel c) => c.Enchantment?.ShouldStartAtBottomOfDrawPile ?? false).ToList();
            foreach (CardModel item in list)
            {
                pile.MoveToBottomInternal(item);
            }
            List<CardModel> list2 = pile.Cards.Where((CardModel c) => c.Keywords.Contains(CardKeyword.Innate)).Except(list).ToList();
            foreach (CardModel item2 in list2)
            {
                pile.MoveToTopInternal(item2);
            }
            handDraw = Math.Max(handDraw, list2.Count);
            handDraw = Math.Min(handDraw, 10m);
        }
        
        await CardPileCmd.Draw(choiceContext, handDraw, this.Owner, fromHandDraw: true);
    }

    protected override void OnUpgrade()
    {
        this.AddKeyword(CardKeyword.Retain);
    }
}