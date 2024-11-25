using System;

// Token: 0x02000ADD RID: 2781
public class CommandConditions : KMonoBehaviour
{
	// Token: 0x0400367E RID: 13950
	public ConditionDestinationReachable reachable;

	// Token: 0x0400367F RID: 13951
	public CargoBayIsEmpty cargoEmpty;

	// Token: 0x04003680 RID: 13952
	public ConditionHasMinimumMass destHasResources;

	// Token: 0x04003681 RID: 13953
	public ConditionAllModulesComplete allModulesComplete;

	// Token: 0x04003682 RID: 13954
	public ConditionHasCargoBayForNoseconeHarvest HasCargoBayForNoseconeHarvest;

	// Token: 0x04003683 RID: 13955
	public ConditionHasEngine hasEngine;

	// Token: 0x04003684 RID: 13956
	public ConditionHasNosecone hasNosecone;

	// Token: 0x04003685 RID: 13957
	public ConditionOnLaunchPad onLaunchPad;

	// Token: 0x04003686 RID: 13958
	public ConditionFlightPathIsClear flightPathIsClear;
}
