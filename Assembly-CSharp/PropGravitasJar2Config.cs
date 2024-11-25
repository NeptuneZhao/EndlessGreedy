using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200035F RID: 863
public class PropGravitasJar2Config : IEntityConfig
{
	// Token: 0x060011EA RID: 4586 RVA: 0x00063016 File Offset: 0x00061216
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060011EB RID: 4587 RVA: 0x00063020 File Offset: 0x00061220
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasJar2";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASJAR2.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASJAR2.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_jar2_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x060011EC RID: 4588 RVA: 0x000630C5 File Offset: 0x000612C5
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060011ED RID: 4589 RVA: 0x000630DC File Offset: 0x000612DC
	public void OnSpawn(GameObject inst)
	{
	}
}
