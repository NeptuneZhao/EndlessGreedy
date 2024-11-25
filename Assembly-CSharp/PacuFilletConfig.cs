using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001B5 RID: 437
public class PacuFilletConfig : IEntityConfig
{
	// Token: 0x060008E8 RID: 2280 RVA: 0x00037A8E File Offset: 0x00035C8E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x00037A98 File Offset: 0x00035C98
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PacuFillet", STRINGS.ITEMS.FOOD.MEAT.NAME, STRINGS.ITEMS.FOOD.MEAT.DESC, 1f, false, Assets.GetAnim("pacufillet_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.FISH_MEAT);
		return gameObject;
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x00037AFE File Offset: 0x00035CFE
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x00037B00 File Offset: 0x00035D00
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400060B RID: 1547
	public const string ID = "PacuFillet";
}
