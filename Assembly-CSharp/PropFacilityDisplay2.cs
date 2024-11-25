using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200034B RID: 843
public class PropFacilityDisplay2 : IEntityConfig
{
	// Token: 0x06001185 RID: 4485 RVA: 0x00061D68 File Offset: 0x0005FF68
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001186 RID: 4486 RVA: 0x00061D70 File Offset: 0x0005FF70
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityDisplay2";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYDISPLAY2.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYDISPLAY2.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_display2_kanim"), "off", Grid.SceneLayer.Building, 2, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("display_prop2", UI.USERMENUACTIONS.READLORE.SEARCH_DISPLAY));
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06001187 RID: 4487 RVA: 0x00061E36 File Offset: 0x00060036
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001188 RID: 4488 RVA: 0x00061E38 File Offset: 0x00060038
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
