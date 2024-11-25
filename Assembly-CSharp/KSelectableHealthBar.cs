using System;

// Token: 0x0200057A RID: 1402
public class KSelectableHealthBar : KSelectable
{
	// Token: 0x0600209C RID: 8348 RVA: 0x000B64E0 File Offset: 0x000B46E0
	public override string GetName()
	{
		int num = (int)(this.progressBar.PercentFull * (float)this.scaleAmount);
		return string.Format("{0} {1}/{2}", this.entityName, num, this.scaleAmount);
	}

	// Token: 0x04001251 RID: 4689
	[MyCmpGet]
	private ProgressBar progressBar;

	// Token: 0x04001252 RID: 4690
	private int scaleAmount = 100;
}
