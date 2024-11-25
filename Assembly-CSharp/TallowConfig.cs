using System;
using UnityEngine;

// Token: 0x020002EC RID: 748
public class TallowConfig : IOreConfig
{
	// Token: 0x17000038 RID: 56
	// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x00059BC4 File Offset: 0x00057DC4
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.Tallow;
		}
	}

	// Token: 0x06000FB3 RID: 4019 RVA: 0x00059BCB File Offset: 0x00057DCB
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000FB4 RID: 4020 RVA: 0x00059BD2 File Offset: 0x00057DD2
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
	}

	// Token: 0x0400098C RID: 2444
	public const string ID = "Tallow";

	// Token: 0x0400098D RID: 2445
	public static readonly Tag TAG = TagManager.Create("Tallow");
}
