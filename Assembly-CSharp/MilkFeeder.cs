using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200072D RID: 1837
public class MilkFeeder : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>
{
	// Token: 0x060030C4 RID: 12484 RVA: 0x0010CFBC File Offset: 0x0010B1BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.Enter(delegate(MilkFeeder.Instance smi)
		{
			smi.UpdateStorageMeter();
		}).EventHandler(GameHashes.OnStorageChange, delegate(MilkFeeder.Instance smi)
		{
			smi.UpdateStorageMeter();
		});
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (MilkFeeder.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.pre).EventTransition(GameHashes.OperationalChanged, this.on.pst, (MilkFeeder.Instance smi) => !smi.GetComponent<Operational>().IsOperational && smi.GetCurrentState() != this.on.pre).EventTransition(GameHashes.OperationalChanged, this.off, (MilkFeeder.Instance smi) => !smi.GetComponent<Operational>().IsOperational && smi.GetCurrentState() == this.on.pre);
		this.on.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
		this.on.working.PlayAnim("on").DefaultState(this.on.working.empty);
		this.on.working.empty.PlayAnim("empty").EnterTransition(this.on.working.refilling, (MilkFeeder.Instance smi) => smi.HasEnoughMilkForOneFeeding()).EventHandler(GameHashes.OnStorageChange, delegate(MilkFeeder.Instance smi)
		{
			if (smi.HasEnoughMilkForOneFeeding())
			{
				smi.GoTo(this.on.working.refilling);
			}
		});
		this.on.working.refilling.PlayAnim("fill").OnAnimQueueComplete(this.on.working.full);
		this.on.working.full.PlayAnim("full").Enter(delegate(MilkFeeder.Instance smi)
		{
			this.isReadyToStartFeeding.Set(true, smi, false);
		}).Exit(delegate(MilkFeeder.Instance smi)
		{
			this.isReadyToStartFeeding.Set(false, smi, false);
		}).ParamTransition<DrinkMilkStates.Instance>(this.currentFeedingCritter, this.on.working.emptying, (MilkFeeder.Instance smi, DrinkMilkStates.Instance val) => val != null);
		this.on.working.emptying.EnterTransition(this.on.working.full, delegate(MilkFeeder.Instance smi)
		{
			DrinkMilkMonitor.Instance smi2 = this.currentFeedingCritter.Get(smi).GetSMI<DrinkMilkMonitor.Instance>();
			return smi2 != null && !smi2.def.consumesMilk;
		}).PlayAnim("emptying").OnAnimQueueComplete(this.on.working.empty).Exit(delegate(MilkFeeder.Instance smi)
		{
			smi.StopFeeding();
		});
		this.on.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
	}

	// Token: 0x04001C99 RID: 7321
	private GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State off;

	// Token: 0x04001C9A RID: 7322
	private MilkFeeder.OnState on;

	// Token: 0x04001C9B RID: 7323
	public StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.BoolParameter isReadyToStartFeeding;

	// Token: 0x04001C9C RID: 7324
	public StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.ObjectParameter<DrinkMilkStates.Instance> currentFeedingCritter;

	// Token: 0x0200157B RID: 5499
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06008EAA RID: 36522 RVA: 0x00344B54 File Offset: 0x00342D54
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			go.GetSMI<MilkFeeder.Instance>();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(CREATURES.MODIFIERS.GOTMILK.NAME, "", Descriptor.DescriptorType.Effect);
			list.Add(item);
			Effect.AddModifierDescriptions(list, "HadMilk", true, "STRINGS.CREATURES.STATS.");
			return list;
		}
	}

	// Token: 0x0200157C RID: 5500
	public class OnState : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State
	{
		// Token: 0x04006CE7 RID: 27879
		public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State pre;

		// Token: 0x04006CE8 RID: 27880
		public MilkFeeder.OnState.WorkingState working;

		// Token: 0x04006CE9 RID: 27881
		public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State pst;

		// Token: 0x0200250E RID: 9486
		public class WorkingState : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State
		{
			// Token: 0x0400A4E5 RID: 42213
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State empty;

			// Token: 0x0400A4E6 RID: 42214
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State refilling;

			// Token: 0x0400A4E7 RID: 42215
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State full;

			// Token: 0x0400A4E8 RID: 42216
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State emptying;
		}
	}

	// Token: 0x0200157D RID: 5501
	public new class Instance : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.GameInstance
	{
		// Token: 0x06008EAD RID: 36525 RVA: 0x00344BB4 File Offset: 0x00342DB4
		public Instance(IStateMachineTarget master, MilkFeeder.Def def) : base(master, def)
		{
			this.milkStorage = base.GetComponent<Storage>();
			this.storageMeter = new MeterController(base.smi.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		}

		// Token: 0x06008EAE RID: 36526 RVA: 0x00344BF2 File Offset: 0x00342DF2
		public override void StartSM()
		{
			base.StartSM();
			Components.MilkFeeders.Add(base.smi.GetMyWorldId(), this);
		}

		// Token: 0x06008EAF RID: 36527 RVA: 0x00344C10 File Offset: 0x00342E10
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Components.MilkFeeders.Remove(base.smi.GetMyWorldId(), this);
		}

		// Token: 0x06008EB0 RID: 36528 RVA: 0x00344C2E File Offset: 0x00342E2E
		public void UpdateStorageMeter()
		{
			this.storageMeter.SetPositionPercent(1f - Mathf.Clamp01(this.milkStorage.RemainingCapacity() / this.milkStorage.capacityKg));
		}

		// Token: 0x06008EB1 RID: 36529 RVA: 0x00344C5D File Offset: 0x00342E5D
		public bool IsOperational()
		{
			return base.GetComponent<Operational>().IsOperational;
		}

		// Token: 0x06008EB2 RID: 36530 RVA: 0x00344C6A File Offset: 0x00342E6A
		public bool IsReserved()
		{
			return base.HasTag(GameTags.Creatures.ReservedByCreature);
		}

		// Token: 0x06008EB3 RID: 36531 RVA: 0x00344C78 File Offset: 0x00342E78
		public void SetReserved(bool isReserved)
		{
			if (isReserved)
			{
				global::Debug.Assert(!base.HasTag(GameTags.Creatures.ReservedByCreature));
				base.GetComponent<KPrefabID>().SetTag(GameTags.Creatures.ReservedByCreature, true);
				return;
			}
			if (base.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.ReservedByCreature);
				return;
			}
			global::Debug.LogWarningFormat(base.smi.gameObject, "Tried to unreserve a MilkFeeder that wasn't reserved", Array.Empty<object>());
		}

		// Token: 0x06008EB4 RID: 36532 RVA: 0x00344CE5 File Offset: 0x00342EE5
		public bool IsReadyToStartFeeding()
		{
			return this.IsOperational() && base.sm.isReadyToStartFeeding.Get(base.smi);
		}

		// Token: 0x06008EB5 RID: 36533 RVA: 0x00344D07 File Offset: 0x00342F07
		public void RequestToStartFeeding(DrinkMilkStates.Instance feedingCritter)
		{
			base.sm.currentFeedingCritter.Set(feedingCritter, base.smi, false);
		}

		// Token: 0x06008EB6 RID: 36534 RVA: 0x00344D24 File Offset: 0x00342F24
		public void StopFeeding()
		{
			DrinkMilkStates.Instance instance = base.sm.currentFeedingCritter.Get(base.smi);
			if (instance != null)
			{
				instance.RequestToStopFeeding();
			}
			base.sm.currentFeedingCritter.Set(null, base.smi, false);
		}

		// Token: 0x06008EB7 RID: 36535 RVA: 0x00344D6A File Offset: 0x00342F6A
		public bool HasEnoughMilkForOneFeeding()
		{
			return this.milkStorage.GetAmountAvailable(MilkFeederConfig.MILK_TAG) >= 5f;
		}

		// Token: 0x06008EB8 RID: 36536 RVA: 0x00344D86 File Offset: 0x00342F86
		public void ConsumeMilkForOneFeeding()
		{
			this.milkStorage.ConsumeIgnoringDisease(MilkFeederConfig.MILK_TAG, 5f);
		}

		// Token: 0x06008EB9 RID: 36537 RVA: 0x00344DA0 File Offset: 0x00342FA0
		public bool IsInCreaturePenRoom()
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
			return roomOfGameObject != null && roomOfGameObject.roomType == Db.Get().RoomTypes.CreaturePen;
		}

		// Token: 0x04006CEA RID: 27882
		public Storage milkStorage;

		// Token: 0x04006CEB RID: 27883
		public MeterController storageMeter;
	}
}
