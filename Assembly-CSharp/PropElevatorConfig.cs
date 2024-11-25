using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000345 RID: 837
public class PropElevatorConfig : IEntityConfig
{
	// Token: 0x06001167 RID: 4455 RVA: 0x00061736 File Offset: 0x0005F936
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001168 RID: 4456 RVA: 0x00061740 File Offset: 0x0005F940
	public GameObject CreatePrefab()
	{
		string id = "PropElevator";
		string name = STRINGS.BUILDINGS.PREFABS.PROPELEVATOR.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPELEVATOR.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_elevator_kanim"), "off", Grid.SceneLayer.Building, 2, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06001169 RID: 4457 RVA: 0x000617EC File Offset: 0x0005F9EC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600116A RID: 4458 RVA: 0x000617F0 File Offset: 0x0005F9F0
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
