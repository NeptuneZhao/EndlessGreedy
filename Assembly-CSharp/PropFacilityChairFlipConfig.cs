using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000347 RID: 839
public class PropFacilityChairFlipConfig : IEntityConfig
{
	// Token: 0x06001171 RID: 4465 RVA: 0x00061940 File Offset: 0x0005FB40
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001172 RID: 4466 RVA: 0x00061948 File Offset: 0x0005FB48
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityChairFlip";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCHAIR.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCHAIR.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_chairFlip_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06001173 RID: 4467 RVA: 0x000619F4 File Offset: 0x0005FBF4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001174 RID: 4468 RVA: 0x000619F8 File Offset: 0x0005FBF8
	public void OnSpawn(GameObject inst)
	{
		OccupyArea component = inst.GetComponent<OccupyArea>();
		int cell = Grid.PosToCell(inst);
		foreach (CellOffset offset in component.OccupiedCellsOffsets)
		{
			Grid.GravitasFacility[Grid.OffsetCell(cell, offset)] = true;
		}
	}
}
