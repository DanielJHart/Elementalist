using Elementalist.ElementalistCode.Character;
using Elementalist.ElementalistCode.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Elementalist.ElementalistCode.Powers;

public class DivertPower : ElementalistPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    
    private const string _divertingKey = "Diverting";

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(_divertingKey)];

    protected override object InitInternalData() => (object) new DivertPower.Data();

    public void AddCoveredCreature(Creature c)
    {
        List<Creature> coveredCreatures = this.GetInternalData<DivertPower.Data>().coveredCreatures;
        if (!this.GetInternalData<DivertPower.Data>().coveredCreatures.Contains(c))
            coveredCreatures.Add(c);
        StringVar dynamicVar = (StringVar) this.DynamicVars[_divertingKey];
        dynamicVar.StringValue = "";
        for (int index = 0; index < coveredCreatures.Count; ++index)
        {
            var player = coveredCreatures[index].Player;
            if (player != null)
                dynamicVar.StringValue += PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, player.NetId);
            if (index == coveredCreatures.Count - 2)
                dynamicVar.StringValue += ", and ";
            else if (index < coveredCreatures.Count - 2)
                dynamicVar.StringValue += ", ";
        }
    }

    public override Decimal ModifyDamageMultiplicative(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target != this.Owner || !props.IsPoweredAttack() || this.Owner.Player == null)
        {
            return 1;
        }

        decimal multiplier = 1;
            
        if (ElementalistUtility.IsAligned(this.Owner.Player, ElementType.Earth))
        {
            multiplier = 0.5m;
        }

        return (this.GetInternalData<DivertPower.Data>().coveredCreatures.Count + 1) * multiplier;
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        DivertPower power = this;
        if (side != CombatSide.Enemy)
            return;
        await PowerCmd.Remove((PowerModel) power);
    }

    private class Data
    {
        public readonly List<Creature> coveredCreatures = new List<Creature>();
    }
}