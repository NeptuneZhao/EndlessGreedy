﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200034E RID: 846
public class PropFacilityGlobeDroorsConfig : IEntityConfig
{
	// Token: 0x06001194 RID: 4500 RVA: 0x000620BC File Offset: 0x000602BC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001195 RID: 4501 RVA: 0x000620C4 File Offset: 0x000602C4
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityGlobeDroors";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYGLOBEDROORS.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYGLOBEDROORS.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_globe_kanim"), "off", Grid.SceneLayer.Building, 1, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("journal_newspaper", UI.USERMENUACTIONS.READLORE.SEARCH_CABINET));
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06001196 RID: 4502 RVA: 0x0006218A File Offset: 0x0006038A
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001197 RID: 4503 RVA: 0x0006218C File Offset: 0x0006038C
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
