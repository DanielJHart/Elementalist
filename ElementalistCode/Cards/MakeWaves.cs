using BaseLib.Utils;
using Elementalist.ElementalistCode.Cards;
using Elementalist.ElementalistCode.Character;
using Elementalist.ElementalistCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Elementalist.ElementalistCode.Cards;


[Pool(typeof(ElementalistCardPool))]
public class MakeWaves() : ChangeElementBase
{
    protected override ElementType ElementToChangeTo => ElementType.Water;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WaterPower>()];
}