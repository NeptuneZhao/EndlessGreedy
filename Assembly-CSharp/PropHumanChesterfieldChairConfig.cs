using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A5 RID: 933
public class PropHumanChesterfieldChairConfig : IEntityConfig
{
	// Token: 0x06001366 RID: 4966 RVA: 0x0006B539 File Offset: 0x00069739
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x0006B540 File Offset: 0x00069740
	public GameObject CreatePrefab()
	{
		string id = "PropHumanChesterfieldChair";
		string name = STRINGS.BUILDINGS.PREFABS.PROPHUMANCHESTERFIELDCHAIR.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPHUMANCHESTERFIELDCHAIR.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_chair_kanim"), "off", Grid.SceneLayer.Building, 5, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001368 RID: 4968 RVA: 0x0006B5D3 File Offset: 0x000697D3
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001369 RID: 4969 RVA: 0x0006B5EA File Offset: 0x000697EA
	public void OnSpawn(GameObject inst)
	{
	}
}
