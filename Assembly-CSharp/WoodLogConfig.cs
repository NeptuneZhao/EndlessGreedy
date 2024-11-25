using System;
using UnityEngine;

// Token: 0x02000228 RID: 552
public class WoodLogConfig : IOreConfig
{
	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000B6C RID: 2924 RVA: 0x000434B5 File Offset: 0x000416B5
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.WoodLog;
		}
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x000434BC File Offset: 0x000416BC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x000434C4 File Offset: 0x000416C4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.prefabInitFn += this.OnInit;
		component.prefabSpawnFn += this.OnSpawn;
		component.RemoveTag(GameTags.HideFromSpawnTool);
		return gameObject;
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x00043511 File Offset: 0x00041711
	public void OnInit(GameObject inst)
	{
		PrimaryElement component = inst.GetComponent<PrimaryElement>();
		component.SetElement(this.ElementID, true);
		Element element = component.Element;
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x0004352C File Offset: 0x0004172C
	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<PrimaryElement>().SetElement(this.ElementID, true);
	}

	// Token: 0x04000792 RID: 1938
	public const string ID = "WoodLog";

	// Token: 0x04000793 RID: 1939
	public const float C02MassEmissionWhenBurned = 0.142f;

	// Token: 0x04000794 RID: 1940
	public const float HeatWhenBurned = 7500f;

	// Token: 0x04000795 RID: 1941
	public const float EnergyWhenBurned = 250f;

	// Token: 0x04000796 RID: 1942
	public static readonly Tag TAG = TagManager.Create("WoodLog");
}
