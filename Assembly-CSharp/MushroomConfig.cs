using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001B2 RID: 434
public class MushroomConfig : IEntityConfig
{
	// Token: 0x060008D6 RID: 2262 RVA: 0x000377F4 File Offset: 0x000359F4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x000377FC File Offset: 0x000359FC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(MushroomConfig.ID, STRINGS.ITEMS.FOOD.MUSHROOM.NAME, STRINGS.ITEMS.FOOD.MUSHROOM.DESC, 1f, false, Assets.GetAnim("funguscap_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.77f, 0.48f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.MUSHROOM);
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x00037860 File Offset: 0x00035A60
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x00037862 File Offset: 0x00035A62
	public void OnSpawn(GameObject inst)
	{
		inst.Subscribe(-10536414, MushroomConfig.OnEatCompleteDelegate);
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x00037878 File Offset: 0x00035A78
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
				if (UnityEngine.Random.value < MushroomConfig.SEEDS_PER_FRUIT_CHANCE)
				{
					num++;
				}
			}
			if (num > 0)
			{
				Vector3 vector = edible.transform.GetPosition() + new Vector3(0f, 0.05f, 0f);
				vector = Grid.CellToPosCCC(Grid.PosToCell(vector), Grid.SceneLayer.Ore);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("MushroomSeed")), vector, Grid.SceneLayer.Ore, null, 0);
				PrimaryElement component = edible.GetComponent<PrimaryElement>();
				PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
				component2.Temperature = component.Temperature;
				component2.Units = (float)num;
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x04000602 RID: 1538
	public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

	// Token: 0x04000603 RID: 1539
	public static string ID = "Mushroom";

	// Token: 0x04000604 RID: 1540
	private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		MushroomConfig.OnEatComplete(component);
	});
}
