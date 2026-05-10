using Elementalist.ElementalistCode.Cards;
using Elementalist.ElementalistCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Elementalist.ElementalistCode.Cards;

public class Aftershock() : ElementalistCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, ValueProp.Move)];

    protected override bool ShouldGlowGoldInternal => this.Aligned;
    
    private bool Aligned 
    {
        get
        {
            if (this.Owner.Character is Character.Elementalist elementalist)
            {
                return elementalist.GetCurrentElement(0) == ElementType.Earth;
            }

            return false;
        }
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;

        Aftershock card = this;
        int hitCount = 1;
        
        // Check alignment
        if (Aligned)
        {
            hitCount = 2;
        }
        
        AttackCommand attackCommand = await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue)
            .WithHitCount(hitCount)
            .FromCard(card)
            .Targeting(play.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}