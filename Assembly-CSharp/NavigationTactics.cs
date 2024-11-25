using System;

// Token: 0x02000592 RID: 1426
public static class NavigationTactics
{
	// Token: 0x040012B6 RID: 4790
	public static NavTactic ReduceTravelDistance = new NavTactic(0, 0, 1, 4);

	// Token: 0x040012B7 RID: 4791
	public static NavTactic Range_2_AvoidOverlaps = new NavTactic(2, 6, 12, 1);

	// Token: 0x040012B8 RID: 4792
	public static NavTactic Range_3_ProhibitOverlap = new NavTactic(3, 6, 9999, 1);
}
