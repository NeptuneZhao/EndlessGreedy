﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000350 RID: 848
public class PropFacilityPaintingConfig : IEntityConfig
{
	// Token: 0x0600119E RID: 4510 RVA: 0x000622DC File Offset: 0x000604DC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600119F RID: 4511 RVA: 0x000622E4 File Offset: 0x000604E4
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityPainting";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYPAINTING.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYPAINTING.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_painting_kanim"), "off", Grid.SceneLayer.Building, 3, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x060011A0 RID: 4512 RVA: 0x00062390 File Offset: 0x00060590
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060011A1 RID: 4513 RVA: 0x00062394 File Offset: 0x00060594
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
