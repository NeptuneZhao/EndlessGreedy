using System;
using STRINGS;

// Token: 0x020007F6 RID: 2038
public class CreatureDiseaseCleaner : GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>
{
	// Token: 0x06003862 RID: 14434 RVA: 0x00133BE8 File Offset: 0x00131DE8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cleaning;
		GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.CLEANING.NAME;
		string tooltip = CREATURES.STATUSITEMS.CLEANING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.cleaning.DefaultState(this.cleaning.clean_pre).ScheduleGoTo((CreatureDiseaseCleaner.Instance smi) => smi.def.cleanDuration, this.cleaning.clean_pst);
		this.cleaning.clean_pre.PlayAnim("clean_water_pre").OnAnimQueueComplete(this.cleaning.clean);
		this.cleaning.clean.Enter(delegate(CreatureDiseaseCleaner.Instance smi)
		{
			smi.EnableDiseaseEmitter(true);
		}).QueueAnim("clean_water_loop", true, null).Transition(this.cleaning.clean_pst, (CreatureDiseaseCleaner.Instance smi) => !smi.GetSMI<CleaningMonitor.Instance>().CanCleanElementState(), UpdateRate.SIM_1000ms).Exit(delegate(CreatureDiseaseCleaner.Instance smi)
		{
			smi.EnableDiseaseEmitter(false);
		});
		this.cleaning.clean_pst.PlayAnim("clean_water_pst").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Cleaning, false);
	}

	// Token: 0x040021D8 RID: 8664
	public GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State behaviourcomplete;

	// Token: 0x040021D9 RID: 8665
	public CreatureDiseaseCleaner.CleaningStates cleaning;

	// Token: 0x040021DA RID: 8666
	public StateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.Signal cellChangedSignal;

	// Token: 0x020016D0 RID: 5840
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600939A RID: 37786 RVA: 0x00359A5D File Offset: 0x00357C5D
		public Def(float duration)
		{
			this.cleanDuration = duration;
		}

		// Token: 0x040070DE RID: 28894
		public float cleanDuration;
	}

	// Token: 0x020016D1 RID: 5841
	public class CleaningStates : GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State
	{
		// Token: 0x040070DF RID: 28895
		public GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State clean_pre;

		// Token: 0x040070E0 RID: 28896
		public GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State clean;

		// Token: 0x040070E1 RID: 28897
		public GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State clean_pst;
	}

	// Token: 0x020016D2 RID: 5842
	public new class Instance : GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.GameInstance
	{
		// Token: 0x0600939C RID: 37788 RVA: 0x00359A74 File Offset: 0x00357C74
		public Instance(Chore<CreatureDiseaseCleaner.Instance> chore, CreatureDiseaseCleaner.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Cleaning);
		}

		// Token: 0x0600939D RID: 37789 RVA: 0x00359A98 File Offset: 0x00357C98
		public void EnableDiseaseEmitter(bool enable = true)
		{
			DiseaseEmitter component = base.GetComponent<DiseaseEmitter>();
			if (component != null)
			{
				component.SetEnable(enable);
			}
		}
	}
}
