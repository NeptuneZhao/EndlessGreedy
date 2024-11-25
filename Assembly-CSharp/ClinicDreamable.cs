using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200053A RID: 1338
[AddComponentMenu("KMonoBehaviour/Workable/Clinic Dreamable")]
public class ClinicDreamable : Workable
{
	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06001E88 RID: 7816 RVA: 0x000A9F3B File Offset: 0x000A813B
	// (set) Token: 0x06001E89 RID: 7817 RVA: 0x000A9F43 File Offset: 0x000A8143
	public bool DreamIsDisturbed { get; private set; }

	// Token: 0x06001E8A RID: 7818 RVA: 0x000A9F4C File Offset: 0x000A814C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.resetProgressOnStop = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Dreaming;
		this.workingStatusItem = null;
	}

	// Token: 0x06001E8B RID: 7819 RVA: 0x000A9F78 File Offset: 0x000A8178
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (ClinicDreamable.dreamJournalPrefab == null)
		{
			ClinicDreamable.dreamJournalPrefab = Assets.GetPrefab(DreamJournalConfig.ID);
			ClinicDreamable.sleepClinic = Db.Get().effects.Get("SleepClinic");
		}
		this.equippable = base.GetComponent<Equippable>();
		global::Debug.Assert(this.equippable != null);
		EquipmentDef def = this.equippable.def;
		def.OnEquipCallBack = (Action<Equippable>)Delegate.Combine(def.OnEquipCallBack, new Action<Equippable>(this.OnEquipPajamas));
		EquipmentDef def2 = this.equippable.def;
		def2.OnUnequipCallBack = (Action<Equippable>)Delegate.Combine(def2.OnUnequipCallBack, new Action<Equippable>(this.OnUnequipPajamas));
		this.OnEquipPajamas(this.equippable);
	}

	// Token: 0x06001E8C RID: 7820 RVA: 0x000AA044 File Offset: 0x000A8244
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.equippable == null)
		{
			return;
		}
		EquipmentDef def = this.equippable.def;
		def.OnEquipCallBack = (Action<Equippable>)Delegate.Remove(def.OnEquipCallBack, new Action<Equippable>(this.OnEquipPajamas));
		EquipmentDef def2 = this.equippable.def;
		def2.OnUnequipCallBack = (Action<Equippable>)Delegate.Remove(def2.OnUnequipCallBack, new Action<Equippable>(this.OnUnequipPajamas));
	}

	// Token: 0x06001E8D RID: 7821 RVA: 0x000AA0C0 File Offset: 0x000A82C0
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.GetPercentComplete() >= 1f)
		{
			Vector3 position = this.dreamer.transform.position;
			position.y += 1f;
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			Util.KInstantiate(ClinicDreamable.dreamJournalPrefab, position, Quaternion.identity, null, null, true, 0).SetActive(true);
			this.workTimeRemaining = this.GetWorkTime();
		}
		return false;
	}

	// Token: 0x06001E8E RID: 7822 RVA: 0x000AA138 File Offset: 0x000A8338
	public void OnEquipPajamas(Equippable eq)
	{
		if (this.equippable == null || this.equippable != eq)
		{
			return;
		}
		MinionAssignablesProxy minionAssignablesProxy = this.equippable.assignee as MinionAssignablesProxy;
		if (minionAssignablesProxy == null)
		{
			return;
		}
		if (minionAssignablesProxy.target is StoredMinionIdentity)
		{
			return;
		}
		GameObject targetGameObject = minionAssignablesProxy.GetTargetGameObject();
		this.effects = targetGameObject.GetComponent<Effects>();
		this.dreamer = targetGameObject.GetComponent<ChoreDriver>();
		this.selectable = targetGameObject.GetComponent<KSelectable>();
		this.dreamer.Subscribe(-1283701846, new Action<object>(this.WorkerStartedSleeping));
		this.dreamer.Subscribe(-2090444759, new Action<object>(this.WorkerStoppedSleeping));
		this.effects.Add(ClinicDreamable.sleepClinic, true);
		this.selectable.AddStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Wearing, null);
	}

	// Token: 0x06001E8F RID: 7823 RVA: 0x000AA220 File Offset: 0x000A8420
	public void OnUnequipPajamas(Equippable eq)
	{
		if (this.dreamer == null)
		{
			return;
		}
		if (this.equippable == null || this.equippable != eq)
		{
			return;
		}
		this.dreamer.Unsubscribe(-1283701846, new Action<object>(this.WorkerStartedSleeping));
		this.dreamer.Unsubscribe(-2090444759, new Action<object>(this.WorkerStoppedSleeping));
		this.selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Wearing, false);
		this.selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Sleeping, false);
		this.effects.Remove(ClinicDreamable.sleepClinic.Id);
		this.StopDreamingThought();
		this.dreamer = null;
		this.selectable = null;
		this.effects = null;
	}

	// Token: 0x06001E90 RID: 7824 RVA: 0x000AA2FC File Offset: 0x000A84FC
	public void WorkerStartedSleeping(object data)
	{
		SleepChore sleepChore = this.dreamer.GetCurrentChore() as SleepChore;
		StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context = sleepChore.smi.sm.isDisturbedByLight.GetContext(sleepChore.smi);
		context.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Combine(context.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
		StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context2 = sleepChore.smi.sm.isDisturbedByMovement.GetContext(sleepChore.smi);
		context2.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Combine(context2.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
		StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context3 = sleepChore.smi.sm.isDisturbedByNoise.GetContext(sleepChore.smi);
		context3.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Combine(context3.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
		StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context4 = sleepChore.smi.sm.isScaredOfDark.GetContext(sleepChore.smi);
		context4.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Combine(context4.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
		this.sleepable = (data as Sleepable);
		this.sleepable.Dreamable = this;
		base.StartWork(this.sleepable.worker);
		this.progressBar.Retarget(this.sleepable.gameObject);
		this.selectable.AddStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Sleeping, this);
		this.StartDreamingThought();
	}

	// Token: 0x06001E91 RID: 7825 RVA: 0x000AA46C File Offset: 0x000A866C
	public void WorkerStoppedSleeping(object data)
	{
		this.selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Sleeping, false);
		SleepChore sleepChore = this.dreamer.GetCurrentChore() as SleepChore;
		if (!sleepChore.IsNullOrDestroyed() && !sleepChore.smi.IsNullOrDestroyed() && !sleepChore.smi.sm.IsNullOrDestroyed())
		{
			StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context = sleepChore.smi.sm.isDisturbedByLight.GetContext(sleepChore.smi);
			context.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Remove(context.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
			StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context2 = sleepChore.smi.sm.isDisturbedByMovement.GetContext(sleepChore.smi);
			context2.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Remove(context2.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
			StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context3 = sleepChore.smi.sm.isDisturbedByNoise.GetContext(sleepChore.smi);
			context3.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Remove(context3.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
			StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context4 = sleepChore.smi.sm.isScaredOfDark.GetContext(sleepChore.smi);
			context4.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Remove(context4.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
		}
		this.StopDreamingThought();
		this.DreamIsDisturbed = false;
		if (base.worker != null)
		{
			base.StopWork(base.worker, false);
		}
		if (this.sleepable != null)
		{
			this.sleepable.Dreamable = null;
			this.sleepable = null;
		}
	}

	// Token: 0x06001E92 RID: 7826 RVA: 0x000AA610 File Offset: 0x000A8810
	private void OnSleepDisturbed(SleepChore.StatesInstance smi)
	{
		SleepChore sleepChore = this.dreamer.GetCurrentChore() as SleepChore;
		bool flag = sleepChore.smi.sm.isDisturbedByLight.Get(sleepChore.smi);
		flag |= sleepChore.smi.sm.isDisturbedByMovement.Get(sleepChore.smi);
		flag |= sleepChore.smi.sm.isDisturbedByNoise.Get(sleepChore.smi);
		flag |= sleepChore.smi.sm.isScaredOfDark.Get(sleepChore.smi);
		this.DreamIsDisturbed = flag;
		if (flag)
		{
			this.StopDreamingThought();
		}
	}

	// Token: 0x06001E93 RID: 7827 RVA: 0x000AA6B4 File Offset: 0x000A88B4
	private void StartDreamingThought()
	{
		if (this.dreamer != null && !this.HasStartedThoughts_Dreaming)
		{
			this.dreamer.GetSMI<Dreamer.Instance>().SetDream(Db.Get().Dreams.CommonDream);
			this.dreamer.GetSMI<Dreamer.Instance>().StartDreaming();
			this.HasStartedThoughts_Dreaming = true;
		}
	}

	// Token: 0x06001E94 RID: 7828 RVA: 0x000AA70D File Offset: 0x000A890D
	private void StopDreamingThought()
	{
		if (this.dreamer != null && this.HasStartedThoughts_Dreaming)
		{
			this.dreamer.GetSMI<Dreamer.Instance>().StopDreaming();
			this.HasStartedThoughts_Dreaming = false;
		}
	}

	// Token: 0x04001131 RID: 4401
	private static GameObject dreamJournalPrefab;

	// Token: 0x04001132 RID: 4402
	private static Effect sleepClinic;

	// Token: 0x04001133 RID: 4403
	public bool HasStartedThoughts_Dreaming;

	// Token: 0x04001135 RID: 4405
	private ChoreDriver dreamer;

	// Token: 0x04001136 RID: 4406
	private Equippable equippable;

	// Token: 0x04001137 RID: 4407
	private Effects effects;

	// Token: 0x04001138 RID: 4408
	private Sleepable sleepable;

	// Token: 0x04001139 RID: 4409
	private KSelectable selectable;

	// Token: 0x0400113A RID: 4410
	private HashedString dreamAnimName = "portal rocket comp";
}
