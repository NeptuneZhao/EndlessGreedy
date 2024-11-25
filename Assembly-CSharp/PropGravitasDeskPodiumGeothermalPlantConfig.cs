using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000358 RID: 856
public class PropGravitasDeskPodiumGeothermalPlantConfig : IEntityConfig
{
	// Token: 0x060011C6 RID: 4550 RVA: 0x00062A0A File Offset: 0x00060C0A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060011C7 RID: 4551 RVA: 0x00062A14 File Offset: 0x00060C14
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasDeskPodiumGeothermalPlant";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDESKPODIUM.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDESKPODIUM.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_desk_podium_kanim"), "off", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new string[]
		{
			"dlc2geoplantinput"
		});
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x00062ABD File Offset: 0x00060CBD
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x00062AD4 File Offset: 0x00060CD4
	public void OnSpawn(GameObject inst)
	{
	}
}
