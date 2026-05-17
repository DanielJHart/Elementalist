using Elementalist.ElementalistCode.Cards;
using Elementalist.ElementalistCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Elementalist.ElementalistCode.Cards;

public class Divert() : ElementalistCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyAlly)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        
        await PowerCmd.Apply<DivertedPower>(play.Target, 1M, this.Owner.Creature, this);
        DivertPower? power = await PowerCmd.Apply<DivertPower>(this.Owner.Creature, 1M, this.Owner.Creature, this);
        power?.AddCoveredCreature(play.Target);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}