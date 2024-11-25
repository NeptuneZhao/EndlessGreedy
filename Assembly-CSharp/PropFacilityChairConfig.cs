using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000346 RID: 838
public class PropFacilityChairConfig : IEntityConfig
{
	// Token: 0x0600116C RID: 4460 RVA: 0x0006183C File Offset: 0x0005FA3C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600116D RID: 4461 RVA: 0x00061844 File Offset: 0x0005FA44
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityChair";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCHAIR.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCHAIR.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_chair_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x0600116E RID: 4462 RVA: 0x000618F0 File Offset: 0x0005FAF0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600116F RID: 4463 RVA: 0x000618F4 File Offset: 0x0005FAF4
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
