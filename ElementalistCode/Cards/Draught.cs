using Elementalist.ElementalistCode.Cards;
using Elementalist.ElementalistCode.Character;
using Elementalist.ElementalistCode.Powers;
using Elementalist.ElementalistCode.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Elementalist.ElementalistCode.Cards;

public class Draught() : ElementalistCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("DrawCount", 2), new IntVar("AlignedDrawCount", 4)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<AirPower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int drawCount = ElementalistUtility.IsAligned(this.Owner, ElementType.Air) ? 
            (int)this.DynamicVars["AlignedDrawCount"].BaseValue : 
            (int)this.DynamicVars["DrawCount"].BaseValue;
        
        await CardPileCmd.Draw(choiceContext, drawCount, this.Owner);
    }

    protected override void OnUpgrade()
    {
        this.AddKeyword(CardKeyword.Retain);
    }
}