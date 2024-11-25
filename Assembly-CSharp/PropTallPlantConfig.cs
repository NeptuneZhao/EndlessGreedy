using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000372 RID: 882
public class PropTallPlantConfig : IEntityConfig
{
	// Token: 0x06001248 RID: 4680 RVA: 0x000643EE File Offset: 0x000625EE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001249 RID: 4681 RVA: 0x000643F8 File Offset: 0x000625F8
	public GameObject CreatePrefab()
	{
		string id = "PropTallPlant";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYTALLPLANT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYTALLPLANT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_tall_plant_kanim"), "off", Grid.SceneLayer.Building, 1, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Polypropylene, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x0600124A RID: 4682 RVA: 0x0006448D File Offset: 0x0006268D
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x0600124B RID: 4683 RVA: 0x000644A4 File Offset: 0x000626A4
	public void OnSpawn(GameObject inst)
	{
	}
}
