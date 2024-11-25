using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000355 RID: 853
public class PropGravitasCreaturePosterConfig : IEntityConfig
{
	// Token: 0x060011B7 RID: 4535 RVA: 0x000627A8 File Offset: 0x000609A8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060011B8 RID: 4536 RVA: 0x000627B0 File Offset: 0x000609B0
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasCreaturePoster";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCREATUREPOSTER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCREATUREPOSTER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_poster_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("storytrait_crittermanipulator_workiversary", UI.USERMENUACTIONS.READLORE.SEARCH_PROPGRAVITASCREATUREPOSTER));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060011B9 RID: 4537 RVA: 0x0006285D File Offset: 0x00060A5D
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060011BA RID: 4538 RVA: 0x00062874 File Offset: 0x00060A74
	public void OnSpawn(GameObject inst)
	{
	}
}
