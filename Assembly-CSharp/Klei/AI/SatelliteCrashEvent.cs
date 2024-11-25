using System;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F68 RID: 3944
	public class SatelliteCrashEvent : GameplayEvent<SatelliteCrashEvent.StatesInstance>
	{
		// Token: 0x06007914 RID: 30996 RVA: 0x002FE7AE File Offset: 0x002FC9AE
		public SatelliteCrashEvent() : base("SatelliteCrash", 0, 0)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.SATELLITE_CRASH.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.SATELLITE_CRASH.DESCRIPTION;
		}

		// Token: 0x06007915 RID: 30997 RVA: 0x002FE7DD File Offset: 0x002FC9DD
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new SatelliteCrashEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0200235A RID: 9050
		public class StatesInstance : GameplayEventStateMachine<SatelliteCrashEvent.States, SatelliteCrashEvent.StatesInstance, GameplayEventManager, SatelliteCrashEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600B67B RID: 46715 RVA: 0x003CBDBD File Offset: 0x003C9FBD
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, SatelliteCrashEvent crashEvent) : base(master, eventInstance, crashEvent)
			{
			}

			// Token: 0x0600B67C RID: 46716 RVA: 0x003CBDC8 File Offset: 0x003C9FC8
			public Notification Plan()
			{
				Vector3 position = new Vector3((float)(Grid.WidthInCells / 2 + UnityEngine.Random.Range(-Grid.WidthInCells / 3, Grid.WidthInCells / 3)), (float)(Grid.HeightInCells - 1), Grid.GetLayerZ(Grid.SceneLayer.FXFront));
				GameObject spawn = Util.KInstantiate(Assets.GetPrefab(SatelliteCometConfig.ID), position);
				spawn.SetActive(true);
				Notification notification = EventInfoScreen.CreateNotification(base.smi.sm.GenerateEventPopupData(base.smi), null);
				notification.clickFocus = spawn.transform;
				Comet component = spawn.GetComponent<Comet>();
				component.OnImpact = (System.Action)Delegate.Combine(component.OnImpact, new System.Action(delegate()
				{
					GameObject gameObject = new GameObject();
					gameObject.transform.position = spawn.transform.position;
					notification.clickFocus = gameObject.transform;
					GridVisibility.Reveal(Grid.PosToXY(gameObject.transform.position).x, Grid.PosToXY(gameObject.transform.position).y, 6, 4f);
				}));
				return notification;
			}
		}

		// Token: 0x0200235B RID: 9051
		public class States : GameplayEventStateMachine<SatelliteCrashEvent.States, SatelliteCrashEvent.StatesInstance, GameplayEventManager, SatelliteCrashEvent>
		{
			// Token: 0x0600B67D RID: 46717 RVA: 0x003CBEA0 File Offset: 0x003CA0A0
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.notify;
				this.notify.ToggleNotification((SatelliteCrashEvent.StatesInstance smi) => smi.Plan());
				this.ending.ReturnSuccess();
			}

			// Token: 0x0600B67E RID: 46718 RVA: 0x003CBEEC File Offset: 0x003CA0EC
			public override EventInfoData GenerateEventPopupData(SatelliteCrashEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				eventInfoData.location = GAMEPLAY_EVENTS.LOCATIONS.SURFACE;
				eventInfoData.whenDescription = GAMEPLAY_EVENTS.TIMES.NOW;
				eventInfoData.AddDefaultOption(delegate
				{
					smi.GoTo(smi.sm.ending);
				});
				return eventInfoData;
			}

			// Token: 0x04009E82 RID: 40578
			public GameStateMachine<SatelliteCrashEvent.States, SatelliteCrashEvent.StatesInstance, GameplayEventManager, object>.State notify;

			// Token: 0x04009E83 RID: 40579
			public GameStateMachine<SatelliteCrashEvent.States, SatelliteCrashEvent.StatesInstance, GameplayEventManager, object>.State ending;
		}
	}
}
