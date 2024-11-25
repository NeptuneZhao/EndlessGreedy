using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200072E RID: 1838
public class MilkSeparator : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>
{
	// Token: 0x060030CC RID: 12492 RVA: 0x0010D378 File Offset: 0x0010B578
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.noOperational;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.RefreshMeters));
		this.noOperational.TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off");
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).PlayAnim("on").DefaultState(this.operational.idle);
		this.operational.idle.EventTransition(GameHashes.OnStorageChange, this.operational.working.pre, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.CanBeginSeparate)).EnterTransition(this.operational.full, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.RequiresEmptying));
		this.operational.working.pre.QueueAnim("separating_pre", false, null).OnAnimQueueComplete(this.operational.working.work);
		this.operational.working.work.Enter(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.BeginSeparation)).PlayAnim("separating_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, this.operational.working.post, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.CanNOTKeepSeparating)).Exit(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.EndSeparation));
		this.operational.working.post.QueueAnim("separating_pst", false, null).OnAnimQueueComplete(this.operational.idle);
		this.operational.full.PlayAnim("ready").ToggleRecurringChore(new Func<MilkSeparator.Instance, Chore>(MilkSeparator.CreateEmptyChore), null).WorkableCompleteTransition((MilkSeparator.Instance smi) => smi.workable, this.operational.emptyComplete).ToggleStatusItem(Db.Get().BuildingStatusItems.MilkSeparatorNeedsEmptying, null);
		this.operational.emptyComplete.Enter(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.DropMilkFat)).ScheduleActionNextFrame("AfterMilkFatDrop", delegate(MilkSeparator.Instance smi)
		{
			smi.GoTo(this.operational.idle);
		});
	}

	// Token: 0x060030CD RID: 12493 RVA: 0x0010D5B5 File Offset: 0x0010B7B5
	public static void BeginSeparation(MilkSeparator.Instance smi)
	{
		smi.operational.SetActive(true, false);
	}

	// Token: 0x060030CE RID: 12494 RVA: 0x0010D5C4 File Offset: 0x0010B7C4
	public static void EndSeparation(MilkSeparator.Instance smi)
	{
		smi.operational.SetActive(false, false);
	}

	// Token: 0x060030CF RID: 12495 RVA: 0x0010D5D3 File Offset: 0x0010B7D3
	public static bool CanBeginSeparate(MilkSeparator.Instance smi)
	{
		return !smi.MilkFatLimitReached && smi.elementConverter.HasEnoughMassToStartConverting(false);
	}

	// Token: 0x060030D0 RID: 12496 RVA: 0x0010D5EB File Offset: 0x0010B7EB
	public static bool CanKeepSeparating(MilkSeparator.Instance smi)
	{
		return !smi.MilkFatLimitReached && smi.elementConverter.CanConvertAtAll();
	}

	// Token: 0x060030D1 RID: 12497 RVA: 0x0010D602 File Offset: 0x0010B802
	public static bool CanNOTKeepSeparating(MilkSeparator.Instance smi)
	{
		return !MilkSeparator.CanKeepSeparating(smi);
	}

	// Token: 0x060030D2 RID: 12498 RVA: 0x0010D60D File Offset: 0x0010B80D
	public static bool RequiresEmptying(MilkSeparator.Instance smi)
	{
		return smi.MilkFatLimitReached;
	}

	// Token: 0x060030D3 RID: 12499 RVA: 0x0010D615 File Offset: 0x0010B815
	public static bool ThereIsCapacityForMilkFat(MilkSeparator.Instance smi)
	{
		return !smi.MilkFatLimitReached;
	}

	// Token: 0x060030D4 RID: 12500 RVA: 0x0010D620 File Offset: 0x0010B820
	public static void DropMilkFat(MilkSeparator.Instance smi)
	{
		smi.DropMilkFat();
	}

	// Token: 0x060030D5 RID: 12501 RVA: 0x0010D628 File Offset: 0x0010B828
	public static void RefreshMeters(MilkSeparator.Instance smi)
	{
		smi.RefreshMeters();
	}

	// Token: 0x060030D6 RID: 12502 RVA: 0x0010D630 File Offset: 0x0010B830
	private static Chore CreateEmptyChore(MilkSeparator.Instance smi)
	{
		return new WorkChore<EmptyMilkSeparatorWorkable>(Db.Get().ChoreTypes.EmptyStorage, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x04001C9D RID: 7325
	public const string WORK_PRE_ANIM_NAME = "separating_pre";

	// Token: 0x04001C9E RID: 7326
	public const string WORK_ANIM_NAME = "separating_loop";

	// Token: 0x04001C9F RID: 7327
	public const string WORK_POST_ANIM_NAME = "separating_pst";

	// Token: 0x04001CA0 RID: 7328
	public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State noOperational;

	// Token: 0x04001CA1 RID: 7329
	public MilkSeparator.OperationalStates operational;

	// Token: 0x0200157F RID: 5503
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06008EC2 RID: 36546 RVA: 0x00344E26 File Offset: 0x00343026
		public Def()
		{
			this.MILK_FAT_TAG = ElementLoader.FindElementByHash(SimHashes.MilkFat).tag;
			this.MILK_TAG = ElementLoader.FindElementByHash(SimHashes.Milk).tag;
		}

		// Token: 0x04006CF3 RID: 27891
		public float MILK_FAT_CAPACITY = 100f;

		// Token: 0x04006CF4 RID: 27892
		public Tag MILK_TAG;

		// Token: 0x04006CF5 RID: 27893
		public Tag MILK_FAT_TAG;
	}

	// Token: 0x02001580 RID: 5504
	public class WorkingStates : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State
	{
		// Token: 0x04006CF6 RID: 27894
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State pre;

		// Token: 0x04006CF7 RID: 27895
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State work;

		// Token: 0x04006CF8 RID: 27896
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State post;
	}

	// Token: 0x02001581 RID: 5505
	public class OperationalStates : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State
	{
		// Token: 0x04006CF9 RID: 27897
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State idle;

		// Token: 0x04006CFA RID: 27898
		public MilkSeparator.WorkingStates working;

		// Token: 0x04006CFB RID: 27899
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State full;

		// Token: 0x04006CFC RID: 27900
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State emptyComplete;
	}

	// Token: 0x02001582 RID: 5506
	public new class Instance : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.GameInstance
	{
		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06008EC5 RID: 36549 RVA: 0x00344E73 File Offset: 0x00343073
		public float MilkFatStored
		{
			get
			{
				return this.storage.GetAmountAvailable(base.def.MILK_FAT_TAG);
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06008EC6 RID: 36550 RVA: 0x00344E8B File Offset: 0x0034308B
		public float MilkFatStoragePercentage
		{
			get
			{
				return Mathf.Clamp(this.MilkFatStored / base.def.MILK_FAT_CAPACITY, 0f, 1f);
			}
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06008EC7 RID: 36551 RVA: 0x00344EAE File Offset: 0x003430AE
		public bool MilkFatLimitReached
		{
			get
			{
				return this.MilkFatStored >= base.def.MILK_FAT_CAPACITY;
			}
		}

		// Token: 0x06008EC8 RID: 36552 RVA: 0x00344EC8 File Offset: 0x003430C8
		public Instance(IStateMachineTarget master, MilkSeparator.Def def) : base(master, def)
		{
			KAnimControllerBase component = base.GetComponent<KBatchedAnimController>();
			this.fatMeter = new MeterController(component, "meter_target_1", "meter_fat", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target_1"
			});
		}

		// Token: 0x06008EC9 RID: 36553 RVA: 0x00344F0B File Offset: 0x0034310B
		public override void StartSM()
		{
			base.StartSM();
			this.workable.OnWork_PST_Begins = new System.Action(this.Play_Empty_MeterAnimation);
			this.RefreshMeters();
		}

		// Token: 0x06008ECA RID: 36554 RVA: 0x00344F30 File Offset: 0x00343130
		private void Play_Empty_MeterAnimation()
		{
			this.fatMeter.SetPositionPercent(0f);
			this.fatMeter.meterController.Play("meter_fat_empty", KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x06008ECB RID: 36555 RVA: 0x00344F68 File Offset: 0x00343168
		public void DropMilkFat()
		{
			List<GameObject> list = new List<GameObject>();
			this.storage.Drop(base.def.MILK_FAT_TAG, list);
			Vector3 dropSpawnLocation = this.GetDropSpawnLocation();
			foreach (GameObject gameObject in list)
			{
				gameObject.transform.position = dropSpawnLocation;
			}
		}

		// Token: 0x06008ECC RID: 36556 RVA: 0x00344FE0 File Offset: 0x003431E0
		private Vector3 GetDropSpawnLocation()
		{
			bool flag;
			Vector3 vector = base.GetComponent<KBatchedAnimController>().GetSymbolTransform(new HashedString("milkfat"), out flag).GetColumn(3);
			vector.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			int num = Grid.PosToCell(vector);
			if (Grid.IsValidCell(num) && !Grid.Solid[num])
			{
				return vector;
			}
			return base.transform.GetPosition();
		}

		// Token: 0x06008ECD RID: 36557 RVA: 0x0034504C File Offset: 0x0034324C
		public void RefreshMeters()
		{
			if (this.fatMeter.meterController.currentAnim != "meter_fat")
			{
				this.fatMeter.meterController.Play("meter_fat", KAnim.PlayMode.Paused, 1f, 0f);
			}
			this.fatMeter.SetPositionPercent(this.MilkFatStoragePercentage);
		}

		// Token: 0x04006CFD RID: 27901
		[MyCmpGet]
		public EmptyMilkSeparatorWorkable workable;

		// Token: 0x04006CFE RID: 27902
		[MyCmpGet]
		public Operational operational;

		// Token: 0x04006CFF RID: 27903
		[MyCmpGet]
		public ElementConverter elementConverter;

		// Token: 0x04006D00 RID: 27904
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04006D01 RID: 27905
		private MeterController fatMeter;
	}
}
