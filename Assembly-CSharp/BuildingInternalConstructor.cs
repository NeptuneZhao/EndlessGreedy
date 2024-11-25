using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000675 RID: 1653
public class BuildingInternalConstructor : GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>
{
	// Token: 0x060028F5 RID: 10485 RVA: 0x000E7AE0 File Offset: 0x000E5CE0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (BuildingInternalConstructor.Instance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(BuildingInternalConstructor.Instance smi)
		{
			smi.ShowConstructionSymbol(false);
		});
		this.operational.DefaultState(this.operational.constructionRequired).EventTransition(GameHashes.OperationalChanged, this.inoperational, (BuildingInternalConstructor.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.operational.constructionRequired.EventTransition(GameHashes.OnStorageChange, this.operational.constructionHappening, (BuildingInternalConstructor.Instance smi) => smi.GetMassForConstruction() != null).EventTransition(GameHashes.OnStorageChange, this.operational.constructionSatisfied, (BuildingInternalConstructor.Instance smi) => smi.HasOutputInStorage()).ToggleFetch((BuildingInternalConstructor.Instance smi) => smi.CreateFetchList(), this.operational.constructionHappening).ParamTransition<bool>(this.constructionRequested, this.operational.constructionSatisfied, GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.IsFalse).Enter(delegate(BuildingInternalConstructor.Instance smi)
		{
			smi.ShowConstructionSymbol(true);
		}).Exit(delegate(BuildingInternalConstructor.Instance smi)
		{
			smi.ShowConstructionSymbol(false);
		});
		this.operational.constructionHappening.EventTransition(GameHashes.OnStorageChange, this.operational.constructionSatisfied, (BuildingInternalConstructor.Instance smi) => smi.HasOutputInStorage()).EventTransition(GameHashes.OnStorageChange, this.operational.constructionRequired, (BuildingInternalConstructor.Instance smi) => smi.GetMassForConstruction() == null).ToggleChore((BuildingInternalConstructor.Instance smi) => smi.CreateWorkChore(), this.operational.constructionHappening, this.operational.constructionHappening).ParamTransition<bool>(this.constructionRequested, this.operational.constructionSatisfied, GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.IsFalse).Enter(delegate(BuildingInternalConstructor.Instance smi)
		{
			smi.ShowConstructionSymbol(true);
		}).Exit(delegate(BuildingInternalConstructor.Instance smi)
		{
			smi.ShowConstructionSymbol(false);
		});
		this.operational.constructionSatisfied.EventTransition(GameHashes.OnStorageChange, this.operational.constructionRequired, (BuildingInternalConstructor.Instance smi) => !smi.HasOutputInStorage() && this.constructionRequested.Get(smi)).ParamTransition<bool>(this.constructionRequested, this.operational.constructionRequired, (BuildingInternalConstructor.Instance smi, bool p) => p && !smi.HasOutputInStorage());
	}

	// Token: 0x04001785 RID: 6021
	public GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.State inoperational;

	// Token: 0x04001786 RID: 6022
	public BuildingInternalConstructor.OperationalStates operational;

	// Token: 0x04001787 RID: 6023
	public StateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.BoolParameter constructionRequested = new StateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.BoolParameter(true);

	// Token: 0x0200145A RID: 5210
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006976 RID: 26998
		public DefComponent<Storage> storage;

		// Token: 0x04006977 RID: 26999
		public float constructionMass;

		// Token: 0x04006978 RID: 27000
		public List<string> outputIDs;

		// Token: 0x04006979 RID: 27001
		public bool spawnIntoStorage;

		// Token: 0x0400697A RID: 27002
		public string constructionSymbol;
	}

	// Token: 0x0200145B RID: 5211
	public class OperationalStates : GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.State
	{
		// Token: 0x0400697B RID: 27003
		public GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.State constructionRequired;

		// Token: 0x0400697C RID: 27004
		public GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.State constructionHappening;

		// Token: 0x0400697D RID: 27005
		public GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.State constructionSatisfied;
	}

	// Token: 0x0200145C RID: 5212
	public new class Instance : GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x06008A44 RID: 35396 RVA: 0x003332F0 File Offset: 0x003314F0
		public Instance(IStateMachineTarget master, BuildingInternalConstructor.Def def) : base(master, def)
		{
			this.storage = def.storage.Get(this);
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new InternalConstructionCompleteCondition(this));
		}

		// Token: 0x06008A45 RID: 35397 RVA: 0x00333320 File Offset: 0x00331520
		protected override void OnCleanUp()
		{
			Element element = null;
			float num = 0f;
			float num2 = 0f;
			byte maxValue = byte.MaxValue;
			int disease_count = 0;
			foreach (string s in base.def.outputIDs)
			{
				GameObject gameObject = this.storage.FindFirst(s);
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					global::Debug.Assert(element == null || element == component.Element);
					element = component.Element;
					num2 = GameUtil.GetFinalTemperature(num, num2, component.Mass, component.Temperature);
					num += component.Mass;
					gameObject.DeleteObject();
				}
			}
			if (element != null)
			{
				element.substance.SpawnResource(base.transform.GetPosition(), num, num2, maxValue, disease_count, false, false, false);
			}
			base.OnCleanUp();
		}

		// Token: 0x06008A46 RID: 35398 RVA: 0x00333420 File Offset: 0x00331620
		public FetchList2 CreateFetchList()
		{
			FetchList2 fetchList = new FetchList2(this.storage, Db.Get().ChoreTypes.Fetch);
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			fetchList.Add(component.Element.tag, null, base.def.constructionMass, Operational.State.None);
			return fetchList;
		}

		// Token: 0x06008A47 RID: 35399 RVA: 0x0033346C File Offset: 0x0033166C
		public PrimaryElement GetMassForConstruction()
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			return this.storage.FindFirstWithMass(component.Element.tag, base.def.constructionMass);
		}

		// Token: 0x06008A48 RID: 35400 RVA: 0x003334A1 File Offset: 0x003316A1
		public bool HasOutputInStorage()
		{
			return this.storage.FindFirst(base.def.outputIDs[0].ToTag());
		}

		// Token: 0x06008A49 RID: 35401 RVA: 0x003334C9 File Offset: 0x003316C9
		public bool IsRequestingConstruction()
		{
			base.sm.constructionRequested.Get(this);
			return base.smi.sm.constructionRequested.Get(base.smi);
		}

		// Token: 0x06008A4A RID: 35402 RVA: 0x003334F8 File Offset: 0x003316F8
		public void ConstructionComplete(bool force = false)
		{
			SimHashes element_id;
			if (!force)
			{
				PrimaryElement massForConstruction = this.GetMassForConstruction();
				element_id = massForConstruction.ElementID;
				float mass = massForConstruction.Mass;
				float num = massForConstruction.Temperature * massForConstruction.Mass;
				massForConstruction.Mass -= base.def.constructionMass;
				Mathf.Clamp(num / mass, 0f, 318.15f);
			}
			else
			{
				element_id = SimHashes.Cuprite;
				float temperature = base.GetComponent<PrimaryElement>().Temperature;
			}
			foreach (string s in base.def.outputIDs)
			{
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(s), base.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0);
				gameObject.GetComponent<PrimaryElement>().SetElement(element_id, false);
				gameObject.SetActive(true);
				if (base.def.spawnIntoStorage)
				{
					this.storage.Store(gameObject, false, false, true, false);
				}
			}
		}

		// Token: 0x06008A4B RID: 35403 RVA: 0x00333600 File Offset: 0x00331800
		public WorkChore<BuildingInternalConstructorWorkable> CreateWorkChore()
		{
			return new WorkChore<BuildingInternalConstructorWorkable>(Db.Get().ChoreTypes.Build, base.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x06008A4C RID: 35404 RVA: 0x00333638 File Offset: 0x00331838
		public void ShowConstructionSymbol(bool show)
		{
			KBatchedAnimController component = base.master.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.SetSymbolVisiblity(base.def.constructionSymbol, show);
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06008A4D RID: 35405 RVA: 0x00333674 File Offset: 0x00331874
		public string SidescreenButtonText
		{
			get
			{
				if (!base.smi.sm.constructionRequested.Get(base.smi))
				{
					return string.Format(UI.UISIDESCREENS.BUTTONMENUSIDESCREEN.ALLOW_INTERNAL_CONSTRUCTOR.text, Assets.GetPrefab(base.def.outputIDs[0]).GetProperName());
				}
				return string.Format(UI.UISIDESCREENS.BUTTONMENUSIDESCREEN.DISALLOW_INTERNAL_CONSTRUCTOR.text, Assets.GetPrefab(base.def.outputIDs[0]).GetProperName());
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x06008A4E RID: 35406 RVA: 0x00333700 File Offset: 0x00331900
		public string SidescreenButtonTooltip
		{
			get
			{
				if (!base.smi.sm.constructionRequested.Get(base.smi))
				{
					return string.Format(UI.UISIDESCREENS.BUTTONMENUSIDESCREEN.ALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP.text, Assets.GetPrefab(base.def.outputIDs[0]).GetProperName());
				}
				return string.Format(UI.UISIDESCREENS.BUTTONMENUSIDESCREEN.DISALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP.text, Assets.GetPrefab(base.def.outputIDs[0]).GetProperName());
			}
		}

		// Token: 0x06008A4F RID: 35407 RVA: 0x0033378C File Offset: 0x0033198C
		public void OnSidescreenButtonPressed()
		{
			base.smi.sm.constructionRequested.Set(!base.smi.sm.constructionRequested.Get(base.smi), base.smi, false);
			if (DebugHandler.InstantBuildMode && base.smi.sm.constructionRequested.Get(base.smi) && !this.HasOutputInStorage())
			{
				this.ConstructionComplete(true);
			}
		}

		// Token: 0x06008A50 RID: 35408 RVA: 0x00333807 File Offset: 0x00331A07
		public void SetButtonTextOverride(ButtonMenuTextOverride text)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06008A51 RID: 35409 RVA: 0x0033380E File Offset: 0x00331A0E
		public bool SidescreenEnabled()
		{
			return true;
		}

		// Token: 0x06008A52 RID: 35410 RVA: 0x00333811 File Offset: 0x00331A11
		public bool SidescreenButtonInteractable()
		{
			return true;
		}

		// Token: 0x06008A53 RID: 35411 RVA: 0x00333814 File Offset: 0x00331A14
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x06008A54 RID: 35412 RVA: 0x00333818 File Offset: 0x00331A18
		public int HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x0400697E RID: 27006
		private Storage storage;

		// Token: 0x0400697F RID: 27007
		[Serialize]
		private float constructionElapsed;

		// Token: 0x04006980 RID: 27008
		private ProgressBar progressBar;
	}
}
