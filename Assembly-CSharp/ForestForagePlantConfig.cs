using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000166 RID: 358
public class ForestForagePlantConfig : IEntityConfig
{
	// Token: 0x06000702 RID: 1794 RVA: 0x0002EABB File Offset: 0x0002CCBB
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x0002EAC4 File Offset: 0x0002CCC4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("ForestForagePlant", STRINGS.ITEMS.FOOD.FORESTFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.FORESTFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("podmelon_fruit_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FORESTFORAGEPLANT);
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x0002EB28 File Offset: 0x0002CD28
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x0002EB2A File Offset: 0x0002CD2A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004F9 RID: 1273
	public const string ID = "ForestForagePlant";
}
