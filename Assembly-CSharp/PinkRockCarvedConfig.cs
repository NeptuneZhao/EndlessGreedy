using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E6 RID: 742
public class PinkRockCarvedConfig : IEntityConfig
{
	// Token: 0x06000F91 RID: 3985 RVA: 0x00059838 File Offset: 0x00057A38
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000F92 RID: 3986 RVA: 0x00059840 File Offset: 0x00057A40
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PinkRockCarved", STRINGS.CREATURES.SPECIES.PINKROCKCARVED.NAME, STRINGS.CREATURES.SPECIES.PINKROCKCARVED.DESC, 1f, true, Assets.GetAnim("pinkrock_decor_kanim"), "idle", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.CIRCLE, 0.5f, 0.5f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.RareMaterials,
			GameTags.MiscPickupable,
			GameTags.PedestalDisplayable,
			GameTags.Experimental
		});
		gameObject.AddOrGet<OccupyArea>();
		DecorProvider decorProvider = gameObject.AddOrGet<DecorProvider>();
		decorProvider.SetValues(TUNING.BUILDINGS.DECOR.BONUS.TIER1);
		decorProvider.overrideName = gameObject.GetProperName();
		Light2D light2D = gameObject.AddOrGet<Light2D>();
		light2D.overlayColour = LIGHT2D.PINKROCK_COLOR;
		light2D.Color = LIGHT2D.PINKROCK_COLOR;
		light2D.Range = 3f;
		light2D.Angle = 0f;
		light2D.Direction = LIGHT2D.PINKROCK_DIRECTION;
		light2D.Offset = LIGHT2D.PINKROCK_OFFSET;
		light2D.shape = global::LightShape.Circle;
		light2D.drawOverlay = true;
		light2D.disableOnStore = true;
		gameObject.GetComponent<KCircleCollider2D>().offset = new Vector2(0f, 0.25f);
		return gameObject;
	}

	// Token: 0x06000F93 RID: 3987 RVA: 0x00059965 File Offset: 0x00057B65
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F94 RID: 3988 RVA: 0x00059967 File Offset: 0x00057B67
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000986 RID: 2438
	public const string ID = "PinkRockCarved";
}
