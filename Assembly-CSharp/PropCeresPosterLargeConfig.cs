using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200033F RID: 831
public class PropCeresPosterLargeConfig : IEntityConfig
{
	// Token: 0x06001149 RID: 4425 RVA: 0x00061204 File Offset: 0x0005F404
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x0600114A RID: 4426 RVA: 0x0006120C File Offset: 0x0005F40C
	public GameObject CreatePrefab()
	{
		string id = "PropCeresPosterLarge";
		string name = STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERLARGE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERLARGE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poster_ceres_7x5_kanim"), "art_7x5", Grid.SceneLayer.Building, 5, 7, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x0600114B RID: 4427 RVA: 0x000612B8 File Offset: 0x0005F4B8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600114C RID: 4428 RVA: 0x000612BA File Offset: 0x0005F4BA
	public void OnSpawn(GameObject inst)
	{
	}
}
