using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200036F RID: 879
public class PropSurfaceSatellite2Config : IEntityConfig
{
	// Token: 0x06001236 RID: 4662 RVA: 0x00063F8C File Offset: 0x0006218C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001237 RID: 4663 RVA: 0x00063F94 File Offset: 0x00062194
	public GameObject CreatePrefab()
	{
		string id = PropSurfaceSatellite2Config.ID;
		string name = STRINGS.BUILDINGS.PREFABS.PROPSURFACESATELLITE2.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPSURFACESATELLITE2.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("satellite2_kanim"), "off", Grid.SceneLayer.Building, 4, 4, tier, tier2, SimHashes.Creature, new List<Tag>
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
		LoreBearerUtil.AddLoreTo(gameObject);
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001238 RID: 4664 RVA: 0x00064074 File Offset: 0x00062274
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

	// Token: 0x06001239 RID: 4665 RVA: 0x000640EC File Offset: 0x000622EC
	public void OnSpawn(GameObject inst)
	{
		RadiationEmitter component = inst.GetComponent<RadiationEmitter>();
		if (component != null)
		{
			component.SetEmitting(true);
		}
	}

	// Token: 0x04000A8D RID: 2701
	public static string ID = "PropSurfaceSatellite2";
}
