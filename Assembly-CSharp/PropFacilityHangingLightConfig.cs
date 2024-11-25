﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200034F RID: 847
public class PropFacilityHangingLightConfig : IEntityConfig
{
	// Token: 0x06001199 RID: 4505 RVA: 0x000621D8 File Offset: 0x000603D8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600119A RID: 4506 RVA: 0x000621E0 File Offset: 0x000603E0
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityHangingLight";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYLAMP.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYLAMP.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_light_kanim"), "off", Grid.SceneLayer.Building, 1, 4, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x0600119B RID: 4507 RVA: 0x0006228C File Offset: 0x0006048C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600119C RID: 4508 RVA: 0x00062290 File Offset: 0x00060490
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
