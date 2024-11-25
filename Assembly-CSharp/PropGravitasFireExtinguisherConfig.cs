using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200035A RID: 858
public class PropGravitasFireExtinguisherConfig : IEntityConfig
{
	// Token: 0x060011D0 RID: 4560 RVA: 0x00062BAE File Offset: 0x00060DAE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060011D1 RID: 4561 RVA: 0x00062BB8 File Offset: 0x00060DB8
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasFireExtinguisher";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIREEXTINGUISHER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIREEXTINGUISHER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_fireextinguisher_kanim"), "off", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060011D2 RID: 4562 RVA: 0x00062C4B File Offset: 0x00060E4B
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060011D3 RID: 4563 RVA: 0x00062C62 File Offset: 0x00060E62
	public void OnSpawn(GameObject inst)
	{
	}
}
