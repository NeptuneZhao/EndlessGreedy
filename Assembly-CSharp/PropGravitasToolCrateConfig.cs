using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000366 RID: 870
public class PropGravitasToolCrateConfig : IEntityConfig
{
	// Token: 0x0600120B RID: 4619 RVA: 0x00063648 File Offset: 0x00061848
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600120C RID: 4620 RVA: 0x00063650 File Offset: 0x00061850
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasToolCrate";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASTOOLCRATE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASTOOLCRATE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_1x1_crate_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x0600120D RID: 4621 RVA: 0x000636E3 File Offset: 0x000618E3
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x0600120E RID: 4622 RVA: 0x000636FA File Offset: 0x000618FA
	public void OnSpawn(GameObject inst)
	{
	}
}
