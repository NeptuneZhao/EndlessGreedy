using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000531 RID: 1329
[AddComponentMenu("KMonoBehaviour/Workable/Breakable")]
public class Breakable : Workable
{
	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06001DFA RID: 7674 RVA: 0x000A5F39 File Offset: 0x000A4139
	public bool IsInvincible
	{
		get
		{
			return this.hp == null || this.hp.invincible;
		}
	}

	// Token: 0x06001DFB RID: 7675 RVA: 0x000A5F56 File Offset: 0x000A4156
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_break_kanim")
		};
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x06001DFC RID: 7676 RVA: 0x000A5F8E File Offset: 0x000A418E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Breakables.Add(this);
	}

	// Token: 0x06001DFD RID: 7677 RVA: 0x000A5FA1 File Offset: 0x000A41A1
	public bool isBroken()
	{
		return this.hp == null || this.hp.HitPoints <= 0;
	}

	// Token: 0x06001DFE RID: 7678 RVA: 0x000A5FC4 File Offset: 0x000A41C4
	public Notification CreateDamageNotification()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		return new Notification(BUILDING.STATUSITEMS.ANGERDAMAGE.NOTIFICATION, NotificationType.BadMinor, (List<Notification> notificationList, object data) => BUILDING.STATUSITEMS.ANGERDAMAGE.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), component.GetProperName(), false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06001DFF RID: 7679 RVA: 0x000A601C File Offset: 0x000A421C
	private static string ToolTipResolver(List<Notification> notificationList, object data)
	{
		string text = "";
		for (int i = 0; i < notificationList.Count; i++)
		{
			Notification notification = notificationList[i];
			text += (string)notification.tooltipData;
			if (i < notificationList.Count - 1)
			{
				text += "\n";
			}
		}
		return string.Format(BUILDING.STATUSITEMS.ANGERDAMAGE.NOTIFICATION_TOOLTIP, text);
	}

	// Token: 0x06001E00 RID: 7680 RVA: 0x000A6084 File Offset: 0x000A4284
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.secondsPerTenPercentDamage = 2f;
		this.tenPercentDamage = Mathf.CeilToInt((float)this.hp.MaxHitPoints * 0.1f);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.AngerDamage, this);
		this.notification = this.CreateDamageNotification();
		base.gameObject.AddOrGet<Notifier>().Add(this.notification, "");
		this.elapsedDamageTime = 0f;
	}

	// Token: 0x06001E01 RID: 7681 RVA: 0x000A6110 File Offset: 0x000A4310
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.elapsedDamageTime >= this.secondsPerTenPercentDamage)
		{
			this.elapsedDamageTime -= this.elapsedDamageTime;
			base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = this.tenPercentDamage,
				source = BUILDINGS.DAMAGESOURCES.MINION_DESTRUCTION,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.MINION_DESTRUCTION
			});
		}
		this.elapsedDamageTime += dt;
		return this.hp.HitPoints <= 0;
	}

	// Token: 0x06001E02 RID: 7682 RVA: 0x000A61A8 File Offset: 0x000A43A8
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.AngerDamage, false);
		base.gameObject.AddOrGet<Notifier>().Remove(this.notification);
		if (worker != null)
		{
			worker.Trigger(-1734580852, null);
		}
	}

	// Token: 0x06001E03 RID: 7683 RVA: 0x000A6203 File Offset: 0x000A4403
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x06001E04 RID: 7684 RVA: 0x000A6206 File Offset: 0x000A4406
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Breakables.Remove(this);
	}

	// Token: 0x040010DC RID: 4316
	private const float TIME_TO_BREAK_AT_FULL_HEALTH = 20f;

	// Token: 0x040010DD RID: 4317
	private Notification notification;

	// Token: 0x040010DE RID: 4318
	private float secondsPerTenPercentDamage = float.PositiveInfinity;

	// Token: 0x040010DF RID: 4319
	private float elapsedDamageTime;

	// Token: 0x040010E0 RID: 4320
	private int tenPercentDamage = int.MaxValue;

	// Token: 0x040010E1 RID: 4321
	[MyCmpGet]
	private BuildingHP hp;
}
