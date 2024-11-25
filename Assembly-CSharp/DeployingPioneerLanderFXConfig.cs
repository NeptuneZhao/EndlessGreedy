using System;
using UnityEngine;

// Token: 0x020002C5 RID: 709
public class DeployingPioneerLanderFXConfig : IEntityConfig
{
	// Token: 0x06000ED1 RID: 3793 RVA: 0x000566FD File Offset: 0x000548FD
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x00056704 File Offset: 0x00054904
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("DeployingPioneerLanderFX", "DeployingPioneerLanderFX", false);
		ClusterFXEntity clusterFXEntity = gameObject.AddOrGet<ClusterFXEntity>();
		clusterFXEntity.kAnimName = "pioneer01_kanim";
		clusterFXEntity.animName = "landing";
		clusterFXEntity.animPlayMode = KAnim.PlayMode.Loop;
		return gameObject;
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x00056738 File Offset: 0x00054938
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000ED4 RID: 3796 RVA: 0x0005673A File Offset: 0x0005493A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400091E RID: 2334
	public const string ID = "DeployingPioneerLanderFX";
}
