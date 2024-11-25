using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000552 RID: 1362
public class LureableMonitor : GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>
{
	// Token: 0x06001F44 RID: 8004 RVA: 0x000AF418 File Offset: 0x000AD618
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cooldown;
		this.cooldown.ScheduleGoTo((LureableMonitor.Instance smi) => smi.def.cooldown, this.nolure);
		this.nolure.PreBrainUpdate(delegate(LureableMonitor.Instance smi)
		{
			smi.FindLure();
		}).ParamTransition<GameObject>(this.targetLure, this.haslure, (LureableMonitor.Instance smi, GameObject p) => p != null);
		this.haslure.ParamTransition<GameObject>(this.targetLure, this.nolure, (LureableMonitor.Instance smi, GameObject p) => p == null).PreBrainUpdate(delegate(LureableMonitor.Instance smi)
		{
			smi.FindLure();
		}).ToggleBehaviour(GameTags.Creatures.MoveToLure, (LureableMonitor.Instance smi) => smi.HasLure(), delegate(LureableMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		});
	}

	// Token: 0x0400119F RID: 4511
	public StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.TargetParameter targetLure;

	// Token: 0x040011A0 RID: 4512
	public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State nolure;

	// Token: 0x040011A1 RID: 4513
	public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State haslure;

	// Token: 0x040011A2 RID: 4514
	public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State cooldown;

	// Token: 0x02001343 RID: 4931
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06008663 RID: 34403 RVA: 0x003290FE File Offset: 0x003272FE
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.BUILDINGEFFECTS.CAPTURE_METHOD_LURE, UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_LURE, Descriptor.DescriptorType.Effect, false)
			};
		}

		// Token: 0x04006613 RID: 26131
		public float cooldown = 20f;

		// Token: 0x04006614 RID: 26132
		public Tag[] lures;
	}

	// Token: 0x02001344 RID: 4932
	public new class Instance : GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.GameInstance
	{
		// Token: 0x06008665 RID: 34405 RVA: 0x00329139 File Offset: 0x00327339
		public Instance(IStateMachineTarget master, LureableMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06008666 RID: 34406 RVA: 0x00329144 File Offset: 0x00327344
		public void FindLure()
		{
			int num = -1;
			GameObject value = null;
			foreach (object obj in GameScenePartitioner.Instance.AsyncSafeEnumerate(Grid.PosToCell(base.smi.transform.GetPosition()), 1, GameScenePartitioner.Instance.lure))
			{
				Lure.Instance instance = obj as Lure.Instance;
				if (instance == null || !instance.IsActive() || !instance.HasAnyLure(base.def.lures))
				{
					return;
				}
				int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(instance.transform.GetPosition()), instance.LurePoints);
				if (navigationCost != -1 && (num == -1 || navigationCost < num))
				{
					num = navigationCost;
					value = instance.gameObject;
				}
			}
			base.sm.targetLure.Set(value, this, false);
		}

		// Token: 0x06008667 RID: 34407 RVA: 0x00329228 File Offset: 0x00327428
		public bool HasLure()
		{
			return base.sm.targetLure.Get(this) != null;
		}

		// Token: 0x06008668 RID: 34408 RVA: 0x00329241 File Offset: 0x00327441
		public GameObject GetTargetLure()
		{
			return base.sm.targetLure.Get(this);
		}

		// Token: 0x04006615 RID: 26133
		[MyCmpReq]
		private Navigator navigator;
	}
}
