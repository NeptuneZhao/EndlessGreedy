using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A7 RID: 935
public class PropHumanMurphyBedConfig : IEntityConfig
{
	// Token: 0x06001370 RID: 4976 RVA: 0x0006B6B0 File Offset: 0x000698B0
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06001371 RID: 4977 RVA: 0x0006B6B8 File Offset: 0x000698B8
	public GameObject CreatePrefab()
	{
		string id = "PropHumanMurphyBed";
		string name = STRINGS.BUILDINGS.PREFABS.PROPHUMANMURPHYBED.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPHUMANMURPHYBED.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_murphybed_kanim"), "on", Grid.SceneLayer.Building, 5, 4, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001372 RID: 4978 RVA: 0x0006B74B File Offset: 0x0006994B
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001373 RID: 4979 RVA: 0x0006B762 File Offset: 0x00069962
	public void OnSpawn(GameObject inst)
	{
	}
}
