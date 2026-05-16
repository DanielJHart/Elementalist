using Elementalist.ElementalistCode.Cards;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Elementalist.ElementalistCode.Cards;

public class Cleanse() : ElementalistCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FrailPower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Remove<FrailPower>(this.Owner.Creature);

        if (IsUpgraded)
        {
            await PowerCmd.Remove<WeakPower>(this.Owner.Creature);
        }
    }

    protected override void OnUpgrade()
    {
        ExtraHoverTips.AddItem(HoverTipFactory.FromPower<WeakPower>());
    }
}