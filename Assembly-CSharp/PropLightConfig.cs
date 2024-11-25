using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200036B RID: 875
public class PropLightConfig : IEntityConfig
{
	// Token: 0x06001221 RID: 4641 RVA: 0x00063AE1 File Offset: 0x00061CE1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001222 RID: 4642 RVA: 0x00063AE8 File Offset: 0x00061CE8
	public GameObject CreatePrefab()
	{
		string id = "PropLight";
		string name = STRINGS.BUILDINGS.PREFABS.PROPLIGHT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPLIGHT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("setpiece_light_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001223 RID: 4643 RVA: 0x00063B94 File Offset: 0x00061D94
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001224 RID: 4644 RVA: 0x00063B96 File Offset: 0x00061D96
	public void OnSpawn(GameObject inst)
	{
	}
}
