using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000354 RID: 852
public class PropGravitasCeilingRobotConfig : IEntityConfig
{
	// Token: 0x060011B2 RID: 4530 RVA: 0x000626EC File Offset: 0x000608EC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060011B3 RID: 4531 RVA: 0x000626F4 File Offset: 0x000608F4
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasCeilingRobot";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCEILINGROBOT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCEILINGROBOT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_ceiling_robot_kanim"), "off", Grid.SceneLayer.Building, 2, 4, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x00062787 File Offset: 0x00060987
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060011B5 RID: 4533 RVA: 0x0006279E File Offset: 0x0006099E
	public void OnSpawn(GameObject inst)
	{
	}
}
