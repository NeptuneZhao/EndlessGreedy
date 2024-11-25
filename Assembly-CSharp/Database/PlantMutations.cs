using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;

namespace Database
{
	// Token: 0x02000E75 RID: 3701
	public class PlantMutations : ResourceSet<PlantMutation>
	{
		// Token: 0x060074C8 RID: 29896 RVA: 0x002D88E0 File Offset: 0x002D6AE0
		public PlantMutation AddPlantMutation(string id)
		{
			StringEntry entry = Strings.Get(new StringKey("STRINGS.CREATURES.PLANT_MUTATIONS." + id.ToUpper() + ".NAME"));
			StringEntry entry2 = Strings.Get(new StringKey("STRINGS.CREATURES.PLANT_MUTATIONS." + id.ToUpper() + ".DESCRIPTION"));
			PlantMutation plantMutation = new PlantMutation(id, entry, entry2);
			base.Add(plantMutation);
			return plantMutation;
		}

		// Token: 0x060074C9 RID: 29897 RVA: 0x002D894C File Offset: 0x002D6B4C
		public PlantMutations(ResourceSet parent) : base("PlantMutations", parent)
		{
			this.moderatelyLoose = this.AddPlantMutation("moderatelyLoose").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.WiltTempRangeMod, 0.5f, true).AttributeModifier(Db.Get().PlantAttributes.YieldAmount, -0.25f, true).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, -0.5f, true).VisualTint(-0.4f, -0.4f, -0.4f);
			this.moderatelyTight = this.AddPlantMutation("moderatelyTight").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.WiltTempRangeMod, -0.5f, true).AttributeModifier(Db.Get().PlantAttributes.YieldAmount, 0.5f, true).VisualTint(0.2f, 0.2f, 0.2f);
			this.extremelyTight = this.AddPlantMutation("extremelyTight").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.WiltTempRangeMod, -0.8f, true).AttributeModifier(Db.Get().PlantAttributes.YieldAmount, 1f, true).VisualTint(0.3f, 0.3f, 0.3f).VisualBGFX("mutate_glow_fx_kanim");
			this.bonusLice = this.AddPlantMutation("bonusLice").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.25f, true).BonusCrop("BasicPlantFood", 1f).VisualSymbolOverride("snapTo_mutate1", "mutate_snaps_kanim", "meal_lice_mutate1").VisualSymbolOverride("snapTo_mutate2", "mutate_snaps_kanim", "meal_lice_mutate2").AddSoundEvent(GlobalAssets.GetSound("Plant_mutation_MealLice", false));
			this.sunnySpeed = this.AddPlantMutation("sunnySpeed").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.MinLightLux, 1000f, false).AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute, -0.5f, true).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.25f, true).VisualSymbolOverride("snapTo_mutate1", "mutate_snaps_kanim", "leaf_mutate1").VisualSymbolOverride("snapTo_mutate2", "mutate_snaps_kanim", "leaf_mutate2").AddSoundEvent(GlobalAssets.GetSound("Plant_mutation_Leaf", false));
			this.slowBurn = this.AddPlantMutation("slowBurn").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, -0.9f, true).AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute, 3.5f, true).VisualTint(-0.3f, -0.3f, -0.5f);
			this.blooms = this.AddPlantMutation("blooms").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().BuildingAttributes.Decor, 20f, false).VisualSymbolOverride("snapTo_mutate1", "mutate_snaps_kanim", "blossom_mutate1").VisualSymbolOverride("snapTo_mutate2", "mutate_snaps_kanim", "blossom_mutate2").AddSoundEvent(GlobalAssets.GetSound("Plant_mutation_PrickleFlower", false));
			this.loadedWithFruit = this.AddPlantMutation("loadedWithFruit").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.YieldAmount, 1f, true).AttributeModifier(Db.Get().PlantAttributes.HarvestTime, 4f, true).AttributeModifier(Db.Get().PlantAttributes.MinLightLux, 200f, false).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.2f, true).VisualSymbolScale("swap_crop01", 1.3f).VisualSymbolScale("swap_crop02", 1.3f);
			this.rottenHeaps = this.AddPlantMutation("rottenHeaps").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute, -0.75f, true).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.5f, true).BonusCrop(RotPileConfig.ID, 4f).AddDiseaseToHarvest(Db.Get().Diseases.GetIndex(Db.Get().Diseases.FoodGerms.Id), 10000).ForcePrefersDarkness().VisualFGFX("mutate_stink_fx_kanim").VisualSymbolTint("swap_crop01", -0.2f, -0.1f, -0.5f).VisualSymbolTint("swap_crop02", -0.2f, -0.1f, -0.5f);
			this.heavyFruit = this.AddPlantMutation("heavyFruit").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.25f, true).ForceSelfHarvestOnGrown().VisualSymbolTint("swap_crop01", -0.1f, -0.5f, -0.5f).VisualSymbolTint("swap_crop02", -0.1f, -0.5f, -0.5f);
		}

		// Token: 0x060074CA RID: 29898 RVA: 0x002D8F38 File Offset: 0x002D7138
		public List<string> GetNamesForMutations(List<string> mutationIDs)
		{
			List<string> list = new List<string>(mutationIDs.Count);
			foreach (string id in mutationIDs)
			{
				list.Add(base.Get(id).Name);
			}
			return list;
		}

		// Token: 0x060074CB RID: 29899 RVA: 0x002D8FA0 File Offset: 0x002D71A0
		public PlantMutation GetRandomMutation(string targetPlantPrefabID)
		{
			return (from m in this.resources
			where !m.originalMutation && !m.restrictedPrefabIDs.Contains(targetPlantPrefabID) && (m.requiredPrefabIDs.Count == 0 || m.requiredPrefabIDs.Contains(targetPlantPrefabID))
			select m).ToList<PlantMutation>().GetRandom<PlantMutation>();
		}

		// Token: 0x04005453 RID: 21587
		public PlantMutation moderatelyLoose;

		// Token: 0x04005454 RID: 21588
		public PlantMutation moderatelyTight;

		// Token: 0x04005455 RID: 21589
		public PlantMutation extremelyTight;

		// Token: 0x04005456 RID: 21590
		public PlantMutation bonusLice;

		// Token: 0x04005457 RID: 21591
		public PlantMutation sunnySpeed;

		// Token: 0x04005458 RID: 21592
		public PlantMutation slowBurn;

		// Token: 0x04005459 RID: 21593
		public PlantMutation blooms;

		// Token: 0x0400545A RID: 21594
		public PlantMutation loadedWithFruit;

		// Token: 0x0400545B RID: 21595
		public PlantMutation heavyFruit;

		// Token: 0x0400545C RID: 21596
		public PlantMutation rottenHeaps;
	}
}
