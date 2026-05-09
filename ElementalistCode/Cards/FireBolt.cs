using BaseLib.Utils;
using Elementalist.ElementalistCode.Cards;
using Elementalist.ElementalistCode.Character;
using Elementalist.ElementalistCode.Powers;
using Elementalist.ElementalistCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Elementalist.ElementalistCode.Cards;

[Pool(typeof(ElementalistCardPool))]
public class FireBolt() : ElementalistCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4M, ValueProp.Move), new BurnVar(5, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BurnPower>(), HoverTipFactory.FromPower<CharredPower>()];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        
        if (play.Target == null)
            return;
        
        AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(play.Target)
            .WithHitFx("vfx/vfx_fire_burning")
            .Execute(choiceContext);
        
        await ElementalistUtility.ApplyBurnPower(play.Target, this.DynamicVars["Burn"].BaseValue, this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["Burn"].UpgradeValueBy(3m);
    }
}