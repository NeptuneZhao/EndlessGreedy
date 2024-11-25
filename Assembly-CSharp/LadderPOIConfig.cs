using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000237 RID: 567
public class LadderPOIConfig : IEntityConfig
{
	// Token: 0x06000BB8 RID: 3000 RVA: 0x00044EB1 File Offset: 0x000430B1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x00044EB8 File Offset: 0x000430B8
	public GameObject CreatePrefab()
	{
		int num = 1;
		int num2 = 1;
		string id = "PropLadder";
		string name = STRINGS.BUILDINGS.PREFABS.PROPLADDER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPLADDER.DESC;
		float mass = 50f;
		int width = num;
		int height = num2;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("ladder_poi_kanim"), "off", Grid.SceneLayer.Building, width, height, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Polypropylene, true);
		component.Temperature = 294.15f;
		Ladder ladder = gameObject.AddOrGet<Ladder>();
		ladder.upwardsMovementSpeedMultiplier = 1.5f;
		ladder.downwardsMovementSpeedMultiplier = 1.5f;
		gameObject.AddOrGet<AnimTileable>();
		UnityEngine.Object.DestroyImmediate(gameObject.AddOrGet<OccupyArea>());
		OccupyArea occupyArea = gameObject.AddOrGet<OccupyArea>();
		occupyArea.SetCellOffsets(EntityTemplates.GenerateOffsets(num, num2));
		occupyArea.objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000BBA RID: 3002 RVA: 0x00044FAC File Offset: 0x000431AC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x00044FAE File Offset: 0x000431AE
	public void OnSpawn(GameObject inst)
	{
	}
}
