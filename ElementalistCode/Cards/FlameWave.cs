using Elementalist.ElementalistCode.Cards;
using Elementalist.ElementalistCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Elementalist.ElementalistCode.Cards;

public class FlameWave() : ElementalistCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(4M, ValueProp.Move), 
        new BurnVar(5, ValueProp.Move)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (this.Owner.Creature.CombatState != null)
        {
            AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .TargetingAllOpponents(this.Owner.Creature.CombatState)
                .WithHitFx("vfx/vfx_fire_burst")
                .Execute(choiceContext);
                
            foreach (var creature in this.Owner.Creature.CombatState.Enemies)
            {
                await ElementalistUtility.ApplyBurnPower(creature, this.DynamicVars["Burn"].BaseValue, this.Owner.Creature, this);
            }
        }

    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["Burn"].UpgradeValueBy(3);
    }
}