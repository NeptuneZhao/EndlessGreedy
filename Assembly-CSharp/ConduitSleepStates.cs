using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000C5 RID: 197
public class ConduitSleepStates : GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>
{
	// Token: 0x06000385 RID: 901 RVA: 0x0001D2F0 File Offset: 0x0001B4F0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.connector.moveToSleepLocation;
		this.root.EventTransition(GameHashes.NewDay, (ConduitSleepStates.Instance smi) => GameClock.Instance, this.behaviourcomplete, null).Exit(new StateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State.Callback(ConduitSleepStates.CleanUp));
		GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State moveToSleepLocation = this.connector.moveToSleepLocation;
		string name = CREATURES.STATUSITEMS.DROWSY.NAME;
		string tooltip = CREATURES.STATUSITEMS.DROWSY.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		moveToSleepLocation.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).MoveTo(delegate(ConduitSleepStates.Instance smi)
		{
			ConduitSleepMonitor.Instance smi2 = smi.GetSMI<ConduitSleepMonitor.Instance>();
			return smi2.sm.targetSleepCell.Get(smi2);
		}, this.drowsy, this.behaviourcomplete, false);
		GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State state = this.drowsy;
		string name2 = CREATURES.STATUSITEMS.DROWSY.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.DROWSY.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main).Enter(delegate(ConduitSleepStates.Instance smi)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Ceiling);
		}).Enter(delegate(ConduitSleepStates.Instance smi)
		{
			if (GameClock.Instance.IsNighttime())
			{
				smi.GoTo(this.connector.sleep);
			}
		}).DefaultState(this.drowsy.loop);
		this.drowsy.loop.PlayAnim("drowsy_pre").QueueAnim("drowsy_loop", true, null).EventTransition(GameHashes.Nighttime, (ConduitSleepStates.Instance smi) => GameClock.Instance, this.drowsy.pst, (ConduitSleepStates.Instance smi) => GameClock.Instance.IsNighttime());
		this.drowsy.pst.PlayAnim("drowsy_pst").OnAnimQueueComplete(this.connector.sleep);
		GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State sleep = this.connector.sleep;
		string name3 = CREATURES.STATUSITEMS.SLEEPING.NAME;
		string tooltip3 = CREATURES.STATUSITEMS.SLEEPING.TOOLTIP;
		string icon3 = "";
		StatusItem.IconType icon_type3 = StatusItem.IconType.Info;
		NotificationType notification_type3 = NotificationType.Neutral;
		bool allow_multiples3 = false;
		main = Db.Get().StatusItemCategories.Main;
		sleep.ToggleStatusItem(name3, tooltip3, icon3, icon_type3, notification_type3, allow_multiples3, default(HashedString), 129022, null, null, main).Enter(delegate(ConduitSleepStates.Instance smi)
		{
			if (!smi.staterpillar.IsConnectorBuildingSpawned())
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Ceiling);
			smi.staterpillar.EnableConnector();
			if (smi.staterpillar.IsConnected())
			{
				smi.GoTo(this.connector.sleep.connected);
				return;
			}
			smi.GoTo(this.connector.sleep.noConnection);
		});
		this.connector.sleep.connected.Enter(delegate(ConduitSleepStates.Instance smi)
		{
			smi.animController.SetSceneLayer(ConduitSleepStates.GetSleepingLayer(smi));
		}).Exit(delegate(ConduitSleepStates.Instance smi)
		{
			smi.animController.SetSceneLayer(Grid.SceneLayer.Creatures);
		}).EventTransition(GameHashes.NewDay, (ConduitSleepStates.Instance smi) => GameClock.Instance, this.connector.connectedWake, null).Transition(this.connector.sleep.noConnection, (ConduitSleepStates.Instance smi) => !smi.staterpillar.IsConnected(), UpdateRate.SIM_200ms).PlayAnim("sleep_charging_pre").QueueAnim("sleep_charging_loop", true, null).Update(new Action<ConduitSleepStates.Instance, float>(ConduitSleepStates.UpdateGulpSymbol), UpdateRate.SIM_1000ms, false).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.GameEvent.Callback(ConduitSleepStates.OnStorageChanged));
		this.connector.sleep.noConnection.PlayAnim("sleep_pre").QueueAnim("sleep_loop", true, null).ToggleStatusItem(new Func<ConduitSleepStates.Instance, StatusItem>(ConduitSleepStates.GetStatusItem), null).EventTransition(GameHashes.NewDay, (ConduitSleepStates.Instance smi) => GameClock.Instance, this.connector.noConnectionWake, null).Transition(this.connector.sleep.connected, (ConduitSleepStates.Instance smi) => smi.staterpillar.IsConnected(), UpdateRate.SIM_200ms);
		this.connector.connectedWake.QueueAnim("sleep_charging_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.connector.noConnectionWake.QueueAnim("sleep_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsConduitConnection, false);
	}

	// Token: 0x06000386 RID: 902 RVA: 0x0001D758 File Offset: 0x0001B958
	private static Grid.SceneLayer GetSleepingLayer(ConduitSleepStates.Instance smi)
	{
		ObjectLayer conduitLayer = smi.staterpillar.conduitLayer;
		Grid.SceneLayer result;
		if (conduitLayer != ObjectLayer.GasConduit)
		{
			if (conduitLayer != ObjectLayer.LiquidConduit)
			{
				if (conduitLayer == ObjectLayer.Wire)
				{
					result = Grid.SceneLayer.SolidConduitBridges;
				}
				else
				{
					result = Grid.SceneLayer.SolidConduitBridges;
				}
			}
			else
			{
				result = Grid.SceneLayer.GasConduitBridges;
			}
		}
		else
		{
			result = Grid.SceneLayer.Gas;
		}
		return result;
	}

	// Token: 0x06000387 RID: 903 RVA: 0x0001D794 File Offset: 0x0001B994
	private static StatusItem GetStatusItem(ConduitSleepStates.Instance smi)
	{
		ObjectLayer conduitLayer = smi.staterpillar.conduitLayer;
		StatusItem result;
		if (conduitLayer != ObjectLayer.GasConduit)
		{
			if (conduitLayer != ObjectLayer.LiquidConduit)
			{
				if (conduitLayer == ObjectLayer.Wire)
				{
					result = Db.Get().BuildingStatusItems.NoWireConnected;
				}
				else
				{
					result = Db.Get().BuildingStatusItems.Normal;
				}
			}
			else
			{
				result = Db.Get().BuildingStatusItems.NeedLiquidOut;
			}
		}
		else
		{
			result = Db.Get().BuildingStatusItems.NeedGasOut;
		}
		return result;
	}

	// Token: 0x06000388 RID: 904 RVA: 0x0001D804 File Offset: 0x0001BA04
	private static void OnStorageChanged(ConduitSleepStates.Instance smi, object obj)
	{
		GameObject gameObject = obj as GameObject;
		if (gameObject != null)
		{
			smi.amountDeposited += gameObject.GetComponent<PrimaryElement>().Mass;
		}
	}

	// Token: 0x06000389 RID: 905 RVA: 0x0001D839 File Offset: 0x0001BA39
	private static void UpdateGulpSymbol(ConduitSleepStates.Instance smi, float dt)
	{
		smi.SetGulpSymbolVisibility(smi.amountDeposited > 0f);
		smi.amountDeposited = 0f;
	}

	// Token: 0x0600038A RID: 906 RVA: 0x0001D85C File Offset: 0x0001BA5C
	private static void CleanUp(ConduitSleepStates.Instance smi)
	{
		ConduitSleepMonitor.Instance smi2 = smi.GetSMI<ConduitSleepMonitor.Instance>();
		if (smi2 != null)
		{
			smi2.sm.targetSleepCell.Set(Grid.InvalidCell, smi2, false);
		}
		smi.staterpillar.DestroyOrphanedConnectorBuilding();
	}

	// Token: 0x04000276 RID: 630
	public ConduitSleepStates.DrowsyStates drowsy;

	// Token: 0x04000277 RID: 631
	public ConduitSleepStates.HasConnectorStates connector;

	// Token: 0x04000278 RID: 632
	public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State behaviourcomplete;

	// Token: 0x0200100C RID: 4108
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005C0A RID: 23562
		public HashedString gulpSymbol = "gulp";
	}

	// Token: 0x0200100D RID: 4109
	public new class Instance : GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.GameInstance
	{
		// Token: 0x06007B1A RID: 31514 RVA: 0x003031C4 File Offset: 0x003013C4
		public Instance(Chore<ConduitSleepStates.Instance> chore, ConduitSleepStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsConduitConnection);
		}

		// Token: 0x06007B1B RID: 31515 RVA: 0x003031E8 File Offset: 0x003013E8
		public void SetGulpSymbolVisibility(bool state)
		{
			string sound = GlobalAssets.GetSound("PlugSlug_Charging_Gulp_LP", false);
			if (this.gulpSymbolVisible != state)
			{
				this.gulpSymbolVisible = state;
				this.animController.SetSymbolVisiblity(base.def.gulpSymbol, state);
				if (state)
				{
					this.loopingSounds.StartSound(sound);
					return;
				}
				this.loopingSounds.StopSound(sound);
			}
		}

		// Token: 0x04005C0B RID: 23563
		[MyCmpReq]
		public KBatchedAnimController animController;

		// Token: 0x04005C0C RID: 23564
		[MyCmpReq]
		public Staterpillar staterpillar;

		// Token: 0x04005C0D RID: 23565
		[MyCmpAdd]
		private LoopingSounds loopingSounds;

		// Token: 0x04005C0E RID: 23566
		public bool gulpSymbolVisible;

		// Token: 0x04005C0F RID: 23567
		public float amountDeposited;
	}

	// Token: 0x0200100E RID: 4110
	public class SleepStates : GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State
	{
		// Token: 0x04005C10 RID: 23568
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State connected;

		// Token: 0x04005C11 RID: 23569
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State noConnection;
	}

	// Token: 0x0200100F RID: 4111
	public class DrowsyStates : GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State
	{
		// Token: 0x04005C12 RID: 23570
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State loop;

		// Token: 0x04005C13 RID: 23571
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State pst;
	}

	// Token: 0x02001010 RID: 4112
	public class HasConnectorStates : GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State
	{
		// Token: 0x04005C14 RID: 23572
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State moveToSleepLocation;

		// Token: 0x04005C15 RID: 23573
		public ConduitSleepStates.SleepStates sleep;

		// Token: 0x04005C16 RID: 23574
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State noConnectionWake;

		// Token: 0x04005C17 RID: 23575
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State connectedWake;
	}
}
