using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Elementalist.ElementalistCode.Cards;
using Elementalist.ElementalistCode.Extensions;
using Elementalist.ElementalistCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

namespace Elementalist.ElementalistCode.Character;

public enum ElementType
{
    Earth,
    Fire,
    Water,
    Air
}

public class Elementalist : PlaceholderCharacterModel
{
    public const string CharacterId = "Elementalist";

    public static readonly Color Color = new("ffffff");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 70;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeElementalist>(),
        ModelDb.Card<StrikeElementalist>(),
        ModelDb.Card<StrikeElementalist>(),
        ModelDb.Card<StrikeElementalist>(),
        ModelDb.Card<DefendElementalist>(),
        ModelDb.Card<DefendElementalist>(),
        ModelDb.Card<DefendElementalist>(),
        ModelDb.Card<DefendElementalist>(),
        ModelDb.Card<FireBolt>(),
        ModelDb.Card<Gust>(),
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<ElementalAttunement>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<ElementalistCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<ElementalistRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<ElementalistPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();

    // ELEMENTALIST LOGIC STARTS

    private List<ElementType> elementCycles = new List<ElementType>();

    public void AddElementCycle(ElementType element)
    {
        elementCycles.Add(element);
    }

    public void RemoveElementCycle(int cycle)
    {
        elementCycles.RemoveAt(cycle);
    }

    public void SetElementCycle(int cycle, ElementType element)
    {
        if (cycle <= elementCycles.Count)
        {
            elementCycles[cycle] = element;
            
            // TODO: update UI.
        }
    }
    
    public void CycleElements(bool isForwards)
    {
        for (int i = 0; i < elementCycles.Count; i++)
        {
            elementCycles[i] = GetNextElement(elementCycles[i], isForwards);
        }
    }

    public ElementType GetCurrentElement(int cycle)
    {
        if (cycle <= elementCycles.Count)
        {
            return elementCycles[cycle];
        }
        
        return ElementType.Earth;
    }

    private ElementType GetNextElement(ElementType currentElement, bool isForwards)
    {
        if (isForwards)
        {
            int current = (int)currentElement;
            ++current;
            if (current > (int)ElementType.Air)
            {
                // Cycle back around.
                current = 0;
            }
            
            return (ElementType)current;
        }
        else
        {
            int current = (int)currentElement;
            --current;
            if (current < 0)
            {
                // Cycle back around.
                current = (int)ElementType.Air;
            }
            
            return (ElementType)current;
        }
    }
}