using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000348 RID: 840
public class PropFacilityChandelierConfig : IEntityConfig
{
	// Token: 0x06001176 RID: 4470 RVA: 0x00061A44 File Offset: 0x0005FC44
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001177 RID: 4471 RVA: 0x00061A4C File Offset: 0x0005FC4C
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityChandelier";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCHANDELIER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCHANDELIER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_chandelier_kanim"), "off", Grid.SceneLayer.Building, 5, 7, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06001178 RID: 4472 RVA: 0x00061AF8 File Offset: 0x0005FCF8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001179 RID: 4473 RVA: 0x00061AFC File Offset: 0x0005FCFC
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
