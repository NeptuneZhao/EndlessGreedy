using System;
using System.Collections.Generic;

// Token: 0x020007CE RID: 1998
public class Faction
{
	// Token: 0x06003718 RID: 14104 RVA: 0x0012B854 File Offset: 0x00129A54
	public HashSet<FactionAlignment> HostileTo()
	{
		HashSet<FactionAlignment> hashSet = new HashSet<FactionAlignment>();
		foreach (KeyValuePair<FactionManager.FactionID, FactionManager.Disposition> keyValuePair in this.Dispositions)
		{
			if (keyValuePair.Value == FactionManager.Disposition.Attack)
			{
				hashSet.UnionWith(FactionManager.Instance.GetFaction(keyValuePair.Key).Members);
			}
		}
		return hashSet;
	}

	// Token: 0x06003719 RID: 14105 RVA: 0x0012B8D0 File Offset: 0x00129AD0
	public Faction(FactionManager.FactionID faction)
	{
		this.ID = faction;
		this.ConfigureAlignments(faction);
	}

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x0600371A RID: 14106 RVA: 0x0012B915 File Offset: 0x00129B15
	// (set) Token: 0x0600371B RID: 14107 RVA: 0x0012B91D File Offset: 0x00129B1D
	public bool CanAttack { get; private set; }

	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x0600371C RID: 14108 RVA: 0x0012B926 File Offset: 0x00129B26
	// (set) Token: 0x0600371D RID: 14109 RVA: 0x0012B92E File Offset: 0x00129B2E
	public bool CanAssist { get; private set; }

	// Token: 0x0600371E RID: 14110 RVA: 0x0012B938 File Offset: 0x00129B38
	private void ConfigureAlignments(FactionManager.FactionID faction)
	{
		switch (faction)
		{
		case FactionManager.FactionID.Duplicant:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Assist);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Assist);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
			break;
		case FactionManager.FactionID.Friendly:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Assist);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Assist);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
			break;
		case FactionManager.FactionID.Hostile:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Attack);
			break;
		case FactionManager.FactionID.Prey:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
			break;
		case FactionManager.FactionID.Predator:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Attack);
			break;
		case FactionManager.FactionID.Pest:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
			break;
		}
		foreach (KeyValuePair<FactionManager.FactionID, FactionManager.Disposition> keyValuePair in this.Dispositions)
		{
			if (keyValuePair.Value == FactionManager.Disposition.Attack)
			{
				this.CanAttack = true;
			}
			if (keyValuePair.Value == FactionManager.Disposition.Assist)
			{
				this.CanAssist = true;
			}
		}
	}

	// Token: 0x040020A0 RID: 8352
	public HashSet<FactionAlignment> Members = new HashSet<FactionAlignment>();

	// Token: 0x040020A1 RID: 8353
	public FactionManager.FactionID ID;

	// Token: 0x040020A2 RID: 8354
	public Dictionary<FactionManager.FactionID, FactionManager.Disposition> Dispositions = new Dictionary<FactionManager.FactionID, FactionManager.Disposition>(default(Faction.FactionIDComparer));

	// Token: 0x02001697 RID: 5783
	public struct FactionIDComparer : IEqualityComparer<FactionManager.FactionID>
	{
		// Token: 0x060092D9 RID: 37593 RVA: 0x0035705A File Offset: 0x0035525A
		public bool Equals(FactionManager.FactionID x, FactionManager.FactionID y)
		{
			return x == y;
		}

		// Token: 0x060092DA RID: 37594 RVA: 0x00357060 File Offset: 0x00355260
		public int GetHashCode(FactionManager.FactionID obj)
		{
			return (int)obj;
		}
	}
}
