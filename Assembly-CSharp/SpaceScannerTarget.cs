using System;

// Token: 0x02000B12 RID: 2834
public readonly struct SpaceScannerTarget
{
	// Token: 0x0600546C RID: 21612 RVA: 0x001E2D77 File Offset: 0x001E0F77
	private SpaceScannerTarget(string id)
	{
		this.id = id;
	}

	// Token: 0x0600546D RID: 21613 RVA: 0x001E2D80 File Offset: 0x001E0F80
	public static SpaceScannerTarget MeteorShower()
	{
		return new SpaceScannerTarget("meteor_shower");
	}

	// Token: 0x0600546E RID: 21614 RVA: 0x001E2D8C File Offset: 0x001E0F8C
	public static SpaceScannerTarget BallisticObject()
	{
		return new SpaceScannerTarget("ballistic_object");
	}

	// Token: 0x0600546F RID: 21615 RVA: 0x001E2D98 File Offset: 0x001E0F98
	public static SpaceScannerTarget RocketBaseGame(LaunchConditionManager rocket)
	{
		return new SpaceScannerTarget(string.Format("rocket_base_game::{0}", rocket.GetComponent<KPrefabID>().InstanceID));
	}

	// Token: 0x06005470 RID: 21616 RVA: 0x001E2DB9 File Offset: 0x001E0FB9
	public static SpaceScannerTarget RocketDlc1(Clustercraft rocket)
	{
		return new SpaceScannerTarget(string.Format("rocket_dlc1::{0}", rocket.GetComponent<KPrefabID>().InstanceID));
	}

	// Token: 0x0400375A RID: 14170
	public readonly string id;
}
