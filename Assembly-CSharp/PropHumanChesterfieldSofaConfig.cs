using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A6 RID: 934
public class PropHumanChesterfieldSofaConfig : IEntityConfig
{
	// Token: 0x0600136B RID: 4971 RVA: 0x0006B5F4 File Offset: 0x000697F4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x0600136C RID: 4972 RVA: 0x0006B5FC File Offset: 0x000697FC
	public GameObject CreatePrefab()
	{
		string id = "PropHumanChesterfieldSofa";
		string name = STRINGS.BUILDINGS.PREFABS.PROPHUMANCHESTERFIELDSOFA.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPHUMANCHESTERFIELDSOFA.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_couch_kanim"), "off", Grid.SceneLayer.Building, 3, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x0600136D RID: 4973 RVA: 0x0006B68F File Offset: 0x0006988F
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x0600136E RID: 4974 RVA: 0x0006B6A6 File Offset: 0x000698A6
	public void OnSpawn(GameObject inst)
	{
	}
}
