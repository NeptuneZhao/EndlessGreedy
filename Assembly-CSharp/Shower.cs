using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000765 RID: 1893
[AddComponentMenu("KMonoBehaviour/Workable/Shower")]
public class Shower : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x060032D0 RID: 13008 RVA: 0x00117529 File Offset: 0x00115729
	private Shower()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x060032D1 RID: 13009 RVA: 0x00117539 File Offset: 0x00115739
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.resetProgressOnStop = true;
		this.smi = new Shower.ShowerSM.Instance(this);
		this.smi.StartSM();
	}

	// Token: 0x060032D2 RID: 13010 RVA: 0x00117560 File Offset: 0x00115760
	protected override void OnStartWork(WorkerBase worker)
	{
		HygieneMonitor.Instance instance = worker.GetSMI<HygieneMonitor.Instance>();
		base.WorkTimeRemaining = this.workTime * instance.GetDirtiness();
		this.accumulatedDisease = SimUtil.DiseaseInfo.Invalid;
		this.smi.SetActive(true);
		base.OnStartWork(worker);
	}

	// Token: 0x060032D3 RID: 13011 RVA: 0x001175A5 File Offset: 0x001157A5
	protected override void OnStopWork(WorkerBase worker)
	{
		this.smi.SetActive(false);
	}

	// Token: 0x060032D4 RID: 13012 RVA: 0x001175B4 File Offset: 0x001157B4
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		Effects component = worker.GetComponent<Effects>();
		for (int i = 0; i < Shower.EffectsRemoved.Length; i++)
		{
			string effect_id = Shower.EffectsRemoved[i];
			component.Remove(effect_id);
		}
		if (!worker.HasTag(GameTags.HasSuitTank))
		{
			GasLiquidExposureMonitor.Instance instance = worker.GetSMI<GasLiquidExposureMonitor.Instance>();
			if (instance != null)
			{
				instance.ResetExposure();
			}
		}
		component.Add(Shower.SHOWER_EFFECT, true);
		HygieneMonitor.Instance instance2 = worker.GetSMI<HygieneMonitor.Instance>();
		if (instance2 != null)
		{
			instance2.SetDirtiness(0f);
		}
	}

	// Token: 0x060032D5 RID: 13013 RVA: 0x00117634 File Offset: 0x00115834
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		PrimaryElement component = worker.GetComponent<PrimaryElement>();
		if (component.DiseaseCount > 0)
		{
			SimUtil.DiseaseInfo diseaseInfo = new SimUtil.DiseaseInfo
			{
				idx = component.DiseaseIdx,
				count = Mathf.CeilToInt((float)component.DiseaseCount * (1f - Mathf.Pow(this.fractionalDiseaseRemoval, dt)) - (float)this.absoluteDiseaseRemoval)
			};
			component.ModifyDiseaseCount(-diseaseInfo.count, "Shower.RemoveDisease");
			this.accumulatedDisease = SimUtil.CalculateFinalDiseaseInfo(this.accumulatedDisease, diseaseInfo);
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(this.outputTargetElement);
			if (primaryElement != null)
			{
				primaryElement.GetComponent<PrimaryElement>().AddDisease(this.accumulatedDisease.idx, this.accumulatedDisease.count, "Shower.RemoveDisease");
				this.accumulatedDisease = SimUtil.DiseaseInfo.Invalid;
			}
		}
		return false;
	}

	// Token: 0x060032D6 RID: 13014 RVA: 0x0011770C File Offset: 0x0011590C
	protected override void OnAbortWork(WorkerBase worker)
	{
		base.OnAbortWork(worker);
		HygieneMonitor.Instance instance = worker.GetSMI<HygieneMonitor.Instance>();
		if (instance != null)
		{
			instance.SetDirtiness(1f - this.GetPercentComplete());
		}
	}

	// Token: 0x060032D7 RID: 13015 RVA: 0x0011773C File Offset: 0x0011593C
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (Shower.EffectsRemoved.Length != 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.REMOVESEFFECTSUBTITLE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVESEFFECTSUBTITLE, Descriptor.DescriptorType.Effect);
			descriptors.Add(item);
			for (int i = 0; i < Shower.EffectsRemoved.Length; i++)
			{
				string text = Shower.EffectsRemoved[i];
				string arg = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME");
				string arg2 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".CAUSE");
				Descriptor item2 = default(Descriptor);
				item2.IncreaseIndent();
				item2.SetupDescriptor("• " + string.Format(UI.BUILDINGEFFECTS.REMOVEDEFFECT, arg), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REMOVEDEFFECT, arg2), Descriptor.DescriptorType.Effect);
				descriptors.Add(item2);
			}
		}
		Effect.AddModifierDescriptions(base.gameObject, descriptors, Shower.SHOWER_EFFECT, true);
		return descriptors;
	}

	// Token: 0x04001E02 RID: 7682
	private Shower.ShowerSM.Instance smi;

	// Token: 0x04001E03 RID: 7683
	public static string SHOWER_EFFECT = "Showered";

	// Token: 0x04001E04 RID: 7684
	public SimHashes outputTargetElement;

	// Token: 0x04001E05 RID: 7685
	public float fractionalDiseaseRemoval;

	// Token: 0x04001E06 RID: 7686
	public int absoluteDiseaseRemoval;

	// Token: 0x04001E07 RID: 7687
	private SimUtil.DiseaseInfo accumulatedDisease;

	// Token: 0x04001E08 RID: 7688
	public const float WATER_PER_USE = 5f;

	// Token: 0x04001E09 RID: 7689
	private static readonly string[] EffectsRemoved = new string[]
	{
		"SoakingWet",
		"WetFeet",
		"MinorIrritation",
		"MajorIrritation"
	};

	// Token: 0x020015F0 RID: 5616
	public class ShowerSM : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower>
	{
		// Token: 0x06009076 RID: 36982 RVA: 0x0034B9D8 File Offset: 0x00349BD8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.root.Update(new Action<Shower.ShowerSM.Instance, float>(this.UpdateStatusItems), UpdateRate.SIM_200ms, false);
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (Shower.ShowerSM.Instance smi) => smi.IsOperational).PlayAnim("off");
			this.operational.DefaultState(this.operational.not_ready).EventTransition(GameHashes.OperationalChanged, this.unoperational, (Shower.ShowerSM.Instance smi) => !smi.IsOperational);
			this.operational.not_ready.EventTransition(GameHashes.OnStorageChange, this.operational.ready, (Shower.ShowerSM.Instance smi) => smi.IsReady()).PlayAnim("off");
			this.operational.ready.ToggleChore(new Func<Shower.ShowerSM.Instance, Chore>(this.CreateShowerChore), this.operational.not_ready);
		}

		// Token: 0x06009077 RID: 36983 RVA: 0x0034BB00 File Offset: 0x00349D00
		private Chore CreateShowerChore(Shower.ShowerSM.Instance smi)
		{
			WorkChore<Shower> workChore = new WorkChore<Shower>(Db.Get().ChoreTypes.Shower, smi.master, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Hygiene, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.IsNotABionic, smi);
			return workChore;
		}

		// Token: 0x06009078 RID: 36984 RVA: 0x0034BB58 File Offset: 0x00349D58
		private void UpdateStatusItems(Shower.ShowerSM.Instance smi, float dt)
		{
			if (smi.OutputFull())
			{
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, this);
				return;
			}
			smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, false);
		}

		// Token: 0x04006E3C RID: 28220
		public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State unoperational;

		// Token: 0x04006E3D RID: 28221
		public Shower.ShowerSM.OperationalState operational;

		// Token: 0x0200252D RID: 9517
		public class OperationalState : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State
		{
			// Token: 0x0400A599 RID: 42393
			public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State not_ready;

			// Token: 0x0400A59A RID: 42394
			public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State ready;
		}

		// Token: 0x0200252E RID: 9518
		public new class Instance : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.GameInstance
		{
			// Token: 0x0600BD8E RID: 48526 RVA: 0x003D7B34 File Offset: 0x003D5D34
			public Instance(Shower master) : base(master)
			{
				this.operational = master.GetComponent<Operational>();
				this.consumer = master.GetComponent<ConduitConsumer>();
				this.dispenser = master.GetComponent<ConduitDispenser>();
			}

			// Token: 0x17000C1A RID: 3098
			// (get) Token: 0x0600BD8F RID: 48527 RVA: 0x003D7B61 File Offset: 0x003D5D61
			public bool IsOperational
			{
				get
				{
					return this.operational.IsOperational && this.consumer.IsConnected && this.dispenser.IsConnected;
				}
			}

			// Token: 0x0600BD90 RID: 48528 RVA: 0x003D7B8A File Offset: 0x003D5D8A
			public void SetActive(bool active)
			{
				this.operational.SetActive(active, false);
			}

			// Token: 0x0600BD91 RID: 48529 RVA: 0x003D7B9C File Offset: 0x003D5D9C
			private bool HasSufficientMass()
			{
				bool result = false;
				PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
				if (primaryElement != null)
				{
					result = (primaryElement.Mass >= 5f);
				}
				return result;
			}

			// Token: 0x0600BD92 RID: 48530 RVA: 0x003D7BD8 File Offset: 0x003D5DD8
			public bool OutputFull()
			{
				PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(SimHashes.DirtyWater);
				return primaryElement != null && primaryElement.Mass >= 5f;
			}

			// Token: 0x0600BD93 RID: 48531 RVA: 0x003D7C11 File Offset: 0x003D5E11
			public bool IsReady()
			{
				return this.HasSufficientMass() && !this.OutputFull();
			}

			// Token: 0x0400A59B RID: 42395
			private Operational operational;

			// Token: 0x0400A59C RID: 42396
			private ConduitConsumer consumer;

			// Token: 0x0400A59D RID: 42397
			private ConduitDispenser dispenser;
		}
	}
}
