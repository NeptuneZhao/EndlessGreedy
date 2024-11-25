using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001E1 RID: 481
public class FossilSiteConfig_Rock : IEntityConfig
{
	// Token: 0x060009DA RID: 2522 RVA: 0x0003A44C File Offset: 0x0003864C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060009DB RID: 2523 RVA: 0x0003A454 File Offset: 0x00038654
	public GameObject CreatePrefab()
	{
		string id = "FossilRock";
		string name = CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_ROCK.NAME;
		string desc = CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_ROCK.DESC;
		float mass = 4000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER4;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("fossil_rock_kanim"), "object", Grid.SceneLayer.BuildingBack, 2, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Fossil, true);
		component.Temperature = 315f;
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<Demolishable>().allowDemolition = false;
		gameObject.AddOrGetDef<MinorFossilDigSite.Def>().fossilQuestCriteriaID = FossilSiteConfig_Rock.FossilQuestCriteriaID;
		gameObject.AddOrGetDef<FossilHuntInitializer.Def>();
		gameObject.AddOrGet<MinorDigSiteWorkable>();
		gameObject.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(gameObject);
		gameObject.AddOrGet<LoopingSounds>();
		return gameObject;
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x0003A52C File Offset: 0x0003872C
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<EntombVulnerable>().SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x0003A55D File Offset: 0x0003875D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400067B RID: 1659
	public static readonly HashedString FossilQuestCriteriaID = "LostRockFossil";

	// Token: 0x0400067C RID: 1660
	public const string ID = "FossilRock";
}
