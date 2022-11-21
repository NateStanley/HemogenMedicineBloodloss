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
            Scribe_Values.Look(ref bloodlossHealAmount, "bloodlossHealAmount");
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
            string bloodLossHealAmountString = settings.bloodlossHealAmount.ToString();

            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("Should hemogen medicine heal bloodloss?", ref settings.shouldMedicineHealBloodloss);
            listingStandard.Label("\t--Only affects the treatment of bloodloss when tending other wounds with hemogen medicine.\n\t--No matter what, you will still be able to perform blood transfusions with hemogen medicine.\n\n");
           
            settings.bloodlossHealAmount = listingStandard.SliderLabeled("Bloodloss heal amount: " + bloodLossHealAmountString + "f", settings.bloodlossHealAmount, 0f, 1f);
            listingStandard.Label("\t--The default vanilla Hemogen Packs heal value is 0.35f");

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Hemogen Meds Cure Bloodloss";
        }

    }
}