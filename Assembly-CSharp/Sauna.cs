using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000A75 RID: 2677
public class Sauna : StateMachineComponent<Sauna.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06004DE9 RID: 19945 RVA: 0x001BF868 File Offset: 0x001BDA68
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004DEA RID: 19946 RVA: 0x001BF87B File Offset: 0x001BDA7B
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06004DEB RID: 19947 RVA: 0x001BF884 File Offset: 0x001BDA84
	private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	// Token: 0x06004DEC RID: 19948 RVA: 0x001BF8EC File Offset: 0x001BDAEC
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Element element = ElementLoader.FindElementByHash(SimHashes.Steam);
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, element.name, GameUtil.GetFormattedMass(this.steamPerUseKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, element.name, GameUtil.GetFormattedMass(this.steamPerUseKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement, false));
		Element element2 = ElementLoader.FindElementByHash(SimHashes.Water);
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTEDPERUSE, element2.name, GameUtil.GetFormattedMass(this.steamPerUseKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTEDPERUSE, element2.name, GameUtil.GetFormattedMass(this.steamPerUseKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Effect, false));
		list.Add(new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_TOOLTIP"), Descriptor.DescriptorType.Effect, false));
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect, false));
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		return list;
	}

	// Token: 0x040033E0 RID: 13280
	public string specificEffect;

	// Token: 0x040033E1 RID: 13281
	public string trackingEffect;

	// Token: 0x040033E2 RID: 13282
	public float steamPerUseKG;

	// Token: 0x040033E3 RID: 13283
	public float waterOutputTemp;

	// Token: 0x040033E4 RID: 13284
	public static readonly Operational.Flag sufficientSteam = new Operational.Flag("sufficientSteam", Operational.Flag.Type.Requirement);

	// Token: 0x02001A8D RID: 6797
	public class States : GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna>
	{
		// Token: 0x0600A080 RID: 41088 RVA: 0x0037FDCC File Offset: 0x0037DFCC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false).ToggleMainStatusItem(Db.Get().BuildingStatusItems.MissingRequirements, null);
			this.operational.TagTransition(GameTags.Operational, this.inoperational, true).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GettingReady, null).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.Transition.ConditionCallback(this.IsReady));
			this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<Sauna.StatesInstance, Chore>(this.CreateChore), this.inoperational).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null);
			this.ready.idle.WorkableStartTransition((Sauna.StatesInstance smi) => smi.master.GetComponent<SaunaWorkable>(), this.ready.working).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.Not(new StateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.Transition.ConditionCallback(this.IsReady)));
			this.ready.working.WorkableCompleteTransition((Sauna.StatesInstance smi) => smi.master.GetComponent<SaunaWorkable>(), this.ready.idle).WorkableStopTransition((Sauna.StatesInstance smi) => smi.master.GetComponent<SaunaWorkable>(), this.ready.idle);
		}

		// Token: 0x0600A081 RID: 41089 RVA: 0x0037FF7C File Offset: 0x0037E17C
		private Chore CreateChore(Sauna.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<SaunaWorkable>();
			WorkChore<SaunaWorkable> workChore = new WorkChore<SaunaWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x0600A082 RID: 41090 RVA: 0x0037FFDC File Offset: 0x0037E1DC
		private bool IsReady(Sauna.StatesInstance smi)
		{
			PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Steam);
			return primaryElement != null && primaryElement.Mass >= smi.master.steamPerUseKG;
		}

		// Token: 0x04007CEA RID: 31978
		private GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.State inoperational;

		// Token: 0x04007CEB RID: 31979
		private GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.State operational;

		// Token: 0x04007CEC RID: 31980
		private Sauna.States.ReadyStates ready;

		// Token: 0x020025FD RID: 9725
		public class ReadyStates : GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.State
		{
			// Token: 0x0400A91D RID: 43293
			public GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.State idle;

			// Token: 0x0400A91E RID: 43294
			public GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.State working;
		}
	}

	// Token: 0x02001A8E RID: 6798
	public class StatesInstance : GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.GameInstance
	{
		// Token: 0x0600A084 RID: 41092 RVA: 0x00380023 File Offset: 0x0037E223
		public StatesInstance(Sauna smi) : base(smi)
		{
		}
	}
}
