using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200036D RID: 877
public class PropSkeletonConfig : IEntityConfig
{
	// Token: 0x0600122B RID: 4651 RVA: 0x00063CBC File Offset: 0x00061EBC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600122C RID: 4652 RVA: 0x00063CC4 File Offset: 0x00061EC4
	public GameObject CreatePrefab()
	{
		string id = "PropSkeleton";
		string name = STRINGS.BUILDINGS.PREFABS.PROPSKELETON.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPSKELETON.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER5;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("skeleton_poi_kanim"), "off", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Creature, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x0600122D RID: 4653 RVA: 0x00063D57 File Offset: 0x00061F57
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x0600122E RID: 4654 RVA: 0x00063D6E File Offset: 0x00061F6E
	public void OnSpawn(GameObject inst)
	{
	}
}
