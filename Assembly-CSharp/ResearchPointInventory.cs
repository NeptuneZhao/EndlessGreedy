using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x02000A4E RID: 2638
public class ResearchPointInventory
{
	// Token: 0x06004C8E RID: 19598 RVA: 0x001B5AF4 File Offset: 0x001B3CF4
	public ResearchPointInventory()
	{
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			this.PointsByTypeID.Add(researchType.id, 0f);
		}
	}

	// Token: 0x06004C8F RID: 19599 RVA: 0x001B5B70 File Offset: 0x001B3D70
	public void AddResearchPoints(string researchTypeID, float points)
	{
		if (!this.PointsByTypeID.ContainsKey(researchTypeID))
		{
			Debug.LogWarning("Research inventory is missing research point key " + researchTypeID);
			return;
		}
		Dictionary<string, float> pointsByTypeID = this.PointsByTypeID;
		pointsByTypeID[researchTypeID] += points;
	}

	// Token: 0x06004C90 RID: 19600 RVA: 0x001B5BB5 File Offset: 0x001B3DB5
	public void RemoveResearchPoints(string researchTypeID, float points)
	{
		this.AddResearchPoints(researchTypeID, -points);
	}

	// Token: 0x06004C91 RID: 19601 RVA: 0x001B5BC0 File Offset: 0x001B3DC0
	[OnDeserialized]
	private void OnDeserialized()
	{
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			if (!this.PointsByTypeID.ContainsKey(researchType.id))
			{
				this.PointsByTypeID.Add(researchType.id, 0f);
			}
		}
	}

	// Token: 0x040032EB RID: 13035
	public Dictionary<string, float> PointsByTypeID = new Dictionary<string, float>();
}
