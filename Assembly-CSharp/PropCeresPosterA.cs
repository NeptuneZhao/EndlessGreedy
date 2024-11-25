using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200033D RID: 829
public class PropCeresPosterA : IEntityConfig
{
	// Token: 0x0600113F RID: 4415 RVA: 0x00061083 File Offset: 0x0005F283
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06001140 RID: 4416 RVA: 0x0006108C File Offset: 0x0005F28C
	public GameObject CreatePrefab()
	{
		string id = "PropCeresPosterA";
		string name = STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERA.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERA.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poster_ceres_a_kanim"), "art_a", Grid.SceneLayer.Building, 2, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06001141 RID: 4417 RVA: 0x00061138 File Offset: 0x0005F338
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001142 RID: 4418 RVA: 0x0006113A File Offset: 0x0005F33A
	public void OnSpawn(GameObject inst)
	{
	}
}
