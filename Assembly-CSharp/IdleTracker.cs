using System;
using System.Collections.Generic;

// Token: 0x020005DF RID: 1503
public class IdleTracker : WorldTracker
{
	// Token: 0x06002481 RID: 9345 RVA: 0x000CB7D8 File Offset: 0x000C99D8
	public IdleTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002482 RID: 9346 RVA: 0x000CB7E4 File Offset: 0x000C99E4
	public override void UpdateData()
	{
		this.objectsOfInterest.Clear();
		int num = 0;
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false);
		for (int i = 0; i < worldItems.Count; i++)
		{
			if (worldItems[i].HasTag(GameTags.Idle))
			{
				num++;
				this.objectsOfInterest.Add(worldItems[i].gameObject);
			}
		}
		base.AddPoint((float)num);
	}

	// Token: 0x06002483 RID: 9347 RVA: 0x000CB857 File Offset: 0x000C9A57
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
