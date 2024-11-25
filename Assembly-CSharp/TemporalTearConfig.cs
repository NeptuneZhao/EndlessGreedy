using System;
using UnityEngine;

// Token: 0x020002EE RID: 750
public class TemporalTearConfig : IEntityConfig
{
	// Token: 0x06000FBC RID: 4028 RVA: 0x00059C25 File Offset: 0x00057E25
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000FBD RID: 4029 RVA: 0x00059C2C File Offset: 0x00057E2C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("TemporalTear", "TemporalTear", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<TemporalTear>();
		return gameObject;
	}

	// Token: 0x06000FBE RID: 4030 RVA: 0x00059C4C File Offset: 0x00057E4C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000FBF RID: 4031 RVA: 0x00059C4E File Offset: 0x00057E4E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400098F RID: 2447
	public const string ID = "TemporalTear";
}
