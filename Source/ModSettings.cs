using Verse;
using RimWorld;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace HemogenMedicineBloodloss
{
    public class HemogenMedicineBloodlossSettings : ModSettings
    {
        /// <summary>
        /// Settings for our mod
        /// </summary>
        public bool shouldMedicineHealBloodloss = true;
        public float bloodlossHealAmount = 0.35f;


        /// <summary>
        /// Writes our settings to file. Saving is by ref
        /// </summary>
        public override void ExposeData()
        {
            Scribe_Values.Look(ref shouldMedicineHealBloodloss, "shouldMedicineHealBloodloss", true);
            Scribe_Values.Look(ref bloodlossHealAmount, "bloodlossHealAmount", 0.8f);
            base.ExposeData();
        }
    }

    public class HemogenMedicineBloodlossConfig : Mod       
    {
        public HemogenMedicineBloodlossSettings settings;

        public HemogenMedicineBloodlossConfig(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<HemogenMedicineBloodlossSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("Should medicine heal bloodloss?", ref settings.shouldMedicineHealBloodloss);
            listingStandard.Label("Bloodloss heal amount (vanilla hemogen heals 0.35f):");
            settings.bloodlossHealAmount = listingStandard.Slider(settings.bloodlossHealAmount, 0.1f, 1f);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Hemogen Medicine Cures Bloodloss";
        }

    }
}