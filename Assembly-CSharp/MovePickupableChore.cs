using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000445 RID: 1093
public class MovePickupableChore : Chore<MovePickupableChore.StatesInstance>
{
	// Token: 0x0600173A RID: 5946 RVA: 0x0007DA00 File Offset: 0x0007BC00
	public MovePickupableChore(IStateMachineTarget target, GameObject pickupable, Action<Chore> onEnd) : base((pickupable.GetComponent<CreatureBrain>() == null) ? Db.Get().ChoreTypes.Fetch : Db.Get().ChoreTypes.Ranch, target, target.GetComponent<ChoreProvider>(), false, null, null, onEnd, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MovePickupableChore.StatesInstance(this);
		Pickupable component = pickupable.GetComponent<Pickupable>();
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, target.GetComponent<Storage>());
		this.AddPrecondition(ChorePreconditions.instance.IsNotARobot, "FetchDrone");
		this.AddPrecondition(ChorePreconditions.instance.IsNotTransferArm, this);
		if (pickupable.GetComponent<CreatureBrain>())
		{
			this.AddPrecondition(MovePickupableChore.CanReachCritter, pickupable);
			this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanWrangleCreatures);
			this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, pickupable.GetComponent<Capturable>());
		}
		else
		{
			this.AddPrecondition(ChorePreconditions.instance.CanPickup, component);
		}
		PrimaryElement primaryElement = component.PrimaryElement;
		base.smi.sm.requestedamount.Set(primaryElement.Mass, base.smi, false);
		base.smi.sm.pickupablesource.Set(pickupable.gameObject, base.smi, false);
		base.smi.sm.deliverypoint.Set(target.gameObject, base.smi, false);
		this.movePlacer = target.gameObject;
		bool flag = MinionGroupProber.Get().IsReachable(Grid.PosToCell(pickupable), OffsetGroups.Standard) && MinionGroupProber.Get().IsReachable(Grid.PosToCell(target.gameObject), OffsetGroups.Standard);
		this.OnReachableChanged(flag);
		pickupable.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
		target.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
		Prioritizable component2 = target.GetComponent<Prioritizable>();
		if (!component2.IsPrioritizable())
		{
			component2.AddRef();
		}
		base.SetPrioritizable(target.GetComponent<Prioritizable>());
	}

	// Token: 0x0600173B RID: 5947 RVA: 0x0007DC14 File Offset: 0x0007BE14
	private void OnReachableChanged(object data)
	{
		Color color = ((bool)data) ? Color.white : new Color(0.91f, 0.21f, 0.2f);
		this.SetColor(this.movePlacer, color);
	}

	// Token: 0x0600173C RID: 5948 RVA: 0x0007DC52 File Offset: 0x0007BE52
	private void SetColor(GameObject visualizer, Color color)
	{
		if (visualizer != null)
		{
			visualizer.GetComponentInChildren<MeshRenderer>().material.color = color;
		}
	}

	// Token: 0x0600173D RID: 5949 RVA: 0x0007DC70 File Offset: 0x0007BE70
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("MovePickupable null context.consumer");
			return;
		}
		if (base.smi == null)
		{
			global::Debug.LogError("MovePickupable null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			global::Debug.LogError("MovePickupable null smi.sm");
			return;
		}
		if (base.smi.sm.pickupablesource == null)
		{
			global::Debug.LogError("MovePickupable null smi.sm.pickupablesource");
			return;
		}
		base.smi.sm.deliverer.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x04000D0D RID: 3341
	public GameObject movePlacer;

	// Token: 0x04000D0E RID: 3342
	public static Chore.Precondition CanReachCritter = new Chore.Precondition
	{
		id = "CanReachCritter",
		description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			GameObject gameObject = (GameObject)data;
			return !(gameObject == null) && gameObject.HasTag(GameTags.Reachable);
		}
	};

	// Token: 0x020011D3 RID: 4563
	public class StatesInstance : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.GameInstance
	{
		// Token: 0x06008138 RID: 33080 RVA: 0x00316297 File Offset: 0x00314497
		public StatesInstance(MovePickupableChore master) : base(master)
		{
		}
	}

	// Token: 0x020011D4 RID: 4564
	public class States : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore>
	{
		// Token: 0x06008139 RID: 33081 RVA: 0x003162A0 File Offset: 0x003144A0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.deliverypoint);
			this.fetch.Target(this.deliverer).DefaultState(this.fetch.approach).Enter(delegate(MovePickupableChore.StatesInstance smi)
			{
				this.pickupablesource.Get<Pickupable>(smi).ClearReservations();
			}).ToggleReserve(this.deliverer, this.pickupablesource, this.requestedamount, this.actualamount).EnterTransition(this.fetch.approachCritter, (MovePickupableChore.StatesInstance smi) => this.IsCritter(smi)).OnTargetLost(this.pickupablesource, null);
			this.fetch.approachCritter.Enter(delegate(MovePickupableChore.StatesInstance smi)
			{
				GameObject gameObject = this.pickupablesource.Get(smi);
				if (!gameObject.HasTag(GameTags.Creatures.Bagged))
				{
					IdleStates.Instance smi2 = gameObject.GetSMI<IdleStates.Instance>();
					if (!smi2.IsNullOrStopped())
					{
						smi2.GoTo(smi2.sm.root);
					}
					FlopStates.Instance smi3 = gameObject.GetSMI<FlopStates.Instance>();
					if (!smi3.IsNullOrStopped())
					{
						smi3.GoTo(smi3.sm.root);
					}
					gameObject.GetComponent<Navigator>().Stop(false, true);
				}
			}).MoveTo<Capturable>(this.pickupablesource, this.fetch.wrangle, null, null, null);
			this.fetch.wrangle.EnterTransition(this.fetch.approach, (MovePickupableChore.StatesInstance smi) => this.pickupablesource.Get(smi).HasTag(GameTags.Creatures.Bagged)).ToggleWork<Capturable>(this.pickupablesource, this.fetch.approach, null, null);
			this.fetch.approach.MoveTo<IApproachable>(this.pickupablesource, this.fetch.pickup, new Func<MovePickupableChore.StatesInstance, CellOffset[]>(this.GetFetcherOffset), null, null);
			this.fetch.pickup.DoPickup(this.pickupablesource, this.pickup, this.actualamount, this.approachstorage, this.delivering.deliverfail);
			this.approachstorage.DefaultState(this.approachstorage.deliveryStorage);
			this.approachstorage.deliveryStorage.InitializeStates(this.deliverer, this.deliverypoint, new Func<MovePickupableChore.StatesInstance, CellOffset[]>(this.GetFetcherOffset), this.delivering.storing, this.delivering.deliverfail, NavigationTactics.ReduceTravelDistance);
			this.delivering.storing.Target(this.deliverer).DoDelivery(this.deliverer, this.deliverypoint, this.success, this.delivering.deliverfail);
			this.delivering.deliverfail.ReturnFailure();
			this.success.Enter(delegate(MovePickupableChore.StatesInstance smi)
			{
				Storage component = this.deliverypoint.Get(smi).GetComponent<Storage>();
				Storage component2 = this.deliverer.Get(smi).GetComponent<Storage>();
				float num = this.actualamount.Get(smi);
				GameObject gameObject = this.pickup.Get(smi);
				num += gameObject.GetComponent<PrimaryElement>().Mass;
				this.actualamount.Set(num, smi, false);
				component2.Transfer(this.pickup.Get(smi), component, false, false);
				this.DropPickupable(component, gameObject);
				CancellableMove component3 = component.GetComponent<CancellableMove>();
				Movable component4 = gameObject.GetComponent<Movable>();
				component3.RemoveMovable(component4);
				component4.ClearMove();
				if (!this.IsDeliveryComplete(smi))
				{
					GameObject go = this.pickupablesource.Get(smi);
					int num2 = Grid.PosToCell(this.deliverypoint.Get(smi));
					if (this.pickupablesource.Get(smi) == null || Grid.PosToCell(go) == num2)
					{
						GameObject nextTarget = component3.GetNextTarget();
						this.pickupablesource.Set(nextTarget, smi, false);
						PrimaryElement component5 = nextTarget.GetComponent<PrimaryElement>();
						smi.sm.requestedamount.Set(component5.Mass, smi, false);
					}
					smi.GoTo(this.fetch);
				}
			}).ReturnSuccess();
		}

		// Token: 0x0600813A RID: 33082 RVA: 0x003164D5 File Offset: 0x003146D5
		private CellOffset[] GetFetcherOffset(MovePickupableChore.StatesInstance smi)
		{
			return this.deliverer.Get(smi).GetComponent<WorkerBase>().GetFetchCellOffsets();
		}

		// Token: 0x0600813B RID: 33083 RVA: 0x003164F0 File Offset: 0x003146F0
		private void DropPickupable(Storage storage, GameObject delivered)
		{
			if (delivered.GetComponent<Capturable>() != null)
			{
				List<GameObject> items = storage.items;
				int count = items.Count;
				Vector3 position = Grid.CellToPosCBC(Grid.PosToCell(storage), Grid.SceneLayer.Creatures);
				for (int i = count - 1; i >= 0; i--)
				{
					GameObject gameObject = items[i];
					storage.Drop(gameObject, true);
					gameObject.transform.SetPosition(position);
					gameObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
				}
				return;
			}
			storage.DropAll(false, false, default(Vector3), true, null);
		}

		// Token: 0x0600813C RID: 33084 RVA: 0x00316574 File Offset: 0x00314774
		private bool IsDeliveryComplete(MovePickupableChore.StatesInstance smi)
		{
			GameObject gameObject = smi.sm.deliverypoint.Get(smi);
			return !(gameObject != null) || gameObject.GetComponent<CancellableMove>().IsDeliveryComplete();
		}

		// Token: 0x0600813D RID: 33085 RVA: 0x003165AC File Offset: 0x003147AC
		private bool IsCritter(MovePickupableChore.StatesInstance smi)
		{
			GameObject gameObject = this.pickupablesource.Get(smi);
			return gameObject != null && gameObject.GetComponent<Capturable>() != null;
		}

		// Token: 0x04006186 RID: 24966
		public static CellOffset[] critterCellOffsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};

		// Token: 0x04006187 RID: 24967
		public static HashedString[] critterReleaseWorkAnims = new HashedString[]
		{
			"place",
			"release"
		};

		// Token: 0x04006188 RID: 24968
		public static KAnimFile[] critterReleaseAnim = new KAnimFile[]
		{
			Assets.GetAnim("anim_restrain_creature_kanim")
		};

		// Token: 0x04006189 RID: 24969
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter deliverer;

		// Token: 0x0400618A RID: 24970
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter pickupablesource;

		// Token: 0x0400618B RID: 24971
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter pickup;

		// Token: 0x0400618C RID: 24972
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter deliverypoint;

		// Token: 0x0400618D RID: 24973
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.FloatParameter requestedamount;

		// Token: 0x0400618E RID: 24974
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.FloatParameter actualamount;

		// Token: 0x0400618F RID: 24975
		public MovePickupableChore.States.FetchState fetch;

		// Token: 0x04006190 RID: 24976
		public MovePickupableChore.States.ApproachStorage approachstorage;

		// Token: 0x04006191 RID: 24977
		public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State success;

		// Token: 0x04006192 RID: 24978
		public MovePickupableChore.States.DeliveryState delivering;

		// Token: 0x020023CF RID: 9167
		public class ApproachStorage : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State
		{
			// Token: 0x04009FE1 RID: 40929
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.ApproachSubState<Storage> deliveryStorage;

			// Token: 0x04009FE2 RID: 40930
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.ApproachSubState<Storage> unbagCritter;
		}

		// Token: 0x020023D0 RID: 9168
		public class DeliveryState : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State
		{
			// Token: 0x04009FE3 RID: 40931
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State storing;

			// Token: 0x04009FE4 RID: 40932
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State deliverfail;
		}

		// Token: 0x020023D1 RID: 9169
		public class FetchState : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State
		{
			// Token: 0x04009FE5 RID: 40933
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.ApproachSubState<Pickupable> approach;

			// Token: 0x04009FE6 RID: 40934
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State pickup;

			// Token: 0x04009FE7 RID: 40935
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State approachCritter;

			// Token: 0x04009FE8 RID: 40936
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State wrangle;
		}
	}
}
