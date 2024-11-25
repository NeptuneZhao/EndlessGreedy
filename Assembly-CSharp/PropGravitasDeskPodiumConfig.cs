using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000357 RID: 855
public class PropGravitasDeskPodiumConfig : IEntityConfig
{
	// Token: 0x060011C1 RID: 4545 RVA: 0x0006293C File Offset: 0x00060B3C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x00062944 File Offset: 0x00060B44
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasDeskPodium";
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
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextDeskPodiumEntry));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060011C3 RID: 4547 RVA: 0x000629E9 File Offset: 0x00060BE9
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060011C4 RID: 4548 RVA: 0x00062A00 File Offset: 0x00060C00
	public void OnSpawn(GameObject inst)
	{
	}
}
