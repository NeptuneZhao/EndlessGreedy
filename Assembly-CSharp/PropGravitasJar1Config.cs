using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200035E RID: 862
public class PropGravitasJar1Config : IEntityConfig
{
	// Token: 0x060011E5 RID: 4581 RVA: 0x00062F48 File Offset: 0x00061148
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060011E6 RID: 4582 RVA: 0x00062F50 File Offset: 0x00061150
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasJar1";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASJAR1.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASJAR1.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_jar1_kanim"), "off", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextDimensionalLore));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060011E7 RID: 4583 RVA: 0x00062FF5 File Offset: 0x000611F5
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060011E8 RID: 4584 RVA: 0x0006300C File Offset: 0x0006120C
	public void OnSpawn(GameObject inst)
	{
	}
}
