using System;

// Token: 0x020007DF RID: 2015
public interface IConduitFlow
{
	// Token: 0x0600378D RID: 14221
	void AddConduitUpdater(Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default);

	// Token: 0x0600378E RID: 14222
	void RemoveConduitUpdater(Action<float> callback);

	// Token: 0x0600378F RID: 14223
	bool IsConduitEmpty(int cell);
}
