using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public class InhaleStates : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>
{
	// Token: 0x06000428 RID: 1064 RVA: 0x00021704 File Offset: 0x0001F904
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingtoeat;
		this.root.Enter("SetTarget", delegate(InhaleStates.Instance smi)
		{
			this.targetCell.Set(smi.monitor.targetCell, smi, false);
		});
		this.goingtoeat.MoveTo((InhaleStates.Instance smi) => this.targetCell.Get(smi), this.inhaling, null, false).ToggleMainStatusItem(new Func<InhaleStates.Instance, StatusItem>(InhaleStates.GetMovingStatusItem), null);
		GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State state = this.inhaling.DefaultState(this.inhaling.inhale);
		string name = CREATURES.STATUSITEMS.INHALING.NAME;
		string tooltip = CREATURES.STATUSITEMS.INHALING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.inhaling.inhale.PlayAnim((InhaleStates.Instance smi) => smi.def.inhaleAnimPre, KAnim.PlayMode.Once).QueueAnim((InhaleStates.Instance smi) => smi.def.inhaleAnimLoop, true, null).Enter("ComputeInhaleAmount", delegate(InhaleStates.Instance smi)
		{
			smi.ComputeInhaleAmounts();
		}).Update("Consume", delegate(InhaleStates.Instance smi, float dt)
		{
			smi.monitor.Consume(dt * smi.consumptionMult);
		}, UpdateRate.SIM_200ms, false).EventTransition(GameHashes.ElementNoLongerAvailable, this.inhaling.pst, null).Enter("StartInhaleSound", delegate(InhaleStates.Instance smi)
		{
			smi.StartInhaleSound();
		}).Exit("StopInhaleSound", delegate(InhaleStates.Instance smi)
		{
			smi.StopInhaleSound();
		}).ScheduleGoTo((InhaleStates.Instance smi) => smi.inhaleTime, this.inhaling.pst);
		this.inhaling.pst.Transition(this.inhaling.full, (InhaleStates.Instance smi) => smi.def.alwaysPlayPstAnim || InhaleStates.IsFull(smi), UpdateRate.SIM_200ms).Transition(this.behaviourcomplete, GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Not(new StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Transition.ConditionCallback(InhaleStates.IsFull)), UpdateRate.SIM_200ms);
		this.inhaling.full.QueueAnim((InhaleStates.Instance smi) => smi.def.inhaleAnimPst, false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete((InhaleStates.Instance smi) => smi.def.behaviourTag, false);
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x000219D0 File Offset: 0x0001FBD0
	private static StatusItem GetMovingStatusItem(InhaleStates.Instance smi)
	{
		if (smi.def.useStorage)
		{
			return smi.def.storageStatusItem;
		}
		return Db.Get().CreatureStatusItems.LookingForFood;
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x000219FC File Offset: 0x0001FBFC
	private static bool IsFull(InhaleStates.Instance smi)
	{
		if (smi.def.useStorage)
		{
			if (smi.storage != null)
			{
				return smi.storage.IsFull();
			}
		}
		else
		{
			CreatureCalorieMonitor.Instance smi2 = smi.GetSMI<CreatureCalorieMonitor.Instance>();
			if (smi2 != null)
			{
				return smi2.stomach.GetFullness() >= 1f;
			}
		}
		return false;
	}

	// Token: 0x040002D0 RID: 720
	public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State goingtoeat;

	// Token: 0x040002D1 RID: 721
	public InhaleStates.InhalingStates inhaling;

	// Token: 0x040002D2 RID: 722
	public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State behaviourcomplete;

	// Token: 0x040002D3 RID: 723
	public StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.IntParameter targetCell;

	// Token: 0x02001076 RID: 4214
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005CD6 RID: 23766
		public string inhaleSound;

		// Token: 0x04005CD7 RID: 23767
		public float inhaleTime = 3f;

		// Token: 0x04005CD8 RID: 23768
		public Tag behaviourTag = GameTags.Creatures.WantsToEat;

		// Token: 0x04005CD9 RID: 23769
		public bool useStorage;

		// Token: 0x04005CDA RID: 23770
		public string inhaleAnimPre = "inhale_pre";

		// Token: 0x04005CDB RID: 23771
		public string inhaleAnimLoop = "inhale_loop";

		// Token: 0x04005CDC RID: 23772
		public string inhaleAnimPst = "inhale_pst";

		// Token: 0x04005CDD RID: 23773
		public bool alwaysPlayPstAnim;

		// Token: 0x04005CDE RID: 23774
		public StatusItem storageStatusItem = Db.Get().CreatureStatusItems.LookingForGas;
	}

	// Token: 0x02001077 RID: 4215
	public new class Instance : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.GameInstance
	{
		// Token: 0x06007C09 RID: 31753 RVA: 0x00304933 File Offset: 0x00302B33
		public Instance(Chore<InhaleStates.Instance> chore, InhaleStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, def.behaviourTag);
			this.inhaleSound = GlobalAssets.GetSound(def.inhaleSound, false);
		}

		// Token: 0x06007C0A RID: 31754 RVA: 0x0030496C File Offset: 0x00302B6C
		public void StartInhaleSound()
		{
			LoopingSounds component = base.GetComponent<LoopingSounds>();
			if (component != null && base.smi.inhaleSound != null)
			{
				component.StartSound(base.smi.inhaleSound);
			}
		}

		// Token: 0x06007C0B RID: 31755 RVA: 0x003049A8 File Offset: 0x00302BA8
		public void StopInhaleSound()
		{
			LoopingSounds component = base.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(base.smi.inhaleSound);
			}
		}

		// Token: 0x06007C0C RID: 31756 RVA: 0x003049D8 File Offset: 0x00302BD8
		public void ComputeInhaleAmounts()
		{
			float num = base.def.inhaleTime;
			this.inhaleTime = num;
			this.consumptionMult = 1f;
			if (!base.def.useStorage && this.monitor.def.diet != null)
			{
				Diet.Info dietInfo = base.smi.monitor.def.diet.GetDietInfo(base.smi.monitor.GetTargetElement().tag);
				if (dietInfo != null)
				{
					CreatureCalorieMonitor.Instance smi = base.smi.gameObject.GetSMI<CreatureCalorieMonitor.Instance>();
					float num2 = Mathf.Clamp01(smi.GetCalories0to1() / 0.9f);
					float num3 = 1f - num2;
					float consumptionRate = base.smi.monitor.def.consumptionRate;
					float num4 = dietInfo.ConvertConsumptionMassToCalories(consumptionRate);
					float num5 = num * num4 + 0.8f * smi.calories.GetMax() * num3 * num3 * num3;
					float num6 = num5 / num4;
					if (num6 > 5f * num)
					{
						this.inhaleTime = 5f * num;
						this.consumptionMult = num5 / (this.inhaleTime * num4);
						return;
					}
					this.inhaleTime = num6;
				}
			}
		}

		// Token: 0x04005CDF RID: 23775
		public string inhaleSound;

		// Token: 0x04005CE0 RID: 23776
		public float inhaleTime;

		// Token: 0x04005CE1 RID: 23777
		public float consumptionMult;

		// Token: 0x04005CE2 RID: 23778
		[MySmiGet]
		public GasAndLiquidConsumerMonitor.Instance monitor;

		// Token: 0x04005CE3 RID: 23779
		[MyCmpGet]
		public Storage storage;
	}

	// Token: 0x02001078 RID: 4216
	public class InhalingStates : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State
	{
		// Token: 0x04005CE4 RID: 23780
		public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State inhale;

		// Token: 0x04005CE5 RID: 23781
		public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State pst;

		// Token: 0x04005CE6 RID: 23782
		public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State full;
	}
}
