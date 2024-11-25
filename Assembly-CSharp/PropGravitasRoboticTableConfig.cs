using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000364 RID: 868
public class PropGravitasRoboticTableConfig : IEntityConfig
{
	// Token: 0x06001201 RID: 4609 RVA: 0x000634C9 File Offset: 0x000616C9
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06001202 RID: 4610 RVA: 0x000634D0 File Offset: 0x000616D0
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasRobitcTable";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASROBTICTABLE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASROBTICTABLE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_robotic_table_kanim"), "off", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06001203 RID: 4611 RVA: 0x00063569 File Offset: 0x00061769
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001204 RID: 4612 RVA: 0x00063580 File Offset: 0x00061780
	public void OnSpawn(GameObject inst)
	{
	}
}
