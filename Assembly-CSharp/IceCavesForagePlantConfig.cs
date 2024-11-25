using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000171 RID: 369
public class IceCavesForagePlantConfig : IEntityConfig
{
	// Token: 0x0600073F RID: 1855 RVA: 0x000308DF File Offset: 0x0002EADF
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x000308E8 File Offset: 0x0002EAE8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("IceCavesForagePlant", STRINGS.ITEMS.FOOD.ICECAVESFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.ICECAVESFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("frozenberries_fruit_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.ICECAVESFORAGEPLANT);
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x0003094C File Offset: 0x0002EB4C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x0003094E File Offset: 0x0002EB4E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400052F RID: 1327
	public const string ID = "IceCavesForagePlant";
}
