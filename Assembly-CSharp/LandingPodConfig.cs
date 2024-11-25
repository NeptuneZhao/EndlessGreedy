using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000239 RID: 569
public class LandingPodConfig : IEntityConfig
{
	// Token: 0x06000BC6 RID: 3014 RVA: 0x00045162 File Offset: 0x00043362
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x0004516C File Offset: 0x0004336C
	public GameObject CreatePrefab()
	{
		string id = "LandingPod";
		string name = STRINGS.BUILDINGS.PREFABS.LANDING_POD.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.LANDING_POD.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("rocket_puft_pod_kanim"), "grounded", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<PodLander>();
		gameObject.AddOrGet<MinionStorage>();
		return gameObject;
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x000451DB File Offset: 0x000433DB
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x000451F2 File Offset: 0x000433F2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007B8 RID: 1976
	public const string ID = "LandingPod";
}
