using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000FF RID: 255
[EntityConfigOrder(1)]
public class DivergentWormConfig : IEntityConfig
{
	// Token: 0x060004B6 RID: 1206 RVA: 0x00024F60 File Offset: 0x00023160
	public static GameObject CreateWorm(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseDivergentConfig.BaseDivergent(id, name, desc, 200f, anim_file, "DivergentWormBaseTrait", is_baby, 8f, null, "DivergentCropTendedWorm", 3, false), DivergentTuning.PEN_SIZE_PER_CREATURE_WORM);
		Trait trait = Db.Get().CreateTrait("DivergentWormBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DivergentTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -DivergentTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name, false, false, true));
		prefab.AddWeapon(2f, 3f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0f);
		List<Diet.Info> list = BaseDivergentConfig.BasicSulfurDiet(SimHashes.Mud.CreateTag(), DivergentWormConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, null, 0f);
		list.Add(new Diet.Info(new HashSet<Tag>
		{
			SimHashes.Sucrose.CreateTag()
		}, SimHashes.Mud.CreateTag(), DivergentWormConfig.CALORIES_PER_KG_OF_SUCROSE, 1f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, null));
		GameObject gameObject = BaseDivergentConfig.SetupDiet(prefab, list, DivergentWormConfig.CALORIES_PER_KG_OF_ORE, DivergentWormConfig.MINI_POOP_SIZE_IN_KG);
		SegmentedCreature.Def def = gameObject.AddOrGetDef<SegmentedCreature.Def>();
		def.segmentTrackerSymbol = new HashedString("segmenttracker");
		def.numBodySegments = 5;
		def.midAnim = Assets.GetAnim("worm_torso_kanim");
		def.tailAnim = Assets.GetAnim("worm_tail_kanim");
		def.animFrameOffset = 2;
		def.pathSpacing = 0.2f;
		def.numPathNodes = 15;
		def.minSegmentSpacing = 0.1f;
		def.maxSegmentSpacing = 0.4f;
		def.retractionSegmentSpeed = 1f;
		def.retractionPathSpeed = 2f;
		def.compressedMaxScale = 0.25f;
		def.headOffset = new Vector3(0.12f, 0.4f, 0f);
		return gameObject;
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x000251AB File Offset: 0x000233AB
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x000251B4 File Offset: 0x000233B4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(DivergentWormConfig.CreateWorm("DivergentWorm", STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.NAME, STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.DESC, "worm_head_kanim", false), "DivergentWormEgg", STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.EGG_NAME, STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.DESC, "egg_worm_kanim", DivergentTuning.EGG_MASS, "DivergentWormBaby", 90f, 30f, DivergentTuning.EGG_CHANCES_WORM, this.GetDlcIds(), DivergentWormConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x00025235 File Offset: 0x00023435
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x00025237 File Offset: 0x00023437
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400032E RID: 814
	public const string ID = "DivergentWorm";

	// Token: 0x0400032F RID: 815
	public const string BASE_TRAIT_ID = "DivergentWormBaseTrait";

	// Token: 0x04000330 RID: 816
	public const string EGG_ID = "DivergentWormEgg";

	// Token: 0x04000331 RID: 817
	private const float LIFESPAN = 150f;

	// Token: 0x04000332 RID: 818
	public const float CROP_TENDED_MULTIPLIER_EFFECT = 0.5f;

	// Token: 0x04000333 RID: 819
	public const float CROP_TENDED_MULTIPLIER_DURATION = 600f;

	// Token: 0x04000334 RID: 820
	private const int NUM_SEGMENTS = 5;

	// Token: 0x04000335 RID: 821
	private const SimHashes EMIT_ELEMENT = SimHashes.Mud;

	// Token: 0x04000336 RID: 822
	private static float KG_ORE_EATEN_PER_CYCLE = 50f;

	// Token: 0x04000337 RID: 823
	private static float KG_SUCROSE_EATEN_PER_CYCLE = 30f;

	// Token: 0x04000338 RID: 824
	private static float CALORIES_PER_KG_OF_ORE = DivergentTuning.STANDARD_CALORIES_PER_CYCLE / DivergentWormConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000339 RID: 825
	private static float CALORIES_PER_KG_OF_SUCROSE = DivergentTuning.STANDARD_CALORIES_PER_CYCLE / DivergentWormConfig.KG_SUCROSE_EATEN_PER_CYCLE;

	// Token: 0x0400033A RID: 826
	public static int EGG_SORT_ORDER = 0;

	// Token: 0x0400033B RID: 827
	private static float MINI_POOP_SIZE_IN_KG = 4f;
}
