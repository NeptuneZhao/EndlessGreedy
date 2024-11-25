using System;
using UnityEngine;

// Token: 0x020002E4 RID: 740
public class MouthAnimation : IEntityConfig
{
	// Token: 0x06000F85 RID: 3973 RVA: 0x00059790 File Offset: 0x00057990
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x00059798 File Offset: 0x00057998
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MouthAnimation.ID, MouthAnimation.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>().AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("anim_mouth_flap_kanim")
		};
		return gameObject;
	}

	// Token: 0x06000F87 RID: 3975 RVA: 0x000597DA File Offset: 0x000579DA
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000F88 RID: 3976 RVA: 0x000597DC File Offset: 0x000579DC
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000984 RID: 2436
	public static string ID = "MouthAnimation";
}
