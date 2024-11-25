using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000896 RID: 2198
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/EquippableWorkable")]
public class EquippableWorkable : Workable, ISaveLoadable
{
	// Token: 0x06003D99 RID: 15769 RVA: 0x0015494C File Offset: 0x00152B4C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Equipping;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_equip_clothing_kanim")
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x06003D9A RID: 15770 RVA: 0x00154999 File Offset: 0x00152B99
	public global::QualityLevel GetQuality()
	{
		return this.quality;
	}

	// Token: 0x06003D9B RID: 15771 RVA: 0x001549A1 File Offset: 0x00152BA1
	public void SetQuality(global::QualityLevel level)
	{
		this.quality = level;
	}

	// Token: 0x06003D9C RID: 15772 RVA: 0x001549AA File Offset: 0x00152BAA
	protected override void OnSpawn()
	{
		base.SetWorkTime(1.5f);
		this.equippable.OnAssign += this.RefreshChore;
	}

	// Token: 0x06003D9D RID: 15773 RVA: 0x001549D0 File Offset: 0x00152BD0
	private void CreateChore()
	{
		global::Debug.Assert(this.chore == null, "chore should be null");
		this.chore = new EquipChore(this);
		Chore chore = this.chore;
		chore.onExit = (Action<Chore>)Delegate.Combine(chore.onExit, new Action<Chore>(this.OnChoreExit));
	}

	// Token: 0x06003D9E RID: 15774 RVA: 0x00154A23 File Offset: 0x00152C23
	private void OnChoreExit(Chore chore)
	{
		if (!chore.isComplete)
		{
			this.RefreshChore(this.currentTarget);
		}
	}

	// Token: 0x06003D9F RID: 15775 RVA: 0x00154A39 File Offset: 0x00152C39
	public void CancelChore(string reason = "")
	{
		if (this.chore != null)
		{
			this.chore.Cancel(reason);
			Prioritizable.RemoveRef(this.equippable.gameObject);
			this.chore = null;
		}
	}

	// Token: 0x06003DA0 RID: 15776 RVA: 0x00154A66 File Offset: 0x00152C66
	private void RefreshChore(IAssignableIdentity target)
	{
		if (this.chore != null)
		{
			this.CancelChore("Equipment Reassigned");
		}
		this.currentTarget = target;
		if (target != null && !target.GetSoleOwner().GetComponent<Equipment>().IsEquipped(this.equippable))
		{
			this.CreateChore();
		}
	}

	// Token: 0x06003DA1 RID: 15777 RVA: 0x00154AA4 File Offset: 0x00152CA4
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (this.equippable.assignee != null)
		{
			Ownables soleOwner = this.equippable.assignee.GetSoleOwner();
			if (soleOwner)
			{
				soleOwner.GetComponent<Equipment>().Equip(this.equippable);
				Prioritizable.RemoveRef(this.equippable.gameObject);
				this.chore = null;
			}
		}
	}

	// Token: 0x06003DA2 RID: 15778 RVA: 0x00154AFF File Offset: 0x00152CFF
	protected override void OnStopWork(WorkerBase worker)
	{
		this.workTimeRemaining = this.GetWorkTime();
		base.OnStopWork(worker);
	}

	// Token: 0x04002598 RID: 9624
	[MyCmpReq]
	private Equippable equippable;

	// Token: 0x04002599 RID: 9625
	private Chore chore;

	// Token: 0x0400259A RID: 9626
	private IAssignableIdentity currentTarget;

	// Token: 0x0400259B RID: 9627
	private global::QualityLevel quality;
}
