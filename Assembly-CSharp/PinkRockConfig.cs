using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E7 RID: 743
public class PinkRockConfig : IEntityConfig
{
	// Token: 0x06000F96 RID: 3990 RVA: 0x00059971 File Offset: 0x00057B71
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000F97 RID: 3991 RVA: 0x00059978 File Offset: 0x00057B78
	public GameObject CreatePrefab()
	{
		string id = this.ID;
		string name = STRINGS.CREATURES.SPECIES.PINKROCK.NAME;
		string desc = STRINGS.CREATURES.SPECIES.PINKROCK.DESC;
		float mass = 25f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("pinkrock_kanim"), "idle", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Experimental
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 235.15f;
		gameObject.AddOrGet<Carvable>().dropItemPrefabId = "PinkRockCarved";
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		Light2D light2D = gameObject.AddOrGet<Light2D>();
		light2D.overlayColour = LIGHT2D.PINKROCK_COLOR;
		light2D.Color = LIGHT2D.PINKROCK_COLOR;
		light2D.Range = 2f;
		light2D.Angle = 0f;
		light2D.Direction = LIGHT2D.PINKROCK_DIRECTION;
		light2D.Offset = LIGHT2D.PINKROCK_OFFSET;
		light2D.shape = global::LightShape.Circle;
		light2D.drawOverlay = true;
		return gameObject;
	}

	// Token: 0x06000F98 RID: 3992 RVA: 0x00059A88 File Offset: 0x00057C88
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F99 RID: 3993 RVA: 0x00059A8A File Offset: 0x00057C8A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000987 RID: 2439
	public string ID = "PinkRock";
}
