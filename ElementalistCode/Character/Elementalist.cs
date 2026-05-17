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
    Air,
    None
}

public enum CycleType
{
    Primary,
    Secondary
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
    public override string CustomCharacterSelectBg => "char_select_bg_elementalist.tscn".ScenesPath();
    public override string CustomVisualPath => "elementalist.tscn".ScenesPath();

    // ELEMENTALIST LOGIC STARTS

    //private List<ElementType> elementCycles = new List<ElementType>();
    
    private Dictionary<CycleType, ElementType> _elementCycles = new  Dictionary<CycleType, ElementType>();

    public Elementalist()
    {
        AddElementCycle(CycleType.Primary, ElementType.Earth);
        AddElementCycle(CycleType.Secondary, ElementType.Water);
    }

    public void AddElementCycle(CycleType cycle, ElementType element)
    {
        _elementCycles.Add(cycle, element);
    }

    public void RemoveElementCycle(CycleType cycle)
    {
        _elementCycles.Remove(cycle);
    }

    public void SetElementCycle(CycleType cycle, ElementType element)
    {
        if (_elementCycles.ContainsKey(cycle))
        {
            _elementCycles[cycle] = element;
            
            // TODO: update UI.
        }
    }
    
    public void CycleElements(bool isForwards)
    {
        foreach (var elementCycle in _elementCycles)
        {
            SetElementCycle(elementCycle.Key, GetNextElement(elementCycle.Value, isForwards));
        }
    }

    public ElementType GetCurrentElement(CycleType cycle)
    {
        return _elementCycles.GetValueOrDefault(cycle, ElementType.None);
    }

    private static ElementType GetNextElement(ElementType currentElement, bool isForwards)
    {
        if (isForwards)
        {
            if (currentElement == ElementType.Air)
            {
                return ElementType.Earth;
            }
            else
            {
                return ++currentElement;
            }
        }
        else
        {
            if (currentElement == ElementType.Earth)
            {
                return ElementType.Air;
            }
            else
            {
                return --currentElement;
            }
        }
    }
}