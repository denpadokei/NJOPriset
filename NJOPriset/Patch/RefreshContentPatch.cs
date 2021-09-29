using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJOPriset.Patch
{
    [HarmonyPatch(typeof(StandardLevelDetailView), nameof(StandardLevelDetailView.RefreshContent))]
    public class RefreshContentPatch
    {
        public static event Action<StandardLevelDetailView> OnRefresh;
        public static void Postfix(StandardLevelDetailView __instance)
        {
            OnRefresh?.Invoke(__instance);
        }
    }
}
