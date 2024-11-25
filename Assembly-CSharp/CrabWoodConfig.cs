using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000FB RID: 251
[EntityConfigOrder(1)]
public class CrabWoodConfig : IEntityConfig
{
	// Token: 0x0600049E RID: 1182 RVA: 0x00024A34 File Offset: 0x00022C34
	public static GameObject CreateCrabWood(string id, string name, string desc, string anim_file, bool is_baby, string deathDropID = "CrabWoodShell")
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseCrabConfig.BaseCrab(id, name, desc, anim_file, "CrabWoodBaseTrait", is_baby, CrabWoodConfig.animPrefix, deathDropID, 5), CrabTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("CrabWoodBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -CrabTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseCrabConfig.DietWithSlime(SimHashes.Sand.CreateTag(), CrabWoodConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_1, null, 0f);
		return BaseCrabConfig.SetupDiet(prefab, diet_infos, CrabWoodConfig.CALORIES_PER_KG_OF_ORE, CrabWoodConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x00024B6C File Offset: 0x00022D6C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x00024B74 File Offset: 0x00022D74
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabWoodConfig.CreateCrabWood("CrabWood", STRINGS.CREATURES.SPECIES.CRAB.VARIANT_WOOD.NAME, STRINGS.CREATURES.SPECIES.CRAB.VARIANT_WOOD.DESC, "pincher_kanim", false, "CrabWoodShell");
		gameObject = EntityTemplates.ExtendEntityToFertileCreature(gameObject, "CrabWoodEgg", STRINGS.CREATURES.SPECIES.CRAB.VARIANT_WOOD.EGG_NAME, STRINGS.CREATURES.SPECIES.CRAB.VARIANT_WOOD.DESC, "egg_pincher_kanim", CrabTuning.EGG_MASS, "CrabWoodBaby", 60.000004f, 20f, CrabTuning.EGG_CHANCES_WOOD, this.GetDlcIds(), CrabWoodConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		EggProtectionMonitor.Def def = gameObject.AddOrGetDef<EggProtectionMonitor.Def>();
		def.allyTags = new Tag[]
		{
			GameTags.Creatures.CrabFriend
		};
		def.animPrefix = CrabWoodConfig.animPrefix;
		MoltDropperMonitor.Def def2 = gameObject.AddOrGetDef<MoltDropperMonitor.Def>();
		def2.onGrowDropID = "CrabWoodShell";
		def2.massToDrop = 100f;
		def2.isReadyToMolt = new Func<MoltDropperMonitor.Instance, bool>(CrabTuning.IsReadyToMolt);
		return gameObject;
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x00024C55 File Offset: 0x00022E55
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x00024C57 File Offset: 0x00022E57
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400031A RID: 794
	public const string ID = "CrabWood";

	// Token: 0x0400031B RID: 795
	public const string BASE_TRAIT_ID = "CrabWoodBaseTrait";

	// Token: 0x0400031C RID: 796
	public const string EGG_ID = "CrabWoodEgg";

	// Token: 0x0400031D RID: 797
	private const SimHashes EMIT_ELEMENT = SimHashes.Sand;

	// Token: 0x0400031E RID: 798
	private static float KG_ORE_EATEN_PER_CYCLE = 70f;

	// Token: 0x0400031F RID: 799
	private static float CALORIES_PER_KG_OF_ORE = CrabTuning.STANDARD_CALORIES_PER_CYCLE / CrabWoodConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000320 RID: 800
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x04000321 RID: 801
	public static int EGG_SORT_ORDER = 0;

	// Token: 0x04000322 RID: 802
	private static string animPrefix = "wood_";
}
