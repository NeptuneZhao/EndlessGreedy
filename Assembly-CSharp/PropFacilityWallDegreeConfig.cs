using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000353 RID: 851
public class PropFacilityWallDegreeConfig : IEntityConfig
{
	// Token: 0x060011AD RID: 4525 RVA: 0x000625E8 File Offset: 0x000607E8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060011AE RID: 4526 RVA: 0x000625F0 File Offset: 0x000607F0
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityWallDegree";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYWALLDEGREE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYWALLDEGREE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_degree_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x060011AF RID: 4527 RVA: 0x0006269C File Offset: 0x0006089C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060011B0 RID: 4528 RVA: 0x000626A0 File Offset: 0x000608A0
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
