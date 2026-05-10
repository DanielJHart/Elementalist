using Elementalist.ElementalistCode.Cards;
using Elementalist.ElementalistCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Elementalist.ElementalistCode.Cards;

public class LightEmUp() : ElementalistCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BurnPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target == null || !play.Target.HasPower<BurnPower>())
            return;
        
        BurnPower? burnPower = play.Target.GetPower<BurnPower>();
        if (burnPower == null)
            return;
        
        decimal currentBurn = play.Target.GetPowerAmount<BurnPower>();
        decimal burnToApply = currentBurn * (IsUpgraded ? 2 : 1);
        await PowerCmd.ModifyAmount(burnPower, burnToApply, this.Owner.Creature, this);
    }
}