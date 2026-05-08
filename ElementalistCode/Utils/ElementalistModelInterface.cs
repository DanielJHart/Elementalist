using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Elementalist.ElementalistCode.Utils;

public interface IElementalistModel
{
    public virtual decimal ModifyBurnDamage(Creature target, decimal amount)
    {
        return amount;
    }
}