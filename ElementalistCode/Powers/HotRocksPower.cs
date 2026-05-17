using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Elementalist.ElementalistCode.Powers;

public class HotRocksPower : ElementalistPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power.Owner == this.Owner 
            && power is BurnPower 
            && amount > 0 
            && this.Applier != null 
            && applier == this.Applier)
        {
            CreatureCmd.GainBlock(this.Applier, Amount, ValueProp.Move, null);
        }
        
        return base.AfterPowerAmountChanged(power, amount, applier, cardSource);
    }
}