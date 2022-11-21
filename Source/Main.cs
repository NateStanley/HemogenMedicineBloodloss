using Verse;
using RimWorld;
using HarmonyLib;

namespace HemogenMedicineBloodloss
{


    [StaticConstructorOnStartup]
    public static class HemogenMedicineBloodloss
    {
        static HemogenMedicineBloodloss()
        {
            //Log.Message("HemogenMedicineBloodloss loaded successfully!");

            Harmony harmony = new Harmony("rimworld.mod.dogwithafro.hemogenmedicinebloodloss");
            harmony.Patch(AccessTools.Method(typeof(TendUtility), nameof(TendUtility.DoTend)), 
                postfix: new HarmonyMethod(typeof(HemogenMedicineBloodloss), nameof(DoTend_PostFix)));
        }

        private static void DoTend_PostFix(Pawn doctor, Pawn patient, Medicine medicine)
        {
            //check if medicine is even present lol
            if(medicine != null)
            {
                //check if medicine has the mod extension "CuresBloodloss" as true
                if (medicine.def.HasModExtension<CuresBloodlossModExtension>())
                {
                    CuresBloodlossModExtension extension = medicine.def.GetModExtension<CuresBloodlossModExtension>();
                    if (extension.CuresBloodloss)
                    {
                        //check if patient has hediff BloodLoss
                        Hediff bloodloss = patient.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss);
                        if (bloodloss != null)
                        {
                            //check if our mod settings allow medicine to heal bloodloss
                            if (LoadedModManager.GetMod<HemogenMedicineBloodlossConfig>().settings.shouldMedicineHealBloodloss)
                            {
                                //lower severity of hediff BloodLoss
                                bloodloss.Severity -= LoadedModManager.GetMod<HemogenMedicineBloodlossConfig>().settings.bloodlossHealAmount;
                            }
                        }
                    }
                }
            }
        }
    }
}


