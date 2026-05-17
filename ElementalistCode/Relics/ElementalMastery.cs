using Elementalist.ElementalistCode.Cards;
using Elementalist.ElementalistCode.Character;
using Elementalist.ElementalistCode.Relics;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace Elementalist.ElementalistCode.Relics;

public class ElementalMastery() : ElementalAttunement
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Align>()];

    public override async Task AfterObtained()
    {
        CardModel card = this.Owner.RunState.CreateCard<Align>(this.Owner);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck), 1.2f, CardPreviewStyle.EventLayout);
    }

    public override Task BeforeCombatStart()
    {
        if (this.Owner.Character is Character.Elementalist elementalist)
        {
            elementalist.SetElementCycle(CycleType.Secondary, ElementType.Water);
        }
        
        return base.BeforeCombatStart();
    }

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
    {
        Task result = base.BeforeSideTurnStart(choiceContext, side, combatState);
        
        if (side == this.Owner.Creature.Side)
        {
            if (this.Owner.Character is Character.Elementalist elementalist)
            {
                // Now we've cycled, apply appropriate power.
                ApplyPowerForElement(elementalist.GetCurrentElement(CycleType.Secondary));
            }
        }

        return Task.CompletedTask;
    }
}