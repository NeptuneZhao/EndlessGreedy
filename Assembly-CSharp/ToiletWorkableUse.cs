using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000789 RID: 1929
[AddComponentMenu("KMonoBehaviour/Workable/ToiletWorkableUse")]
public class ToiletWorkableUse : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x0600348C RID: 13452 RVA: 0x0011E842 File Offset: 0x0011CA42
	private ToiletWorkableUse()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x0600348D RID: 13453 RVA: 0x0011E85D File Offset: 0x0011CA5D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
		base.SetWorkTime(8.5f);
	}

	// Token: 0x0600348E RID: 13454 RVA: 0x0011E894 File Offset: 0x0011CA94
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (Sim.IsRadiationEnabled() && worker.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
		{
			worker.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
		}
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			roomOfGameObject.roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), worker.GetComponent<Effects>());
		}
	}

	// Token: 0x0600348F RID: 13455 RVA: 0x0011E926 File Offset: 0x0011CB26
	protected override void OnStopWork(WorkerBase worker)
	{
		if (Sim.IsRadiationEnabled())
		{
			worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
		}
		base.OnStopWork(worker);
	}

	// Token: 0x06003490 RID: 13456 RVA: 0x0011E957 File Offset: 0x0011CB57
	protected override void OnAbortWork(WorkerBase worker)
	{
		if (Sim.IsRadiationEnabled())
		{
			worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
		}
		base.OnAbortWork(worker);
	}

	// Token: 0x06003491 RID: 13457 RVA: 0x0011E988 File Offset: 0x0011CB88
	protected override void OnCompleteWork(WorkerBase worker)
	{
		AmountInstance amountInstance = Db.Get().Amounts.Bladder.Lookup(worker);
		if (amountInstance != null)
		{
			this.lastAmountOfWasteMassRemovedFromDupe = DUPLICANTSTATS.STANDARD.Secretions.PEE_PER_TOILET_PEE;
			this.lastElementRemovedFromDupe = SimHashes.DirtyWater;
			amountInstance.SetValue(0f);
		}
		else
		{
			GunkMonitor.Instance smi = worker.GetSMI<GunkMonitor.Instance>();
			if (smi != null)
			{
				this.lastAmountOfWasteMassRemovedFromDupe = smi.CurrentGunkMass;
				this.lastElementRemovedFromDupe = GunkMonitor.GunkElement;
				smi.SetGunkMassValue(0f);
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_GunkedToilet, true);
			}
		}
		if (Sim.IsRadiationEnabled())
		{
			worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
			AmountInstance amountInstance2 = Db.Get().Amounts.RadiationBalance.Lookup(worker);
			RadiationMonitor.Instance smi2 = worker.GetSMI<RadiationMonitor.Instance>();
			float num = Math.Min(amountInstance2.value, 100f * smi2.difficultySettingMod);
			if (num >= 1f)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Math.Floor((double)num).ToString() + UI.UNITSUFFIXES.RADIATION.RADS, worker.transform, Vector3.up * 2f, 1.5f, false, false);
			}
			amountInstance2.ApplyDelta(-num);
		}
		this.timesUsed++;
		if (amountInstance != null)
		{
			base.Trigger(-350347868, worker);
		}
		else
		{
			base.Trigger(261445693, worker);
		}
		base.OnCompleteWork(worker);
	}

	// Token: 0x04001F0D RID: 7949
	[Serialize]
	public int timesUsed;

	// Token: 0x04001F0E RID: 7950
	public SimHashes lastElementRemovedFromDupe = SimHashes.DirtyWater;

	// Token: 0x04001F0F RID: 7951
	public float lastAmountOfWasteMassRemovedFromDupe;
}
