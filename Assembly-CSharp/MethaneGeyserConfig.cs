using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000175 RID: 373
public class MethaneGeyserConfig : IEntityConfig
{
	// Token: 0x06000753 RID: 1875 RVA: 0x00030DB1 File Offset: 0x0002EFB1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x00030DB8 File Offset: 0x0002EFB8
	public GameObject CreatePrefab()
	{
		string id = "MethaneGeyser";
		string name = STRINGS.CREATURES.SPECIES.METHANEGEYSER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.METHANEGEYSER.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("geyser_side_methane_kanim"), "inactive", Grid.SceneLayer.BuildingBack, 4, 2, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.IgneousRock, true);
		component.Temperature = 372.15f;
		gameObject.AddOrGet<Geyser>().outputOffset = new Vector2I(0, 1);
		GeyserConfigurator geyserConfigurator = gameObject.AddOrGet<GeyserConfigurator>();
		geyserConfigurator.presetType = "methane";
		geyserConfigurator.presetMin = 0.35f;
		geyserConfigurator.presetMax = 0.65f;
		Studyable studyable = gameObject.AddOrGet<Studyable>();
		studyable.meterTrackerSymbol = "geotracker_target";
		studyable.meterAnim = "tracker";
		gameObject.AddOrGet<LoopingSounds>();
		SoundEventVolumeCache.instance.AddVolume("geyser_side_methane_kanim", "GeyserMethane_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
		SoundEventVolumeCache.instance.AddVolume("geyser_side_methane_kanim", "GeyserMethane_erupt_LP", NOISE_POLLUTION.NOISY.TIER6);
		return gameObject;
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x00030ED7 File Offset: 0x0002F0D7
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x00030ED9 File Offset: 0x0002F0D9
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000539 RID: 1337
	public const string ID = "MethaneGeyser";
}
