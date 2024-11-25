using System;

// Token: 0x02000CB4 RID: 3252
public interface IPersonalPriorityManager
{
	// Token: 0x06006428 RID: 25640
	int GetAssociatedSkillLevel(ChoreGroup group);

	// Token: 0x06006429 RID: 25641
	int GetPersonalPriority(ChoreGroup group);

	// Token: 0x0600642A RID: 25642
	void SetPersonalPriority(ChoreGroup group, int value);

	// Token: 0x0600642B RID: 25643
	bool IsChoreGroupDisabled(ChoreGroup group);

	// Token: 0x0600642C RID: 25644
	void ResetPersonalPriorities();
}
