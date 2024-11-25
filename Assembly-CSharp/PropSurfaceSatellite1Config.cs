using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200036E RID: 878
public class PropSurfaceSatellite1Config : IEntityConfig
{
	// Token: 0x06001230 RID: 4656 RVA: 0x00063D78 File Offset: 0x00061F78
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001231 RID: 4657 RVA: 0x00063D80 File Offset: 0x00061F80
	public GameObject CreatePrefab()
	{
		string id = "PropSurfaceSatellite1";
		string name = STRINGS.BUILDINGS.PREFABS.PROPSURFACESATELLITE1.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPSURFACESATELLITE1.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("satellite1_kanim"), "off", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		SetLocker setLocker = gameObject.AddOrGet<SetLocker>();
		setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
		setLocker.dropOffset = new Vector2I(0, 1);
		setLocker.numDataBanks = new int[]
		{
			4,
			9
		};
		gameObject.AddOrGet<Demolishable>();
		LoreBearerUtil.AddLoreTo(gameObject);
		return gameObject;
	}

	// Token: 0x06001232 RID: 4658 RVA: 0x00063E60 File Offset: 0x00062060
	public static string[][] GetLockerBaseContents()
	{
		string text = DlcManager.FeatureClusterSpaceEnabled() ? "OrbitalResearchDatabank" : "ResearchDatabank";
		return new string[][]
		{
			new string[]
			{
				text,
				text,
				text
			},
			new string[]
			{
				"ColdBreatherSeed",
				"ColdBreatherSeed",
				"ColdBreatherSeed"
			},
			new string[]
			{
				"Atmo_Suit",
				"Glom",
				"Glom",
				"Glom"
			}
		};
	}

	// Token: 0x06001233 RID: 4659 RVA: 0x00063EE8 File Offset: 0x000620E8
	public void OnPrefabInit(GameObject inst)
	{
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = PropSurfaceSatellite1Config.GetLockerBaseContents();
		component.ChooseContents();
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		RadiationEmitter radiationEmitter = inst.AddOrGet<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emitRadiusX = 12;
		radiationEmitter.emitRadiusY = 12;
		radiationEmitter.emitRads = 2400f / ((float)radiationEmitter.emitRadiusX / 6f);
	}

	// Token: 0x06001234 RID: 4660 RVA: 0x00063F60 File Offset: 0x00062160
	public void OnSpawn(GameObject inst)
	{
		RadiationEmitter component = inst.GetComponent<RadiationEmitter>();
		if (component != null)
		{
			component.SetEmitting(true);
		}
	}

	// Token: 0x04000A8C RID: 2700
	public const string ID = "PropSurfaceSatellite1";
}
