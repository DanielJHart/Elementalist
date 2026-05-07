using BaseLib.Abstracts;
using Elementalist.ElementalistCode.Extensions;
using Godot;

namespace Elementalist.ElementalistCode.Character;

public class ElementalistRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Elementalist.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}