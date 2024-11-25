using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020002A1 RID: 673
public class MinorFossilDigSite : GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>
{
	// Token: 0x06000DE8 RID: 3560 RVA: 0x00050174 File Offset: 0x0004E374
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Idle;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.Idle.Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, false);
		}).Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			smi.SetDecorState(true);
		}).PlayAnim("object_dirty").ParamTransition<bool>(this.IsQuestCompleted, this.Completed, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.IsRevealed, this.WaitingForQuestCompletion, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.MarkedForDig, this.Workable, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue);
		this.Workable.PlayAnim("object_dirty").Toggle("Activate Entombed Status Item If Required", delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}, delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, false);
		}).DefaultState(this.Workable.NonOperational).ParamTransition<bool>(this.MarkedForDig, this.Idle, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsFalse);
		this.Workable.NonOperational.TagTransition(GameTags.Operational, this.Workable.Operational, false);
		this.Workable.Operational.Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.StartWorkChore)).Exit(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.CancelWorkChore)).TagTransition(GameTags.Operational, this.Workable.NonOperational, true).WorkableCompleteTransition((MinorFossilDigSite.Instance smi) => smi.GetWorkable(), this.WaitingForQuestCompletion);
		this.WaitingForQuestCompletion.Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			smi.SetDecorState(false);
		}).Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.Reveal)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.ProgressStoryTrait)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.MakeItDemolishable)).PlayAnim("object").ParamTransition<bool>(this.IsQuestCompleted, this.Completed, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue);
		this.Completed.Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			smi.SetDecorState(false);
		}).Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).PlayAnim("object").Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.MakeItDemolishable));
	}

	// Token: 0x06000DE9 RID: 3561 RVA: 0x00050468 File Offset: 0x0004E668
	public static void MakeItDemolishable(MinorFossilDigSite.Instance smi)
	{
		smi.gameObject.GetComponent<Demolishable>().allowDemolition = true;
	}

	// Token: 0x06000DEA RID: 3562 RVA: 0x0005047B File Offset: 0x0004E67B
	public static void DestroyUIExcavateButton(MinorFossilDigSite.Instance smi)
	{
		smi.DestroyExcavateButton();
	}

	// Token: 0x06000DEB RID: 3563 RVA: 0x00050483 File Offset: 0x0004E683
	public static void SetEntombedStatusItemVisibility(MinorFossilDigSite.Instance smi, bool val)
	{
		smi.SetEntombStatusItemVisibility(val);
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x0005048C File Offset: 0x0004E68C
	public static void UnregisterFromComponents(MinorFossilDigSite.Instance smi)
	{
		Components.MinorFossilDigSites.Remove(smi);
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x00050499 File Offset: 0x0004E699
	public static void SelfDestroy(MinorFossilDigSite.Instance smi)
	{
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x000504A6 File Offset: 0x0004E6A6
	public static void StartWorkChore(MinorFossilDigSite.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x000504AE File Offset: 0x0004E6AE
	public static void CancelWorkChore(MinorFossilDigSite.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x000504B6 File Offset: 0x0004E6B6
	public static void Reveal(MinorFossilDigSite.Instance smi)
	{
		bool flag = !smi.sm.IsRevealed.Get(smi);
		smi.sm.IsRevealed.Set(true, smi, false);
		if (flag)
		{
			smi.ShowCompletionNotification();
			MinorFossilDigSite.DropLoot(smi);
		}
	}

	// Token: 0x06000DF1 RID: 3569 RVA: 0x000504F0 File Offset: 0x0004E6F0
	public static void DropLoot(MinorFossilDigSite.Instance smi)
	{
		PrimaryElement component = smi.GetComponent<PrimaryElement>();
		int cell = Grid.PosToCell(smi.transform.GetPosition());
		Element element = ElementLoader.GetElement(component.Element.tag);
		if (element != null)
		{
			float num = component.Mass;
			int num2 = 0;
			while ((float)num2 < component.Mass / 400f)
			{
				float num3 = num;
				if (num > 400f)
				{
					num3 = 400f;
					num -= 400f;
				}
				int disease_count = (int)((float)component.DiseaseCount * (num3 / component.Mass));
				element.substance.SpawnResource(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore), num3, component.Temperature, component.DiseaseIdx, disease_count, false, false, false);
				num2++;
			}
		}
	}

	// Token: 0x06000DF2 RID: 3570 RVA: 0x000505A1 File Offset: 0x0004E7A1
	public static void ProgressStoryTrait(MinorFossilDigSite.Instance smi)
	{
		MinorFossilDigSite.ProgressQuest(smi);
	}

	// Token: 0x06000DF3 RID: 3571 RVA: 0x000505AC File Offset: 0x0004E7AC
	public static QuestInstance ProgressQuest(MinorFossilDigSite.Instance smi)
	{
		QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (instance != null)
		{
			Quest.ItemData data = new Quest.ItemData
			{
				CriteriaId = smi.def.fossilQuestCriteriaID,
				CurrentValue = 1f
			};
			bool flag;
			bool flag2;
			instance.TrackProgress(data, out flag, out flag2);
			return instance;
		}
		return null;
	}

	// Token: 0x040008BB RID: 2235
	public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State Idle;

	// Token: 0x040008BC RID: 2236
	public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State Completed;

	// Token: 0x040008BD RID: 2237
	public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State WaitingForQuestCompletion;

	// Token: 0x040008BE RID: 2238
	public MinorFossilDigSite.ReadyToBeWorked Workable;

	// Token: 0x040008BF RID: 2239
	public StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.BoolParameter MarkedForDig;

	// Token: 0x040008C0 RID: 2240
	public StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.BoolParameter IsRevealed;

	// Token: 0x040008C1 RID: 2241
	public StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.BoolParameter IsQuestCompleted;

	// Token: 0x040008C2 RID: 2242
	private const string UNEXCAVATED_ANIM_NAME = "object_dirty";

	// Token: 0x040008C3 RID: 2243
	private const string EXCAVATED_ANIM_NAME = "object";

	// Token: 0x020010FF RID: 4351
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005E8B RID: 24203
		public HashedString fossilQuestCriteriaID;
	}

	// Token: 0x02001100 RID: 4352
	public class ReadyToBeWorked : GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State
	{
		// Token: 0x04005E8C RID: 24204
		public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State Operational;

		// Token: 0x04005E8D RID: 24205
		public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State NonOperational;
	}

	// Token: 0x02001101 RID: 4353
	public new class Instance : GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.GameInstance, ICheckboxListGroupControl
	{
		// Token: 0x06007DCF RID: 32207 RVA: 0x003092C4 File Offset: 0x003074C4
		public Instance(IStateMachineTarget master, MinorFossilDigSite.Def def) : base(master, def)
		{
			Components.MinorFossilDigSites.Add(this);
			this.negativeDecorModifier = new AttributeModifier(Db.Get().Attributes.Decor.Id, -1f, CODEX.STORY_TRAITS.FOSSILHUNT.MISC.DECREASE_DECOR_ATTRIBUTE, true, false, true);
		}

		// Token: 0x06007DD0 RID: 32208 RVA: 0x00309315 File Offset: 0x00307515
		public void SetDecorState(bool isDusty)
		{
			if (isDusty)
			{
				base.gameObject.GetComponent<DecorProvider>().decor.Add(this.negativeDecorModifier);
				return;
			}
			base.gameObject.GetComponent<DecorProvider>().decor.Remove(this.negativeDecorModifier);
		}

		// Token: 0x06007DD1 RID: 32209 RVA: 0x00309354 File Offset: 0x00307554
		public override void StartSM()
		{
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance != null)
			{
				StoryInstance storyInstance2 = storyInstance;
				storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Combine(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStorytraitProgressChanged));
			}
			if (!base.sm.IsRevealed.Get(this))
			{
				this.CreateExcavateButton();
			}
			QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			this.workable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
			base.StartSM();
			this.RefreshUI();
		}

		// Token: 0x06007DD2 RID: 32210 RVA: 0x00309421 File Offset: 0x00307621
		private void OnQuestProgressChanged(QuestInstance quest, Quest.State previousState, float progressIncreased)
		{
			if (quest.CurrentState == Quest.State.Completed && base.sm.IsRevealed.Get(this))
			{
				this.OnQuestCompleted(quest);
			}
			this.RefreshUI();
		}

		// Token: 0x06007DD3 RID: 32211 RVA: 0x0030944C File Offset: 0x0030764C
		public void OnQuestCompleted(QuestInstance quest)
		{
			base.sm.IsQuestCompleted.Set(true, this, false);
			quest.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(quest.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
		}

		// Token: 0x06007DD4 RID: 32212 RVA: 0x00309484 File Offset: 0x00307684
		protected override void OnCleanUp()
		{
			MinorFossilDigSite.ProgressQuest(base.smi);
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance != null)
			{
				StoryInstance storyInstance2 = storyInstance;
				storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Remove(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStorytraitProgressChanged));
			}
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null)
			{
				QuestInstance questInstance = instance;
				questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			}
			Components.MinorFossilDigSites.Remove(this);
			base.OnCleanUp();
		}

		// Token: 0x06007DD5 RID: 32213 RVA: 0x00309531 File Offset: 0x00307731
		public void OnStorytraitProgressChanged(StoryInstance.State state)
		{
			if (state != StoryInstance.State.IN_PROGRESS)
			{
				return;
			}
			this.RefreshUI();
		}

		// Token: 0x06007DD6 RID: 32214 RVA: 0x00309542 File Offset: 0x00307742
		public void RefreshUI()
		{
			base.Trigger(1980521255, null);
		}

		// Token: 0x06007DD7 RID: 32215 RVA: 0x00309550 File Offset: 0x00307750
		public Workable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x06007DD8 RID: 32216 RVA: 0x00309558 File Offset: 0x00307758
		public void CreateWorkableChore()
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<MinorDigSiteWorkable>(Db.Get().ChoreTypes.ExcavateFossil, this.workable, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06007DD9 RID: 32217 RVA: 0x0030959E File Offset: 0x0030779E
		public void CancelWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("MinorFossilDigsite.CancelChore");
				this.chore = null;
			}
		}

		// Token: 0x06007DDA RID: 32218 RVA: 0x003095BF File Offset: 0x003077BF
		public void SetEntombStatusItemVisibility(bool visible)
		{
			this.entombComponent.SetShowStatusItemOnEntombed(visible);
		}

		// Token: 0x06007DDB RID: 32219 RVA: 0x003095D0 File Offset: 0x003077D0
		public void ShowCompletionNotification()
		{
			FossilHuntInitializer.Instance smi = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			if (smi != null)
			{
				smi.ShowObjectiveCompletedNotification();
			}
		}

		// Token: 0x06007DDC RID: 32220 RVA: 0x003095F4 File Offset: 0x003077F4
		public void OnExcavateButtonPressed()
		{
			base.sm.MarkedForDig.Set(!base.sm.MarkedForDig.Get(this), this, false);
			this.workable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
		}

		// Token: 0x06007DDD RID: 32221 RVA: 0x00309644 File Offset: 0x00307844
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

		// Token: 0x06007DDE RID: 32222 RVA: 0x003096B4 File Offset: 0x003078B4
		public void DestroyExcavateButton()
		{
			this.workable.SetShouldShowSkillPerkStatusItem(false);
			if (this.excavateButton != null)
			{
				UnityEngine.Object.DestroyImmediate(this.excavateButton);
				this.excavateButton = null;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06007DDF RID: 32223 RVA: 0x003096E2 File Offset: 0x003078E2
		public string Title
		{
			get
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.NAME;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06007DE0 RID: 32224 RVA: 0x003096EE File Offset: 0x003078EE
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

		// Token: 0x06007DE1 RID: 32225 RVA: 0x00309718 File Offset: 0x00307918
		public bool SidescreenEnabled()
		{
			return !base.sm.IsQuestCompleted.Get(this);
		}

		// Token: 0x06007DE2 RID: 32226 RVA: 0x0030972E File Offset: 0x0030792E
		public ICheckboxListGroupControl.ListGroup[] GetData()
		{
			return FossilHuntInitializer.GetFossilHuntQuestData();
		}

		// Token: 0x06007DE3 RID: 32227 RVA: 0x00309735 File Offset: 0x00307935
		public int CheckboxSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x04005E8E RID: 24206
		[MyCmpGet]
		private MinorDigSiteWorkable workable;

		// Token: 0x04005E8F RID: 24207
		[MyCmpGet]
		private EntombVulnerable entombComponent;

		// Token: 0x04005E90 RID: 24208
		private ExcavateButton excavateButton;

		// Token: 0x04005E91 RID: 24209
		private Chore chore;

		// Token: 0x04005E92 RID: 24210
		private AttributeModifier negativeDecorModifier;
	}
}
