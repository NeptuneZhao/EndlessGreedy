using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002D4 RID: 724
public class IceBellyPoopConfig : IEntityConfig
{
	// Token: 0x06000F20 RID: 3872 RVA: 0x0005802E File Offset: 0x0005622E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000F21 RID: 3873 RVA: 0x00058038 File Offset: 0x00056238
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("IceBellyPoop", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ICE_BELLY_POOP.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ICE_BELLY_POOP.DESC, 100f, false, Assets.GetAnim("bammoth_poop_kanim"), "idle3", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.4f, 0.4f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.PedestalDisplayable
		});
		gameObject.GetComponent<KCollider2D>().offset = new Vector2(0f, 0.05f);
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		DecorProvider decorProvider = gameObject.AddOrGet<DecorProvider>();
		decorProvider.SetValues(DECOR.PENALTY.TIER3);
		decorProvider.overrideName = STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ICE_BELLY_POOP.NAME;
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000F22 RID: 3874 RVA: 0x000580FF File Offset: 0x000562FF
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F23 RID: 3875 RVA: 0x00058101 File Offset: 0x00056301
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000954 RID: 2388
	public const string ID = "IceBellyPoop";
}
