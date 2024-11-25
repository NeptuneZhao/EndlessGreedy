using System;
using UnityEngine;

// Token: 0x020002A8 RID: 680
public class BackgroundEarthConfig : IEntityConfig
{
	// Token: 0x06000E0B RID: 3595 RVA: 0x0005155A File Offset: 0x0004F75A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x00051564 File Offset: 0x0004F764
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(BackgroundEarthConfig.ID, BackgroundEarthConfig.ID, true);
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("earth_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "idle";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		gameObject.AddOrGet<LoopingSounds>();
		return gameObject;
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x000515CF File Offset: 0x0004F7CF
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x000515D1 File Offset: 0x0004F7D1
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x040008D7 RID: 2263
	public static string ID = "BackgroundEarth";
}
