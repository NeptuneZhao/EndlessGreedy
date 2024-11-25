using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002CA RID: 714
public class EmptyElectrobankConfig : IEntityConfig
{
	// Token: 0x06000EE9 RID: 3817 RVA: 0x00056C40 File Offset: 0x00054E40
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("EmptyElectrobank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_EMPTY.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_EMPTY.DESC, 20f, true, Assets.GetAnim("electrobank_large_depleted_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.Aluminum, new List<Tag>
		{
			GameTags.EmptyPortableBattery,
			GameTags.PedestalDisplayable
		});
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		return gameObject;
	}

	// Token: 0x06000EEA RID: 3818 RVA: 0x00056CE8 File Offset: 0x00054EE8
	public string[] GetDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x00056CEF File Offset: 0x00054EEF
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000EEC RID: 3820 RVA: 0x00056CF1 File Offset: 0x00054EF1
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400092C RID: 2348
	public const string ID = "EmptyElectrobank";

	// Token: 0x0400092D RID: 2349
	public const float MASS = 20f;
}
