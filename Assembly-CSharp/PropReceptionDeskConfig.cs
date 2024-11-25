using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200036C RID: 876
public class PropReceptionDeskConfig : IEntityConfig
{
	// Token: 0x06001226 RID: 4646 RVA: 0x00063BA0 File Offset: 0x00061DA0
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x00063BA8 File Offset: 0x00061DA8
	public GameObject CreatePrefab()
	{
		string id = "PropReceptionDesk";
		string name = STRINGS.BUILDINGS.PREFABS.PROPRECEPTIONDESK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPRECEPTIONDESK.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_reception_kanim"), "off", Grid.SceneLayer.Building, 5, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("email_pens", UI.USERMENUACTIONS.READLORE.SEARCH_ELLIESDESK));
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06001228 RID: 4648 RVA: 0x00063C6E File Offset: 0x00061E6E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001229 RID: 4649 RVA: 0x00063C70 File Offset: 0x00061E70
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
