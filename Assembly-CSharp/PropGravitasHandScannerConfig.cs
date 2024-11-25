using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200035D RID: 861
public class PropGravitasHandScannerConfig : IEntityConfig
{
	// Token: 0x060011E0 RID: 4576 RVA: 0x00062E8A File Offset: 0x0006108A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060011E1 RID: 4577 RVA: 0x00062E94 File Offset: 0x00061094
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasHandScanner";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASHANDSCANNER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASHANDSCANNER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_hand_scanner_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060011E2 RID: 4578 RVA: 0x00062F27 File Offset: 0x00061127
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060011E3 RID: 4579 RVA: 0x00062F3E File Offset: 0x0006113E
	public void OnSpawn(GameObject inst)
	{
	}
}
