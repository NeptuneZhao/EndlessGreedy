using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A2 RID: 930
public class PropExoShelfLongConfig : IEntityConfig
{
	// Token: 0x06001357 RID: 4951 RVA: 0x0006B1DA File Offset: 0x000693DA
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x0006B1E4 File Offset: 0x000693E4
	public GameObject CreatePrefab()
	{
		string id = "PropExoShelfLong";
		string name = STRINGS.BUILDINGS.PREFABS.PROPEXOSHELFLONG.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPEXOSHELFLONG.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_shelf_long_kanim"), "off", Grid.SceneLayer.Building, 3, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x0006B277 File Offset: 0x00069477
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x0006B28E File Offset: 0x0006948E
	public void OnSpawn(GameObject inst)
	{
	}
}
