using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020007A5 RID: 1957
public class CargoDropperMinion : GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>
{
	// Token: 0x0600358C RID: 13708 RVA: 0x00123454 File Offset: 0x00121654
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.notLanded;
		this.root.ParamTransition<bool>(this.hasLanded, this.complete, GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.IsTrue);
		this.notLanded.EventHandlerTransition(GameHashes.JettisonCargo, this.landed, (CargoDropperMinion.StatesInstance smi, object obj) => true);
		this.landed.Enter(delegate(CargoDropperMinion.StatesInstance smi)
		{
			smi.JettisonCargo(null);
			smi.GoTo(this.exiting);
		});
		this.exiting.Update(delegate(CargoDropperMinion.StatesInstance smi, float dt)
		{
			if (!smi.SyncMinionExitAnimation())
			{
				smi.GoTo(this.complete);
			}
		}, UpdateRate.SIM_200ms, false);
		this.complete.Enter(delegate(CargoDropperMinion.StatesInstance smi)
		{
			this.hasLanded.Set(true, smi, false);
		});
	}

	// Token: 0x04001FD3 RID: 8147
	private GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State notLanded;

	// Token: 0x04001FD4 RID: 8148
	private GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State landed;

	// Token: 0x04001FD5 RID: 8149
	private GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State exiting;

	// Token: 0x04001FD6 RID: 8150
	private GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State complete;

	// Token: 0x04001FD7 RID: 8151
	public StateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.BoolParameter hasLanded = new StateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.BoolParameter(false);

	// Token: 0x0200165B RID: 5723
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006F70 RID: 28528
		public Vector3 dropOffset;

		// Token: 0x04006F71 RID: 28529
		public string kAnimName;

		// Token: 0x04006F72 RID: 28530
		public string animName;

		// Token: 0x04006F73 RID: 28531
		public Grid.SceneLayer animLayer = Grid.SceneLayer.Move;

		// Token: 0x04006F74 RID: 28532
		public bool notifyOnJettison;
	}

	// Token: 0x0200165C RID: 5724
	public class StatesInstance : GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.GameInstance
	{
		// Token: 0x060091F6 RID: 37366 RVA: 0x00352870 File Offset: 0x00350A70
		public StatesInstance(IStateMachineTarget master, CargoDropperMinion.Def def) : base(master, def)
		{
		}

		// Token: 0x060091F7 RID: 37367 RVA: 0x0035287C File Offset: 0x00350A7C
		public void JettisonCargo(object data = null)
		{
			Vector3 pos = base.master.transform.GetPosition() + base.def.dropOffset;
			MinionStorage component = base.GetComponent<MinionStorage>();
			if (component != null)
			{
				List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
				for (int i = storedMinionInfo.Count - 1; i >= 0; i--)
				{
					MinionStorage.Info info = storedMinionInfo[i];
					GameObject gameObject = component.DeserializeMinion(info.id, pos);
					this.escapingMinion = gameObject.GetComponent<MinionIdentity>();
					gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
					ChoreProvider component2 = gameObject.GetComponent<ChoreProvider>();
					if (component2 != null)
					{
						this.exitAnimChore = new EmoteChore(component2, Db.Get().ChoreTypes.EmoteHighPriority, base.def.kAnimName, new HashedString[]
						{
							base.def.animName
						}, KAnim.PlayMode.Once, false);
						Vector3 position = gameObject.transform.GetPosition();
						position.z = Grid.GetLayerZ(base.def.animLayer);
						gameObject.transform.SetPosition(position);
						gameObject.GetMyWorld().SetDupeVisited();
					}
					if (base.def.notifyOnJettison)
					{
						gameObject.GetComponent<Notifier>().Add(this.CreateCrashLandedNotification(), "");
					}
				}
			}
		}

		// Token: 0x060091F8 RID: 37368 RVA: 0x003529D8 File Offset: 0x00350BD8
		public bool SyncMinionExitAnimation()
		{
			if (this.escapingMinion != null && this.exitAnimChore != null && !this.exitAnimChore.isComplete)
			{
				KBatchedAnimController component = this.escapingMinion.GetComponent<KBatchedAnimController>();
				KBatchedAnimController component2 = base.master.GetComponent<KBatchedAnimController>();
				if (component2.CurrentAnim.name == base.def.animName)
				{
					component.SetElapsedTime(component2.GetElapsedTime());
					return true;
				}
			}
			return false;
		}

		// Token: 0x060091F9 RID: 37369 RVA: 0x00352A4C File Offset: 0x00350C4C
		public Notification CreateCrashLandedNotification()
		{
			return new Notification(MISC.NOTIFICATIONS.DUPLICANT_CRASH_LANDED.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.DUPLICANT_CRASH_LANDED.TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
		}

		// Token: 0x04006F75 RID: 28533
		public MinionIdentity escapingMinion;

		// Token: 0x04006F76 RID: 28534
		public Chore exitAnimChore;
	}
}
