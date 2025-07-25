using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;

namespace HemogenMedicineBloodloss
{
    public class Recipe_BloodTransfusionHemogenMed : Recipe_Surgery
    {
		public override bool CompletableEver(Pawn surgeryTarget)
		{
			return surgeryTarget.health.hediffSet.HasHediff(HediffDefOf.BloodLoss, false) && base.CompletableEver(surgeryTarget);
		}

		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			Pawn pawn;
            return thing.MapHeld != null && thing.MapHeld.listerThings.ThingsOfDef(HemogenMedicineThingDefOf.MedicineHemogen).Count != 0 && ((pawn = (thing as Pawn)) == null ||
                pawn.health.hediffSet.HasHediff(HediffDefOf.BloodLoss, false)) && base.AvailableOnNow(thing, part);
        }

		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < ingredients.Count; i++)
			{
                num += LoadedModManager.GetMod<HemogenMedicineBloodlossConfig>().settings.bloodlossHealAmount * (float)ingredients[i].stackCount;
                num2 += LoadedModManager.GetMod<HemogenMedicineBloodlossConfig>().settings.bloodlossHealAmount * (float)ingredients[i].stackCount;
			}
			if (num > 0f)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
				if (firstHediffOfDef != null)
				{
					firstHediffOfDef.Severity -= num;
				}
			}
			if (num2 > 0f)
			{
				Pawn_GeneTracker genes = pawn.genes;
				if (((genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null) != null)
				{
					GeneUtility.OffsetHemogen(pawn, num2, true);
				}
			}
			for (int j = 0; j < ingredients.Count; j++)
			{
				ingredients[j].Destroy(DestroyMode.Vanish);
			}
		}

		public override float GetIngredientCount(IngredientCount ing, Bill bill)
		{
			BillStack billStack = bill.billStack;
			Pawn pawn = ((billStack != null) ? billStack.billGiver : null) as Pawn;
			if (pawn == null)
			{
				return base.GetIngredientCount(ing, bill);
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			if (firstHediffOfDef == null)
			{
				return base.GetIngredientCount(ing, bill);
			}
            
            //Log.Message("Ingredient count for hemogen blood transfusion: " + Mathf.Min((float)bill.Map.listerThings.ThingsOfDef(HemogenMedicineThingDefOf.MedicineHemogen).Sum((Thing x) => x.stackCount), firstHediffOfDef.Severity / LoadedModManager.GetMod<HemogenMedicineBloodlossConfig>().settings.bloodlossHealAmount));
            return Mathf.Min((float)bill.Map.listerThings.ThingsOfDef(HemogenMedicineThingDefOf.MedicineHemogen).Sum((Thing x) => x.stackCount), firstHediffOfDef.Severity / LoadedModManager.GetMod<HemogenMedicineBloodlossConfig>().settings.bloodlossHealAmount);

        }
    }
}