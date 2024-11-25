using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000349 RID: 841
public class PropFacilityCouchConfig : IEntityConfig
{
	// Token: 0x0600117B RID: 4475 RVA: 0x00061B48 File Offset: 0x0005FD48
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600117C RID: 4476 RVA: 0x00061B50 File Offset: 0x0005FD50
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityCouch";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCOUCH.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCOUCH.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_couch_kanim"), "off", Grid.SceneLayer.Building, 4, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x0600117D RID: 4477 RVA: 0x00061BFC File Offset: 0x0005FDFC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600117E RID: 4478 RVA: 0x00061C00 File Offset: 0x0005FE00
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
