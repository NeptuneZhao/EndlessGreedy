using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020002EB RID: 747
public class StoredMinionConfig : IEntityConfig
{
	// Token: 0x06000FAC RID: 4012 RVA: 0x00059B4D File Offset: 0x00057D4D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000FAD RID: 4013 RVA: 0x00059B54 File Offset: 0x00057D54
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(StoredMinionConfig.ID, StoredMinionConfig.ID, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<KPrefabID>();
		gameObject.AddOrGet<Traits>();
		gameObject.AddOrGet<Schedulable>();
		gameObject.AddOrGet<StoredMinionIdentity>();
		gameObject.AddOrGet<KSelectable>().IsSelectable = false;
		gameObject.AddOrGet<MinionModifiers>().addBaseTraits = false;
		return gameObject;
	}

	// Token: 0x06000FAE RID: 4014 RVA: 0x00059BAC File Offset: 0x00057DAC
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000FAF RID: 4015 RVA: 0x00059BAE File Offset: 0x00057DAE
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400098B RID: 2443
	public static string ID = "StoredMinion";
}
