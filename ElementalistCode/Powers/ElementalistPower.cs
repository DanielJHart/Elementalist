using BaseLib.Abstracts;
using BaseLib.Extensions;
using Elementalist.ElementalistCode.Extensions;
using Godot;

namespace Elementalist.ElementalistCode.Powers;

public abstract class ElementalistPower : CustomPowerModel
{
    //Loads from Elementalist/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}