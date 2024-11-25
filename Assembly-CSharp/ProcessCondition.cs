using System;

// Token: 0x02000A03 RID: 2563
public abstract class ProcessCondition
{
	// Token: 0x06004A4E RID: 19022
	public abstract ProcessCondition.Status EvaluateCondition();

	// Token: 0x06004A4F RID: 19023
	public abstract bool ShowInUI();

	// Token: 0x06004A50 RID: 19024
	public abstract string GetStatusMessage(ProcessCondition.Status status);

	// Token: 0x06004A51 RID: 19025 RVA: 0x001A89B6 File Offset: 0x001A6BB6
	public string GetStatusMessage()
	{
		return this.GetStatusMessage(this.EvaluateCondition());
	}

	// Token: 0x06004A52 RID: 19026
	public abstract string GetStatusTooltip(ProcessCondition.Status status);

	// Token: 0x06004A53 RID: 19027 RVA: 0x001A89C4 File Offset: 0x001A6BC4
	public string GetStatusTooltip()
	{
		return this.GetStatusTooltip(this.EvaluateCondition());
	}

	// Token: 0x06004A54 RID: 19028 RVA: 0x001A89D2 File Offset: 0x001A6BD2
	public virtual StatusItem GetStatusItem(ProcessCondition.Status status)
	{
		return null;
	}

	// Token: 0x06004A55 RID: 19029 RVA: 0x001A89D5 File Offset: 0x001A6BD5
	public virtual ProcessCondition GetParentCondition()
	{
		return this.parentCondition;
	}

	// Token: 0x040030BD RID: 12477
	protected ProcessCondition parentCondition;

	// Token: 0x02001A21 RID: 6689
	public enum ProcessConditionType
	{
		// Token: 0x04007B66 RID: 31590
		RocketFlight,
		// Token: 0x04007B67 RID: 31591
		RocketPrep,
		// Token: 0x04007B68 RID: 31592
		RocketStorage,
		// Token: 0x04007B69 RID: 31593
		RocketBoard,
		// Token: 0x04007B6A RID: 31594
		All
	}

	// Token: 0x02001A22 RID: 6690
	public enum Status
	{
		// Token: 0x04007B6C RID: 31596
		Failure,
		// Token: 0x04007B6D RID: 31597
		Warning,
		// Token: 0x04007B6E RID: 31598
		Ready
	}
}
