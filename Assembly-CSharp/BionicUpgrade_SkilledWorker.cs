using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000667 RID: 1639
public class BionicUpgrade_SkilledWorker : BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>
{
	// Token: 0x06002869 RID: 10345 RVA: 0x000E4E58 File Offset: 0x000E3058
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.Inactive;
		this.root.Enter(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.ApplySkills)).ToggleEffect(new Func<BionicUpgrade_SkilledWorker.Instance, string>(BionicUpgrade_SkilledWorker.GetEffectName)).Exit(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.RemoveSkills));
		this.Inactive.EventTransition(GameHashes.ScheduleBlocksChanged, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.ScheduleChanged, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.BionicOnline, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.StartWork, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null);
		this.Active.EventTransition(GameHashes.ScheduleBlocksChanged, this.Inactive, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsInBatterySaveMode)).EventTransition(GameHashes.ScheduleChanged, this.Inactive, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsInBatterySaveMode)).EventTransition(GameHashes.BionicOffline, this.Inactive, null).EventTransition(GameHashes.StopWork, this.Inactive, null).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null).Enter(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.CreateFX)).Exit(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.ClearFX));
	}

	// Token: 0x0600286A RID: 10346 RVA: 0x000E4FBE File Offset: 0x000E31BE
	public static string GetEffectName(BionicUpgrade_SkilledWorker.Instance smi)
	{
		return ((BionicUpgrade_SkilledWorker.Def)smi.def).EFFECT_NAME;
	}

	// Token: 0x0600286B RID: 10347 RVA: 0x000E4FD0 File Offset: 0x000E31D0
	public static void ApplySkills(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.ApplySkills();
	}

	// Token: 0x0600286C RID: 10348 RVA: 0x000E4FD8 File Offset: 0x000E31D8
	public static void RemoveSkills(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.RemoveSkills();
	}

	// Token: 0x0600286D RID: 10349 RVA: 0x000E4FE0 File Offset: 0x000E31E0
	public static bool IsMinionWorkingOnlineAndNotInBatterySaveMode(BionicUpgrade_SkilledWorker.Instance smi)
	{
		return BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsOnline(smi) && !BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsInBatterySaveMode(smi) && BionicUpgrade_SkilledWorker.IsMinionWorkingWithAttribute(smi);
	}

	// Token: 0x0600286E RID: 10350 RVA: 0x000E4FFC File Offset: 0x000E31FC
	public static bool IsMinionWorkingWithAttribute(BionicUpgrade_SkilledWorker.Instance smi)
	{
		Workable workable = smi.worker.GetWorkable();
		return workable != null && smi.worker.GetState() == WorkerBase.State.Working && workable.GetWorkAttribute() != null && workable.GetWorkAttribute().Id == ((BionicUpgrade_SkilledWorker.Def)smi.def).ATTRIBUTE_ID;
	}

	// Token: 0x0600286F RID: 10351 RVA: 0x000E5056 File Offset: 0x000E3256
	public static void CreateFX(BionicUpgrade_SkilledWorker.Instance smi)
	{
		BionicUpgrade_SkilledWorker.CreateAndReturnFX(smi);
	}

	// Token: 0x06002870 RID: 10352 RVA: 0x000E5060 File Offset: 0x000E3260
	public static BionicAttributeUseFx.Instance CreateAndReturnFX(BionicUpgrade_SkilledWorker.Instance smi)
	{
		if (!smi.isMasterNull)
		{
			smi.fx = new BionicAttributeUseFx.Instance(smi.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.FXFront)));
			smi.fx.StartSM();
			return smi.fx;
		}
		return null;
	}

	// Token: 0x06002871 RID: 10353 RVA: 0x000E50AF File Offset: 0x000E32AF
	public static void ClearFX(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.fx.sm.destroyFX.Trigger(smi.fx);
		smi.fx = null;
	}

	// Token: 0x0200144A RID: 5194
	public new class Def : BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def
	{
		// Token: 0x06008A07 RID: 35335 RVA: 0x003320B5 File Offset: 0x003302B5
		public Def(string upgradeID, string attributeID, string effectID, string[] skills = null) : base(upgradeID)
		{
			this.ATTRIBUTE_ID = attributeID;
			this.EFFECT_NAME = effectID;
			this.SKILLS_IDS = skills;
		}

		// Token: 0x0400694D RID: 26957
		public string EFFECT_NAME;

		// Token: 0x0400694E RID: 26958
		public string[] SKILLS_IDS;

		// Token: 0x0400694F RID: 26959
		public string ATTRIBUTE_ID;
	}

	// Token: 0x0200144B RID: 5195
	public new class Instance : BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.BaseInstance
	{
		// Token: 0x06008A08 RID: 35336 RVA: 0x003320D4 File Offset: 0x003302D4
		public Instance(IStateMachineTarget master, BionicUpgrade_SkilledWorker.Def def) : base(master, def)
		{
			this.worker = base.GetComponent<WorkerBase>();
			this.resume = base.GetComponent<MinionResume>();
		}

		// Token: 0x06008A09 RID: 35337 RVA: 0x003320F6 File Offset: 0x003302F6
		public override float GetCurrentWattageCost()
		{
			if (base.IsInsideState(base.sm.Active))
			{
				return base.Data.WattageCost;
			}
			return 0f;
		}

		// Token: 0x06008A0A RID: 35338 RVA: 0x0033211C File Offset: 0x0033031C
		public override string GetCurrentWattageCostName()
		{
			float currentWattageCost = this.GetCurrentWattageCost();
			if (base.IsInsideState(base.sm.Active))
			{
				string str = "<b>" + ((currentWattageCost >= 0f) ? "+" : "-") + "</b>";
				return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.STANDARD_ACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), str + GameUtil.GetFormattedWattage(currentWattageCost, GameUtil.WattageFormatterUnit.Automatic, true));
			}
			return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.STANDARD_INACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(this.upgradeComponent.PotentialWattage, GameUtil.WattageFormatterUnit.Automatic, true));
		}

		// Token: 0x06008A0B RID: 35339 RVA: 0x003321BC File Offset: 0x003303BC
		public void ApplySkills()
		{
			BionicUpgrade_SkilledWorker.Def def = (BionicUpgrade_SkilledWorker.Def)base.def;
			if (def.SKILLS_IDS != null)
			{
				for (int i = 0; i < def.SKILLS_IDS.Length; i++)
				{
					string skillId = def.SKILLS_IDS[i];
					this.resume.GrantSkill(skillId);
				}
			}
		}

		// Token: 0x06008A0C RID: 35340 RVA: 0x00332208 File Offset: 0x00330408
		public void RemoveSkills()
		{
			BionicUpgrade_SkilledWorker.Def def = (BionicUpgrade_SkilledWorker.Def)base.def;
			if (def.SKILLS_IDS != null)
			{
				for (int i = 0; i < def.SKILLS_IDS.Length; i++)
				{
					string skillId = def.SKILLS_IDS[i];
					this.resume.UngrantSkill(skillId);
				}
			}
		}

		// Token: 0x04006950 RID: 26960
		public WorkerBase worker;

		// Token: 0x04006951 RID: 26961
		public BionicAttributeUseFx.Instance fx;

		// Token: 0x04006952 RID: 26962
		private MinionResume resume;
	}
}
