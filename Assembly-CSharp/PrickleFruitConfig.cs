using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001BA RID: 442
public class PrickleFruitConfig : IEntityConfig
{
	// Token: 0x06000901 RID: 2305 RVA: 0x00037D22 File Offset: 0x00035F22
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x00037D2C File Offset: 0x00035F2C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(PrickleFruitConfig.ID, STRINGS.ITEMS.FOOD.PRICKLEFRUIT.NAME, STRINGS.ITEMS.FOOD.PRICKLEFRUIT.DESC, 1f, false, Assets.GetAnim("bristleberry_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.PRICKLEFRUIT);
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x00037D90 File Offset: 0x00035F90
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x00037D92 File Offset: 0x00035F92
	public void OnSpawn(GameObject inst)
	{
		inst.Subscribe(-10536414, PrickleFruitConfig.OnEatCompleteDelegate);
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x00037DA8 File Offset: 0x00035FA8
	private static void OnEatComplete(Edible edible)
	{
		if (edible != null)
		{
			int num = 0;
			float unitsConsumed = edible.unitsConsumed;
			int num2 = Mathf.FloorToInt(unitsConsumed);
			float num3 = unitsConsumed % 1f;
			if (UnityEngine.Random.value < num3)
			{
				num2++;
			}
			for (int i = 0; i < num2; i++)
			{
				if (UnityEngine.Random.value < PrickleFruitConfig.SEEDS_PER_FRUIT_CHANCE)
				{
					num++;
				}
			}
			if (num > 0)
			{
				Vector3 vector = edible.transform.GetPosition() + new Vector3(0f, 0.05f, 0f);
				vector = Grid.CellToPosCCC(Grid.PosToCell(vector), Grid.SceneLayer.Ore);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("PrickleFlowerSeed")), vector, Grid.SceneLayer.Ore, null, 0);
				PrimaryElement component = edible.GetComponent<PrimaryElement>();
				PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
				component2.Temperature = component.Temperature;
				component2.Units = (float)num;
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x04000613 RID: 1555
	public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

	// Token: 0x04000614 RID: 1556
	public static string ID = "PrickleFruit";

	// Token: 0x04000615 RID: 1557
	private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		PrickleFruitConfig.OnEatComplete(component);
	});
}
