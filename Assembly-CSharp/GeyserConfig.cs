using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200016C RID: 364
public class GeyserConfig : IEntityConfig
{
	// Token: 0x06000724 RID: 1828 RVA: 0x0002F75F File Offset: 0x0002D95F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x0002F768 File Offset: 0x0002D968
	public GameObject CreatePrefab()
	{
		string id = "Geyser";
		string name = STRINGS.CREATURES.SPECIES.GEYSER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.GEYSER.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("geyser_side_steam_kanim"), "inactive", Grid.SceneLayer.BuildingBack, 4, 2, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.IgneousRock, true);
		component.Temperature = 372.15f;
		gameObject.AddOrGet<Geyser>().outputOffset = new Vector2I(0, 1);
		gameObject.AddOrGet<UserNameable>();
		GeyserConfigurator geyserConfigurator = gameObject.AddOrGet<GeyserConfigurator>();
		geyserConfigurator.presetType = "steam";
		geyserConfigurator.presetMin = 0.5f;
		geyserConfigurator.presetMax = 0.75f;
		Studyable studyable = gameObject.AddOrGet<Studyable>();
		studyable.meterTrackerSymbol = "geotracker_target";
		studyable.meterAnim = "tracker";
		gameObject.AddOrGet<LoopingSounds>();
		SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
		SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_erupt_LP", NOISE_POLLUTION.NOISY.TIER6);
		return gameObject;
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x0002F88E File Offset: 0x0002DA8E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x0002F890 File Offset: 0x0002DA90
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400050A RID: 1290
	public const int GEOTUNERS_REQUIRED_FOR_MAJOR_TRACKER_ANIMATION = 5;

	// Token: 0x020010B6 RID: 4278
	public enum TrackerMeterAnimNames
	{
		// Token: 0x04005D87 RID: 23943
		tracker,
		// Token: 0x04005D88 RID: 23944
		geotracker,
		// Token: 0x04005D89 RID: 23945
		geotracker_minor,
		// Token: 0x04005D8A RID: 23946
		geotracker_major
	}
}
