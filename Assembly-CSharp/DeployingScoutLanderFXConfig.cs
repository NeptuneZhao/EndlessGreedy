using System;
using UnityEngine;

// Token: 0x020002C6 RID: 710
public class DeployingScoutLanderFXConfig : IEntityConfig
{
	// Token: 0x06000ED6 RID: 3798 RVA: 0x00056744 File Offset: 0x00054944
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x0005674B File Offset: 0x0005494B
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("DeployingScoutLanderFXConfig", "DeployingScoutLanderFXConfig", false);
		ClusterFXEntity clusterFXEntity = gameObject.AddOrGet<ClusterFXEntity>();
		clusterFXEntity.kAnimName = "rover01_kanim";
		clusterFXEntity.animName = "landing";
		clusterFXEntity.animPlayMode = KAnim.PlayMode.Loop;
		return gameObject;
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x0005677F File Offset: 0x0005497F
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x00056781 File Offset: 0x00054981
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400091F RID: 2335
	public const string ID = "DeployingScoutLanderFXConfig";
}
