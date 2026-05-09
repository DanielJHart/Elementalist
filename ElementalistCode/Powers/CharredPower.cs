using Elementalist.ElementalistCode.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Elementalist.ElementalistCode.Powers;

public class CharredPower() : ElementalistPower, IElementalistModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public decimal ModifyBurnDamage(Creature target, decimal amount)
    {
        if (target == this.Owner && this.Amount > 0)
        {
            return amount + this.Amount;
        }

        return amount;
    }

    public override Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        PowerCmd.Remove(this);
        return Task.CompletedTask;
    }
}