using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002CE RID: 718
public class GarbageElectrobankConfig : IEntityConfig
{
	// Token: 0x06000EFF RID: 3839 RVA: 0x00056F2C File Offset: 0x0005512C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("GarbageElectrobank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_GARBAGE.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_GARBAGE.DESC, 20f, true, Assets.GetAnim("electrobank_large_destroyed_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.Aluminum, new List<Tag>
		{
			GameTags.PedestalDisplayable
		});
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		return gameObject;
	}

	// Token: 0x06000F00 RID: 3840 RVA: 0x00056FC9 File Offset: 0x000551C9
	public string[] GetDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06000F01 RID: 3841 RVA: 0x00056FD0 File Offset: 0x000551D0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F02 RID: 3842 RVA: 0x00056FD2 File Offset: 0x000551D2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000931 RID: 2353
	public const string ID = "GarbageElectrobank";

	// Token: 0x04000932 RID: 2354
	public const float MASS = 20f;
}
