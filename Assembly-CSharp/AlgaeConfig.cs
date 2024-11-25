using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000312 RID: 786
public class AlgaeConfig : IOreConfig
{
	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06001081 RID: 4225 RVA: 0x0005D59F File Offset: 0x0005B79F
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.Algae;
		}
	}

	// Token: 0x06001082 RID: 4226 RVA: 0x0005D5A6 File Offset: 0x0005B7A6
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001083 RID: 4227 RVA: 0x0005D5AD File Offset: 0x0005B7AD
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateSolidOreEntity(this.ElementID, new List<Tag>
		{
			GameTags.Life
		});
	}
}
