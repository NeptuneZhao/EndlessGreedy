using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200019B RID: 411
public class CarrotConfig : IEntityConfig
{
	// Token: 0x0600085F RID: 2143 RVA: 0x00036B16 File Offset: 0x00034D16
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x00036B20 File Offset: 0x00034D20
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(CarrotConfig.ID, STRINGS.ITEMS.FOOD.CARROT.NAME, STRINGS.ITEMS.FOOD.CARROT.DESC, 1f, false, Assets.GetAnim("purplerootVegetable_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.CARROT);
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x00036B84 File Offset: 0x00034D84
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00036B86 File Offset: 0x00034D86
	public void OnSpawn(GameObject inst)
	{
		inst.Subscribe(-10536414, CarrotConfig.OnEatCompleteDelegate);
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00036B9C File Offset: 0x00034D9C
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
				if (UnityEngine.Random.value < CarrotConfig.SEEDS_PER_FRUIT_CHANCE)
				{
					num++;
				}
			}
			if (num > 0)
			{
				Vector3 vector = edible.transform.GetPosition() + new Vector3(0f, 0.05f, 0f);
				vector = Grid.CellToPosCCC(Grid.PosToCell(vector), Grid.SceneLayer.Ore);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("CarrotPlantSeed")), vector, Grid.SceneLayer.Ore, null, 0);
				PrimaryElement component = edible.GetComponent<PrimaryElement>();
				PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
				component2.Temperature = component.Temperature;
				component2.Units = (float)num;
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x040005D6 RID: 1494
	public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

	// Token: 0x040005D7 RID: 1495
	public static string ID = "Carrot";

	// Token: 0x040005D8 RID: 1496
	private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		CarrotConfig.OnEatComplete(component);
	});
}
