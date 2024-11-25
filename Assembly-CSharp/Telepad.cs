using System;
using System.Collections;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200077F RID: 1919
public class Telepad : StateMachineComponent<Telepad.StatesInstance>
{
	// Token: 0x06003426 RID: 13350 RVA: 0x0011CC14 File Offset: 0x0011AE14
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<Deconstructable>().allowDeconstruction = false;
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		if (num == 0)
		{
			global::Debug.LogError(string.Concat(new string[]
			{
				"Headquarters spawned at: (",
				num.ToString(),
				",",
				num2.ToString(),
				")"
			}));
		}
	}

	// Token: 0x06003427 RID: 13351 RVA: 0x0011CC88 File Offset: 0x0011AE88
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Telepads.Add(this);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimController>().SetDirty();
		base.smi.StartSM();
	}

	// Token: 0x06003428 RID: 13352 RVA: 0x0011CD0A File Offset: 0x0011AF0A
	protected override void OnCleanUp()
	{
		Components.Telepads.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06003429 RID: 13353 RVA: 0x0011CD20 File Offset: 0x0011AF20
	public void Update()
	{
		if (base.smi.IsColonyLost())
		{
			return;
		}
		if (Immigration.Instance.ImmigrantsAvailable && base.GetComponent<Operational>().IsOperational)
		{
			base.smi.sm.openPortal.Trigger(base.smi);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.NewDuplicantsAvailable, this);
		}
		else
		{
			base.smi.sm.closePortal.Trigger(base.smi);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Wattson, this);
		}
		if (this.GetTimeRemaining() < -120f)
		{
			Messenger.Instance.QueueMessage(new DuplicantsLeftMessage());
			Immigration.Instance.EndImmigration();
		}
	}

	// Token: 0x0600342A RID: 13354 RVA: 0x0011CE09 File Offset: 0x0011B009
	public void RejectAll()
	{
		Immigration.Instance.EndImmigration();
		base.smi.sm.closePortal.Trigger(base.smi);
	}

	// Token: 0x0600342B RID: 13355 RVA: 0x0011CE34 File Offset: 0x0011B034
	public void OnAcceptDelivery(ITelepadDeliverable delivery)
	{
		int cell = Grid.PosToCell(this);
		Immigration.Instance.EndImmigration();
		GameObject gameObject = delivery.Deliver(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
		MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
		if (component != null)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.PersonalTime, GameClock.Instance.GetTimeSinceStartOfReport(), string.Format(UI.ENDOFDAYREPORT.NOTES.PERSONAL_TIME, DUPLICANTS.CHORES.NOT_EXISTING_TASK), gameObject.GetProperName());
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.gameObject.GetComponent<KSelectable>().GetMyWorldId(), false))
			{
				minionIdentity.GetComponent<Effects>().Add("NewCrewArrival", true);
			}
			MinionResume component2 = component.GetComponent<MinionResume>();
			int num = 0;
			while ((float)num < this.startingSkillPoints)
			{
				component2.ForceAddSkillPoint();
				num++;
			}
			if (component.HasTag(GameTags.Minions.Models.Bionic))
			{
				GameScheduler.Instance.Schedule("BonusBatteryDelivery", 5f, delegate(object data)
				{
					base.Trigger(1982288670, null);
				}, null, null);
			}
		}
		base.smi.sm.closePortal.Trigger(base.smi);
	}

	// Token: 0x0600342C RID: 13356 RVA: 0x0011CF78 File Offset: 0x0011B178
	public float GetTimeRemaining()
	{
		return Immigration.Instance.GetTimeRemaining();
	}

	// Token: 0x04001ED4 RID: 7892
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001ED5 RID: 7893
	private MeterController meter;

	// Token: 0x04001ED6 RID: 7894
	private const float MAX_IMMIGRATION_TIME = 120f;

	// Token: 0x04001ED7 RID: 7895
	private const int NUM_METER_NOTCHES = 8;

	// Token: 0x04001ED8 RID: 7896
	private List<MinionStartingStats> minionStats;

	// Token: 0x04001ED9 RID: 7897
	public float startingSkillPoints;

	// Token: 0x04001EDA RID: 7898
	public static readonly HashedString[] PortalBirthAnim = new HashedString[]
	{
		"portalbirth"
	};

	// Token: 0x02001625 RID: 5669
	public class StatesInstance : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.GameInstance
	{
		// Token: 0x0600911A RID: 37146 RVA: 0x0034F0A9 File Offset: 0x0034D2A9
		public StatesInstance(Telepad master) : base(master)
		{
		}

		// Token: 0x0600911B RID: 37147 RVA: 0x0034F0B2 File Offset: 0x0034D2B2
		public bool IsColonyLost()
		{
			return GameFlowManager.Instance != null && GameFlowManager.Instance.IsGameOver();
		}

		// Token: 0x0600911C RID: 37148 RVA: 0x0034F0D0 File Offset: 0x0034D2D0
		public void UpdateMeter()
		{
			float timeRemaining = Immigration.Instance.GetTimeRemaining();
			float totalWaitTime = Immigration.Instance.GetTotalWaitTime();
			float positionPercent = Mathf.Clamp01(1f - timeRemaining / totalWaitTime);
			base.master.meter.SetPositionPercent(positionPercent);
		}

		// Token: 0x0600911D RID: 37149 RVA: 0x0034F113 File Offset: 0x0034D313
		public IEnumerator SpawnExtraPowerBanks()
		{
			int cellTarget = Grid.OffsetCell(Grid.PosToCell(base.gameObject), 1, 2);
			int count = 5;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, MISC.POPFX.EXTRA_POWERBANKS_BIONIC, base.gameObject.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("SandboxTool_Spawner", false));
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_BasicSingleHarvestPlant"), Grid.CellToPosCBC(cellTarget, Grid.SceneLayer.Front) - Vector3.right / 2f);
				gameObject.SetActive(true);
				Vector2 initial_velocity = new Vector2((-2.5f + 5f * ((float)i / 5f)) / 2f, 2f);
				if (GameComps.Fallers.Has(gameObject))
				{
					GameComps.Fallers.Remove(gameObject);
				}
				GameComps.Fallers.Add(gameObject, initial_velocity);
				yield return new WaitForSeconds(0.25f);
				num = i;
			}
			yield return 0;
			yield break;
		}
	}

	// Token: 0x02001626 RID: 5670
	public class States : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad>
	{
		// Token: 0x0600911E RID: 37150 RVA: 0x0034F124 File Offset: 0x0034D324
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.OnSignal(this.idlePortal, this.resetToIdle).EventTransition(GameHashes.BonusTelepadDelivery, this.bonusDelivery.pre, null);
			this.resetToIdle.GoTo(this.idle);
			this.idle.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.UpdateMeter();
			}).Update("TelepadMeter", delegate(Telepad.StatesInstance smi, float dt)
			{
				smi.UpdateMeter();
			}, UpdateRate.SIM_4000ms, false).EventTransition(GameHashes.OperationalChanged, this.unoperational, (Telepad.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).PlayAnim("idle").OnSignal(this.openPortal, this.opening);
			this.unoperational.PlayAnim("idle").Enter("StopImmigration", delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(0f);
			}).EventTransition(GameHashes.OperationalChanged, this.idle, (Telepad.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.opening.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(1f);
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.open);
			this.open.OnSignal(this.closePortal, this.close).Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(1f);
			}).PlayAnim("working_loop", KAnim.PlayMode.Loop).Transition(this.close, (Telepad.StatesInstance smi) => smi.IsColonyLost(), UpdateRate.SIM_200ms).EventTransition(GameHashes.OperationalChanged, this.close, (Telepad.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.close.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(0f);
			}).PlayAnims((Telepad.StatesInstance smi) => Telepad.States.workingAnims, KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
			this.bonusDelivery.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.bonusDelivery.loop);
			this.bonusDelivery.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ScheduleAction("SpawnBonusDelivery", 1f, delegate(Telepad.StatesInstance smi)
			{
				smi.master.StartCoroutine(smi.SpawnExtraPowerBanks());
			}).ScheduleGoTo(3f, this.bonusDelivery.pst);
			this.bonusDelivery.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.idle);
		}

		// Token: 0x04006EC2 RID: 28354
		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal openPortal;

		// Token: 0x04006EC3 RID: 28355
		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal closePortal;

		// Token: 0x04006EC4 RID: 28356
		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal idlePortal;

		// Token: 0x04006EC5 RID: 28357
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State idle;

		// Token: 0x04006EC6 RID: 28358
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State resetToIdle;

		// Token: 0x04006EC7 RID: 28359
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State opening;

		// Token: 0x04006EC8 RID: 28360
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State open;

		// Token: 0x04006EC9 RID: 28361
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State close;

		// Token: 0x04006ECA RID: 28362
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State unoperational;

		// Token: 0x04006ECB RID: 28363
		public Telepad.States.BonusDeliveryStates bonusDelivery;

		// Token: 0x04006ECC RID: 28364
		private static readonly HashedString[] workingAnims = new HashedString[]
		{
			"working_loop",
			"working_pst"
		};

		// Token: 0x02002545 RID: 9541
		public class BonusDeliveryStates : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State
		{
			// Token: 0x0400A60B RID: 42507
			public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State pre;

			// Token: 0x0400A60C RID: 42508
			public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State loop;

			// Token: 0x0400A60D RID: 42509
			public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State pst;
		}
	}
}
