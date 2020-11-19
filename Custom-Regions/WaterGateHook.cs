﻿using CustomRegions.Mod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomRegions
{
    static class WaterGateHook
    {
        public static void ApplyHooks()
        {
            //On.WaterGate.ctor += WaterGate_ctor;
        }

        private static void WaterGate_ctor(On.WaterGate.orig_ctor orig, WaterGate self, Room room)
        {
            /*
            foreach (KeyValuePair<string, CustomWorldMod.RegionInformation> entries in CustomWorldMod.availableRegions)
            {
                if (!entries.Value.activated || entries.Value.electricGates == null || entries.Value.electricGates.Count == 0)
                {
                    continue;
                }

                if (entries.Value.electricGates.ContainsKey(room.abstractRoom.name))
                {
                    CustomWorldMod.Log($"This gate [{room.abstractRoom.name}] should be electric gate, returning...");
                    return;
                }
            }
            */
            CustomWorldMod.Log("Water gate created, checking if it should be electric...");
            foreach(KeyValuePair<string, string> regions in CustomWorldMod.loadedRegions)
            {
                if (CustomWorldMod.availableRegions[regions.Key].electricGates != null)
                {
                    if(CustomWorldMod.availableRegions[regions.Key].electricGates.ContainsKey(room.abstractRoom.name))
                    {
                        CustomWorldMod.Log($"This gate [{room.abstractRoom.name}] should be electric gate, returning...");
                        return;
                    }
                }
            }

            orig(self, room);
        }

        private static void ElectricGate_ctor(On.ElectricGate.orig_ctor orig, ElectricGate self, Room room)
        {
            foreach (KeyValuePair<string, CustomWorldMod.RegionInformation> entries in CustomWorldMod.availableRegions)
            {
                if (!entries.Value.activated || entries.Value.electricGates == null || entries.Value.electricGates.Count == 0)
                {
                    continue;
                }

                if (entries.Value.electricGates.ContainsKey(self.room.abstractRoom.name))
                {
                    return;
                }
            }

            orig(self, room);
        }
    }
}