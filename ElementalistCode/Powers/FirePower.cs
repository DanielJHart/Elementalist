using BaseLib.Abstracts;
using Elementalist.ElementalistCode.Relics;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Elementalist.ElementalistCode.Powers;

public class FirePower : TemporaryStrengthPower, ICustomModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.None;
    public override AbstractModel OriginModel => (AbstractModel) ModelDb.Relic<ElementalAttunement>();
}