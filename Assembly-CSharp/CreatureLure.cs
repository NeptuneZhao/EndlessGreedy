using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000C21 RID: 3105
[SerializationConfig(MemberSerialization.OptIn)]
public class CreatureLure : StateMachineComponent<CreatureLure.StatesInstance>
{
	// Token: 0x06005F32 RID: 24370 RVA: 0x00235F3E File Offset: 0x0023413E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.operational = base.GetComponent<Operational>();
		base.Subscribe<CreatureLure>(-905833192, CreatureLure.OnCopySettingsDelegate);
	}

	// Token: 0x06005F33 RID: 24371 RVA: 0x00235F64 File Offset: 0x00234164
	private void OnCopySettings(object data)
	{
		CreatureLure component = ((GameObject)data).GetComponent<CreatureLure>();
		if (component != null)
		{
			this.ChangeBaitSetting(component.activeBaitSetting);
		}
	}

	// Token: 0x06005F34 RID: 24372 RVA: 0x00235F94 File Offset: 0x00234194
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.activeBaitSetting == Tag.Invalid)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoLureElementSelected, null);
		}
		else
		{
			this.ChangeBaitSetting(this.activeBaitSetting);
			this.OnStorageChange(null);
		}
		base.Subscribe<CreatureLure>(-1697596308, CreatureLure.OnStorageChangeDelegate);
	}

	// Token: 0x06005F35 RID: 24373 RVA: 0x00236008 File Offset: 0x00234208
	private void OnStorageChange(object data = null)
	{
		bool value = this.baitStorage.GetAmountAvailable(this.activeBaitSetting) > 0f;
		this.operational.SetFlag(CreatureLure.baited, value);
	}

	// Token: 0x06005F36 RID: 24374 RVA: 0x00236040 File Offset: 0x00234240
	public void ChangeBaitSetting(Tag baitSetting)
	{
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("SwitchedResource");
		}
		if (baitSetting != this.activeBaitSetting)
		{
			this.activeBaitSetting = baitSetting;
			this.baitStorage.DropAll(false, false, default(Vector3), true, null);
		}
		base.smi.GoTo(base.smi.sm.idle);
		this.baitStorage.storageFilters = new List<Tag>
		{
			this.activeBaitSetting
		};
		if (baitSetting != Tag.Invalid)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoLureElementSelected, false);
			if (base.smi.master.baitStorage.IsEmpty())
			{
				this.CreateFetchChore();
				return;
			}
		}
		else
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoLureElementSelected, null);
		}
	}

	// Token: 0x06005F37 RID: 24375 RVA: 0x0023612C File Offset: 0x0023432C
	protected void CreateFetchChore()
	{
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("Overwrite");
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.AwaitingBaitDelivery, false);
		if (this.activeBaitSetting == Tag.Invalid)
		{
			return;
		}
		this.fetchChore = new FetchChore(Db.Get().ChoreTypes.RanchingFetch, this.baitStorage, 100f, new HashSet<Tag>
		{
			this.activeBaitSetting
		}, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, null, null, null, Operational.State.None, 0);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.AwaitingBaitDelivery, null);
	}

	// Token: 0x04004006 RID: 16390
	public static float CONSUMPTION_RATE = 1f;

	// Token: 0x04004007 RID: 16391
	[Serialize]
	public Tag activeBaitSetting;

	// Token: 0x04004008 RID: 16392
	public List<Tag> baitTypes;

	// Token: 0x04004009 RID: 16393
	public Storage baitStorage;

	// Token: 0x0400400A RID: 16394
	protected FetchChore fetchChore;

	// Token: 0x0400400B RID: 16395
	private Operational operational;

	// Token: 0x0400400C RID: 16396
	private static readonly Operational.Flag baited = new Operational.Flag("Baited", Operational.Flag.Type.Requirement);

	// Token: 0x0400400D RID: 16397
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400400E RID: 16398
	private static readonly EventSystem.IntraObjectHandler<CreatureLure> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<CreatureLure>(delegate(CreatureLure component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400400F RID: 16399
	private static readonly EventSystem.IntraObjectHandler<CreatureLure> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<CreatureLure>(delegate(CreatureLure component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001D0B RID: 7435
	public class StatesInstance : GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.GameInstance
	{
		// Token: 0x0600A787 RID: 42887 RVA: 0x0039AB7D File Offset: 0x00398D7D
		public StatesInstance(CreatureLure master) : base(master)
		{
		}
	}

	// Token: 0x02001D0C RID: 7436
	public class States : GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure>
	{
		// Token: 0x0600A788 RID: 42888 RVA: 0x0039AB88 File Offset: 0x00398D88
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("off", KAnim.PlayMode.Loop).Enter(delegate(CreatureLure.StatesInstance smi)
			{
				if (smi.master.activeBaitSetting != Tag.Invalid)
				{
					if (smi.master.baitStorage.IsEmpty())
					{
						smi.master.CreateFetchChore();
						return;
					}
					if (smi.master.operational.IsOperational)
					{
						smi.GoTo(this.working);
					}
				}
			}).EventTransition(GameHashes.OnStorageChange, this.working, (CreatureLure.StatesInstance smi) => !smi.master.baitStorage.IsEmpty() && smi.master.activeBaitSetting != Tag.Invalid && smi.master.operational.IsOperational).EventTransition(GameHashes.OperationalChanged, this.working, (CreatureLure.StatesInstance smi) => !smi.master.baitStorage.IsEmpty() && smi.master.activeBaitSetting != Tag.Invalid && smi.master.operational.IsOperational);
			this.working.Enter(delegate(CreatureLure.StatesInstance smi)
			{
				smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.AwaitingBaitDelivery, false);
				HashedString batchTag = ElementLoader.FindElementByName(smi.master.activeBaitSetting.ToString()).substance.anim.batchTag;
				KAnim.Build build = ElementLoader.FindElementByName(smi.master.activeBaitSetting.ToString()).substance.anim.GetData().build;
				KAnim.Build.Symbol symbol = build.GetSymbol(new KAnimHashedString(build.name));
				HashedString target_symbol = "slime_mold";
				SymbolOverrideController component = smi.GetComponent<SymbolOverrideController>();
				component.TryRemoveSymbolOverride(target_symbol, 0);
				component.AddSymbolOverride(target_symbol, symbol, 0);
				smi.GetSMI<Lure.Instance>().SetActiveLures(new Tag[]
				{
					smi.master.activeBaitSetting
				});
			}).Exit(new StateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State.Callback(CreatureLure.States.ClearBait)).QueueAnim("working_pre", false, null).QueueAnim("working_loop", true, null).EventTransition(GameHashes.OnStorageChange, this.empty, (CreatureLure.StatesInstance smi) => smi.master.baitStorage.IsEmpty() && smi.master.activeBaitSetting != Tag.Invalid).EventTransition(GameHashes.OperationalChanged, this.idle, (CreatureLure.StatesInstance smi) => !smi.master.operational.IsOperational && !smi.master.baitStorage.IsEmpty());
			this.empty.QueueAnim("working_pst", false, null).QueueAnim("off", false, null).Enter(delegate(CreatureLure.StatesInstance smi)
			{
				smi.master.CreateFetchChore();
			}).EventTransition(GameHashes.OnStorageChange, this.working, (CreatureLure.StatesInstance smi) => !smi.master.baitStorage.IsEmpty() && smi.master.operational.IsOperational).EventTransition(GameHashes.OperationalChanged, this.working, (CreatureLure.StatesInstance smi) => !smi.master.baitStorage.IsEmpty() && smi.master.operational.IsOperational);
		}

		// Token: 0x0600A789 RID: 42889 RVA: 0x0039AD71 File Offset: 0x00398F71
		private static void ClearBait(StateMachine.Instance smi)
		{
			if (smi.GetSMI<Lure.Instance>() != null)
			{
				smi.GetSMI<Lure.Instance>().SetActiveLures(null);
			}
		}

		// Token: 0x040085DA RID: 34266
		public GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State idle;

		// Token: 0x040085DB RID: 34267
		public GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State working;

		// Token: 0x040085DC RID: 34268
		public GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State empty;
	}
}
