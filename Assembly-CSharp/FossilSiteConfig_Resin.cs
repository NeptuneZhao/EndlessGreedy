using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001E0 RID: 480
public class FossilSiteConfig_Resin : IEntityConfig
{
	// Token: 0x060009D4 RID: 2516 RVA: 0x0003A320 File Offset: 0x00038520
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x0003A328 File Offset: 0x00038528
	public GameObject CreatePrefab()
	{
		string id = "FossilResin";
		string name = CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_RESIN.NAME;
		string desc = CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_RESIN.DESC;
		float mass = 4000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER4;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("fossil_resin_kanim"), "object", Grid.SceneLayer.BuildingBack, 2, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Fossil, true);
		component.Temperature = 315f;
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<Demolishable>().allowDemolition = false;
		gameObject.AddOrGetDef<MinorFossilDigSite.Def>().fossilQuestCriteriaID = FossilSiteConfig_Resin.FossilQuestCriteriaID;
		gameObject.AddOrGetDef<FossilHuntInitializer.Def>();
		gameObject.AddOrGet<MinorDigSiteWorkable>();
		gameObject.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(gameObject);
		gameObject.AddOrGet<LoopingSounds>();
		return gameObject;
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x0003A400 File Offset: 0x00038600
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<EntombVulnerable>().SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x0003A431 File Offset: 0x00038631
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000679 RID: 1657
	public static readonly HashedString FossilQuestCriteriaID = "LostResinFossil";

	// Token: 0x0400067A RID: 1658
	public const string ID = "FossilResin";
}
