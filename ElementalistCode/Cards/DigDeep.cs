using Elementalist.ElementalistCode.Cards;
using Elementalist.ElementalistCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Elementalist.ElementalistCode.Cards;

public class DigDeep() : ElementalistCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DexterityPower>(2M)];

    protected override async Task OnPlay( PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<DigDeepPower>(this.Owner.Creature, this.DynamicVars.Dexterity.BaseValue, this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Dexterity.UpgradeValueBy(1);
    }
}