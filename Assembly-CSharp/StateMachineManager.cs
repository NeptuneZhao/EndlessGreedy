using System;
using System.Collections.Generic;

// Token: 0x020004D2 RID: 1234
public class StateMachineManager : Singleton<StateMachineManager>, IScheduler
{
	// Token: 0x06001A9A RID: 6810 RVA: 0x0008C0DD File Offset: 0x0008A2DD
	public void RegisterScheduler(Scheduler scheduler)
	{
		this.scheduler = scheduler;
	}

	// Token: 0x06001A9B RID: 6811 RVA: 0x0008C0E6 File Offset: 0x0008A2E6
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, time, callback, callback_data, group);
	}

	// Token: 0x06001A9C RID: 6812 RVA: 0x0008C0FA File Offset: 0x0008A2FA
	public SchedulerHandle ScheduleNextFrame(string name, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, 0f, callback, callback_data, group);
	}

	// Token: 0x06001A9D RID: 6813 RVA: 0x0008C111 File Offset: 0x0008A311
	public SchedulerGroup CreateSchedulerGroup()
	{
		return new SchedulerGroup(this.scheduler);
	}

	// Token: 0x06001A9E RID: 6814 RVA: 0x0008C120 File Offset: 0x0008A320
	public StateMachine CreateStateMachine(Type type)
	{
		StateMachine stateMachine = null;
		if (!this.stateMachines.TryGetValue(type, out stateMachine))
		{
			stateMachine = (StateMachine)Activator.CreateInstance(type);
			stateMachine.CreateStates(stateMachine);
			stateMachine.BindStates();
			stateMachine.InitializeStateMachine();
			this.stateMachines[type] = stateMachine;
			List<Action<StateMachine>> list;
			if (this.stateMachineCreatedCBs.TryGetValue(type, out list))
			{
				foreach (Action<StateMachine> action in list)
				{
					action(stateMachine);
				}
			}
		}
		return stateMachine;
	}

	// Token: 0x06001A9F RID: 6815 RVA: 0x0008C1BC File Offset: 0x0008A3BC
	public T CreateStateMachine<T>()
	{
		return (T)((object)this.CreateStateMachine(typeof(T)));
	}

	// Token: 0x06001AA0 RID: 6816 RVA: 0x0008C1D4 File Offset: 0x0008A3D4
	public static void ResetParameters()
	{
		for (int i = 0; i < StateMachineManager.parameters.Length; i++)
		{
			StateMachineManager.parameters[i] = null;
		}
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x0008C1FB File Offset: 0x0008A3FB
	public StateMachine.Instance CreateSMIFromDef(IStateMachineTarget master, StateMachine.BaseDef def)
	{
		StateMachineManager.parameters[0] = master;
		StateMachineManager.parameters[1] = def;
		return (StateMachine.Instance)Activator.CreateInstance(Singleton<StateMachineManager>.Instance.CreateStateMachine(def.GetStateMachineType()).GetStateMachineInstanceType(), StateMachineManager.parameters);
	}

	// Token: 0x06001AA2 RID: 6818 RVA: 0x0008C231 File Offset: 0x0008A431
	public void Clear()
	{
		if (this.scheduler != null)
		{
			this.scheduler.FreeResources();
		}
		if (this.stateMachines != null)
		{
			this.stateMachines.Clear();
		}
	}

	// Token: 0x06001AA3 RID: 6819 RVA: 0x0008C25C File Offset: 0x0008A45C
	public void AddStateMachineCreatedCallback(Type sm_type, Action<StateMachine> cb)
	{
		List<Action<StateMachine>> list;
		if (!this.stateMachineCreatedCBs.TryGetValue(sm_type, out list))
		{
			list = new List<Action<StateMachine>>();
			this.stateMachineCreatedCBs[sm_type] = list;
		}
		list.Add(cb);
	}

	// Token: 0x06001AA4 RID: 6820 RVA: 0x0008C294 File Offset: 0x0008A494
	public void RemoveStateMachineCreatedCallback(Type sm_type, Action<StateMachine> cb)
	{
		List<Action<StateMachine>> list;
		if (this.stateMachineCreatedCBs.TryGetValue(sm_type, out list))
		{
			list.Remove(cb);
		}
	}

	// Token: 0x04000F15 RID: 3861
	private Scheduler scheduler;

	// Token: 0x04000F16 RID: 3862
	private Dictionary<Type, StateMachine> stateMachines = new Dictionary<Type, StateMachine>();

	// Token: 0x04000F17 RID: 3863
	private Dictionary<Type, List<Action<StateMachine>>> stateMachineCreatedCBs = new Dictionary<Type, List<Action<StateMachine>>>();

	// Token: 0x04000F18 RID: 3864
	private static object[] parameters = new object[2];
}
