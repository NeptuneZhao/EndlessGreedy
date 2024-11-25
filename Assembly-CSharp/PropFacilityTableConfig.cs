using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000352 RID: 850
public class PropFacilityTableConfig : IEntityConfig
{
	// Token: 0x060011A8 RID: 4520 RVA: 0x000624E4 File Offset: 0x000606E4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060011A9 RID: 4521 RVA: 0x000624EC File Offset: 0x000606EC
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityTable";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYTABLE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYTABLE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_table_kanim"), "off", Grid.SceneLayer.Building, 4, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x060011AA RID: 4522 RVA: 0x00062598 File Offset: 0x00060798
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060011AB RID: 4523 RVA: 0x0006259C File Offset: 0x0006079C
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
