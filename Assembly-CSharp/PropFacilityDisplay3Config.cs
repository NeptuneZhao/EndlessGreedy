using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200034C RID: 844
public class PropFacilityDisplay3Config : IEntityConfig
{
	// Token: 0x0600118A RID: 4490 RVA: 0x00061E84 File Offset: 0x00060084
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600118B RID: 4491 RVA: 0x00061E8C File Offset: 0x0006008C
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityDisplay3";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYDISPLAY3.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYDISPLAY3.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_display3_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("display_prop3", UI.USERMENUACTIONS.READLORE.SEARCH_DISPLAY));
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x0600118C RID: 4492 RVA: 0x00061F52 File Offset: 0x00060152
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600118D RID: 4493 RVA: 0x00061F54 File Offset: 0x00060154
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
