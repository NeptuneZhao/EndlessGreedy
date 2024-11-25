using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class StaterpillarLiquidConfig : IEntityConfig
{
	// Token: 0x0600066A RID: 1642 RVA: 0x0002B77C File Offset: 0x0002997C
	public static GameObject CreateStaterpillarLiquid(string id, string name, string desc, string anim_file, bool is_baby)
	{
		InhaleStates.Def inhaleDef = new InhaleStates.Def
		{
			behaviourTag = GameTags.Creatures.WantsToStore,
			inhaleAnimPre = "liquid_consume_pre",
			inhaleAnimLoop = "liquid_consume_loop",
			inhaleAnimPst = "liquid_consume_pst",
			useStorage = true,
			alwaysPlayPstAnim = true,
			inhaleTime = StaterpillarLiquidConfig.INHALE_TIME,
			storageStatusItem = Db.Get().CreatureStatusItems.LookingForLiquid
		};
		GameObject gameObject = BaseStaterpillarConfig.BaseStaterpillar(id, name, desc, anim_file, "StaterpillarLiquidBaseTrait", is_baby, ObjectLayer.LiquidConduit, StaterpillarLiquidConnectorConfig.ID, GameTags.Unbreathable, "wtr_", 263.15f, 313.15f, 173.15f, 373.15f, inhaleDef);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, TUNING.CREATURES.SPACE_REQUIREMENTS.TIER3);
		if (!is_baby)
		{
			GasAndLiquidConsumerMonitor.Def def = gameObject.AddOrGetDef<GasAndLiquidConsumerMonitor.Def>();
			def.behaviourTag = GameTags.Creatures.WantsToStore;
			def.consumableElementTag = GameTags.Liquid;
			def.transitionTag = new Tag[]
			{
				GameTags.Creature
			};
			def.minCooldown = StaterpillarLiquidConfig.COOLDOWN_MIN;
			def.maxCooldown = StaterpillarLiquidConfig.COOLDOWN_MAX;
			def.consumptionRate = StaterpillarLiquidConfig.CONSUMPTION_RATE;
		}
		Trait trait = Db.Get().CreateTrait("StaterpillarLiquidBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StaterpillarTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = new List<Diet.Info>();
		list.AddRange(BaseStaterpillarConfig.RawMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarLiquidConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		list.AddRange(BaseStaterpillarConfig.RefinedMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarLiquidConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		gameObject = BaseStaterpillarConfig.SetupDiet(gameObject, list);
		Storage storage = gameObject.AddComponent<Storage>();
		storage.capacityKg = StaterpillarLiquidConfig.STORAGE_CAPACITY;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		return gameObject;
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x0002B9D3 File Offset: 0x00029BD3
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x0002B9DC File Offset: 0x00029BDC
	public virtual GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(StaterpillarLiquidConfig.CreateStaterpillarLiquid("StaterpillarLiquid", STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.DESC, "caterpillar_kanim", false), "StaterpillarLiquidEgg", STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.EGG_NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.DESC, "egg_caterpillar_kanim", StaterpillarTuning.EGG_MASS, "StaterpillarLiquidBaby", 60.000004f, 20f, StaterpillarTuning.EGG_CHANCES_LIQUID, this.GetDlcIds(), 2, true, false, true, 1f, false);
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x0002BA59 File Offset: 0x00029C59
	public void OnPrefabInit(GameObject prefab)
	{
		KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
		component.SetSymbolVisiblity("electric_bolt_c_bloom", false);
		component.SetSymbolVisiblity("gulp", false);
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x0002BA82 File Offset: 0x00029C82
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000489 RID: 1161
	public const string ID = "StaterpillarLiquid";

	// Token: 0x0400048A RID: 1162
	public const string BASE_TRAIT_ID = "StaterpillarLiquidBaseTrait";

	// Token: 0x0400048B RID: 1163
	public const string EGG_ID = "StaterpillarLiquidEgg";

	// Token: 0x0400048C RID: 1164
	public const int EGG_SORT_ORDER = 2;

	// Token: 0x0400048D RID: 1165
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x0400048E RID: 1166
	private static float CALORIES_PER_KG_OF_ORE = StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / StaterpillarLiquidConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x0400048F RID: 1167
	private static float STORAGE_CAPACITY = 1000f;

	// Token: 0x04000490 RID: 1168
	private static float COOLDOWN_MIN = 20f;

	// Token: 0x04000491 RID: 1169
	private static float COOLDOWN_MAX = 40f;

	// Token: 0x04000492 RID: 1170
	private static float CONSUMPTION_RATE = 10f;

	// Token: 0x04000493 RID: 1171
	private static float INHALE_TIME = 6f;
}
