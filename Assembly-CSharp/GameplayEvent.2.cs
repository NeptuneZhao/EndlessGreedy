using System;

// Token: 0x02000474 RID: 1140
public abstract class GameplayEvent<StateMachineInstanceType> : GameplayEvent where StateMachineInstanceType : StateMachine.Instance
{
	// Token: 0x0600189C RID: 6300 RVA: 0x00083433 File Offset: 0x00081633
	public GameplayEvent(string id, int priority = 0, int importance = 0) : base(id, priority, importance)
	{
	}
}
