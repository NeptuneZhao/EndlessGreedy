﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000120 RID: 288
public class MoleConfig : IEntityConfig
{
	// Token: 0x0600057C RID: 1404 RVA: 0x00028054 File Offset: 0x00026254
	public static GameObject CreateMole(string id, string name, string desc, string anim_file, bool is_baby = false)
	{
		GameObject gameObject = BaseMoleConfig.BaseMole(id, name, STRINGS.CREATURES.SPECIES.MOLE.DESC, "MoleBaseTrait", anim_file, is_baby, 173.15f, 673.15f, 73.149994f, 773.15f, null, 10);
		gameObject.AddTag(GameTags.Creatures.Digger);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, MoleTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("MoleBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MoleTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -MoleTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		Diet diet = new Diet(BaseMoleConfig.SimpleOreDiet(new List<Tag>
		{
			SimHashes.Regolith.CreateTag(),
			SimHashes.Dirt.CreateTag(),
			SimHashes.IronOre.CreateTag()
		}, MoleConfig.CALORIES_PER_KG_OF_DIRT, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL).ToArray());
		CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minConsumedCaloriesBeforePooping = MoleConfig.MIN_POOP_SIZE_IN_CALORIES;
		gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		gameObject.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
		gameObject.AddOrGet<LoopingSounds>();
		foreach (HashedString hash in MoleTuning.GINGER_SYMBOL_NAMES)
		{
			gameObject.GetComponent<KAnimControllerBase>().SetSymbolVisiblity(hash, false);
		}
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x00028243 File Offset: 0x00026443
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x0002824C File Offset: 0x0002644C
	public GameObject CreatePrefab()
	{
		GameObject prefab = MoleConfig.CreateMole("Mole", STRINGS.CREATURES.SPECIES.MOLE.NAME, STRINGS.CREATURES.SPECIES.MOLE.DESC, "driller_kanim", false);
		string eggId = "MoleEgg";
		string eggName = STRINGS.CREATURES.SPECIES.MOLE.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.MOLE.DESC;
		string egg_anim = "egg_driller_kanim";
		float egg_MASS = MoleTuning.EGG_MASS;
		string baby_id = "MoleBaby";
		float fertility_cycles = 60.000004f;
		float incubation_cycles = 20f;
		int egg_SORT_ORDER = MoleConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, MoleTuning.EGG_CHANCES_BASE, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x000282CF File Offset: 0x000264CF
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x000282D1 File Offset: 0x000264D1
	public void OnSpawn(GameObject inst)
	{
		MoleConfig.SetSpawnNavType(inst);
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x000282DC File Offset: 0x000264DC
	public static void SetSpawnNavType(GameObject inst)
	{
		int cell = Grid.PosToCell(inst);
		Navigator component = inst.GetComponent<Navigator>();
		Pickupable component2 = inst.GetComponent<Pickupable>();
		if (component != null && (component2 == null || component2.storage == null))
		{
			if (Grid.IsSolidCell(cell))
			{
				component.SetCurrentNavType(NavType.Solid);
				inst.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.FXFront));
				inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.FXFront);
				return;
			}
			inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
		}
	}

	// Token: 0x040003CA RID: 970
	public const string ID = "Mole";

	// Token: 0x040003CB RID: 971
	public const string BASE_TRAIT_ID = "MoleBaseTrait";

	// Token: 0x040003CC RID: 972
	public const string EGG_ID = "MoleEgg";

	// Token: 0x040003CD RID: 973
	private static float MIN_POOP_SIZE_IN_CALORIES = 2400000f;

	// Token: 0x040003CE RID: 974
	private static float CALORIES_PER_KG_OF_DIRT = 1000f;

	// Token: 0x040003CF RID: 975
	public static int EGG_SORT_ORDER = 800;
}
