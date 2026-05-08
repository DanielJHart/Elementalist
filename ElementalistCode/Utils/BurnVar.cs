using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Elementalist.ElementalistCode.Utils;

public class BurnVar : DynamicVar
{
    public const string defaultName = "Burn";

    public ValueProp Props { get; set; }

    public BurnVar(Decimal damage, ValueProp props)
        : base(defaultName, damage)
    {
        this.Props = props;
    }

    public BurnVar(string name, Decimal damage, ValueProp props)
        : base(name, damage)
    {
        this.Props = props;
    }

    public override void UpdateCardPreview(
        CardModel card,
        CardPreviewMode previewMode,
        Creature? target,
        bool runGlobalHooks)
    {
        Decimal originalDamage1 = this.BaseValue;

        if (runGlobalHooks && card.CombatState != null && target != null)
        { 
            originalDamage1 = ElementalistHooks.ModifyBurnDamage(card.CombatState, target, this.BaseValue, out IEnumerable<AbstractModel> _);
        }
        
        this.PreviewValue = originalDamage1;
    }
}