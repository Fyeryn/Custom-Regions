﻿using CustomRegions.Mod;
using System.Collections.Generic;

namespace CustomRegions.Creatures
{
    static class DaddyLongLegsHook
    {
        public static void ApplyHooks()
        {
            On.DaddyLongLegs.ctor += DaddyLongLegs_ctor;
        }

        /// <summary>
        /// Checks if the region has colored BLLs/DLLs configured
        /// </summary>
        private static void DaddyLongLegs_ctor(On.DaddyLongLegs.orig_ctor orig, DaddyLongLegs self, AbstractCreature abstractCreature, World world)
        {
            orig(self, abstractCreature, world);
            if (world != null && !world.singleRoomWorld)
            {
                //CustomWorldMod.Log($"Region Name [{self.region.name}]");
                foreach (KeyValuePair<string, string> keyValues in CustomWorldMod.activatedPacks)
                {
                    //CustomWorldMod.Log($"Checking in [{CustomWorldMod.availableRegions[keyValues.Key].regionName}]");
                    if (CustomWorldMod.installedPacks[keyValues.Key].regionConfig.TryGetValue(world.region.name, out CustomWorldStructs.RegionConfiguration config))
                    {
                        if (!config.bllVanilla)
                        {
                            //CustomWorldMod.Log($"Spawning custom DDL/BLL in [{world.region.name}] from [{CustomWorldMod.availableRegions[keyValues.Key].regionName}]");
                            self.colorClass = true;
                            self.effectColor = config.bllColor ?? new UnityEngine.Color(0, 0, 1);
                            self.eyeColor = self.effectColor;
                        }
                        break;
                    }
                }
            }
        }
    }
}
