using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A18 RID: 2584
[AddComponentMenu("KMonoBehaviour/Workable/RelaxationPoint")]
public class RelaxationPoint : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06004AFB RID: 19195 RVA: 0x001ACA5E File Offset: 0x001AAC5E
	public RelaxationPoint()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.showProgressBar = false;
	}

	// Token: 0x06004AFC RID: 19196 RVA: 0x001ACA78 File Offset: 0x001AAC78
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.lightEfficiencyBonus = false;
		base.GetComponent<KPrefabID>().AddTag(TagManager.Create("RelaxationPoint", MISC.TAGS.RELAXATION_POINT), false);
		if (RelaxationPoint.stressReductionEffect == null)
		{
			RelaxationPoint.stressReductionEffect = this.CreateEffect();
			RelaxationPoint.roomStressReductionEffect = this.CreateRoomEffect();
		}
	}

	// Token: 0x06004AFD RID: 19197 RVA: 0x001ACAD0 File Offset: 0x001AACD0
	public Effect CreateEffect()
	{
		Effect effect = new Effect("StressReduction", DUPLICANTS.MODIFIERS.STRESSREDUCTION.NAME, DUPLICANTS.MODIFIERS.STRESSREDUCTION.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		AttributeModifier modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, this.stressModificationValue / 600f, DUPLICANTS.MODIFIERS.STRESSREDUCTION.NAME, false, false, true);
		effect.Add(modifier);
		return effect;
	}

	// Token: 0x06004AFE RID: 19198 RVA: 0x001ACB54 File Offset: 0x001AAD54
	public Effect CreateRoomEffect()
	{
		Effect effect = new Effect("RoomRelaxationEffect", DUPLICANTS.MODIFIERS.STRESSREDUCTION_CLINIC.NAME, DUPLICANTS.MODIFIERS.STRESSREDUCTION_CLINIC.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		AttributeModifier modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, this.roomStressModificationValue / 600f, DUPLICANTS.MODIFIERS.STRESSREDUCTION_CLINIC.NAME, false, false, true);
		effect.Add(modifier);
		return effect;
	}

	// Token: 0x06004AFF RID: 19199 RVA: 0x001ACBD7 File Offset: 0x001AADD7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new RelaxationPoint.RelaxationPointSM.Instance(this);
		this.smi.StartSM();
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x06004B00 RID: 19200 RVA: 0x001ACC04 File Offset: 0x001AAE04
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (this.roomTracker != null && this.roomTracker.room != null && this.roomTracker.room.roomType == Db.Get().RoomTypes.MassageClinic)
		{
			worker.GetComponent<Effects>().Add(RelaxationPoint.roomStressReductionEffect, false);
		}
		else
		{
			worker.GetComponent<Effects>().Add(RelaxationPoint.stressReductionEffect, false);
		}
		base.GetComponent<Operational>().SetActive(true, false);
	}

	// Token: 0x06004B01 RID: 19201 RVA: 0x001ACC87 File Offset: 0x001AAE87
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (Db.Get().Amounts.Stress.Lookup(worker.gameObject).value <= this.stopStressingValue)
		{
			return true;
		}
		base.OnWorkTick(worker, dt);
		return false;
	}

	// Token: 0x06004B02 RID: 19202 RVA: 0x001ACCBC File Offset: 0x001AAEBC
	protected override void OnStopWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Remove(RelaxationPoint.stressReductionEffect);
		worker.GetComponent<Effects>().Remove(RelaxationPoint.roomStressReductionEffect);
		base.GetComponent<Operational>().SetActive(false, false);
		base.OnStopWork(worker);
	}

	// Token: 0x06004B03 RID: 19203 RVA: 0x001ACCF2 File Offset: 0x001AAEF2
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
	}

	// Token: 0x06004B04 RID: 19204 RVA: 0x001ACCFB File Offset: 0x001AAEFB
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x06004B05 RID: 19205 RVA: 0x001ACD00 File Offset: 0x001AAF00
	protected virtual WorkChore<RelaxationPoint> CreateWorkChore()
	{
		return new WorkChore<RelaxationPoint>(Db.Get().ChoreTypes.Relax, this, null, false, null, null, null, false, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x06004B06 RID: 19206 RVA: 0x001ACD34 File Offset: 0x001AAF34
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
		descriptors.Add(item);
		return descriptors;
	}

	// Token: 0x04003121 RID: 12577
	[MyCmpGet]
	private RoomTracker roomTracker;

	// Token: 0x04003122 RID: 12578
	[Serialize]
	protected float stopStressingValue;

	// Token: 0x04003123 RID: 12579
	public float stressModificationValue;

	// Token: 0x04003124 RID: 12580
	public float roomStressModificationValue;

	// Token: 0x04003125 RID: 12581
	private RelaxationPoint.RelaxationPointSM.Instance smi;

	// Token: 0x04003126 RID: 12582
	private static Effect stressReductionEffect;

	// Token: 0x04003127 RID: 12583
	private static Effect roomStressReductionEffect;

	// Token: 0x02001A34 RID: 6708
	public class RelaxationPointSM : GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint>
	{
		// Token: 0x06009F54 RID: 40788 RVA: 0x0037C074 File Offset: 0x0037A274
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (RelaxationPoint.RelaxationPointSM.Instance smi) => smi.GetComponent<Operational>().IsOperational).PlayAnim("off");
			this.operational.ToggleChore((RelaxationPoint.RelaxationPointSM.Instance smi) => smi.master.CreateWorkChore(), this.unoperational);
		}

		// Token: 0x04007BBA RID: 31674
		public GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint, object>.State unoperational;

		// Token: 0x04007BBB RID: 31675
		public GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint, object>.State operational;

		// Token: 0x020025F0 RID: 9712
		public new class Instance : GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint, object>.GameInstance
		{
			// Token: 0x0600C0C5 RID: 49349 RVA: 0x003DD0AD File Offset: 0x003DB2AD
			public Instance(RelaxationPoint master) : base(master)
			{
			}
		}
	}
}
