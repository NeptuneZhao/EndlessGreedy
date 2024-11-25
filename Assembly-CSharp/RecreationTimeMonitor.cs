using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000997 RID: 2455
public class RecreationTimeMonitor : GameStateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>
{
	// Token: 0x0600479A RID: 18330 RVA: 0x00199EB0 File Offset: 0x001980B0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.ScheduleBlocksTick, delegate(RecreationTimeMonitor.Instance smi)
		{
			smi.OnScheduleBlocksTick();
		}).Update(delegate(RecreationTimeMonitor.Instance smi, float dt)
		{
			smi.RefreshTimes();
		}, UpdateRate.SIM_200ms, false);
		this.bonusActive.ToggleEffect((RecreationTimeMonitor.Instance smi) => smi.moraleEffect).EventHandler(GameHashes.ScheduleBlocksTick, delegate(RecreationTimeMonitor.Instance smi)
		{
			smi.OnScheduleBlocksTick();
		}).Update(delegate(RecreationTimeMonitor.Instance smi, float dt)
		{
			smi.RefreshTimes();
		}, UpdateRate.SIM_200ms, false);
	}

	// Token: 0x04002ECA RID: 11978
	public GameStateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>.State idle;

	// Token: 0x04002ECB RID: 11979
	public GameStateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>.State bonusActive;

	// Token: 0x0200196C RID: 6508
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200196D RID: 6509
	public new class Instance : GameStateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>.GameInstance
	{
		// Token: 0x06009C83 RID: 40067 RVA: 0x003725E0 File Offset: 0x003707E0
		public Instance(IStateMachineTarget master, RecreationTimeMonitor.Def def) : base(master, def)
		{
			this.schedulable = master.GetComponent<Schedulable>();
			this.moraleModifier = new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, 0f, () => Strings.Get("STRINGS.DUPLICANTS.MODIFIERS.BREAK" + this.moraleAddedTimes.Count.ToString() + ".NAME"), false, false);
			this.moraleEffect.Add(this.moraleModifier);
			if ((SaveLoader.Instance.GameInfo.saveMajorVersion != 0 || SaveLoader.Instance.GameInfo.saveMinorVersion != 0) && SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 35))
			{
				this.RestoreFromSchedule();
			}
			this.RefreshTimes();
		}

		// Token: 0x06009C84 RID: 40068 RVA: 0x003726D0 File Offset: 0x003708D0
		public void RefreshTimes()
		{
			for (int i = this.moraleAddedTimes.Count - 1; i >= 0; i--)
			{
				if (GameClock.Instance.GetTime() - this.moraleAddedTimes[i] > 600f)
				{
					this.moraleAddedTimes.RemoveAt(i);
				}
			}
			int max = 5;
			int num = Math.Clamp(this.moraleAddedTimes.Count - 1, 0, max);
			this.moraleModifier.SetValue((float)num);
			if (num > 0)
			{
				if (base.smi.GetCurrentState() != base.smi.sm.bonusActive)
				{
					base.smi.GoTo(base.smi.sm.bonusActive);
					return;
				}
			}
			else if (base.smi.GetCurrentState() != base.smi.sm.idle)
			{
				base.smi.GoTo(base.smi.sm.idle);
			}
		}

		// Token: 0x06009C85 RID: 40069 RVA: 0x003727B8 File Offset: 0x003709B8
		public void OnScheduleBlocksTick()
		{
			if (ScheduleManager.Instance.GetSchedule(this.schedulable).GetPreviousScheduleBlock().GroupId == Db.Get().ScheduleGroups.Recreation.Id)
			{
				this.moraleAddedTimes.Add(GameClock.Instance.GetTime());
			}
		}

		// Token: 0x06009C86 RID: 40070 RVA: 0x00372810 File Offset: 0x00370A10
		private void RestoreFromSchedule()
		{
			Effects component = base.GetComponent<Effects>();
			foreach (string effect_id in new string[]
			{
				"Break1",
				"Break2",
				"Break3",
				"Break4",
				"Break5"
			})
			{
				if (component.HasEffect(effect_id))
				{
					component.Remove(effect_id);
				}
			}
			Schedule schedule = ScheduleManager.Instance.GetSchedule(this.schedulable);
			List<ScheduleBlock> blocks = schedule.GetBlocks();
			int currentBlockIdx = schedule.GetCurrentBlockIdx();
			int num = 24;
			if (GameClock.Instance.GetTime() <= 600f)
			{
				num = Math.Min(currentBlockIdx, Mathf.FloorToInt(GameClock.Instance.GetTime() / 25f));
			}
			for (int j = currentBlockIdx - num; j < currentBlockIdx; j++)
			{
				int k = j;
				global::Debug.Assert(blocks.Count > 0);
				while (k < 0)
				{
					k += blocks.Count;
				}
				if (blocks[k].GroupId == Db.Get().ScheduleGroups.Recreation.Id)
				{
					int num2;
					if (k > currentBlockIdx)
					{
						num2 = blocks.Count - k + currentBlockIdx - 1;
					}
					else
					{
						num2 = currentBlockIdx - k - 1;
					}
					float num3 = (float)num2 * 25f;
					float num4 = GameClock.Instance.GetTime() - num3;
					global::Debug.Assert(num4 > 0f);
					this.moraleAddedTimes.Add(num4);
				}
			}
		}

		// Token: 0x04007963 RID: 31075
		[Serialize]
		public List<float> moraleAddedTimes = new List<float>();

		// Token: 0x04007964 RID: 31076
		public Effect moraleEffect = new Effect("RecTimeEffect", "Rec Time Effect", "Rec Time Effect Description", 0f, false, false, false, null, -1f, 0f, null, "");

		// Token: 0x04007965 RID: 31077
		private Schedulable schedulable;

		// Token: 0x04007966 RID: 31078
		private AttributeModifier moraleModifier;

		// Token: 0x04007967 RID: 31079
		private int shiftValue;
	}
}
