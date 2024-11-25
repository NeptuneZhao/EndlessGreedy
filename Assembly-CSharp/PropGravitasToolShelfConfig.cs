using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000367 RID: 871
public class PropGravitasToolShelfConfig : IEntityConfig
{
	// Token: 0x06001210 RID: 4624 RVA: 0x00063704 File Offset: 0x00061904
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001211 RID: 4625 RVA: 0x0006370C File Offset: 0x0006190C
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasToolShelf";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASTOOLSHELF.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASTOOLSHELF.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_toolshelf_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001212 RID: 4626 RVA: 0x0006379F File Offset: 0x0006199F
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001213 RID: 4627 RVA: 0x000637B6 File Offset: 0x000619B6
	public void OnSpawn(GameObject inst)
	{
	}
}
