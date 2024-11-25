﻿using System;
using System.Collections.Generic;
using FoodRehydrator;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000437 RID: 1079
public class EatChore : Chore<EatChore.StatesInstance>
{
	// Token: 0x060016EB RID: 5867 RVA: 0x0007BC2C File Offset: 0x00079E2C
	public EatChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.Eat, master, master.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		base.smi = new EatChore.StatesInstance(this);
		this.showAvailabilityInHoverText = false;
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(EatChore.EdibleIsNotNull, null);
	}

	// Token: 0x060016EC RID: 5868 RVA: 0x0007BC94 File Offset: 0x00079E94
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("EATCHORE null context.consumer");
			return;
		}
		RationMonitor.Instance smi = context.consumerState.consumer.GetSMI<RationMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("EATCHORE null RationMonitor.Instance");
			return;
		}
		Edible edible = smi.GetEdible();
		if (edible.gameObject == null)
		{
			global::Debug.LogError("EATCHORE null edible.gameObject");
			return;
		}
		if (base.smi == null)
		{
			global::Debug.LogError("EATCHORE null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			global::Debug.LogError("EATCHORE null smi.sm");
			return;
		}
		if (base.smi.sm.ediblesource == null)
		{
			global::Debug.LogError("EATCHORE null smi.sm.ediblesource");
			return;
		}
		base.smi.sm.ediblesource.Set(edible.gameObject, base.smi, false);
		KCrashReporter.Assert(edible.FoodInfo.CaloriesPerUnit > 0f, edible.GetProperName() + " has invalid calories per unit. Will result in NaNs", null);
		AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(this.gameObject);
		float num = (amountInstance.GetMax() - amountInstance.value) / edible.FoodInfo.CaloriesPerUnit;
		KCrashReporter.Assert(num > 0f, "EatChore is requesting an invalid amount of food", null);
		base.smi.sm.requestedfoodunits.Set(num, base.smi, false);
		base.smi.sm.eater.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x04000CF4 RID: 3316
	public static readonly Chore.Precondition EdibleIsNotNull = new Chore.Precondition
	{
		id = "EdibleIsNotNull",
		description = DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return null != context.consumerState.consumer.GetSMI<RationMonitor.Instance>().GetEdible();
		}
	};

	// Token: 0x020011B0 RID: 4528
	public class StatesInstance : GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.GameInstance
	{
		// Token: 0x060080C0 RID: 32960 RVA: 0x003128A8 File Offset: 0x00310AA8
		public StatesInstance(EatChore master) : base(master)
		{
		}

		// Token: 0x060080C1 RID: 32961 RVA: 0x003128B4 File Offset: 0x00310AB4
		public void UpdateMessStation()
		{
			Ownables soleOwner = base.sm.eater.Get(base.smi).GetComponent<MinionIdentity>().GetSoleOwner();
			List<Assignable> preferredAssignables = Game.Instance.assignmentManager.GetPreferredAssignables(soleOwner, Db.Get().AssignableSlots.MessStation);
			if (preferredAssignables.Count == 0)
			{
				soleOwner.AutoAssignSlot(Db.Get().AssignableSlots.MessStation);
				preferredAssignables = Game.Instance.assignmentManager.GetPreferredAssignables(soleOwner, Db.Get().AssignableSlots.MessStation);
			}
			Assignable value = (preferredAssignables.Count > 0) ? preferredAssignables[0] : null;
			base.smi.sm.messstation.Set(value, base.smi);
		}

		// Token: 0x060080C2 RID: 32962 RVA: 0x00312970 File Offset: 0x00310B70
		public bool UseSalt()
		{
			if (base.smi.sm.messstation != null && base.smi.sm.messstation.Get(base.smi) != null)
			{
				MessStation component = base.smi.sm.messstation.Get(base.smi).GetComponent<MessStation>();
				return component != null && component.HasSalt;
			}
			return false;
		}

		// Token: 0x060080C3 RID: 32963 RVA: 0x003129E8 File Offset: 0x00310BE8
		public void CreateLocator()
		{
			int num = base.sm.eater.Get<Sensors>(base.smi).GetSensor<SafeCellSensor>().GetCellQuery();
			if (num == Grid.InvalidCell)
			{
				num = Grid.PosToCell(base.sm.eater.Get<Transform>(base.smi).GetPosition());
			}
			Vector3 pos = Grid.CellToPosCBC(num, Grid.SceneLayer.Move);
			Grid.Reserved[num] = true;
			GameObject value = ChoreHelpers.CreateLocator("EatLocator", pos);
			base.sm.locator.Set(value, this, false);
			this.locatorCell = num;
		}

		// Token: 0x060080C4 RID: 32964 RVA: 0x00312A7B File Offset: 0x00310C7B
		public void DestroyLocator()
		{
			Grid.Reserved[this.locatorCell] = false;
			ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
			base.sm.locator.Set(null, this);
		}

		// Token: 0x060080C5 RID: 32965 RVA: 0x00312AB8 File Offset: 0x00310CB8
		public void SetZ(GameObject go, float z)
		{
			Vector3 position = go.transform.GetPosition();
			position.z = z;
			go.transform.SetPosition(position);
		}

		// Token: 0x060080C6 RID: 32966 RVA: 0x00312AE8 File Offset: 0x00310CE8
		public void ApplyRoomEffects()
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.sm.messstation.Get(base.smi).gameObject);
			if (roomOfGameObject != null)
			{
				roomOfGameObject.roomType.TriggerRoomEffects(base.sm.messstation.Get(base.smi).gameObject.GetComponent<KPrefabID>(), base.sm.eater.Get(base.smi).gameObject.GetComponent<Effects>());
			}
		}

		// Token: 0x060080C7 RID: 32967 RVA: 0x00312B70 File Offset: 0x00310D70
		public void ApplySaltEffect()
		{
			Storage component = base.sm.messstation.Get(base.smi).gameObject.GetComponent<Storage>();
			if (component != null && component.Has(TableSaltConfig.ID.ToTag()))
			{
				component.ConsumeIgnoringDisease(TableSaltConfig.ID.ToTag(), TableSaltTuning.CONSUMABLE_RATE);
				base.sm.eater.Get(base.smi).gameObject.GetComponent<WorkerBase>().GetComponent<Effects>().Add("MessTableSalt", true);
				base.sm.messstation.Get(base.smi).gameObject.Trigger(1356255274, null);
			}
		}

		// Token: 0x04006102 RID: 24834
		private int locatorCell;
	}

	// Token: 0x020011B1 RID: 4529
	public class States : GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore>
	{
		// Token: 0x060080C8 RID: 32968 RVA: 0x00312C28 File Offset: 0x00310E28
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.chooseaction;
			base.Target(this.eater);
			this.root.Enter("SetMessStation", delegate(EatChore.StatesInstance smi)
			{
				smi.UpdateMessStation();
			}).EventHandler(GameHashes.AssignablesChanged, delegate(EatChore.StatesInstance smi)
			{
				smi.UpdateMessStation();
			});
			this.chooseaction.EnterTransition(this.rehydrate, (EatChore.StatesInstance smi) => this.ediblesource.Get(smi).HasTag(GameTags.Dehydrated)).EnterTransition(this.fetch, (EatChore.StatesInstance smi) => true);
			this.rehydrate.Enter(delegate(EatChore.StatesInstance smi)
			{
				DehydratedFoodPackage component = this.ediblesource.Get(smi).GetComponent<Pickupable>().storage.gameObject.GetComponent<DehydratedFoodPackage>();
				this.rehydrate.foodpackage.Set(component, smi);
				GameObject rehydrator = component.Rehydrator;
				this.rehydrate.rehydrator.Set((rehydrator != null) ? component.Rehydrator.GetComponent<AccessabilityManager>() : null, smi, false);
				AccessabilityManager accessabilityManager = this.rehydrate.rehydrator.Get(smi);
				if (!(accessabilityManager != null))
				{
					smi.GoTo(null);
					return;
				}
				GameObject worker = this.eater.Get(smi);
				if (accessabilityManager.CanAccess(worker))
				{
					accessabilityManager.Reserve(this.eater.Get(smi));
					return;
				}
				smi.GoTo(null);
			}).Exit(delegate(EatChore.StatesInstance smi)
			{
				AccessabilityManager accessabilityManager = this.rehydrate.rehydrator.Get(smi);
				if (accessabilityManager != null)
				{
					accessabilityManager.Unreserve();
				}
			}).DefaultState(this.rehydrate.approach);
			this.rehydrate.approach.InitializeStates(this.eater, this.rehydrate.foodpackage, this.rehydrate.work, null, null, NavigationTactics.ReduceTravelDistance).OnTargetLost(this.ediblesource, null);
			this.rehydrate.work.ToggleWork("Rehydrate", delegate(EatChore.StatesInstance smi)
			{
				WorkerBase workerBase = this.eater.Get<WorkerBase>(smi);
				DehydratedFoodPackage pkg = this.rehydrate.foodpackage.Get<DehydratedFoodPackage>(smi);
				workerBase.StartWork(new DehydratedFoodPackage.RehydrateStartWorkItem(pkg, delegate(GameObject result)
				{
					this.ediblechunk.Set(result, smi, false);
				}));
			}, delegate(EatChore.StatesInstance smi)
			{
				AccessabilityManager accessabilityManager = this.rehydrate.rehydrator.Get(smi);
				return !(accessabilityManager == null) && accessabilityManager.CanAccess(this.eater.Get<WorkerBase>(smi).gameObject);
			}, this.eatatmessstation, null);
			this.fetch.InitializeStates(this.eater, this.ediblesource, this.ediblechunk, this.requestedfoodunits, this.actualfoodunits, this.eatatmessstation, null);
			this.eatatmessstation.DefaultState(this.eatatmessstation.moveto).ParamTransition<GameObject>(this.messstation, this.eatonfloorstate, (EatChore.StatesInstance smi, GameObject p) => p == null).ParamTransition<GameObject>(this.messstation, this.eatonfloorstate, (EatChore.StatesInstance smi, GameObject p) => p != null && !p.GetComponent<Operational>().IsOperational);
			this.eatatmessstation.moveto.InitializeStates(this.eater, this.messstation, this.eatatmessstation.eat, this.eatonfloorstate, null, null);
			this.eatatmessstation.eat.Enter("AnimOverride", delegate(EatChore.StatesInstance smi)
			{
				smi.GetComponent<KAnimControllerBase>().AddAnimOverrides(Assets.GetAnim("anim_eat_table_kanim"), 0f);
			}).DoEat(this.ediblechunk, this.actualfoodunits, null, null).Enter(delegate(EatChore.StatesInstance smi)
			{
				smi.SetZ(this.eater.Get(smi), Grid.GetLayerZ(Grid.SceneLayer.BuildingFront));
				smi.ApplyRoomEffects();
				smi.ApplySaltEffect();
			}).Exit(delegate(EatChore.StatesInstance smi)
			{
				smi.SetZ(this.eater.Get(smi), Grid.GetLayerZ(Grid.SceneLayer.Move));
				smi.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(Assets.GetAnim("anim_eat_table_kanim"));
			});
			this.eatonfloorstate.DefaultState(this.eatonfloorstate.moveto).Enter("CreateLocator", delegate(EatChore.StatesInstance smi)
			{
				smi.CreateLocator();
			}).Exit("DestroyLocator", delegate(EatChore.StatesInstance smi)
			{
				smi.DestroyLocator();
			});
			this.eatonfloorstate.moveto.InitializeStates(this.eater, this.locator, this.eatonfloorstate.eat, this.eatonfloorstate.eat, null, null);
			this.eatonfloorstate.eat.ToggleAnims("anim_eat_floor_kanim", 0f).DoEat(this.ediblechunk, this.actualfoodunits, null, null);
		}

		// Token: 0x04006103 RID: 24835
		public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter eater;

		// Token: 0x04006104 RID: 24836
		public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter ediblesource;

		// Token: 0x04006105 RID: 24837
		public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter ediblechunk;

		// Token: 0x04006106 RID: 24838
		public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter messstation;

		// Token: 0x04006107 RID: 24839
		public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.FloatParameter requestedfoodunits;

		// Token: 0x04006108 RID: 24840
		public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.FloatParameter actualfoodunits;

		// Token: 0x04006109 RID: 24841
		public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter locator;

		// Token: 0x0400610A RID: 24842
		public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State chooseaction;

		// Token: 0x0400610B RID: 24843
		public EatChore.States.RehydrateSubState rehydrate;

		// Token: 0x0400610C RID: 24844
		public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.FetchSubState fetch;

		// Token: 0x0400610D RID: 24845
		public EatChore.States.EatOnFloorState eatonfloorstate;

		// Token: 0x0400610E RID: 24846
		public EatChore.States.EatAtMessStationState eatatmessstation;

		// Token: 0x020023B4 RID: 9140
		public class EatOnFloorState : GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State
		{
			// Token: 0x04009F6B RID: 40811
			public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.ApproachSubState<IApproachable> moveto;

			// Token: 0x04009F6C RID: 40812
			public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State eat;
		}

		// Token: 0x020023B5 RID: 9141
		public class EatAtMessStationState : GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State
		{
			// Token: 0x04009F6D RID: 40813
			public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.ApproachSubState<MessStation> moveto;

			// Token: 0x04009F6E RID: 40814
			public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State eat;
		}

		// Token: 0x020023B6 RID: 9142
		public class RehydrateSubState : GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State
		{
			// Token: 0x04009F6F RID: 40815
			public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter foodpackage;

			// Token: 0x04009F70 RID: 40816
			public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.ObjectParameter<AccessabilityManager> rehydrator;

			// Token: 0x04009F71 RID: 40817
			public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.ApproachSubState<DehydratedFoodPackage> approach;

			// Token: 0x04009F72 RID: 40818
			public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State work;
		}
	}
}
