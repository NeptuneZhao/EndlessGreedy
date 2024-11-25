using System;

// Token: 0x0200057B RID: 1403
public class KSelectableProgressBar : KSelectable
{
	// Token: 0x0600209E RID: 8350 RVA: 0x000B6534 File Offset: 0x000B4734
	public override string GetName()
	{
		int num = (int)(this.progressBar.PercentFull * (float)this.scaleAmount);
		return string.Format("{0} {1}/{2}", this.entityName, num, this.scaleAmount);
	}

	// Token: 0x04001253 RID: 4691
	[MyCmpGet]
	private ProgressBar progressBar;

	// Token: 0x04001254 RID: 4692
	private int scaleAmount = 100;
}
