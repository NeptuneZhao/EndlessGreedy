using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001B0 RID: 432
public class MeatConfig : IEntityConfig
{
	// Token: 0x060008CB RID: 2251 RVA: 0x0003764C File Offset: 0x0003584C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x00037654 File Offset: 0x00035854
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Meat", STRINGS.ITEMS.FOOD.MEAT.NAME, STRINGS.ITEMS.FOOD.MEAT.DESC, 1f, false, Assets.GetAnim("creaturemeat_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.MEAT);
		return gameObject;
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x000376BA File Offset: 0x000358BA
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x000376BC File Offset: 0x000358BC
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005FF RID: 1535
	public const string ID = "Meat";
}
