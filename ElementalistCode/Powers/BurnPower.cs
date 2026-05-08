using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Elementalist.ElementalistCode.Powers;

public class BurnPower() : ElementalistPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power == this && amount > 0)
        {
            return PowerCmd.Apply<CharredPower>(this.Owner, 1, applier, cardSource);
        }
        
        return base.AfterPowerAmountChanged(power, amount, applier, cardSource);
    }

    public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == CombatSide.Player)
        {
            await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner, (Decimal) this.Amount, ValueProp.Unpowered, null, null);

            if (!this.Owner.IsAlive)
            {
                await Cmd.CustomScaledWait(0.1f, 0.25f);
            }
            
            await PowerCmd.Remove(this);
        }
        
        await base.BeforeTurnEnd(choiceContext, side);
    }
}