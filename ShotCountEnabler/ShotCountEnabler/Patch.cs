using BattleTech;
using Harmony;
using System;

namespace ShotCountEnabler {

    [HarmonyPatch(typeof(BallisticEffect), "Update")]
    public static class BallisticEffect_Update {
        static int shotcount = 1;

        static void Prefix(ref BallisticEffect __instance) {
            try {
               
                bool allbullets = (bool)ReflectionHelper.InvokePrivateMethode(__instance, "AllBulletsComplete", null);

                if (__instance.currentState == WeaponEffect.WeaponEffectState.WaitingForImpact && allbullets) {

                    ReflectionHelper.SetPrivateField(__instance, "hitIndex", shotcount - 1);
                    if (shotcount == __instance.hitInfo.numberOfShots) {
                        shotcount = 1;                  
                        ReflectionHelper.InvokePrivateMethode(__instance, "OnImpact", new object[] { __instance.weapon.DamagePerShot });
                    }
                    else {
                        shotcount++;
                        ReflectionHelper.InvokePrivateMethode(__instance, "OnImpact", new object[] { __instance.weapon.DamagePerShot });
                        __instance.Fire(__instance.hitInfo, 0, 0);
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