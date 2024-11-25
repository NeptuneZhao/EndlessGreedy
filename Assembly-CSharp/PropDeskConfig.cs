using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000342 RID: 834
public class PropDeskConfig : IEntityConfig
{
	// Token: 0x06001158 RID: 4440 RVA: 0x000614D5 File Offset: 0x0005F6D5
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001159 RID: 4441 RVA: 0x000614DC File Offset: 0x0005F6DC
	public GameObject CreatePrefab()
	{
		string id = "PropDesk";
		string name = STRINGS.BUILDINGS.PREFABS.PROPDESK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPDESK.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("setpiece_desk_kanim"), "off", Grid.SceneLayer.Building, 3, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextEmail));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x0600115A RID: 4442 RVA: 0x00061583 File Offset: 0x0005F783
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x0600115B RID: 4443 RVA: 0x0006159A File Offset: 0x0005F79A
	public void OnSpawn(GameObject inst)
	{
	}
}
