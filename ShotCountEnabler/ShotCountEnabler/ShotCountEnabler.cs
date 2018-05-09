using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShotCountEnabler
{
    public class ShotCountEnabler
    {
        public static void Init() {
            var harmony = HarmonyInstance.Create("de.morphyum.ShotCountEnabler");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

    }
}
