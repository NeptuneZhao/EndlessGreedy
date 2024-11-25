using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000365 RID: 869
public class PropGravitasShelfConfig : IEntityConfig
{
	// Token: 0x06001206 RID: 4614 RVA: 0x0006358A File Offset: 0x0006178A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06001207 RID: 4615 RVA: 0x00063594 File Offset: 0x00061794
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasShelf";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASSHELF.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASSHELF.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_shelf_kanim"), "off", Grid.SceneLayer.Building, 2, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001208 RID: 4616 RVA: 0x00063627 File Offset: 0x00061827
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001209 RID: 4617 RVA: 0x0006363E File Offset: 0x0006183E
	public void OnSpawn(GameObject inst)
	{
	}
}
