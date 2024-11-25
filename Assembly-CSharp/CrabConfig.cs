﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000F7 RID: 247
[EntityConfigOrder(1)]
public class CrabConfig : IEntityConfig
{
	// Token: 0x06000486 RID: 1158 RVA: 0x000243AC File Offset: 0x000225AC
	public static GameObject CreateCrab(string id, string name, string desc, string anim_file, bool is_baby, string deathDropID)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseCrabConfig.BaseCrab(id, name, desc, anim_file, "CrabBaseTrait", is_baby, null, deathDropID, 1), CrabTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("CrabBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -CrabTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseCrabConfig.BasicDiet(SimHashes.Sand.CreateTag(), CrabConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f);
		GameObject gameObject = BaseCrabConfig.SetupDiet(prefab, diet_infos, CrabConfig.CALORIES_PER_KG_OF_ORE, CrabConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x000244EB File Offset: 0x000226EB
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x000244F4 File Offset: 0x000226F4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabConfig.CreateCrab("Crab", STRINGS.CREATURES.SPECIES.CRAB.NAME, STRINGS.CREATURES.SPECIES.CRAB.DESC, "pincher_kanim", false, "CrabShell");
		gameObject = EntityTemplates.ExtendEntityToFertileCreature(gameObject, "CrabEgg", STRINGS.CREATURES.SPECIES.CRAB.EGG_NAME, STRINGS.CREATURES.SPECIES.CRAB.DESC, "egg_pincher_kanim", CrabTuning.EGG_MASS, "CrabBaby", 60.000004f, 20f, CrabTuning.EGG_CHANCES_BASE, this.GetDlcIds(), CrabConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		gameObject.AddOrGetDef<EggProtectionMonitor.Def>().allyTags = new Tag[]
		{
			GameTags.Creatures.CrabFriend
		};
		return gameObject;
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x0002459B File Offset: 0x0002279B
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x0002459D File Offset: 0x0002279D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000307 RID: 775
	public const string ID = "Crab";

	// Token: 0x04000308 RID: 776
	public const string BASE_TRAIT_ID = "CrabBaseTrait";

	// Token: 0x04000309 RID: 777
	public const string EGG_ID = "CrabEgg";

	// Token: 0x0400030A RID: 778
	private const SimHashes EMIT_ELEMENT = SimHashes.Sand;

	// Token: 0x0400030B RID: 779
	private static float KG_ORE_EATEN_PER_CYCLE = 70f;

	// Token: 0x0400030C RID: 780
	private static float CALORIES_PER_KG_OF_ORE = CrabTuning.STANDARD_CALORIES_PER_CYCLE / CrabConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x0400030D RID: 781
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x0400030E RID: 782
	public static int EGG_SORT_ORDER = 0;
}
