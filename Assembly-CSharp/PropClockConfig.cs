using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000340 RID: 832
public class PropClockConfig : IEntityConfig
{
	// Token: 0x0600114E RID: 4430 RVA: 0x000612C4 File Offset: 0x0005F4C4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600114F RID: 4431 RVA: 0x000612CC File Offset: 0x0005F4CC
	public GameObject CreatePrefab()
	{
		string id = "PropClock";
		string name = STRINGS.BUILDINGS.PREFABS.PROPCLOCK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPCLOCK.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("clock_poi_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001150 RID: 4432 RVA: 0x00061378 File Offset: 0x0005F578
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001151 RID: 4433 RVA: 0x0006137A File Offset: 0x0005F57A
	public void OnSpawn(GameObject inst)
	{
	}
}
