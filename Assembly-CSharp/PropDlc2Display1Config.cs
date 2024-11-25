using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000343 RID: 835
public class PropDlc2Display1Config : IEntityConfig
{
	// Token: 0x0600115D RID: 4445 RVA: 0x000615A4 File Offset: 0x0005F7A4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x0600115E RID: 4446 RVA: 0x000615AC File Offset: 0x0005F7AC
	public GameObject CreatePrefab()
	{
		string id = "PropDlc2Display1";
		string name = STRINGS.BUILDINGS.PREFABS.PROPDLC2DISPLAY1.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPDLC2DISPLAY1.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_display_showroom_kanim"), "off", Grid.SceneLayer.Building, 1, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextEmail));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x0600115F RID: 4447 RVA: 0x00061651 File Offset: 0x0005F851
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001160 RID: 4448 RVA: 0x00061668 File Offset: 0x0005F868
	public void OnSpawn(GameObject inst)
	{
	}
}
