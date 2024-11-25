using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002D1 RID: 721
public class GoldBellyCrownConfig : IEntityConfig
{
	// Token: 0x06000F0F RID: 3855 RVA: 0x000570EE File Offset: 0x000552EE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000F10 RID: 3856 RVA: 0x000570F8 File Offset: 0x000552F8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("GoldBellyCrown", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.GOLD_BELLY_CROWN.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.GOLD_BELLY_CROWN.DESC, 1f, true, Assets.GetAnim("bammoth_crown_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.5f, true, 0, SimHashes.GoldAmalgam, new List<Tag>
		{
			GameTags.PedestalDisplayable
		});
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		DecorProvider decorProvider = gameObject.AddOrGet<DecorProvider>();
		decorProvider.SetValues(DECOR.BONUS.TIER2);
		decorProvider.overrideName = STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.GOLD_BELLY_CROWN.NAME;
		return gameObject;
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x000571A5 File Offset: 0x000553A5
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F12 RID: 3858 RVA: 0x000571A7 File Offset: 0x000553A7
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000937 RID: 2359
	public const string ID = "GoldBellyCrown";
}
