using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000371 RID: 881
public class PropTableConfig : IEntityConfig
{
	// Token: 0x06001243 RID: 4675 RVA: 0x00064330 File Offset: 0x00062530
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001244 RID: 4676 RVA: 0x00064338 File Offset: 0x00062538
	public GameObject CreatePrefab()
	{
		string id = "PropTable";
		string name = STRINGS.BUILDINGS.PREFABS.PROPTABLE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPTABLE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("table_breakroom_kanim"), "off", Grid.SceneLayer.Building, 3, 1, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001245 RID: 4677 RVA: 0x000643CD File Offset: 0x000625CD
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001246 RID: 4678 RVA: 0x000643E4 File Offset: 0x000625E4
	public void OnSpawn(GameObject inst)
	{
	}
}
