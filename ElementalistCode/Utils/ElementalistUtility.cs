using Elementalist.ElementalistCode.Character;
using Elementalist.ElementalistCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace Elementalist.ElementalistCode.Utils;

public static class ElementalistUtility
{
    public static async Task ApplyBurnPower(Creature target, decimal amount, Creature? applier, CardModel? source)
    {
        decimal finalAmount = amount;
        
        if (target.CombatState != null)
        {
            finalAmount = ElementalistHooks.ModifyBurnDamage(target.CombatState, target, amount, out _);
        }

        await PowerCmd.Apply<BurnPower>(target, finalAmount, applier, source);
        await Task.CompletedTask;
    }

    public static bool IsAligned(Player player, ElementType element)
    {
        if (player.Character is Character.Elementalist elementalist)
        {
            return elementalist.GetCurrentElement(0) == element || elementalist.GetCurrentElement(1) == element;
        }

        return false;
    }
}