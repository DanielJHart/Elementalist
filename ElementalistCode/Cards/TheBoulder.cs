using Elementalist.ElementalistCode.Cards;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Elementalist.ElementalistCode.Cards;

public class TheBoulder() : ElementalistCard(3,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(25m, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        
        //await CreatureCmd.Damage(choiceContext, play.Target, this.DynamicVars.Damage, this);
        
        List<Task> damageTasks = new List<Task>();
        NRollingBoulderVfx? vfx = NRollingBoulderVfx.Create([play.Target], this.DynamicVars.Damage.BaseValue);
        // ISSUE: object of a compiler-generated type is created
        if (vfx == null)
            return;
        
        long num = (long) vfx.Connect(NRollingBoulderVfx.SignalName.HitCreature, Callable.From<NCreature>((Action<NCreature>) (c => damageTasks.Add((Task) this.DoDamage(choiceContext, [c.Entity])))));
        Callable.From((Action) (() =>
        {
            NCombatRoom? instance = NCombatRoom.Instance;
            if (instance != null)
                instance.CombatVfxContainer.AddChildSafely((Godot.Node) vfx);
            if (!vfx.IsInsideTree())
                throw new InvalidOperationException("VFX is not inside tree after adding it to combat room!");
        })).CallDeferred();
        Variant[] signal = await vfx.ToSignal((GodotObject) vfx, Godot.Node.SignalName.TreeExiting);
        await Task.WhenAll((IEnumerable<Task>) damageTasks);
    }
    
    private Task<IEnumerable<DamageResult>> DoDamage(
        PlayerChoiceContext choiceContext,
        IEnumerable<Creature> targets)
    {
        return CreatureCmd.Damage(choiceContext, targets, this.DynamicVars.Damage.BaseValue, ValueProp.Move, this.Owner.Creature);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(7m);
    }
}