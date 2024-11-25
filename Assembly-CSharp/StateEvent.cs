using System;

// Token: 0x020004C8 RID: 1224
public abstract class StateEvent
{
	// Token: 0x06001A4A RID: 6730 RVA: 0x0008B335 File Offset: 0x00089535
	public StateEvent(string name)
	{
		this.name = name;
		this.debugName = "(Event)" + name;
	}

	// Token: 0x06001A4B RID: 6731 RVA: 0x0008B355 File Offset: 0x00089555
	public virtual StateEvent.Context Subscribe(StateMachine.Instance smi)
	{
		return new StateEvent.Context(this);
	}

	// Token: 0x06001A4C RID: 6732 RVA: 0x0008B35D File Offset: 0x0008955D
	public virtual void Unsubscribe(StateMachine.Instance smi, StateEvent.Context context)
	{
	}

	// Token: 0x06001A4D RID: 6733 RVA: 0x0008B35F File Offset: 0x0008955F
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x06001A4E RID: 6734 RVA: 0x0008B367 File Offset: 0x00089567
	public string GetDebugName()
	{
		return this.debugName;
	}

	// Token: 0x04000EFC RID: 3836
	protected string name;

	// Token: 0x04000EFD RID: 3837
	private string debugName;

	// Token: 0x0200128A RID: 4746
	public struct Context
	{
		// Token: 0x0600841D RID: 33821 RVA: 0x0032275C File Offset: 0x0032095C
		public Context(StateEvent state_event)
		{
			this.stateEvent = state_event;
			this.data = 0;
		}

		// Token: 0x040063AA RID: 25514
		public StateEvent stateEvent;

		// Token: 0x040063AB RID: 25515
		public int data;
	}
}
