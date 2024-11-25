using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001D8 RID: 472
public class FossilBitsLargeConfig : IEntityConfig
{
	// Token: 0x060009AC RID: 2476 RVA: 0x00039619 File Offset: 0x00037819
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x00039620 File Offset: 0x00037820
	public GameObject CreatePrefab()
	{
		string id = "FossilBitsLarge";
		string name = CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_BITS.NAME;
		string desc = CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_BITS.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("fossil_bits_kanim"), "object", Grid.SceneLayer.BuildingBack, 2, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Fossil, true);
		component.Temperature = 315f;
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<FossilBits>();
		gameObject.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(gameObject);
		gameObject.AddOrGet<LoopingSounds>();
		return gameObject;
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x000396D5 File Offset: 0x000378D5
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		EntombVulnerable component = inst.GetComponent<EntombVulnerable>();
		component.SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
		component.SetShowStatusItemOnEntombed(false);
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x0003970D File Offset: 0x0003790D
	public void OnSpawn(GameObject inst)
	{
	}
}
