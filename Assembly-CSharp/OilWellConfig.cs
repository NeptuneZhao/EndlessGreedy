using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class OilWellConfig : IEntityConfig
{
	// Token: 0x06000762 RID: 1890 RVA: 0x0003117E File Offset: 0x0002F37E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x00031188 File Offset: 0x0002F388
	public GameObject CreatePrefab()
	{
		string id = "OilWell";
		string name = STRINGS.CREATURES.SPECIES.OIL_WELL.NAME;
		string desc = STRINGS.CREATURES.SPECIES.OIL_WELL.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("geyser_side_oil_kanim"), "off", Grid.SceneLayer.BuildingBack, 4, 2, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.SedimentaryRock, true);
		component.Temperature = 372.15f;
		gameObject.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 0), GameTags.OilWell, null)
		};
		SoundEventVolumeCache.instance.AddVolume("geyser_side_methane_kanim", "GeyserMethane_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
		SoundEventVolumeCache.instance.AddVolume("geyser_side_methane_kanim", "GeyserMethane_erupt_LP", NOISE_POLLUTION.NOISY.TIER6);
		return gameObject;
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x00031278 File Offset: 0x0002F478
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x0003127A File Offset: 0x0002F47A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400053E RID: 1342
	public const string ID = "OilWell";
}
