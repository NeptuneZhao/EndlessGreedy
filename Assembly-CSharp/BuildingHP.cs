using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000674 RID: 1652
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/BuildingHP")]
public class BuildingHP : Workable
{
	// Token: 0x17000203 RID: 515
	// (get) Token: 0x060028E3 RID: 10467 RVA: 0x000E779A File Offset: 0x000E599A
	public int HitPoints
	{
		get
		{
			return this.hitpoints;
		}
	}

	// Token: 0x060028E4 RID: 10468 RVA: 0x000E77A2 File Offset: 0x000E59A2
	public void SetHitPoints(int hp)
	{
		this.hitpoints = hp;
	}

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x060028E5 RID: 10469 RVA: 0x000E77AB File Offset: 0x000E59AB
	public int MaxHitPoints
	{
		get
		{
			return this.building.Def.HitPoints;
		}
	}

	// Token: 0x060028E6 RID: 10470 RVA: 0x000E77BD File Offset: 0x000E59BD
	public BuildingHP.DamageSourceInfo GetDamageSourceInfo()
	{
		return this.damageSourceInfo;
	}

	// Token: 0x060028E7 RID: 10471 RVA: 0x000E77C5 File Offset: 0x000E59C5
	protected override void OnLoadLevel()
	{
		this.smi = null;
		base.OnLoadLevel();
	}

	// Token: 0x060028E8 RID: 10472 RVA: 0x000E77D4 File Offset: 0x000E59D4
	public void DoDamage(int damage)
	{
		if (!this.invincible)
		{
			damage = Math.Max(0, damage);
			this.hitpoints = Math.Max(0, this.hitpoints - damage);
			base.Trigger(-1964935036, this);
		}
	}

	// Token: 0x060028E9 RID: 10473 RVA: 0x000E7808 File Offset: 0x000E5A08
	public void Repair(int repair_amount)
	{
		if (this.hitpoints + repair_amount < this.hitpoints)
		{
			this.hitpoints = this.building.Def.HitPoints;
		}
		else
		{
			this.hitpoints = Math.Min(this.hitpoints + repair_amount, this.building.Def.HitPoints);
		}
		base.Trigger(-1699355994, this);
		if (this.hitpoints >= this.building.Def.HitPoints)
		{
			base.Trigger(-1735440190, this);
		}
	}

