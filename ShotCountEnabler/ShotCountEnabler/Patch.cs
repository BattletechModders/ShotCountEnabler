using BattleTech;
using Harmony;
using System;
using System.Collections.Generic;

namespace ShotCountEnabler {

    [HarmonyPatch(typeof(BallisticEffect), "Update")]
    public static class BallisticEffect_Update {
        static Dictionary<int, int> shotcountHolder = new Dictionary<int, int>();

        static void Prefix(ref BallisticEffect __instance) {
            try {
               
                bool allbullets = (bool)ReflectionHelper.InvokePrivateMethode(__instance, "AllBulletsComplete", null);

                if (__instance.currentState == WeaponEffect.WeaponEffectState.WaitingForImpact && allbullets) {

                    int effectId = __instance.GetInstanceID();

                    if(!shotcountHolder.ContainsKey(effectId))
                    {
                        shotcountHolder[effectId] = 1;
                        //Logger.LogLine("effectId: " + effectId + " added");
                    }

                    ReflectionHelper.SetPrivateField(__instance, "hitIndex", shotcountHolder[effectId] - 1);
                    if (shotcountHolder[effectId] >= __instance.hitInfo.numberOfShots) {
                        shotcountHolder[effectId] = 1;             
                        ReflectionHelper.InvokePrivateMethode(__instance, "OnImpact", new object[] { __instance.weapon.DamagePerShot });
                        //Logger.LogLine("effectId: " + effectId + " shotcount reset"); 
                    }
                    else {
                        shotcountHolder[effectId]++;
                        ReflectionHelper.InvokePrivateMethode(__instance, "OnImpact", new object[] { __instance.weapon.DamagePerShot });
                        __instance.Fire(__instance.hitInfo, 0, 0);
                       // Logger.LogLine("effectId: " + effectId + " shotcount incremented to:" + shotcountHolder[effectId]);
                    }
                }
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }
    }

    [HarmonyPatch(typeof(BallisticEffect), "OnComplete")]
    public static class BallisticEffect_OnComplete {

        static void Prefix(ref BallisticEffect __instance,ref float __state) {
            try {
                Weapon weapon = __instance.weapon;
                __state = weapon.DamagePerShot;
                StatCollection collection = weapon.StatCollection;
                collection.Set<float>("DamagePerShot", 0);

            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }

        static void Postfix(ref BallisticEffect __instance, float __state) {
            try {
                Weapon weapon = __instance.weapon;
                StatCollection collection = weapon.StatCollection;
                collection.Set<float>("DamagePerShot", __state);
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }
    }
}