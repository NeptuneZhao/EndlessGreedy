using System;
using STRINGS;

// Token: 0x020000DE RID: 222
public class HiveHarvestMonitor : GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>
{
	// Token: 0x06000404 RID: 1028 RVA: 0x00020788 File Offset: 0x0001E988
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.do_not_harvest;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.EventHandler(GameHashes.RefreshUserMenu, delegate(HiveHarvestMonitor.Instance smi)
		{
			smi.OnRefreshUserMenu();
		});
		this.do_not_harvest.ParamTransition<bool>(this.shouldHarvest, this.harvest, (HiveHarvestMonitor.Instance smi, bool bShouldHarvest) => bShouldHarvest);
		this.harvest.ParamTransition<bool>(this.shouldHarvest, this.do_not_harvest, (HiveHarvestMonitor.Instance smi, bool bShouldHarvest) => !bShouldHarvest).DefaultState(this.harvest.not_ready);
		this.harvest.not_ready.EventTransition(GameHashes.OnStorageChange, this.harvest.ready, (HiveHarvestMonitor.Instance smi) => smi.storage.GetMassAvailable(smi.def.producedOre) >= smi.def.harvestThreshold);
		this.harvest.ready.ToggleChore((HiveHarvestMonitor.Instance smi) => smi.CreateHarvestChore(), this.harvest.not_ready).EventTransition(GameHashes.OnStorageChange, this.harvest.not_ready, (HiveHarvestMonitor.Instance smi) => smi.storage.GetMassAvailable(smi.def.producedOre) < smi.def.harvestThreshold);
	}

	// Token: 0x040002BE RID: 702
	public StateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.BoolParameter shouldHarvest;

	// Token: 0x040002BF RID: 703
	public GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State do_not_harvest;

	// Token: 0x040002C0 RID: 704
	public HiveHarvestMonitor.HarvestStates harvest;

	// Token: 0x0200105F RID: 4191
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005CA2 RID: 23714
		public Tag producedOre;

		// Token: 0x04005CA3 RID: 23715
		public float harvestThreshold;
	}

	// Token: 0x02001060 RID: 4192
	public class HarvestStates : GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State
	{
		// Token: 0x04005CA4 RID: 23716
		public GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State not_ready;

		// Token: 0x04005CA5 RID: 23717
		public GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State ready;
	}

	// Token: 0x02001061 RID: 4193
	public new class Instance : GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.GameInstance
	{
		// Token: 0x06007BCE RID: 31694 RVA: 0x003042E6 File Offset: 0x003024E6
		public Instance(IStateMachineTarget master, HiveHarvestMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06007BCF RID: 31695 RVA: 0x003042F0 File Offset: 0x003024F0
		public void OnRefreshUserMenu()
		{
			if (base.sm.shouldHarvest.Get(this))
			{
				Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_building_disabled", UI.USERMENUACTIONS.CANCELEMPTYBEEHIVE.NAME, delegate()
				{
					base.sm.shouldHarvest.Set(false, this, false);
				}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELEMPTYBEEHIVE.TOOLTIP, true), 1f);
				return;
			}
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYBEEHIVE.NAME, delegate()
			{
				base.sm.shouldHarvest.Set(true, this, false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYBEEHIVE.TOOLTIP, true), 1f);
		}

		// Token: 0x06007BD0 RID: 31696 RVA: 0x003043AC File Offset: 0x003025AC
		public Chore CreateHarvestChore()
		{
			return new WorkChore<HiveWorkableEmpty>(Db.Get().ChoreTypes.Ranch, base.master.GetComponent<HiveWorkableEmpty>(), null, true, new Action<Chore>(base.smi.OnEmptyComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x06007BD1 RID: 31697 RVA: 0x003043F9 File Offset: 0x003025F9
		public void OnEmptyComplete(Chore chore)
		{
			base.smi.storage.Drop(base.smi.def.producedOre);
		}

		// Token: 0x04005CA6 RID: 23718
		[MyCmpReq]
		public Storage storage;
	}
}
