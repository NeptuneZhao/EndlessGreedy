using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002C9 RID: 713
public class ElectrobankConfig : IEntityConfig
{
	// Token: 0x06000EE4 RID: 3812 RVA: 0x00056B78 File Offset: 0x00054D78
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Electrobank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK.DESC, 20f, true, Assets.GetAnim("electrobank_large_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.Aluminum, new List<Tag>
		{
			GameTags.ChargedPortableBattery,
			GameTags.PedestalDisplayable
		});
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddComponent<Electrobank>().rechargeable = true;
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		return gameObject;
	}

	// Token: 0x06000EE5 RID: 3813 RVA: 0x00056C2C File Offset: 0x00054E2C
	public string[] GetDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06000EE6 RID: 3814 RVA: 0x00056C33 File Offset: 0x00054E33
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x00056C35 File Offset: 0x00054E35
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000928 RID: 2344
	public const string ID = "Electrobank";

	// Token: 0x04000929 RID: 2345
	public const float MASS = 20f;

	// Token: 0x0400092A RID: 2346
	public const float POWER_CAPACITY = 120000f;

	// Token: 0x0400092B RID: 2347
	public static ComplexRecipe recipe;
}
