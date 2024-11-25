using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000359 RID: 857
public class PropGravitasDisplay4Config : IEntityConfig
{
	// Token: 0x060011CB RID: 4555 RVA: 0x00062ADE File Offset: 0x00060CDE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060011CC RID: 4556 RVA: 0x00062AE8 File Offset: 0x00060CE8
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasDisplay4";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDISPLAY4.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDISPLAY4.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_display4_kanim"), "off", Grid.SceneLayer.Building, 1, 3, tier, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x060011CD RID: 4557 RVA: 0x00062B8D File Offset: 0x00060D8D
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x00062BA4 File Offset: 0x00060DA4
	public void OnSpawn(GameObject inst)
	{
	}
}
