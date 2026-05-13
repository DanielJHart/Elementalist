using Elementalist.ElementalistCode.Character;
using Elementalist.ElementalistCode.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Elementalist.ElementalistCode.Powers;

public class FlowStatePower : ElementalistPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    private bool ShouldModifyPlayCount(CardModel card)
    {
        bool returnValue = card.Type == CardType.Skill;
        
        if (this.Owner.Player == null)
            return returnValue;
        
        return returnValue || ElementalistUtility.IsAligned(this.Owner.Player, ElementType.Water);
    }
    
    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card.Owner.Creature == this.Owner && ShouldModifyPlayCount(card))
        {
            return playCount * 2;
        }
        
        return base.ModifyCardPlayCount(card, target, playCount);
    }

    public override Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        PowerCmd.Remove(this);
        return Task.CompletedTask;
    }

    public override async Task AfterModifyingCardPlayCount(CardModel card)
    {
        await PowerCmd.Decrement((PowerModel) this);
    }
}