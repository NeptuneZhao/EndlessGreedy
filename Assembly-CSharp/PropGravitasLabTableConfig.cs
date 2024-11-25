using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000360 RID: 864
public class PropGravitasLabTableConfig : IEntityConfig
{
	// Token: 0x060011EF RID: 4591 RVA: 0x000630E6 File Offset: 0x000612E6
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060011F0 RID: 4592 RVA: 0x000630F0 File Offset: 0x000612F0
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasLabTable";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASLABTABLE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASLABTABLE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_lab_table_kanim"), "off", Grid.SceneLayer.Building, 3, 2, tier, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x060011F1 RID: 4593 RVA: 0x00063189 File Offset: 0x00061389
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060011F2 RID: 4594 RVA: 0x000631A0 File Offset: 0x000613A0
	public void OnSpawn(GameObject inst)
	{
	}
}
