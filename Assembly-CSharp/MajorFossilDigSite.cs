using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000281 RID: 641
public class MajorFossilDigSite : GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>
{
	// Token: 0x06000D3D RID: 3389 RVA: 0x0004BE30 File Offset: 0x0004A030
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Idle;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.Idle.PlayAnim("covered").Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.TurnOffLight)).Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, false);
		}).ParamTransition<bool>(this.IsQuestCompleted, this.Completed, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.IsRevealed, this.WaitingForQuestCompletion, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.MarkedForDig, this.Workable, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue);
		this.Workable.PlayAnim("covered").Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).DefaultState(this.Workable.NonOperational).ParamTransition<bool>(this.MarkedForDig, this.Idle, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsFalse);
		this.Workable.NonOperational.TagTransition(GameTags.Operational, this.Workable.Operational, false);
		this.Workable.Operational.Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.StartWorkChore)).Exit(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CancelWorkChore)).TagTransition(GameTags.Operational, this.Workable.NonOperational, true).WorkableCompleteTransition((MajorFossilDigSite.Instance smi) => smi.GetWorkable(), this.WaitingForQuestCompletion);
		this.WaitingForQuestCompletion.OnSignal(this.CompleteStorySignal, this.Completed).Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).PlayAnim("reveal").Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.Reveal)).ScheduleActionNextFrame("Refresh UI", new Action<MajorFossilDigSite.Instance>(MajorFossilDigSite.RefreshUI)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CheckForQuestCompletion)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.ProgressStoryTrait));
		this.Completed.Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.TurnOnLight)).Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CompleteStory)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.UnlockFossilMine)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.MakeItDemolishable));
	}

	// Token: 0x06000D3E RID: 3390 RVA: 0x0004C0DF File Offset: 0x0004A2DF
	public static void MakeItDemolishable(MajorFossilDigSite.Instance smi)
	{
		smi.gameObject.GetComponent<Demolishable>().allowDemolition = true;
	}

	// Token: 0x06000D3F RID: 3391 RVA: 0x0004C0F4 File Offset: 0x0004A2F4
	public static void ProgressStoryTrait(MajorFossilDigSite.Instance smi)
	{
		QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (instance != null)
		{
			Quest.ItemData data = new Quest.ItemData
			{
				CriteriaId = smi.def.questCriteria,
				CurrentValue = 1f
			};
			bool flag;
			bool flag2;
			instance.TrackProgress(data, out flag, out flag2);
		}
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x0004C153 File Offset: 0x0004A353
	public static void TurnOnLight(MajorFossilDigSite.Instance smi)
	{
		smi.SetLightOnState(true);
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x0004C15C File Offset: 0x0004A35C
	public static void TurnOffLight(MajorFossilDigSite.Instance smi)
	{
		smi.SetLightOnState(false);
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x0004C168 File Offset: 0x0004A368
	public static void CheckForQuestCompletion(MajorFossilDigSite.Instance smi)
	{
		QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (questInstance != null && questInstance.CurrentState == Quest.State.Completed)
		{
			smi.OnQuestCompleted(questInstance);
		}
	}

	// Token: 0x06000D43 RID: 3395 RVA: 0x0004C1A2 File Offset: 0x0004A3A2
	public static void SetEntombedStatusItemVisibility(MajorFossilDigSite.Instance smi, bool val)
	{
		smi.SetEntombStatusItemVisibility(val);
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x0004C1AB File Offset: 0x0004A3AB
	public static void UnlockFossilMine(MajorFossilDigSite.Instance smi)
	{
		smi.UnlockFossilMine();
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x0004C1B3 File Offset: 0x0004A3B3
	public static void DestroyUIExcavateButton(MajorFossilDigSite.Instance smi)
	{
		smi.DestroyExcavateButton();
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x0004C1BB File Offset: 0x0004A3BB
	public static void CompleteStory(MajorFossilDigSite.Instance smi)
	{
		if (smi.sm.IsQuestCompleted.Get(smi))
		{
			return;
		}
		smi.sm.IsQuestCompleted.Set(true, smi, false);
		smi.CompleteStoryTrait();
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x0004C1EC File Offset: 0x0004A3EC
	public static void Reveal(MajorFossilDigSite.Instance smi)
	{
		bool flag = !smi.sm.IsRevealed.Get(smi);
		smi.sm.IsRevealed.Set(true, smi, false);
		if (flag)
		{
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null && !instance.IsComplete)
			{
				smi.ShowCompletionNotification();
			}
		}
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x0004C24E File Offset: 0x0004A44E
	public static void RevealMinorDigSites(MajorFossilDigSite.Instance smi)
	{
		smi.RevealMinorDigSites();
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x0004C256 File Offset: 0x0004A456
	public static void RefreshUI(MajorFossilDigSite.Instance smi)
	{
		smi.RefreshUI();
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x0004C25E File Offset: 0x0004A45E
	public static void StartWorkChore(MajorFossilDigSite.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x06000D4B RID: 3403 RVA: 0x0004C266 File Offset: 0x0004A466
	public static void CancelWorkChore(MajorFossilDigSite.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x0400083C RID: 2108
	public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Idle;

	// Token: 0x0400083D RID: 2109
	public MajorFossilDigSite.ReadyToBeWorked Workable;

	// Token: 0x0400083E RID: 2110
	public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State WaitingForQuestCompletion;

	// Token: 0x0400083F RID: 2111
	public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Completed;

	// Token: 0x04000840 RID: 2112
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter MarkedForDig;

	// Token: 0x04000841 RID: 2113
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter IsRevealed;

	// Token: 0x04000842 RID: 2114
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter IsQuestCompleted;

	// Token: 0x04000843 RID: 2115
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.Signal CompleteStorySignal;

	// Token: 0x04000844 RID: 2116
	public const string ANIM_COVERED_NAME = "covered";

	// Token: 0x04000845 RID: 2117
	public const string ANIM_REVEALED_NAME = "reveal";

	// Token: 0x020010F7 RID: 4343
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005E71 RID: 24177
		public HashedString questCriteria;
	}

	// Token: 0x020010F8 RID: 4344
	public class ReadyToBeWorked : GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State
	{
		// Token: 0x04005E72 RID: 24178
		public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Operational;

		// Token: 0x04005E73 RID: 24179
		public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State NonOperational;
	}

	// Token: 0x020010F9 RID: 4345
	public new class Instance : GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.GameInstance, ICheckboxListGroupControl
	{
		// Token: 0x06007D9D RID: 32157 RVA: 0x00308AB0 File Offset: 0x00306CB0
		public Instance(IStateMachineTarget master, MajorFossilDigSite.Def def) : base(master, def)
		{
			Components.MajorFossilDigSites.Add(this);
		}

		// Token: 0x06007D9E RID: 32158 RVA: 0x00308AC8 File Offset: 0x00306CC8
		public override void StartSM()
		{
			this.entombComponent.SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
			this.storyInitializer = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			base.GetComponent<KPrefabID>();
			QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			if (!base.sm.IsRevealed.Get(this))
			{
				this.CreateExcavateButton();
			}
			this.fossilMine.SetActiveState(base.sm.IsQuestCompleted.Get(this));
			if (base.sm.IsQuestCompleted.Get(this))
			{
				this.UnlockStandarBuildingButtons();
				this.ScheduleNextFrame(delegate(object obj)
				{
					this.ChangeUIDescriptionToCompleted();
				}, null);
			}
			this.excavateWorkable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
			base.StartSM();
			this.RefreshUI();
		}

		// Token: 0x06007D9F RID: 32159 RVA: 0x00308BCC File Offset: 0x00306DCC
		public void SetLightOnState(bool isOn)
		{
			FossilDigsiteLampLight component = base.gameObject.GetComponent<FossilDigsiteLampLight>();
			component.SetIndependentState(isOn, true);
			if (!isOn)
			{
				component.enabled = false;
			}
		}

		// Token: 0x06007DA0 RID: 32160 RVA: 0x00308BF7 File Offset: 0x00306DF7
		public Workable GetWorkable()
		{
			return this.excavateWorkable;
		}

		// Token: 0x06007DA1 RID: 32161 RVA: 0x00308C00 File Offset: 0x00306E00
		public void CreateWorkableChore()
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<MajorDigSiteWorkable>(Db.Get().ChoreTypes.ExcavateFossil, this.excavateWorkable, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06007DA2 RID: 32162 RVA: 0x00308C46 File Offset: 0x00306E46
		public void CancelWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("MajorFossilDigsite.CancelChore");
				this.chore = null;
			}
		}

		// Token: 0x06007DA3 RID: 32163 RVA: 0x00308C67 File Offset: 0x00306E67
		public void SetEntombStatusItemVisibility(bool visible)
		{
			this.entombComponent.SetShowStatusItemOnEntombed(visible);
		}

		// Token: 0x06007DA4 RID: 32164 RVA: 0x00308C78 File Offset: 0x00306E78
		public void OnExcavateButtonPressed()
		{
			base.sm.MarkedForDig.Set(!base.sm.MarkedForDig.Get(this), this, false);
			this.excavateWorkable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
		}

		// Token: 0x06007DA5 RID: 32165 RVA: 0x00308CC8 File Offset: 0x00306EC8
		public ExcavateButton CreateExcavateButton()
		{
			if (this.excavateButton == null)
			{
				this.excavateButton = base.gameObject.AddComponent<ExcavateButton>();
				ExcavateButton excavateButton = this.excavateButton;
				excavateButton.OnButtonPressed = (System.Action)Delegate.Combine(excavateButton.OnButtonPressed, new System.Action(this.OnExcavateButtonPressed));
				this.excavateButton.isMarkedForDig = (() => base.sm.MarkedForDig.Get(this));
			}
			return this.excavateButton;
		}

		// Token: 0x06007DA6 RID: 32166 RVA: 0x00308D38 File Offset: 0x00306F38
		public void DestroyExcavateButton()
		{
			this.excavateWorkable.SetShouldShowSkillPerkStatusItem(false);
			if (this.excavateButton != null)
			{
				UnityEngine.Object.DestroyImmediate(this.excavateButton);
				this.excavateButton = null;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06007DA7 RID: 32167 RVA: 0x00308D66 File Offset: 0x00306F66
		public string Title
		{
			get
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.NAME;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06007DA8 RID: 32168 RVA: 0x00308D72 File Offset: 0x00306F72
		public string Description
		{
			get
			{
				if (base.sm.IsRevealed.Get(this))
				{
					return CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION_REVEALED;
				}
				return CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION_BUILDINGMENU_COVERED;
			}
		}

		// Token: 0x06007DA9 RID: 32169 RVA: 0x00308D9C File Offset: 0x00306F9C
		public bool SidescreenEnabled()
		{
			return !base.sm.IsQuestCompleted.Get(this);
		}

		// Token: 0x06007DAA RID: 32170 RVA: 0x00308DB2 File Offset: 0x00306FB2
		public void RevealMinorDigSites()
		{
			if (this.storyInitializer == null)
			{
				this.storyInitializer = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			}
			if (this.storyInitializer != null)
			{
				this.storyInitializer.RevealMinorFossilDigSites();
			}
		}

		// Token: 0x06007DAB RID: 32171 RVA: 0x00308DE0 File Offset: 0x00306FE0
		private void OnQuestProgressChanged(QuestInstance quest, Quest.State previousState, float progressIncreased)
		{
			if (quest.CurrentState == Quest.State.Completed && base.sm.IsRevealed.Get(this))
			{
				this.OnQuestCompleted(quest);
			}
			this.RefreshUI();
		}

		// Token: 0x06007DAC RID: 32172 RVA: 0x00308E0B File Offset: 0x0030700B
		public void OnQuestCompleted(QuestInstance quest)
		{
			base.sm.CompleteStorySignal.Trigger(this);
			quest.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(quest.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
		}

		// Token: 0x06007DAD RID: 32173 RVA: 0x00308E40 File Offset: 0x00307040
		public void CompleteStoryTrait()
		{
			FossilHuntInitializer.Instance smi = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			smi.sm.CompleteStory.Trigger(smi);
		}

		// Token: 0x06007DAE RID: 32174 RVA: 0x00308E6A File Offset: 0x0030706A
		public void UnlockFossilMine()
		{
			this.fossilMine.SetActiveState(true);
			this.UnlockStandarBuildingButtons();
			this.ChangeUIDescriptionToCompleted();
		}

		// Token: 0x06007DAF RID: 32175 RVA: 0x00308E84 File Offset: 0x00307084
		private void ChangeUIDescriptionToCompleted()
		{
			BuildingComplete component = base.gameObject.GetComponent<BuildingComplete>();
			base.gameObject.GetComponent<KSelectable>().SetName(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.NAME);
			component.SetDescriptionFlavour(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.EFFECT);
			component.SetDescription(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.DESC);
		}

		// Token: 0x06007DB0 RID: 32176 RVA: 0x00308ED5 File Offset: 0x003070D5
		private void UnlockStandarBuildingButtons()
		{
			base.gameObject.AddOrGet<BuildingEnabledButton>();
		}

		// Token: 0x06007DB1 RID: 32177 RVA: 0x00308EE3 File Offset: 0x003070E3
		public void RefreshUI()
		{
			base.gameObject.Trigger(1980521255, null);
		}

		// Token: 0x06007DB2 RID: 32178 RVA: 0x00308EF8 File Offset: 0x003070F8
		protected override void OnCleanUp()
		{
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null)
			{
				QuestInstance questInstance = instance;
				questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			}
			Components.MajorFossilDigSites.Remove(this);
			base.OnCleanUp();
		}

		// Token: 0x06007DB3 RID: 32179 RVA: 0x00308F55 File Offset: 0x00307155
		public int CheckboxSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x06007DB4 RID: 32180 RVA: 0x00308F59 File Offset: 0x00307159
		public ICheckboxListGroupControl.ListGroup[] GetData()
		{
			return FossilHuntInitializer.GetFossilHuntQuestData();
		}

		// Token: 0x06007DB5 RID: 32181 RVA: 0x00308F60 File Offset: 0x00307160
		public void ShowCompletionNotification()
		{
			FossilHuntInitializer.Instance smi = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			if (smi != null)
			{
				smi.ShowObjectiveCompletedNotification();
			}
		}

		// Token: 0x04005E74 RID: 24180
		[MyCmpGet]
		private Operational operational;

		// Token: 0x04005E75 RID: 24181
		[MyCmpGet]
		private MajorDigSiteWorkable excavateWorkable;

		// Token: 0x04005E76 RID: 24182
		[MyCmpGet]
		private FossilMine fossilMine;

		// Token: 0x04005E77 RID: 24183
		[MyCmpGet]
		private EntombVulnerable entombComponent;

		// Token: 0x04005E78 RID: 24184
		private Chore chore;

		// Token: 0x04005E79 RID: 24185
		private FossilHuntInitializer.Instance storyInitializer;

		// Token: 0x04005E7A RID: 24186
		private ExcavateButton excavateButton;
	}
}
