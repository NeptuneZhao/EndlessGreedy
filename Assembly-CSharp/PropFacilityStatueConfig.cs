﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000351 RID: 849
public class PropFacilityStatueConfig : IEntityConfig
{
	// Token: 0x060011A3 RID: 4515 RVA: 0x000623E0 File Offset: 0x000605E0
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060011A4 RID: 4516 RVA: 0x000623E8 File Offset: 0x000605E8
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityStatue";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYSTATUE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYSTATUE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_statue_kanim"), "off", Grid.SceneLayer.Building, 5, 9, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x060011A5 RID: 4517 RVA: 0x00062495 File Offset: 0x00060695
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060011A6 RID: 4518 RVA: 0x00062498 File Offset: 0x00060698
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
