using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020009C8 RID: 2504
public class NuclearResearchCenter : StateMachineComponent<NuclearResearchCenter.StatesInstance>, IResearchCenter, IGameObjectEffectDescriptor
{
	// Token: 0x060048B9 RID: 18617 RVA: 0x0019FE40 File Offset: 0x0019E040
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.ResearchCenters.Add(this);
		this.particleMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", this.particleMeterOffset, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target"
		});
		base.Subscribe<NuclearResearchCenter>(-1837862626, NuclearResearchCenter.OnStorageChangeDelegate);
		this.RefreshMeter();
		base.smi.StartSM();
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
	}

	// Token: 0x060048BA RID: 18618 RVA: 0x0019FEBF File Offset: 0x0019E0BF
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.ResearchCenters.Remove(this);
	}

	// Token: 0x060048BB RID: 18619 RVA: 0x0019FED2 File Offset: 0x0019E0D2
	public string GetResearchType()
	{
		return this.researchTypeID;
	}

	// Token: 0x060048BC RID: 18620 RVA: 0x0019FEDA File Offset: 0x0019E0DA
	private void OnStorageChange(object data)
	{
		this.RefreshMeter();
	}

	// Token: 0x060048BD RID: 18621 RVA: 0x0019FEE4 File Offset: 0x0019E0E4
	private void RefreshMeter()
	{
		float positionPercent = Mathf.Clamp01(this.particleStorage.Particles / this.particleStorage.Capacity());
		this.particleMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x060048BE RID: 18622 RVA: 0x0019FF1C File Offset: 0x0019E11C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.RESEARCH_MATERIALS, this.inputMaterial.ProperName(), GameUtil.GetFormattedByTag(this.inputMaterial, this.materialPerPoint, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.RESEARCH_MATERIALS, this.inputMaterial.ProperName(), GameUtil.GetFormattedByTag(this.inputMaterial, this.materialPerPoint, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Requirement, false),
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.PRODUCES_RESEARCH_POINTS, Research.Instance.researchTypes.GetResearchType(this.researchTypeID).name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.PRODUCES_RESEARCH_POINTS, Research.Instance.researchTypes.GetResearchType(this.researchTypeID).name), Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x04002F98 RID: 12184
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002F99 RID: 12185
	public string researchTypeID;

	// Token: 0x04002F9A RID: 12186
	public float materialPerPoint = 50f;

	// Token: 0x04002F9B RID: 12187
	public float timePerPoint;

	// Token: 0x04002F9C RID: 12188
	public Tag inputMaterial;

	// Token: 0x04002F9D RID: 12189
	[MyCmpReq]
	private HighEnergyParticleStorage particleStorage;

	// Token: 0x04002F9E RID: 12190
	public Meter.Offset particleMeterOffset;

	// Token: 0x04002F9F RID: 12191
	private MeterController particleMeter;

	// Token: 0x04002FA0 RID: 12192
	private static readonly EventSystem.IntraObjectHandler<NuclearResearchCenter> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<NuclearResearchCenter>(delegate(NuclearResearchCenter component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x020019D4 RID: 6612
	public class States : GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter>
	{
		// Token: 0x06009E22 RID: 40482 RVA: 0x00376F10 File Offset: 0x00375110
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.requirements, false);
			this.requirements.PlayAnim("on").TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.requirements.highEnergyParticlesNeeded);
			this.requirements.highEnergyParticlesNeeded.ToggleMainStatusItem(Db.Get().BuildingStatusItems.WaitingForHighEnergyParticles, null).EventTransition(GameHashes.OnParticleStorageChanged, this.requirements.noResearchSelected, new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsReady));
			this.requirements.noResearchSelected.Enter(delegate(NuclearResearchCenter.StatesInstance smi)
			{
				this.UpdateNoResearchSelectedStatusItem(smi, true);
			}).Exit(delegate(NuclearResearchCenter.StatesInstance smi)
			{
				this.UpdateNoResearchSelectedStatusItem(smi, false);
			}).EventTransition(GameHashes.ActiveResearchChanged, this.requirements.noApplicableResearch, new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchSelected));
			this.requirements.noApplicableResearch.EventTransition(GameHashes.ActiveResearchChanged, this.ready, new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchApplicable)).EventTransition(GameHashes.ActiveResearchChanged, this.requirements, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchSelected)));
			this.ready.Enter(delegate(NuclearResearchCenter.StatesInstance smi)
			{
				smi.CreateChore();
			}).TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).Exit(delegate(NuclearResearchCenter.StatesInstance smi)
			{
				smi.DestroyChore();
			}).EventTransition(GameHashes.ActiveResearchChanged, this.requirements.noResearchSelected, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchSelected))).EventTransition(GameHashes.ActiveResearchChanged, this.requirements.noApplicableResearch, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchApplicable))).EventTransition(GameHashes.ResearchPointsChanged, this.requirements.noApplicableResearch, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchApplicable))).EventTransition(GameHashes.OnParticleStorageEmpty, this.requirements.highEnergyParticlesNeeded, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.HasRadiation)));
			this.ready.idle.WorkableStartTransition((NuclearResearchCenter.StatesInstance smi) => smi.master.GetComponent<NuclearResearchCenterWorkable>(), this.ready.working);
			this.ready.working.Enter("SetActive(true)", delegate(NuclearResearchCenter.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit("SetActive(false)", delegate(NuclearResearchCenter.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).WorkableStopTransition((NuclearResearchCenter.StatesInstance smi) => smi.master.GetComponent<NuclearResearchCenterWorkable>(), this.ready.idle).WorkableCompleteTransition((NuclearResearchCenter.StatesInstance smi) => smi.master.GetComponent<NuclearResearchCenterWorkable>(), this.ready.idle);
		}

		// Token: 0x06009E23 RID: 40483 RVA: 0x00377254 File Offset: 0x00375454
		protected bool IsAllResearchComplete()
		{
			using (List<Tech>.Enumerator enumerator = Db.Get().Techs.resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsComplete())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06009E24 RID: 40484 RVA: 0x003772B8 File Offset: 0x003754B8
		private void UpdateNoResearchSelectedStatusItem(NuclearResearchCenter.StatesInstance smi, bool entering)
		{
			bool flag = entering && !this.IsResearchSelected(smi) && !this.IsAllResearchComplete();
			KSelectable component = smi.GetComponent<KSelectable>();
			if (flag)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.NoResearchSelected, null);
				return;
			}
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, false);
		}

		// Token: 0x06009E25 RID: 40485 RVA: 0x00377324 File Offset: 0x00375524
		private bool IsReady(NuclearResearchCenter.StatesInstance smi)
		{
			return smi.GetComponent<HighEnergyParticleStorage>().Particles > smi.master.materialPerPoint;
		}

		// Token: 0x06009E26 RID: 40486 RVA: 0x0037733E File Offset: 0x0037553E
		private bool IsResearchSelected(NuclearResearchCenter.StatesInstance smi)
		{
			return Research.Instance.GetActiveResearch() != null;
		}

		// Token: 0x06009E27 RID: 40487 RVA: 0x00377350 File Offset: 0x00375550
		private bool IsResearchApplicable(NuclearResearchCenter.StatesInstance smi)
		{
			TechInstance activeResearch = Research.Instance.GetActiveResearch();
			if (activeResearch != null && activeResearch.tech.costsByResearchTypeID.ContainsKey(smi.master.researchTypeID))
			{
				float num = activeResearch.progressInventory.PointsByTypeID[smi.master.researchTypeID];
				float num2 = activeResearch.tech.costsByResearchTypeID[smi.master.researchTypeID];
				return num < num2;
			}
			return false;
		}

		// Token: 0x06009E28 RID: 40488 RVA: 0x003773C4 File Offset: 0x003755C4
		private bool HasRadiation(NuclearResearchCenter.StatesInstance smi)
		{
			return !smi.GetComponent<HighEnergyParticleStorage>().IsEmpty();
		}

		// Token: 0x04007AB0 RID: 31408
		public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State inoperational;

		// Token: 0x04007AB1 RID: 31409
		public NuclearResearchCenter.States.RequirementsState requirements;

		// Token: 0x04007AB2 RID: 31410
		public NuclearResearchCenter.States.ReadyState ready;

		// Token: 0x020025CA RID: 9674
		public class RequirementsState : GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State
		{
			// Token: 0x0400A835 RID: 43061
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State highEnergyParticlesNeeded;

			// Token: 0x0400A836 RID: 43062
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State noResearchSelected;

			// Token: 0x0400A837 RID: 43063
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State noApplicableResearch;
		}

		// Token: 0x020025CB RID: 9675
		public class ReadyState : GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State
		{
			// Token: 0x0400A838 RID: 43064
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State idle;

			// Token: 0x0400A839 RID: 43065
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State working;
		}
	}

	// Token: 0x020019D5 RID: 6613
	public class StatesInstance : GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.GameInstance
	{
		// Token: 0x06009E2C RID: 40492 RVA: 0x003773F0 File Offset: 0x003755F0
		public StatesInstance(NuclearResearchCenter master) : base(master)
		{
		}

		// Token: 0x06009E2D RID: 40493 RVA: 0x003773FC File Offset: 0x003755FC
		public void CreateChore()
		{
			Workable component = base.smi.master.GetComponent<NuclearResearchCenterWorkable>();
			this.chore = new WorkChore<NuclearResearchCenterWorkable>(Db.Get().ChoreTypes.Research, component, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			this.chore.preemption_cb = new Func<Chore.Precondition.Context, bool>(NuclearResearchCenter.StatesInstance.CanPreemptCB);
		}

		// Token: 0x06009E2E RID: 40494 RVA: 0x0037745D File Offset: 0x0037565D
		public void DestroyChore()
		{
			this.chore.Cancel("destroy me!");
			this.chore = null;
		}

		// Token: 0x06009E2F RID: 40495 RVA: 0x00377478 File Offset: 0x00375678
		private static bool CanPreemptCB(Chore.Precondition.Context context)
		{
			WorkerBase component = context.chore.driver.GetComponent<WorkerBase>();
			float num = Db.Get().AttributeConverters.ResearchSpeed.Lookup(component).Evaluate();
			WorkerBase worker = context.consumerState.worker;
			float num2 = Db.Get().AttributeConverters.ResearchSpeed.Lookup(worker).Evaluate();
			TechInstance activeResearch = Research.Instance.GetActiveResearch();
			if (activeResearch != null)
			{
				NuclearResearchCenter.StatesInstance smi = context.chore.gameObject.GetSMI<NuclearResearchCenter.StatesInstance>();
				if (smi != null)
				{
					return num2 > num && activeResearch.PercentageCompleteResearchType(smi.master.researchTypeID) < 1f;
				}
			}
			return false;
		}

		// Token: 0x04007AB3 RID: 31411
		private WorkChore<NuclearResearchCenterWorkable> chore;
	}
}
