using BaseLib.Abstracts;
using BaseLib.Utils;
using Elementalist.ElementalistCode.Character;

namespace Elementalist.ElementalistCode.Potions;

[Pool(typeof(ElementalistPotionPool))]
public abstract class ElementalistPotion : CustomPotionModel;