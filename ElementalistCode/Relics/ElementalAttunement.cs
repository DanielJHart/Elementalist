using BaseLib.Utils;
using Elementalist.ElementalistCode.Character;
using Elementalist.ElementalistCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace Elementalist.ElementalistCode.Relics;

[Pool(typeof(ElementalistRelicPool))]
public class ElementalAttunement : ElementalistRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("NextElement", "Earth")];

    private bool _isCyclingForwards = true;

    private Character.Elementalist? GetElementalist()
    {
        return this.Owner.Character as Character.Elementalist;
    }

    public override Task BeforeCombatStart()
    {
        Character.Elementalist?  elementalist = GetElementalist();
        
        if (elementalist != null)
        {
            elementalist.AddElementCycle(ElementType.Earth);
        }
        
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        Character.Elementalist?  elementalist = GetElementalist();
        
        if (elementalist != null)
        {
            elementalist.RemoveElementCycle(0);
        }
        
        return base.AfterCombatEnd(room);
    }

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
    {
        if (side == this.Owner.Creature.Side)
        {
            Character.Elementalist?  elementalist = GetElementalist();

            if (elementalist != null)
            {
                elementalist.CycleElements(_isCyclingForwards);
                
                // Now we've cycled, apply appropriate power.
                ApplyPowerForElement(elementalist.GetCurrentElement(0));
            }
        }
        
        return Task.CompletedTask;
    }

    private void ApplyPowerForElement(ElementType element)
    {
        switch (element)
        {
            case ElementType.Earth:
                PowerCmd.Apply<EarthPower>(this.Owner.Creature, 2m, this.Owner.Creature, null);
                break;
            case ElementType.Fire:
                PowerCmd.Apply<FirePower>(this.Owner.Creature, 2m, this.Owner.Creature, null);
                break;
            case ElementType.Water:
                PowerCmd.Apply<WaterPower>(this.Owner.Creature, 1m, this.Owner.Creature, null);
                break;
            case ElementType.Air:
                PowerCmd.Apply<AirPower>(this.Owner.Creature, 1m, this.Owner.Creature, null);
                break;
        }
    }

    private void RemovePowerForElement(ElementType element)
    {
        switch (element)
        {
            case ElementType.Earth:
                PowerCmd.Remove<EarthPower>(this.Owner.Creature);
                break;
            case ElementType.Fire:
                PowerCmd.Remove<FirePower>(this.Owner.Creature);
                break;
            case ElementType.Water:
                PowerCmd.Remove<WaterPower>(this.Owner.Creature);
                break;
            case ElementType.Air:
                PowerCmd.Remove<AirPower>(this.Owner.Creature);
                break;
        }
    }

    public void SetElement(ElementType element)
    {
        Character.Elementalist?  elementalist = GetElementalist();

        if (elementalist != null)
        {
            //RemovePowerForElement(elementalist.GetCurrentElement(0));
            ApplyPowerForElement(element);
            elementalist.SetElementCycle(0, element);
        }
    }

    public void ReverseCycleDirection()
    {
        _isCyclingForwards = !_isCyclingForwards;
    }
}