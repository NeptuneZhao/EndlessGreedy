using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200033E RID: 830
public class PropCeresPosterB : IEntityConfig
{
	// Token: 0x06001144 RID: 4420 RVA: 0x00061144 File Offset: 0x0005F344
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06001145 RID: 4421 RVA: 0x0006114C File Offset: 0x0005F34C
	public GameObject CreatePrefab()
	{
		string id = "PropCeresPosterB";
		string name = STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERB.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERB.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poster_ceres_b_kanim"), "art_b", Grid.SceneLayer.Building, 2, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06001146 RID: 4422 RVA: 0x000611F8 File Offset: 0x0005F3F8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001147 RID: 4423 RVA: 0x000611FA File Offset: 0x0005F3FA
	public void OnSpawn(GameObject inst)
	{
	}
}
