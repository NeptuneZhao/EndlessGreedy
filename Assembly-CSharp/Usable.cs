using System;

// Token: 0x0200046F RID: 1135
public abstract class Usable : KMonoBehaviour, IStateMachineTarget
{
	// Token: 0x06001872 RID: 6258
	public abstract void StartUsing(User user);

	// Token: 0x06001873 RID: 6259 RVA: 0x00082BFC File Offset: 0x00080DFC
	protected void StartUsing(StateMachine.Instance smi, User user)
	{
		DebugUtil.Assert(this.smi == null);
		DebugUtil.Assert(smi != null);
		this.smi = smi;
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi.OnStop, new Action<string, StateMachine.Status>(user.OnStateMachineStop));
		smi.StartSM();
	}

	// Token: 0x06001874 RID: 6260 RVA: 0x00082C50 File Offset: 0x00080E50
	public void StopUsing(User user)
	{
		if (this.smi != null)
		{
			StateMachine.Instance instance = this.smi;
			instance.OnStop = (Action<string, StateMachine.Status>)Delegate.Remove(instance.OnStop, new Action<string, StateMachine.Status>(user.OnStateMachineStop));
			this.smi.StopSM("Usable.StopUsing");
			this.smi = null;
		}
	}

	// Token: 0x04000D8F RID: 3471
	private StateMachine.Instance smi;
}
