using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200035C RID: 860
public class PropGravitasFloorRobotConfig : IEntityConfig
{
	// Token: 0x060011DB RID: 4571 RVA: 0x00062DC8 File Offset: 0x00060FC8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060011DC RID: 4572 RVA: 0x00062DD0 File Offset: 0x00060FD0
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasFloorRobot";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFLOORROBOT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFLOORROBOT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_floor_robot_kanim"), "off", Grid.SceneLayer.Building, 2, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject);
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060011DD RID: 4573 RVA: 0x00062E69 File Offset: 0x00061069
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060011DE RID: 4574 RVA: 0x00062E80 File Offset: 0x00061080
	public void OnSpawn(GameObject inst)
	{
	}
}
