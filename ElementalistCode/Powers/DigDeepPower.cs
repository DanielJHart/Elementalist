using BaseLib.Abstracts;
using Elementalist.ElementalistCode.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Elementalist.ElementalistCode.Powers;

public class DigDeepPower : TemporaryDexterityPower, ICustomPower
{
    public override AbstractModel OriginModel => (AbstractModel) ModelDb.Card<DigDeep>();
}