using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020008C0 RID: 2240
[SerializationConfig(MemberSerialization.OptIn)]
public class GameFlowManager : StateMachineComponent<GameFlowManager.StatesInstance>, ISaveLoadable
{
	// Token: 0x06003F14 RID: 16148 RVA: 0x0015E73B File Offset: 0x0015C93B
	public static void DestroyInstance()
	{
		GameFlowManager.Instance = null;
	}

	// Token: 0x06003F15 RID: 16149 RVA: 0x0015E743 File Offset: 0x0015C943
	protected override void OnPrefabInit()
	{
		GameFlowManager.Instance = this;
	}

	// Token: 0x06003F16 RID: 16150 RVA: 0x0015E74B File Offset: 0x0015C94B
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06003F17 RID: 16151 RVA: 0x0015E758 File Offset: 0x0015C958
	public bool IsGameOver()
	{
		return base.smi.IsInsideState(base.smi.sm.gameover);
	}

	// Token: 0x04002718 RID: 10008
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04002719 RID: 10009
	public static GameFlowManager Instance;

	// Token: 0x020017CE RID: 6094
	public class StatesInstance : GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.GameInstance
	{
		// Token: 0x060096AD RID: 38573 RVA: 0x003628FD File Offset: 0x00360AFD
		public bool IsIncapacitated(GameObject go)
		{
			return false;
		}

		// Token: 0x060096AE RID: 38574 RVA: 0x00362900 File Offset: 0x00360B00
		public void CheckForGameOver()
		{
			if (!Game.Instance.GameStarted())
			{
				return;
			}
			if (GenericGameSettings.instance.disableGameOver)
			{
				return;
			}
			bool flag = false;
			if (Components.LiveMinionIdentities.Count == 0)
			{
				flag = true;
			}
			else
			{
				flag = true;
				foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
				{
					if (!this.IsIncapacitated(minionIdentity.gameObject))
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				this.GoTo(base.sm.gameover.pending);
			}
		}

		// Token: 0x060096AF RID: 38575 RVA: 0x003629AC File Offset: 0x00360BAC
		public StatesInstance(GameFlowManager smi) : base(smi)
		{
		}

		// Token: 0x040073CF RID: 29647
		public Notification colonyLostNotification = new Notification(MISC.NOTIFICATIONS.COLONYLOST.NAME, NotificationType.Bad, null, null, false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x020017CF RID: 6095
	public class States : GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager>
	{
		// Token: 0x060096B0 RID: 38576 RVA: 0x003629E4 File Offset: 0x00360BE4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.loading;
			this.loading.ScheduleGoTo(4f, this.running);
			this.running.Update("CheckForGameOver", delegate(GameFlowManager.StatesInstance smi, float dt)
			{
				smi.CheckForGameOver();
			}, UpdateRate.SIM_200ms, false);
			this.gameover.TriggerOnEnter(GameHashes.GameOver, null).ToggleNotification((GameFlowManager.StatesInstance smi) => smi.colonyLostNotification);
			this.gameover.pending.Enter("Goto(gameover.active)", delegate(GameFlowManager.StatesInstance smi)
			{
				UIScheduler.Instance.Schedule("Goto(gameover.active)", 4f, delegate(object d)
				{
					smi.GoTo(this.gameover.active);
				}, null, null);
			});
			this.gameover.active.Enter(delegate(GameFlowManager.StatesInstance smi)
			{
				if (GenericGameSettings.instance.demoMode)
				{
					DemoTimer.Instance.EndDemo();
					return;
				}
				GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.GameOverScreen, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<KScreen>().Show(true);
			});
		}

		// Token: 0x040073D0 RID: 29648
		public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State loading;

		// Token: 0x040073D1 RID: 29649
		public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State running;

		// Token: 0x040073D2 RID: 29650
		public GameFlowManager.States.GameOverState gameover;

		// Token: 0x02002590 RID: 9616
		public class GameOverState : GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State
		{
			// Token: 0x0400A743 RID: 42819
			public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State pending;

			// Token: 0x0400A744 RID: 42820
			public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State active;
		}
	}
}
