using System;

// Token: 0x02000D7E RID: 3454
public interface IEmptyableCargo
{
	// Token: 0x06006CAC RID: 27820
	bool CanEmptyCargo();

	// Token: 0x06006CAD RID: 27821
	void EmptyCargo();

	// Token: 0x17000797 RID: 1943
	// (get) Token: 0x06006CAE RID: 27822
	IStateMachineTarget master { get; }

	// Token: 0x17000798 RID: 1944
	// (get) Token: 0x06006CAF RID: 27823
	bool CanAutoDeploy { get; }

	// Token: 0x17000799 RID: 1945
	// (get) Token: 0x06006CB0 RID: 27824
	// (set) Token: 0x06006CB1 RID: 27825
	bool AutoDeploy { get; set; }

	// Token: 0x1700079A RID: 1946
	// (get) Token: 0x06006CB2 RID: 27826
	bool ChooseDuplicant { get; }

	// Token: 0x1700079B RID: 1947
	// (get) Token: 0x06006CB3 RID: 27827
	bool ModuleDeployed { get; }

	// Token: 0x1700079C RID: 1948
	// (get) Token: 0x06006CB4 RID: 27828
	// (set) Token: 0x06006CB5 RID: 27829
	MinionIdentity ChosenDuplicant { get; set; }
}
