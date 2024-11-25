using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000FD RID: 253
[EntityConfigOrder(1)]
public class DivergentBeetleConfig : IEntityConfig
{
	// Token: 0x060004AA RID: 1194 RVA: 0x00024D00 File Offset: 0x00022F00
	public static GameObject CreateDivergentBeetle(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseDivergentConfig.BaseDivergent(id, name, desc, 50f, anim_file, "DivergentBeetleBaseTrait", is_baby, 8f, null, "DivergentCropTended", 1, true), DivergentTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("DivergentBeetleBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DivergentTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -DivergentTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseDivergentConfig.BasicSulfurDiet(SimHashes.Sucrose.CreateTag(), DivergentBeetleConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f);
		GameObject gameObject = BaseDivergentConfig.SetupDiet(prefab, diet_infos, DivergentBeetleConfig.CALORIES_PER_KG_OF_ORE, DivergentBeetleConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x00024E4D File Offset: 0x0002304D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x00024E54 File Offset: 0x00023054
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(DivergentBeetleConfig.CreateDivergentBeetle("DivergentBeetle", STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.NAME, STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.DESC, "critter_kanim", false), "DivergentBeetleEgg", STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.EGG_NAME, STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.DESC, "egg_critter_kanim", DivergentTuning.EGG_MASS, "DivergentBeetleBaby", 45f, 15f, DivergentTuning.EGG_CHANCES_BEETLE, this.GetDlcIds(), DivergentBeetleConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x00024ED5 File Offset: 0x000230D5
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x00024ED7 File Offset: 0x000230D7
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000324 RID: 804
	public const string ID = "DivergentBeetle";

	// Token: 0x04000325 RID: 805
	public const string BASE_TRAIT_ID = "DivergentBeetleBaseTrait";

	// Token: 0x04000326 RID: 806
	public const string EGG_ID = "DivergentBeetleEgg";

	// Token: 0x04000327 RID: 807
	private const float LIFESPAN = 75f;

	// Token: 0x04000328 RID: 808
	private const SimHashes EMIT_ELEMENT = SimHashes.Sucrose;

	// Token: 0x04000329 RID: 809
	private static float KG_ORE_EATEN_PER_CYCLE = 20f;

	// Token: 0x0400032A RID: 810
	private static float CALORIES_PER_KG_OF_ORE = DivergentTuning.STANDARD_CALORIES_PER_CYCLE / DivergentBeetleConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x0400032B RID: 811
	private static float MIN_POOP_SIZE_IN_KG = 4f;

	// Token: 0x0400032C RID: 812
	public static int EGG_SORT_ORDER = 0;
}
