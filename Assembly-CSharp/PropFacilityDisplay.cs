using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200034D RID: 845
public class PropFacilityDisplay : IEntityConfig
{
	// Token: 0x0600118F RID: 4495 RVA: 0x00061FA0 File Offset: 0x000601A0
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001190 RID: 4496 RVA: 0x00061FA8 File Offset: 0x000601A8
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityDisplay";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYDISPLAY1.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYDISPLAY1.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_display1_kanim"), "off", Grid.SceneLayer.Building, 2, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("display_prop1", UI.USERMENUACTIONS.READLORE.SEARCH_DISPLAY));
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06001191 RID: 4497 RVA: 0x0006206E File Offset: 0x0006026E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001192 RID: 4498 RVA: 0x00062070 File Offset: 0x00060270
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
