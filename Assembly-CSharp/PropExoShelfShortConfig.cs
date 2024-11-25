using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A3 RID: 931
public class PropExoShelfShortConfig : IEntityConfig
{
	// Token: 0x0600135C RID: 4956 RVA: 0x0006B298 File Offset: 0x00069498
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x0600135D RID: 4957 RVA: 0x0006B2A0 File Offset: 0x000694A0
	public GameObject CreatePrefab()
	{
		string id = "PropExoShelfShort";
		string name = STRINGS.BUILDINGS.PREFABS.PROPEXOSHELSHORT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPEXOSHELSHORT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_shelf_short_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x0600135E RID: 4958 RVA: 0x0006B333 File Offset: 0x00069533
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x0006B34A File Offset: 0x0006954A
	public void OnSpawn(GameObject inst)
	{
	}
}
