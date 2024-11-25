using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000199 RID: 409
public class BurgerConfig : IEntityConfig
{
	// Token: 0x06000854 RID: 2132 RVA: 0x00036A0A File Offset: 0x00034C0A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x00036A14 File Offset: 0x00034C14
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Burger", STRINGS.ITEMS.FOOD.BURGER.NAME, STRINGS.ITEMS.FOOD.BURGER.DESC, 1f, false, Assets.GetAnim("frost_burger_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BURGER);
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x00036A78 File Offset: 0x00034C78
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x00036A7A File Offset: 0x00034C7A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005D0 RID: 1488
	public const string ID = "Burger";

	// Token: 0x040005D1 RID: 1489
	public static ComplexRecipe recipe;
}
