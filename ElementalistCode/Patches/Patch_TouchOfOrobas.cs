using Elementalist.ElementalistCode.Relics;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace Elementalist.ElementalistCode.Patches;

// This solution taken from https://github.com/YuWan886/Sts2-YuWanCard/blob/59293432afe58c52678f36bb57297298eff80872/YuWanCardCode/Patches/TouchOfOrobasPatch.cs#L9

[HarmonyPatch(typeof(TouchOfOrobas), nameof(TouchOfOrobas.AfterObtained))]
public static class Patch_TouchOfOrobas
{
    [HarmonyPostfix]
    [HarmonyPatch("RefinementUpgrades", MethodType.Getter)]
    public static void AddElementalAttunementUpgrade(ref Dictionary<ModelId, RelicModel> __result)
    {
        var relic = ModelDb.Relic<ElementalAttunement>();
        var upgradedRelic = ModelDb.Relic<ElementalMastery>();
        
        if (!__result.ContainsKey(relic.Id))
        {
            __result[relic.Id] = upgradedRelic;
        }
    }
}