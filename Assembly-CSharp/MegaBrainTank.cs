using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000728 RID: 1832
public class MegaBrainTank : StateMachineComponent<MegaBrainTank.StatesInstance>
{
	// Token: 0x0600309D RID: 12445 RVA: 0x0010C273 File Offset: 0x0010A473
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600309E RID: 12446 RVA: 0x0010C27C File Offset: 0x0010A47C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		StoryManager.Instance.ForceCreateStory(Db.Get().Stories.MegaBrainTank, base.gameObject.GetMyWorldId());
		base.smi.StartSM();
		base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
		base.GetComponent<Activatable>().SetWorkTime(5f);
		base.smi.JournalDelivery.refillMass = 25f;
		base.smi.JournalDelivery.FillToCapacity = true;
	}

	// Token: 0x0600309F RID: 12447 RVA: 0x0010C30C File Offset: 0x0010A50C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		base.Unsubscribe(-1503271301);
	}

	// Token: 0x060030A0 RID: 12448 RVA: 0x0010C320 File Offset: 0x0010A520
	private void OnBuildingSelect(object obj)
	{
		if (!(bool)obj)
		{
			return;
		}
		if (!this.introDisplayed)
		{
			this.introDisplayed = true;
			EventInfoScreen.ShowPopup(EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.BEGIN_POPUP.NAME, CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.BEGIN_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.CLOSE_BUTTON, "braintankdiscovered_kanim", EventInfoDataHelper.PopupType.BEGIN, null, null, new System.Action(this.DoInitialUnlock)));
		}
		base.smi.ShowEventCompleteUI(null);
	}

	// Token: 0x060030A1 RID: 12449 RVA: 0x0010C38E File Offset: 0x0010A58E
	private void DoInitialUnlock()
	{
		Game.Instance.unlocks.Unlock("story_trait_mega_brain_tank_initial", true);
	}

	// Token: 0x04001C83 RID: 7299
	[Serialize]
	private bool introDisplayed;

	// Token: 0x02001572 RID: 5490
	public class States : GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank>
	{
		// Token: 0x06008E6D RID: 36461 RVA: 0x003436F0 File Offset: 0x003418F0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			default_state = this.root;
			this.root.Enter(delegate(MegaBrainTank.StatesInstance smi)
			{
				if (!StoryManager.Instance.CheckState(StoryInstance.State.COMPLETE, Db.Get().Stories.MegaBrainTank))
				{
					smi.GoTo(this.common.dormant);
					return;
				}
				if (smi.IsHungry)
				{
					smi.GoTo(this.common.idle);
					return;
				}
				smi.GoTo(this.common.active);
			});
			this.common.Update(delegate(MegaBrainTank.StatesInstance smi, float dt)
			{
				smi.IncrementMeter(dt);
				if (smi.UnitsFromLastStore != 0)
				{
					smi.ShelveJournals(dt);
				}
				bool flag = smi.ElementConverter.HasEnoughMass(GameTags.Oxygen, true);
				smi.Selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.MegaBrainNotEnoughOxygen, !flag, null);
			}, UpdateRate.SIM_33ms, false);
			this.common.dormant.Enter(delegate(MegaBrainTank.StatesInstance smi)
			{
				smi.SetBonusActive(false);
				smi.ElementConverter.SetAllConsumedActive(false);
				smi.ElementConverter.SetConsumedElementActive(DreamJournalConfig.ID, false);
				smi.Selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankDreamAnalysis, false);
				smi.master.GetComponent<Light2D>().enabled = false;
			}).Exit(delegate(MegaBrainTank.StatesInstance smi)
			{
				smi.ElementConverter.SetConsumedElementActive(DreamJournalConfig.ID, true);
				smi.ElementConverter.SetConsumedElementActive(GameTags.Oxygen, true);
				RequireInputs component = smi.GetComponent<RequireInputs>();
				component.requireConduitHasMass = true;
				component.visualizeRequirements = RequireInputs.Requirements.All;
			}).Update(delegate(MegaBrainTank.StatesInstance smi, float dt)
			{
				smi.ActivateBrains(dt);
			}, UpdateRate.SIM_33ms, false).OnSignal(this.storyTraitCompleted, this.common.active);
			this.common.idle.Enter(delegate(MegaBrainTank.StatesInstance smi)
			{
				smi.CleanTank(false);
			}).UpdateTransition(this.common.active, (MegaBrainTank.StatesInstance smi, float _) => !smi.IsHungry && smi.gameObject.GetComponent<Operational>().enabled, UpdateRate.SIM_1000ms, false);
			this.common.active.Enter(delegate(MegaBrainTank.StatesInstance smi)
			{
				smi.CleanTank(true);
			}).Update(delegate(MegaBrainTank.StatesInstance smi, float dt)
			{
				smi.Digest(dt);
			}, UpdateRate.SIM_33ms, false).UpdateTransition(this.common.idle, (MegaBrainTank.StatesInstance smi, float _) => smi.IsHungry || !smi.gameObject.GetComponent<Operational>().enabled, UpdateRate.SIM_1000ms, false);
			this.StatBonus = new Effect("MegaBrainTankBonus", DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.NAME, DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.TOOLTIP, 0f, true, true, false, null, -1f, 0f, null, "");
			object[,] stat_BONUSES = MegaBrainTankConfig.STAT_BONUSES;
			int length = stat_BONUSES.GetLength(0);
			for (int i = 0; i < length; i++)
			{
				string attribute_id = stat_BONUSES[i, 0] as string;
				float? num = (float?)stat_BONUSES[i, 1];
				Units? units = (Units?)stat_BONUSES[i, 2];
				this.StatBonus.Add(new AttributeModifier(attribute_id, ModifierSet.ConvertValue(num.Value, units.Value), DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.NAME, false, false, true));
			}
		}

		// Token: 0x04006CB9 RID: 27833
		public MegaBrainTank.States.CommonState common;

		// Token: 0x04006CBA RID: 27834
		public StateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.Signal storyTraitCompleted;

		// Token: 0x04006CBB RID: 27835
		public Effect StatBonus;

		// Token: 0x02002509 RID: 9481
		public class CommonState : GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State
		{
			// Token: 0x0400A4CF RID: 42191
			public GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State dormant;

			// Token: 0x0400A4D0 RID: 42192
			public GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State idle;

			// Token: 0x0400A4D1 RID: 42193
			public GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State active;
		}
	}

	// Token: 0x02001573 RID: 5491
	public class StatesInstance : GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.GameInstance
	{
		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06008E70 RID: 36464 RVA: 0x003439EE File Offset: 0x00341BEE
		public KBatchedAnimController BrainController
		{
			get
			{
				return this.controllers[0];
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06008E71 RID: 36465 RVA: 0x003439F8 File Offset: 0x00341BF8
		public KBatchedAnimController ShelfController
		{
			get
			{
				return this.controllers[1];
			}
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06008E72 RID: 36466 RVA: 0x00343A02 File Offset: 0x00341C02
		// (set) Token: 0x06008E73 RID: 36467 RVA: 0x00343A0A File Offset: 0x00341C0A
		public Storage BrainStorage { get; private set; }

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x06008E74 RID: 36468 RVA: 0x00343A13 File Offset: 0x00341C13
		// (set) Token: 0x06008E75 RID: 36469 RVA: 0x00343A1B File Offset: 0x00341C1B
		public KSelectable Selectable { get; private set; }

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x06008E76 RID: 36470 RVA: 0x00343A24 File Offset: 0x00341C24
		// (set) Token: 0x06008E77 RID: 36471 RVA: 0x00343A2C File Offset: 0x00341C2C
		public Operational Operational { get; private set; }

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x06008E78 RID: 36472 RVA: 0x00343A35 File Offset: 0x00341C35
		// (set) Token: 0x06008E79 RID: 36473 RVA: 0x00343A3D File Offset: 0x00341C3D
		public ElementConverter ElementConverter { get; private set; }

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x06008E7A RID: 36474 RVA: 0x00343A46 File Offset: 0x00341C46
		// (set) Token: 0x06008E7B RID: 36475 RVA: 0x00343A4E File Offset: 0x00341C4E
		public ManualDeliveryKG JournalDelivery { get; private set; }

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x06008E7C RID: 36476 RVA: 0x00343A57 File Offset: 0x00341C57
		// (set) Token: 0x06008E7D RID: 36477 RVA: 0x00343A5F File Offset: 0x00341C5F
		public LoopingSounds BrainSounds { get; private set; }

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x06008E7E RID: 36478 RVA: 0x00343A68 File Offset: 0x00341C68
		public bool IsHungry
		{
			get
			{
				return !this.ElementConverter.HasEnoughMassToStartConverting(true);
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06008E7F RID: 36479 RVA: 0x00343A79 File Offset: 0x00341C79
		public int TimeTilDigested
		{
			get
			{
				return (int)this.timeTilDigested;
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06008E80 RID: 36480 RVA: 0x00343A82 File Offset: 0x00341C82
		public int ActivationProgress
		{
			get
			{
				return (int)(25f * this.meterFill);
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06008E81 RID: 36481 RVA: 0x00343A91 File Offset: 0x00341C91
		public HashedString CurrentActivationAnim
		{
			get
			{
				return MegaBrainTankConfig.ACTIVATION_ANIMS[(int)(this.nextActiveBrain - 1)];
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06008E82 RID: 36482 RVA: 0x00343AA8 File Offset: 0x00341CA8
		private HashedString currentActivationLoop
		{
			get
			{
				int num = (int)(this.nextActiveBrain - 1 + 5);
				return MegaBrainTankConfig.ACTIVATION_ANIMS[num];
			}
		}

		// Token: 0x06008E83 RID: 36483 RVA: 0x00343ACC File Offset: 0x00341CCC
		public StatesInstance(MegaBrainTank master) : base(master)
		{
			this.BrainSounds = base.GetComponent<LoopingSounds>();
			this.BrainStorage = base.GetComponent<Storage>();
			this.ElementConverter = base.GetComponent<ElementConverter>();
			this.JournalDelivery = base.GetComponent<ManualDeliveryKG>();
			this.Operational = base.GetComponent<Operational>();
			this.Selectable = base.GetComponent<KSelectable>();
			this.notifier = base.GetComponent<Notifier>();
			this.controllers = base.gameObject.GetComponentsInChildren<KBatchedAnimController>();
			this.meter = new MeterController(this.BrainController, "meter_oxygen_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, MegaBrainTankConfig.METER_SYMBOLS);
			this.fxLink = new KAnimLink(this.BrainController, this.ShelfController);
		}

		// Token: 0x06008E84 RID: 36484 RVA: 0x00343B94 File Offset: 0x00341D94
		public override void StartSM()
		{
			this.InitializeEffectsList();
			base.StartSM();
			this.BrainController.onAnimComplete += this.OnAnimComplete;
			this.ShelfController.onAnimComplete += this.OnAnimComplete;
			Storage brainStorage = this.BrainStorage;
			brainStorage.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(brainStorage.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnJournalDeliveryStateChanged));
			this.brainHum = GlobalAssets.GetSound("MegaBrainTank_brain_wave_LP", false);
			StoryManager.Instance.DiscoverStoryEvent(Db.Get().Stories.MegaBrainTank);
			float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			if (this.GetCurrentState() == base.sm.common.dormant)
			{
				this.meterFill = (this.targetProgress = unitsAvailable / 25f);
				this.meter.SetPositionPercent(this.meterFill);
				short num = (short)(5f * this.meterFill);
				if (num > 0)
				{
					this.nextActiveBrain = num;
					this.BrainSounds.StartSound(this.brainHum);
					this.BrainSounds.SetParameter(this.brainHum, "BrainTankProgress", (float)num);
					this.CompleteBrainActivation();
				}
				return;
			}
			this.timeTilDigested = unitsAvailable * 60f;
			this.meterFill = this.timeTilDigested - this.timeTilDigested % 0.04f;
			this.meterFill /= 1500f;
			this.meter.SetPositionPercent(this.meterFill);
			StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.MegaBrainTank);
			this.nextActiveBrain = 5;
			this.CompleteBrainActivation();
		}

		// Token: 0x06008E85 RID: 36485 RVA: 0x00343D3C File Offset: 0x00341F3C
		public override void StopSM(string reason)
		{
			this.BrainController.onAnimComplete -= this.OnAnimComplete;
			this.ShelfController.onAnimComplete -= this.OnAnimComplete;
			Storage brainStorage = this.BrainStorage;
			brainStorage.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(brainStorage.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnJournalDeliveryStateChanged));
			base.StopSM(reason);
		}

		// Token: 0x06008E86 RID: 36486 RVA: 0x00343DA8 File Offset: 0x00341FA8
		private void InitializeEffectsList()
		{
			Components.Cmps<MinionIdentity> liveMinionIdentities = Components.LiveMinionIdentities;
			liveMinionIdentities.OnAdd += this.OnLiveMinionIdAdded;
			liveMinionIdentities.OnRemove += this.OnLiveMinionIdRemoved;
			MegaBrainTank.StatesInstance.minionEffects = new List<Effects>((liveMinionIdentities.Count > 32) ? liveMinionIdentities.Count : 32);
			for (int i = 0; i < liveMinionIdentities.Count; i++)
			{
				this.OnLiveMinionIdAdded(liveMinionIdentities[i]);
			}
		}

		// Token: 0x06008E87 RID: 36487 RVA: 0x00343E1C File Offset: 0x0034201C
		private void OnLiveMinionIdAdded(MinionIdentity id)
		{
			Effects component = id.GetComponent<Effects>();
			MegaBrainTank.StatesInstance.minionEffects.Add(component);
			if (this.GetCurrentState() == base.sm.common.active)
			{
				component.Add(base.sm.StatBonus, false);
			}
		}

		// Token: 0x06008E88 RID: 36488 RVA: 0x00343E68 File Offset: 0x00342068
		private void OnLiveMinionIdRemoved(MinionIdentity id)
		{
			Effects component = id.GetComponent<Effects>();
			MegaBrainTank.StatesInstance.minionEffects.Remove(component);
		}

		// Token: 0x06008E89 RID: 36489 RVA: 0x00343E88 File Offset: 0x00342088
		public void SetBonusActive(bool active)
		{
			for (int i = 0; i < MegaBrainTank.StatesInstance.minionEffects.Count; i++)
			{
				if (active)
				{
					MegaBrainTank.StatesInstance.minionEffects[i].Add(base.sm.StatBonus, false);
				}
				else
				{
					MegaBrainTank.StatesInstance.minionEffects[i].Remove(base.sm.StatBonus);
				}
			}
		}

		// Token: 0x06008E8A RID: 36490 RVA: 0x00343EE8 File Offset: 0x003420E8
		private void OnAnimComplete(HashedString anim)
		{
			if (anim == MegaBrainTankConfig.KACHUNK)
			{
				this.StoreJournals();
				return;
			}
			if ((anim == base.smi.CurrentActivationAnim || anim == MegaBrainTankConfig.ACTIVATE_ALL) && this.GetCurrentState() != base.sm.common.idle)
			{
				this.CompleteBrainActivation();
			}
		}

		// Token: 0x06008E8B RID: 36491 RVA: 0x00343F48 File Offset: 0x00342148
		private void OnJournalDeliveryStateChanged(Workable w, Workable.WorkableEvent state)
		{
			if (state == Workable.WorkableEvent.WorkStopped)
			{
				return;
			}
			if (state != Workable.WorkableEvent.WorkStarted)
			{
				this.ShelfController.Play(MegaBrainTankConfig.KACHUNK, KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			FetchAreaChore.StatesInstance smi = w.worker.GetSMI<FetchAreaChore.StatesInstance>();
			if (smi.IsNullOrStopped())
			{
				return;
			}
			GameObject gameObject = smi.sm.deliveryObject.Get(smi);
			if (gameObject == null)
			{
				return;
			}
			Pickupable component = gameObject.GetComponent<Pickupable>();
			this.UnitsFromLastStore = (short)component.PrimaryElement.Units;
			float num = Mathf.Clamp01(component.PrimaryElement.Units / 5f);
			this.BrainStorage.SetWorkTime(num * this.BrainStorage.storageWorkTime);
		}

		// Token: 0x06008E8C RID: 36492 RVA: 0x00343FF4 File Offset: 0x003421F4
		public void ShelveJournals(float dt)
		{
			float num = this.lastRemainingTime - this.BrainStorage.WorkTimeRemaining;
			if (num <= 0f)
			{
				num = this.BrainStorage.storageWorkTime - this.BrainStorage.WorkTimeRemaining;
			}
			this.lastRemainingTime = this.BrainStorage.WorkTimeRemaining;
			if (this.BrainStorage.storageWorkTime / 5f - this.journalActivationTimer > 0.001f)
			{
				this.journalActivationTimer += num;
				return;
			}
			int num2 = -1;
			this.journalActivationTimer = 0f;
			for (int i = 0; i < MegaBrainTankConfig.JOURNAL_SYMBOLS.Length; i++)
			{
				byte b = (byte)(1 << i);
				bool flag = (this.activatedJournals & b) == 0;
				if (flag && num2 == -1)
				{
					num2 = i;
				}
				if (flag & UnityEngine.Random.Range(0f, 1f) >= 0.5f)
				{
					num2 = -1;
					this.activatedJournals |= b;
					this.ShelfController.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[i], true);
					break;
				}
			}
			if (num2 != -1)
			{
				this.ShelfController.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[num2], true);
			}
			this.UnitsFromLastStore -= 1;
		}

		// Token: 0x06008E8D RID: 36493 RVA: 0x00344128 File Offset: 0x00342328
		public void StoreJournals()
		{
			this.lastRemainingTime = 0f;
			this.activatedJournals = 0;
			for (int i = 0; i < MegaBrainTankConfig.JOURNAL_SYMBOLS.Length; i++)
			{
				this.ShelfController.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[i], false);
			}
			this.ShelfController.PlayMode = KAnim.PlayMode.Paused;
			this.ShelfController.SetPositionPercent(0f);
			float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			this.targetProgress = Mathf.Clamp01(unitsAvailable / 25f);
		}

		// Token: 0x06008E8E RID: 36494 RVA: 0x003441B4 File Offset: 0x003423B4
		public void ActivateBrains(float dt)
		{
			if (this.currentlyActivating)
			{
				return;
			}
			this.currentlyActivating = ((float)this.nextActiveBrain / 5f - this.meterFill <= 0.001f);
			if (!this.currentlyActivating)
			{
				return;
			}
			this.BrainController.QueueAndSyncTransition(this.CurrentActivationAnim, KAnim.PlayMode.Once, 1f, 0f);
			if (this.nextActiveBrain > 0)
			{
				this.BrainSounds.StartSound(this.brainHum);
				this.BrainSounds.SetParameter(this.brainHum, "BrainTankProgress", (float)this.nextActiveBrain);
			}
		}

		// Token: 0x06008E8F RID: 36495 RVA: 0x00344250 File Offset: 0x00342450
		public void CompleteBrainActivation()
		{
			this.BrainController.Play(this.currentActivationLoop, KAnim.PlayMode.Loop, 1f, 0f);
			this.nextActiveBrain += 1;
			this.currentlyActivating = false;
			if (this.nextActiveBrain > 5)
			{
				float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
				this.timeTilDigested = unitsAvailable * 60f;
				this.CompleteEvent();
			}
		}

		// Token: 0x06008E90 RID: 36496 RVA: 0x003442BC File Offset: 0x003424BC
		public void Digest(float dt)
		{
			float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			this.timeTilDigested = unitsAvailable * 60f;
			if (this.targetProgress - this.meterFill > Mathf.Epsilon)
			{
				return;
			}
			this.targetProgress = 0f;
			float num = this.meterFill - this.timeTilDigested / 1500f;
			if (num >= 0.04f)
			{
				this.meterFill -= num - num % 0.04f;
				this.meter.SetPositionPercent(this.meterFill);
			}
		}

		// Token: 0x06008E91 RID: 36497 RVA: 0x0034434C File Offset: 0x0034254C
		public void CleanTank(bool active)
		{
			this.SetBonusActive(active);
			base.GetComponent<Light2D>().enabled = active;
			this.Selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankDreamAnalysis, active, this);
			this.ElementConverter.SetAllConsumedActive(active);
			this.BrainController.ClearQueue();
			float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			this.timeTilDigested = unitsAvailable * 60f;
			if (active)
			{
				this.nextActiveBrain = 5;
				this.BrainController.QueueAndSyncTransition(MegaBrainTankConfig.ACTIVATE_ALL, KAnim.PlayMode.Once, 1f, 0f);
				this.BrainSounds.StartSound(this.brainHum);
				this.BrainSounds.SetParameter(this.brainHum, "BrainTankProgress", (float)this.nextActiveBrain);
				return;
			}
			if (this.timeTilDigested < 0.016666668f)
			{
				this.BrainStorage.ConsumeAllIgnoringDisease(DreamJournalConfig.ID);
				this.timeTilDigested = 0f;
				this.meterFill = 0f;
				this.meter.SetPositionPercent(this.meterFill);
			}
			this.BrainController.QueueAndSyncTransition(MegaBrainTankConfig.DEACTIVATE_ALL, KAnim.PlayMode.Once, 1f, 0f);
			this.BrainSounds.StopSound(this.brainHum);
		}

		// Token: 0x06008E92 RID: 36498 RVA: 0x00344488 File Offset: 0x00342688
		public bool IncrementMeter(float dt)
		{
			if (this.targetProgress - this.meterFill <= Mathf.Epsilon)
			{
				return false;
			}
			this.meterFill += Mathf.Lerp(0f, 1f, 0.04f * dt);
			if (1f - this.meterFill <= 0.001f)
			{
				this.meterFill = 1f;
			}
			this.meter.SetPositionPercent(this.meterFill);
			return this.targetProgress - this.meterFill > 0.001f;
		}

		// Token: 0x06008E93 RID: 36499 RVA: 0x00344514 File Offset: 0x00342714
		public void CompleteEvent()
		{
			this.Selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankActivationProgress, false);
			this.Selectable.AddStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankComplete, base.smi);
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MegaBrainTank.HashId);
			if (storyInstance == null || storyInstance.CurrentState == StoryInstance.State.COMPLETE)
			{
				return;
			}
			this.eventInfo = EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.NAME, CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.BUTTON, "braintankcomplete_kanim", EventInfoDataHelper.PopupType.COMPLETE, null, null, null);
			base.smi.Selectable.AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
			this.eventComplete = EventInfoScreen.CreateNotification(this.eventInfo, new Notification.ClickCallback(this.ShowEventCompleteUI));
			this.notifier.Add(this.eventComplete, "");
		}

		// Token: 0x06008E94 RID: 36500 RVA: 0x00344618 File Offset: 0x00342818
		public void ShowEventCompleteUI(object _ = null)
		{
			if (this.eventComplete == null)
			{
				return;
			}
			base.smi.Selectable.RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
			this.notifier.Remove(this.eventComplete);
			this.eventComplete = null;
			Game.Instance.unlocks.Unlock("story_trait_mega_brain_tank_competed", true);
			Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), new CellOffset(0, 3)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.MegaBrainTank, base.master, new FocusTargetSequence.Data
			{
				WorldId = base.master.GetMyWorldId(),
				OrthographicSize = 6f,
				TargetSize = 6f,
				Target = target,
				PopupData = this.eventInfo,
				CompleteCB = new System.Action(this.OnCompleteStorySequence),
				CanCompleteCB = null
			});
		}

		// Token: 0x06008E95 RID: 36501 RVA: 0x00344720 File Offset: 0x00342920
		private void OnCompleteStorySequence()
		{
			Vector3 keepsakeSpawnPosition = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), new CellOffset(0, 2)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.MegaBrainTank, keepsakeSpawnPosition);
			this.eventInfo = null;
			base.sm.storyTraitCompleted.Trigger(this);
		}

		// Token: 0x04006CBC RID: 27836
		private static List<Effects> minionEffects;

		// Token: 0x04006CC3 RID: 27843
		public short UnitsFromLastStore;

		// Token: 0x04006CC4 RID: 27844
		private float meterFill = 0.04f;

		// Token: 0x04006CC5 RID: 27845
		private float targetProgress;

		// Token: 0x04006CC6 RID: 27846
		private float timeTilDigested;

		// Token: 0x04006CC7 RID: 27847
		private float journalActivationTimer;

		// Token: 0x04006CC8 RID: 27848
		private float lastRemainingTime;

		// Token: 0x04006CC9 RID: 27849
		private byte activatedJournals;

		// Token: 0x04006CCA RID: 27850
		private bool currentlyActivating;

		// Token: 0x04006CCB RID: 27851
		private short nextActiveBrain = 1;

		// Token: 0x04006CCC RID: 27852
		private string brainHum;

		// Token: 0x04006CCD RID: 27853
		private KBatchedAnimController[] controllers;

		// Token: 0x04006CCE RID: 27854
		private KAnimLink fxLink;

		// Token: 0x04006CCF RID: 27855
		private MeterController meter;

		// Token: 0x04006CD0 RID: 27856
		private EventInfoData eventInfo;

		// Token: 0x04006CD1 RID: 27857
		private Notification eventComplete;

		// Token: 0x04006CD2 RID: 27858
		private Notifier notifier;
	}
}
