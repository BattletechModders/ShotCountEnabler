using Harmony;
using System.Reflection;


namespace ShotCountEnabler
{
    public class ShotCountEnabler
    {
        internal static string ModDirectory;
        public static void Init(string modDirectory, string settingsJSON) {
            var harmony = HarmonyInstance.Create("de.morphyum.ShotCountEnabler");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            ModDirectory = modDirectory;
        }

    }
}
