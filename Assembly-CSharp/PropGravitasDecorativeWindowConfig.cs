using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000356 RID: 854
public class PropGravitasDecorativeWindowConfig : IEntityConfig
{
	// Token: 0x060011BC RID: 4540 RVA: 0x0006287E File Offset: 0x00060A7E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060011BD RID: 4541 RVA: 0x00062888 File Offset: 0x00060A88
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasDecorativeWindow";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDECORATIVEWINDOW.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDECORATIVEWINDOW.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER2;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_top_window_kanim"), "on", Grid.SceneLayer.Building, 2, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Glass, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060011BE RID: 4542 RVA: 0x0006291B File Offset: 0x00060B1B
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060011BF RID: 4543 RVA: 0x00062932 File Offset: 0x00060B32
	public void OnSpawn(GameObject inst)
	{
	}
}
