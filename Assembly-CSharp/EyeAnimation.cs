using System;
using UnityEngine;

// Token: 0x020002CC RID: 716
public class EyeAnimation : IEntityConfig
{
	// Token: 0x06000EF3 RID: 3827 RVA: 0x00056D3B File Offset: 0x00054F3B
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000EF4 RID: 3828 RVA: 0x00056D44 File Offset: 0x00054F44
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(EyeAnimation.ID, EyeAnimation.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>().AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("anim_blinks_kanim")
		};
		return gameObject;
	}

	// Token: 0x06000EF5 RID: 3829 RVA: 0x00056D86 File Offset: 0x00054F86
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000EF6 RID: 3830 RVA: 0x00056D88 File Offset: 0x00054F88
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400092F RID: 2351
	public static string ID = "EyeAnimation";
}
