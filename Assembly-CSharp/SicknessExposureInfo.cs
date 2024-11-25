using System;

// Token: 0x0200099C RID: 2460
[Serializable]
public struct SicknessExposureInfo
{
	// Token: 0x060047B6 RID: 18358 RVA: 0x0019A6AC File Offset: 0x001988AC
	public SicknessExposureInfo(string id, string infection_source_info)
	{
		this.sicknessID = id;
		this.sourceInfo = infection_source_info;
	}

	// Token: 0x04002EDD RID: 11997
	public string sicknessID;

	// Token: 0x04002EDE RID: 11998
	public string sourceInfo;
}
