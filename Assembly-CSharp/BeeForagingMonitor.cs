using System;
using UnityEngine;

// Token: 0x020000BD RID: 189
public class BeeForagingMonitor : GameStateMachine<BeeForagingMonitor, BeeForagingMonitor.Instance, IStateMachineTarget, BeeForagingMonitor.Def>
{
	// Token: 0x06000365 RID: 869 RVA: 0x0001C72C File Offset: 0x0001A92C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToForage, new StateMachine<BeeForagingMonitor, BeeForagingMonitor.Instance, IStateMachineTarget, BeeForagingMonitor.Def>.Transition.ConditionCallback(BeeForagingMonitor.ShouldForage), delegate(BeeForagingMonitor.Instance smi)
		{
			smi.RefreshSearchTime();
		});
	}

	// Token: 0x06000366 RID: 870 RVA: 0x0001C780 File Offset: 0x0001A980
	public static bool ShouldForage(BeeForagingMonitor.Instance smi)
	{
		bool flag = GameClock.Instance.GetTimeInCycles() >= smi.nextSearchTime;
		KPrefabID kprefabID = smi.master.GetComponent<Bee>().FindHiveInRoom();
		if (kprefabID != null)
		{
			BeehiveCalorieMonitor.Instance smi2 = kprefabID.GetSMI<BeehiveCalorieMonitor.Instance>();
			if (smi2 == null || !smi2.IsHungry())
			{
				flag = false;
			}
		}
		return flag && kprefabID != null;
	}

	// Token: 0x02000FF2 RID: 4082
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005BD9 RID: 23513
		public float searchMinInterval = 0.25f;

		// Token: 0x04005BDA RID: 23514
		public float searchMaxInterval = 0.3f;
	}

	// Token: 0x02000FF3 RID: 4083
	public new class Instance : GameStateMachine<BeeForagingMonitor, BeeForagingMonitor.Instance, IStateMachineTarget, BeeForagingMonitor.Def>.GameInstance
	{
		// Token: 0x06007ADC RID: 31452 RVA: 0x00302C33 File Offset: 0x00300E33
		public Instance(IStateMachineTarget master, BeeForagingMonitor.Def def) : base(master, def)
		{
			this.RefreshSearchTime();
		}

		// Token: 0x06007ADD RID: 31453 RVA: 0x00302C43 File Offset: 0x00300E43
		public void RefreshSearchTime()
		{
			this.nextSearchTime = GameClock.Instance.GetTimeInCycles() + Mathf.Lerp(base.def.searchMinInterval, base.def.searchMaxInterval, UnityEngine.Random.value);
		}

		// Token: 0x04005BDB RID: 23515
		public float nextSearchTime;
	}
}
