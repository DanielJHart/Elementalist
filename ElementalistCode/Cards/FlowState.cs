using Elementalist.ElementalistCode.Cards;
using Elementalist.ElementalistCode.Character;
using Elementalist.ElementalistCode.Powers;
using Elementalist.ElementalistCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Elementalist.ElementalistCode.Cards;

public class FlowState() : ElementalistCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override bool ShouldGlowGoldInternal => IsAligned;
    
    private bool IsAligned => ElementalistUtility.IsAligned(this.Owner, ElementType.Water);
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<FlowStatePower>(this.Owner.Creature, 1, this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}