﻿using CustomRegions.Mod;
using Menu;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomRegions.CustomMenu
{
    static class SlugcatSelectMenuHook
    {
        public static void ApplyHooks() 
        {
            On.Menu.SlugcatSelectMenu.SlugcatPageContinue.ctor += SlugcatPageContinue_ctor;
            On.Menu.SlugcatSelectMenu.ctor += SlugcatSelectMenu_ctor;
        }


        private static void SlugcatSelectMenu_ctor(On.Menu.SlugcatSelectMenu.orig_ctor orig, SlugcatSelectMenu self, ProcessManager manager)
        {
            orig(self, manager);

            int saveSlot = self.manager.rainWorld.options.saveSlot;
            if (CustomWorldMod.saveProblems[saveSlot].AnyProblems)
            {
                bool allNewGame = true;
                string errorText = CustomWorldMod.Translate("Problems found in your save, please check the tab SaveAnalyzer in the config screen for more information.");
                for (int m = 0; m < self.slugcatPages.Length; m++)
                { 
                    if (self.saveGameData[m] != null)
                    {
                        allNewGame = false;
                        break;
                    }
                }
                if(allNewGame)
                {
                    errorText = CustomWorldMod.Translate("Problems found in your save, please use the Reset Progress button in the RW options menu");
                }

                MenuLabel menuLabel = new MenuLabel(self, self.pages[0],
                errorText,
                new Vector2(self.manager.rainWorld.options.ScreenSize.x * 0.5f, self.manager.rainWorld.options.ScreenSize.y * 0.85f),
                new Vector2(0, 0), false);

                menuLabel.label.color = new Color((108f / 255f), 0.001f, 0.001f);
                menuLabel.label.alignment = FLabelAlignment.Center;

                self.pages[0].subObjects.Add(menuLabel);
            }
        }

        /// <summary>
        /// Retrieves the region name to show it in the slugcat select menu
        /// </summary>
        private static void SlugcatPageContinue_ctor(On.Menu.SlugcatSelectMenu.SlugcatPageContinue.orig_ctor orig, Menu.SlugcatSelectMenu.SlugcatPageContinue self, Menu.Menu menu, Menu.MenuObject owner, int pageIndex, int slugcatNumber)
        {
            orig(self, menu, owner, pageIndex, slugcatNumber);

            if (self.saveGameData.shelterName != null && self.saveGameData.shelterName.Length > 2)
            {
                string regID = self.saveGameData.shelterName.Substring(0, 2);

                bool customRegion = true;
                List<string> vanillaRegions = CustomWorldMod.VanillaRegions().ToList();
                for (int i = 0; i < vanillaRegions.Count; i++)
                {
                    if (regID == vanillaRegions[i])
                    {
                        customRegion = false;
                    }
                }
                if (customRegion)
                {
                    foreach (MenuObject label in self.subObjects)
                    {
                        if (label is MenuLabel && label == self.regionLabel && (label as MenuLabel).text.Length < 3)
                        {
                            string fullRegionName = "N / A";
                            //CustomWorldMod.activatedPacks.TryGetValue(text2, out fullRegionName);
                            if (CustomWorldMod.activeModdedRegions.Contains(regID))
                            {
                                foreach(KeyValuePair<string, string> entry in CustomWorldMod.activatedPacks)
                                {
                                    if (CustomWorldMod.installedPacks[entry.Key].regions.Contains(regID))
                                    {
                                        fullRegionName = entry.Value;
                                        CustomWorldMod.Log($"Displaying region name: [{fullRegionName}]. If you pack contains multiple regions, contact @Garrakx.");
                                        break;
                                    }
                                }
                            }
                                if (fullRegionName != null)
                                {
                                    if (fullRegionName.Length > 0)
                                    {
                                        regID = fullRegionName;

                                        fullRegionName = string.Concat(new object[]
                                        {
                                regID,
                                " - ",
                                menu.Translate("Cycle"),
                                " ",
                                (slugcatNumber != 2) ? self.saveGameData.cycle : (RedsIllness.RedsCycles(self.saveGameData.redsExtraCycles) - self.saveGameData.cycle)
                                        });
                                    }
                                    (label as MenuLabel).text = fullRegionName;
                                    break;
                                }
                        }
                    }
                }
            }
        }
    }
}