	// Token: 0x060028EA RID: 10474 RVA: 0x000E7890 File Offset: 0x000E5A90
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(10f);
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
	}

	// Token: 0x060028EB RID: 10475 RVA: 0x000E78C4 File Offset: 0x000E5AC4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new BuildingHP.SMInstance(this);
		this.smi.StartSM();
		base.Subscribe<BuildingHP>(-794517298, BuildingHP.OnDoBuildingDamageDelegate);
		if (this.destroyOnDamaged)
		{
			base.Subscribe<BuildingHP>(774203113, BuildingHP.DestroyOnDamagedDelegate);
		}
		if (this.hitpoints <= 0)
		{
			base.Trigger(774203113, this);
		}
	}

	// Token: 0x060028EC RID: 10476 RVA: 0x000E792D File Offset: 0x000E5B2D
	private void DestroyOnDamaged(object data)
	{
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060028ED RID: 10477 RVA: 0x000E793C File Offset: 0x000E5B3C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		int num = (int)Db.Get().Attributes.Machinery.Lookup(worker).GetTotalValue();
		int repair_amount = 10 + Math.Max(0, num * 10);
		this.Repair(repair_amount);
	}

	// Token: 0x060028EE RID: 10478 RVA: 0x000E797A File Offset: 0x000E5B7A
	private void OnDoBuildingDamage(object data)
	{
		if (this.invincible)
		{
			return;
		}
		this.damageSourceInfo = (BuildingHP.DamageSourceInfo)data;
		this.DoDamage(this.damageSourceInfo.damage);
		this.DoDamagePopFX(this.damageSourceInfo);
		this.DoTakeDamageFX(this.damageSourceInfo);
	}

	// Token: 0x060028EF RID: 10479 RVA: 0x000E79BC File Offset: 0x000E5BBC
	private void DoTakeDamageFX(BuildingHP.DamageSourceInfo info)
	{
		if (info.takeDamageEffect != SpawnFXHashes.None)
		{
			BuildingDef def = base.GetComponent<BuildingComplete>().Def;
			int cell = Grid.OffsetCell(Grid.PosToCell(this), 0, def.HeightInCells - 1);
			Game.Instance.SpawnFX(info.takeDamageEffect, cell, 0f);
		}
	}

	// Token: 0x060028F0 RID: 10480 RVA: 0x000E7A08 File Offset: 0x000E5C08
	private void DoDamagePopFX(BuildingHP.DamageSourceInfo info)
	{
		if (info.popString != null && Time.time > this.lastPopTime + this.minDamagePopInterval)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, info.popString, base.gameObject.transform, 1.5f, false);
			this.lastPopTime = Time.time;
		}
	}

	// Token: 0x17000205 RID: 517
	// (get) Token: 0x060028F1 RID: 10481 RVA: 0x000E7A68 File Offset: 0x000E5C68
	public bool IsBroken
	{
		get
		{
			return this.hitpoints == 0;
		}
	}

	// Token: 0x17000206 RID: 518
	// (get) Token: 0x060028F2 RID: 10482 RVA: 0x000E7A73 File Offset: 0x000E5C73
	public bool NeedsRepairs
	{
		get
		{
			return this.HitPoints < this.building.Def.HitPoints;
		}
	}

	// Token: 0x0400177A RID: 6010
	[Serialize]
	[SerializeField]
	private int hitpoints;

	// Token: 0x0400177B RID: 6011
	[Serialize]
	private BuildingHP.DamageSourceInfo damageSourceInfo;

	// Token: 0x0400177C RID: 6012
	private static readonly EventSystem.IntraObjectHandler<BuildingHP> OnDoBuildingDamageDelegate = new EventSystem.IntraObjectHandler<BuildingHP>(delegate(BuildingHP component, object data)
	{
		component.OnDoBuildingDamage(data);
	});

	// Token: 0x0400177D RID: 6013
	private static readonly EventSystem.IntraObjectHandler<BuildingHP> DestroyOnDamagedDelegate = new EventSystem.IntraObjectHandler<BuildingHP>(delegate(BuildingHP component, object data)
	{
		component.DestroyOnDamaged(data);
	});

	// Token: 0x0400177E RID: 6014
	public static List<Meter> kbacQueryList = new List<Meter>();

	// Token: 0x0400177F RID: 6015
	public bool destroyOnDamaged;

	// Token: 0x04001780 RID: 6016
	public bool invincible;

	// Token: 0x04001781 RID: 6017
	[MyCmpGet]
	private Building building;

	// Token: 0x04001782 RID: 6018
	private BuildingHP.SMInstance smi;

	// Token: 0x04001783 RID: 6019
	private float minDamagePopInterval = 4f;

	// Token: 0x04001784 RID: 6020
	private float lastPopTime;

	// Token: 0x02001456 RID: 5206
	public struct DamageSourceInfo
	{
		// Token: 0x06008A2F RID: 35375 RVA: 0x003329F0 File Offset: 0x00330BF0
		public override string ToString()
		{
			return this.source;
		}

		// Token: 0x0400696B RID: 26987
		public int damage;

		// Token: 0x0400696C RID: 26988
		public string source;

		// Token: 0x0400696D RID: 26989
		public string popString;

		// Token: 0x0400696E RID: 26990
		public SpawnFXHashes takeDamageEffect;

		// Token: 0x0400696F RID: 26991
		public string fullDamageEffectName;

		// Token: 0x04006970 RID: 26992
		public string statusItemID;
	}

	// Token: 0x02001457 RID: 5207
	public class SMInstance : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.GameInstance
	{
		// Token: 0x06008A30 RID: 35376 RVA: 0x003329F8 File Offset: 0x00330BF8
		public SMInstance(BuildingHP master) : base(master)
		{
		}

		// Token: 0x06008A31 RID: 35377 RVA: 0x00332A04 File Offset: 0x00330C04
		public Notification CreateBrokenMachineNotification()
		{
			return new Notification(MISC.NOTIFICATIONS.BROKENMACHINE.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.BROKENMACHINE.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + base.master.damageSourceInfo.source, false, 0f, null, null, null, true, false, false);
		}

		// Token: 0x06008A32 RID: 35378 RVA: 0x00332A68 File Offset: 0x00330C68
		public void ShowProgressBar(bool show)
		{
			if (show && Grid.IsValidCell(Grid.PosToCell(base.gameObject)) && Grid.IsVisible(Grid.PosToCell(base.gameObject)))
			{
				this.CreateProgressBar();
				return;
			}
			if (this.progressBar != null)
			{
				this.progressBar.gameObject.DeleteObject();
				this.progressBar = null;
			}
		}

		// Token: 0x06008A33 RID: 35379 RVA: 0x00332AC8 File Offset: 0x00330CC8
		public void UpdateMeter()
		{
			if (this.progressBar == null)
			{
				this.ShowProgressBar(true);
			}
			if (this.progressBar)
			{
				this.progressBar.Update();
			}
		}

		// Token: 0x06008A34 RID: 35380 RVA: 0x00332AF7 File Offset: 0x00330CF7
		private float HealthPercent()
		{
			return (float)base.smi.master.HitPoints / (float)base.smi.master.building.Def.HitPoints;
		}

		// Token: 0x06008A35 RID: 35381 RVA: 0x00332B28 File Offset: 0x00330D28
		private void CreateProgressBar()
		{
			if (this.progressBar != null)
			{
				return;
			}
			this.progressBar = Util.KInstantiateUI<ProgressBar>(ProgressBarsConfig.Instance.progressBarPrefab, null, false);
			this.progressBar.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform);
			this.progressBar.name = base.smi.master.name + "." + base.smi.master.GetType().Name + " ProgressBar";
			this.progressBar.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("ProgressBar");
			this.progressBar.SetUpdateFunc(new Func<float>(this.HealthPercent));
			this.progressBar.barColor = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
			CanvasGroup component = this.progressBar.GetComponent<CanvasGroup>();
			component.interactable = false;
			component.blocksRaycasts = false;
			this.progressBar.Update();
			float d = 0.15f;
			Vector3 vector = base.gameObject.transform.GetPosition() + Vector3.down * d;
			vector.z += 0.05f;
			Rotatable component2 = base.GetComponent<Rotatable>();
			if (component2 == null || component2.GetOrientation() == Orientation.Neutral || base.smi.master.building.Def.WidthInCells < 2 || base.smi.master.building.Def.HeightInCells < 2)
			{
				vector -= Vector3.right * 0.5f * (float)(base.smi.master.building.Def.WidthInCells % 2);
			}
			else
			{
				vector += Vector3.left * (1f + 0.5f * (float)(base.smi.master.building.Def.WidthInCells % 2));
			}
			this.progressBar.transform.SetPosition(vector);
			this.progressBar.SetVisibility(true);
		}

		// Token: 0x06008A36 RID: 35382 RVA: 0x00332D58 File Offset: 0x00330F58
		private static string ToolTipResolver(List<Notification> notificationList, object data)
		{
			string text = "";
			for (int i = 0; i < notificationList.Count; i++)
			{
				Notification notification = notificationList[i];
				text += string.Format(BUILDINGS.DAMAGESOURCES.NOTIFICATION_TOOLTIP, notification.NotifierName, (string)notification.tooltipData);
				if (i < notificationList.Count - 1)
				{
					text += "\n";
				}
			}
			return text;
		}

		// Token: 0x06008A37 RID: 35383 RVA: 0x00332DC4 File Offset: 0x00330FC4
		public void ShowDamagedEffect()
		{
			if (base.master.damageSourceInfo.takeDamageEffect != SpawnFXHashes.None)
			{
				BuildingDef def = base.master.GetComponent<BuildingComplete>().Def;
				int cell = Grid.OffsetCell(Grid.PosToCell(base.master), 0, def.HeightInCells - 1);
				Game.Instance.SpawnFX(base.master.damageSourceInfo.takeDamageEffect, cell, 0f);
			}
		}

		// Token: 0x06008A38 RID: 35384 RVA: 0x00332E30 File Offset: 0x00331030
		public FXAnim.Instance InstantiateDamageFX()
		{
			if (base.master.damageSourceInfo.fullDamageEffectName == null)
			{
				return null;
			}
			BuildingDef def = base.master.GetComponent<BuildingComplete>().Def;
			Vector3 zero = Vector3.zero;
			if (def.HeightInCells > 1)
			{
				zero = new Vector3(0f, (float)(def.HeightInCells - 1), 0f);
			}
			else
			{
				zero = new Vector3(0f, 0.5f, 0f);
			}
			return new FXAnim.Instance(base.smi.master, base.master.damageSourceInfo.fullDamageEffectName, "idle", KAnim.PlayMode.Loop, zero, Color.white);
		}

		// Token: 0x06008A39 RID: 35385 RVA: 0x00332ED4 File Offset: 0x003310D4
		public void SetCrackOverlayValue(float value)
		{
			KBatchedAnimController component = base.master.GetComponent<KBatchedAnimController>();
			if (component == null)
			{
				return;
			}
			component.SetBlendValue(value);
			BuildingHP.kbacQueryList.Clear();
			base.master.GetComponentsInChildren<Meter>(BuildingHP.kbacQueryList);
			for (int i = 0; i < BuildingHP.kbacQueryList.Count; i++)
			{
				BuildingHP.kbacQueryList[i].GetComponent<KBatchedAnimController>().SetBlendValue(value);
			}
		}

		// Token: 0x04006971 RID: 26993
		private ProgressBar progressBar;
	}

	// Token: 0x02001458 RID: 5208
	public class States : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP>
	{
		// Token: 0x06008A3A RID: 35386 RVA: 0x00332F44 File Offset: 0x00331144
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.healthy;
			this.healthy.DefaultState(this.healthy.imperfect).EventTransition(GameHashes.BuildingReceivedDamage, this.damaged, (BuildingHP.SMInstance smi) => smi.master.HitPoints <= 0);
			this.healthy.imperfect.Enter(delegate(BuildingHP.SMInstance smi)
			{
				smi.ShowProgressBar(true);
			}).DefaultState(this.healthy.imperfect.playEffect).EventTransition(GameHashes.BuildingPartiallyRepaired, this.healthy.perfect, (BuildingHP.SMInstance smi) => smi.master.HitPoints == smi.master.building.Def.HitPoints).EventHandler(GameHashes.BuildingPartiallyRepaired, delegate(BuildingHP.SMInstance smi)
			{
				smi.UpdateMeter();
			}).ToggleStatusItem(delegate(BuildingHP.SMInstance smi)
			{
				if (smi.master.damageSourceInfo.statusItemID == null)
				{
					return null;
				}
				return Db.Get().BuildingStatusItems.Get(smi.master.damageSourceInfo.statusItemID);
			}, null).Exit(delegate(BuildingHP.SMInstance smi)
			{
				smi.ShowProgressBar(false);
			});
			this.healthy.imperfect.playEffect.Transition(this.healthy.imperfect.waiting, (BuildingHP.SMInstance smi) => true, UpdateRate.SIM_200ms);
			this.healthy.imperfect.waiting.ScheduleGoTo((BuildingHP.SMInstance smi) => UnityEngine.Random.Range(15f, 30f), this.healthy.imperfect.playEffect);
			this.healthy.perfect.EventTransition(GameHashes.BuildingReceivedDamage, this.healthy.imperfect, (BuildingHP.SMInstance smi) => smi.master.HitPoints < smi.master.building.Def.HitPoints);
			this.damaged.Enter(delegate(BuildingHP.SMInstance smi)
			{
				Operational component = smi.GetComponent<Operational>();
				if (component != null)
				{
					component.SetFlag(BuildingHP.States.healthyFlag, false);
				}
				smi.ShowProgressBar(true);
				smi.master.Trigger(774203113, smi.master);
				smi.SetCrackOverlayValue(1f);
			}).ToggleNotification((BuildingHP.SMInstance smi) => smi.CreateBrokenMachineNotification()).ToggleStatusItem(Db.Get().BuildingStatusItems.Broken, null).ToggleFX((BuildingHP.SMInstance smi) => smi.InstantiateDamageFX()).EventTransition(GameHashes.BuildingPartiallyRepaired, this.healthy.perfect, (BuildingHP.SMInstance smi) => smi.master.HitPoints == smi.master.building.Def.HitPoints).EventHandler(GameHashes.BuildingPartiallyRepaired, delegate(BuildingHP.SMInstance smi)
			{
				smi.UpdateMeter();
			}).Exit(delegate(BuildingHP.SMInstance smi)
			{
				Operational component = smi.GetComponent<Operational>();
				if (component != null)
				{
					component.SetFlag(BuildingHP.States.healthyFlag, true);
				}
				smi.ShowProgressBar(false);
				smi.SetCrackOverlayValue(0f);
			});
		}

		// Token: 0x06008A3B RID: 35387 RVA: 0x00333268 File Offset: 0x00331468
		private Chore CreateRepairChore(BuildingHP.SMInstance smi)
		{
			return new WorkChore<BuildingHP>(Db.Get().ChoreTypes.Repair, smi.master, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x04006972 RID: 26994
		private static readonly Operational.Flag healthyFlag = new Operational.Flag("healthy", Operational.Flag.Type.Functional);

		// Token: 0x04006973 RID: 26995
		public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State damaged;

		// Token: 0x04006974 RID: 26996
		public BuildingHP.States.Healthy healthy;

		// Token: 0x020024B8 RID: 9400
		public class Healthy : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State
		{
			// Token: 0x0400A2BF RID: 41663
			public BuildingHP.States.ImperfectStates imperfect;

			// Token: 0x0400A2C0 RID: 41664
			public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State perfect;
		}

		// Token: 0x020024B9 RID: 9401
		public class ImperfectStates : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State
		{
			// Token: 0x0400A2C1 RID: 41665
			public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State playEffect;

			// Token: 0x0400A2C2 RID: 41666
			public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State waiting;
		}
	}
}
