using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace Elementalist.ElementalistCode.Utils;

public static class ElementalistHooks
{
    public static decimal ModifyBurnDamage(CombatState combatState, Creature target, decimal originalAmount,
        out IEnumerable<AbstractModel> modifiers)
    {
        decimal num = originalAmount;
        List<AbstractModel> list = new List<AbstractModel>();
        foreach (AbstractModel item in combatState.IterateHookListeners())
        {
            if (item is IElementalistModel eleItem)
            {
                decimal num2 = num;
                num = eleItem.ModifyBurnDamage(target, num);
                if ((int)num2 != (int)num)
                {
                    list.Add(item);
                }   
            }
        }
        
        modifiers = list;
        return num;
    }
}