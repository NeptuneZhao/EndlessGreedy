using System;
using UnityEngine;

// Token: 0x020007CD RID: 1997
[AddComponentMenu("KMonoBehaviour/scripts/FactionManager")]
public class FactionManager : KMonoBehaviour
{
	// Token: 0x06003712 RID: 14098 RVA: 0x0012B74C File Offset: 0x0012994C
	public static void DestroyInstance()
	{
		FactionManager.Instance = null;
	}

	// Token: 0x06003713 RID: 14099 RVA: 0x0012B754 File Offset: 0x00129954
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		FactionManager.Instance = this;
	}

	// Token: 0x06003714 RID: 14100 RVA: 0x0012B762 File Offset: 0x00129962
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06003715 RID: 14101 RVA: 0x0012B76C File Offset: 0x0012996C
	public Faction GetFaction(FactionManager.FactionID faction)
	{
		switch (faction)
		{
		case FactionManager.FactionID.Duplicant:
			return this.Duplicant;
		case FactionManager.FactionID.Friendly:
			return this.Friendly;
		case FactionManager.FactionID.Hostile:
			return this.Hostile;
		case FactionManager.FactionID.Prey:
			return this.Prey;
		case FactionManager.FactionID.Predator:
			return this.Predator;
		case FactionManager.FactionID.Pest:
			return this.Pest;
		default:
			return null;
		}
	}

	// Token: 0x06003716 RID: 14102 RVA: 0x0012B7C4 File Offset: 0x001299C4
	public FactionManager.Disposition GetDisposition(FactionManager.FactionID of_faction, FactionManager.FactionID to_faction)
	{
		if (FactionManager.Instance.GetFaction(of_faction).Dispositions.ContainsKey(to_faction))
		{
			return FactionManager.Instance.GetFaction(of_faction).Dispositions[to_faction];
		}
		return FactionManager.Disposition.Neutral;
	}

	// Token: 0x04002099 RID: 8345
	public static FactionManager Instance;

	// Token: 0x0400209A RID: 8346
	public Faction Duplicant = new Faction(FactionManager.FactionID.Duplicant);

	// Token: 0x0400209B RID: 8347
	public Faction Friendly = new Faction(FactionManager.FactionID.Friendly);

	// Token: 0x0400209C RID: 8348
	public Faction Hostile = new Faction(FactionManager.FactionID.Hostile);

	// Token: 0x0400209D RID: 8349
	public Faction Predator = new Faction(FactionManager.FactionID.Predator);

	// Token: 0x0400209E RID: 8350
	public Faction Prey = new Faction(FactionManager.FactionID.Prey);

	// Token: 0x0400209F RID: 8351
	public Faction Pest = new Faction(FactionManager.FactionID.Pest);

	// Token: 0x02001695 RID: 5781
	public enum FactionID
	{
		// Token: 0x04007017 RID: 28695
		Duplicant,
		// Token: 0x04007018 RID: 28696
		Friendly,
		// Token: 0x04007019 RID: 28697
		Hostile,
		// Token: 0x0400701A RID: 28698
		Prey,
		// Token: 0x0400701B RID: 28699
		Predator,
		// Token: 0x0400701C RID: 28700
		Pest,
		// Token: 0x0400701D RID: 28701
		NumberOfFactions
	}

	// Token: 0x02001696 RID: 5782
	public enum Disposition
	{
		// Token: 0x0400701F RID: 28703
		Assist,
		// Token: 0x04007020 RID: 28704
		Neutral,
		// Token: 0x04007021 RID: 28705
		Attack
	}
}
