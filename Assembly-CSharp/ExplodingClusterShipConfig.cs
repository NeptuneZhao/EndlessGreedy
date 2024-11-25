using System;
using UnityEngine;

// Token: 0x020002CB RID: 715
public class ExplodingClusterShipConfig : IEntityConfig
{
	// Token: 0x06000EEE RID: 3822 RVA: 0x00056CFB File Offset: 0x00054EFB
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000EEF RID: 3823 RVA: 0x00056D02 File Offset: 0x00054F02
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("ExplodingClusterShip", "ExplodingClusterShip", false);
		ClusterFXEntity clusterFXEntity = gameObject.AddOrGet<ClusterFXEntity>();
		clusterFXEntity.kAnimName = "rocket_self_destruct_kanim";
		clusterFXEntity.animName = "explode";
		return gameObject;
	}

	// Token: 0x06000EF0 RID: 3824 RVA: 0x00056D2F File Offset: 0x00054F2F
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000EF1 RID: 3825 RVA: 0x00056D31 File Offset: 0x00054F31
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400092E RID: 2350
	public const string ID = "ExplodingClusterShip";
}
