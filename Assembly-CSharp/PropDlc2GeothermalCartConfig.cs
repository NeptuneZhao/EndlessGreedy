using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000344 RID: 836
public class PropDlc2GeothermalCartConfig : IEntityConfig
{
	// Token: 0x06001162 RID: 4450 RVA: 0x00061672 File Offset: 0x0005F872
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06001163 RID: 4451 RVA: 0x0006167C File Offset: 0x0005F87C
	public GameObject CreatePrefab()
	{
		string id = "PropDlc2GeothermalCart";
		string name = STRINGS.BUILDINGS.PREFABS.PROPDLC2GEOTHERMALCART.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPDLC2GEOTHERMALCART.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_geothermal_cart_kanim"), "on", Grid.SceneLayer.Building, 2, 3, tier, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06001164 RID: 4452 RVA: 0x00061715 File Offset: 0x0005F915
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001165 RID: 4453 RVA: 0x0006172C File Offset: 0x0005F92C
	public void OnSpawn(GameObject inst)
	{
	}
}
