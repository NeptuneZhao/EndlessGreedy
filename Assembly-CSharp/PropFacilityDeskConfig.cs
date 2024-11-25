using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200034A RID: 842
public class PropFacilityDeskConfig : IEntityConfig
{
	// Token: 0x06001180 RID: 4480 RVA: 0x00061C4C File Offset: 0x0005FE4C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001181 RID: 4481 RVA: 0x00061C54 File Offset: 0x0005FE54
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityDesk";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYDESK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYDESK.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_desk_kanim"), "off", Grid.SceneLayer.Building, 4, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("journal_magazine", UI.USERMENUACTIONS.READLORE.SEARCH_STERNSDESK));
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06001182 RID: 4482 RVA: 0x00061D1A File Offset: 0x0005FF1A
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001183 RID: 4483 RVA: 0x00061D1C File Offset: 0x0005FF1C
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
