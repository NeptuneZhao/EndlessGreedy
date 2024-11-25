using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A31 RID: 2609
public class DiseaseVisualization : ScriptableObject
{
	// Token: 0x06004B93 RID: 19347 RVA: 0x001AF4A8 File Offset: 0x001AD6A8
	public DiseaseVisualization.Info GetInfo(HashedString id)
	{
		foreach (DiseaseVisualization.Info info in this.info)
		{
			if (id == info.name)
			{
				return info;
			}
		}
		return default(DiseaseVisualization.Info);
	}

	// Token: 0x0400317C RID: 12668
	public Sprite overlaySprite;

	// Token: 0x0400317D RID: 12669
	public List<DiseaseVisualization.Info> info = new List<DiseaseVisualization.Info>();

	// Token: 0x02001A45 RID: 6725
	[Serializable]
	public struct Info
	{
		// Token: 0x06009F8F RID: 40847 RVA: 0x0037D4E1 File Offset: 0x0037B6E1
		public Info(string name)
		{
			this.name = name;
			this.overlayColourName = "germFoodPoisoning";
		}

		// Token: 0x04007BE0 RID: 31712
		public string name;

		// Token: 0x04007BE1 RID: 31713
		public string overlayColourName;
	}
}
