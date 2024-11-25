using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200012C RID: 300
[EntityConfigOrder(1)]
public class PacuCleanerConfig : IEntityConfig
{
	// Token: 0x060005C7 RID: 1479 RVA: 0x00029240 File Offset: 0x00027440
	public static GameObject CreatePacu(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BasePacuConfig.CreatePrefab(id, "PacuCleanerBaseTrait", name, desc, anim_file, is_baby, "glp_", 243.15f, 278.15f, 223.15f, 298.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, PacuTuning.PEN_SIZE_PER_CREATURE, false);
		if (!is_baby)
		{
			Storage storage = gameObject.AddComponent<Storage>();
			storage.capacityKg = 10f;
			storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
			PassiveElementConsumer passiveElementConsumer = gameObject.AddOrGet<PassiveElementConsumer>();
			passiveElementConsumer.elementToConsume = SimHashes.DirtyWater;
			passiveElementConsumer.consumptionRate = 0.2f;
			passiveElementConsumer.capacityKG = 10f;
			passiveElementConsumer.consumptionRadius = 3;
			passiveElementConsumer.showInStatusPanel = true;
			passiveElementConsumer.sampleCellOffset = new Vector3(0f, 0f, 0f);
			passiveElementConsumer.isRequired = false;
			passiveElementConsumer.storeOnConsume = true;
			passiveElementConsumer.showDescriptor = false;
			gameObject.AddOrGet<UpdateElementConsumerPosition>();
			BubbleSpawner bubbleSpawner = gameObject.AddComponent<BubbleSpawner>();
			bubbleSpawner.element = SimHashes.Water;
			bubbleSpawner.emitMass = 2f;
			bubbleSpawner.emitVariance = 0.5f;
			bubbleSpawner.initialVelocity = new Vector2f(0, 1);
			ElementConverter elementConverter = gameObject.AddOrGet<ElementConverter>();
			elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(SimHashes.DirtyWater.CreateTag(), 0.2f, true)
			};
			elementConverter.outputElements = new ElementConverter.OutputElement[]
			{
				new ElementConverter.OutputElement(0.2f, SimHashes.Water, 0f, true, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true)
			};
		}
		return gameObject;
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x000293B7 File Offset: 0x000275B7
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x000293C0 File Offset: 0x000275C0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(EntityTemplates.ExtendEntityToWildCreature(PacuCleanerConfig.CreatePacu("PacuCleaner", STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.DESC, "pacu_kanim", false), PacuTuning.PEN_SIZE_PER_CREATURE, false), "PacuCleanerEgg", STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.EGG_NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuCleanerBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_CLEANER, this.GetDlcIds(), 501, false, true, false, 0.75f, false);
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x0002944C File Offset: 0x0002764C
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x00029450 File Offset: 0x00027650
	public void OnSpawn(GameObject inst)
	{
		ElementConsumer component = inst.GetComponent<ElementConsumer>();
		if (component != null)
		{
			component.EnableConsumption(true);
		}
	}

	// Token: 0x04000409 RID: 1033
	public const string ID = "PacuCleaner";

	// Token: 0x0400040A RID: 1034
	public const string BASE_TRAIT_ID = "PacuCleanerBaseTrait";

	// Token: 0x0400040B RID: 1035
	public const string EGG_ID = "PacuCleanerEgg";

	// Token: 0x0400040C RID: 1036
	public const float POLLUTED_WATER_CONVERTED_PER_CYCLE = 120f;

	// Token: 0x0400040D RID: 1037
	public const SimHashes INPUT_ELEMENT = SimHashes.DirtyWater;

	// Token: 0x0400040E RID: 1038
	public const SimHashes OUTPUT_ELEMENT = SimHashes.Water;

	// Token: 0x0400040F RID: 1039
	public static readonly EffectorValues DECOR = TUNING.BUILDINGS.DECOR.BONUS.TIER4;

	// Token: 0x04000410 RID: 1040
	public const int EGG_SORT_ORDER = 501;
}
