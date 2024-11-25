using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F67 RID: 3943
	public class PlantBlightEvent : GameplayEvent<PlantBlightEvent.StatesInstance>
	{
		// Token: 0x06007912 RID: 30994 RVA: 0x002FE758 File Offset: 0x002FC958
		public PlantBlightEvent(string id, string targetPlantPrefab, float infectionDuration, float incubationDuration) : base(id, 0, 0)
		{
			this.targetPlantPrefab = targetPlantPrefab;
			this.infectionDuration = infectionDuration;
			this.incubationDuration = incubationDuration;
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.PLANT_BLIGHT.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.PLANT_BLIGHT.DESCRIPTION;
		}

		// Token: 0x06007913 RID: 30995 RVA: 0x002FE7A4 File Offset: 0x002FC9A4
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new PlantBlightEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x04005A7E RID: 23166
		private const float BLIGHT_DISTANCE = 6f;

		// Token: 0x04005A7F RID: 23167
		public string targetPlantPrefab;

		// Token: 0x04005A80 RID: 23168
		public float infectionDuration;

		// Token: 0x04005A81 RID: 23169
		public float incubationDuration;

		// Token: 0x02002358 RID: 9048
		public class States : GameplayEventStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, PlantBlightEvent>
		{
			// Token: 0x0600B670 RID: 46704 RVA: 0x003CB854 File Offset: 0x003C9A54
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				base.InitializeStates(out default_state);
				default_state = this.planning;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.planning.Enter(delegate(PlantBlightEvent.StatesInstance smi)
				{
					smi.InfectAPlant(true);
				}).GoTo(this.running);
				this.running.ToggleNotification((PlantBlightEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null)).EventHandlerTransition(GameHashes.Uprooted, this.finished, new Func<PlantBlightEvent.StatesInstance, object, bool>(this.NoBlightedPlants)).DefaultState(this.running.waiting).OnSignal(this.doFinish, this.finished);
				this.running.waiting.ParamTransition<float>(this.nextInfection, this.running.infect, (PlantBlightEvent.StatesInstance smi, float p) => p <= 0f).Update(delegate(PlantBlightEvent.StatesInstance smi, float dt)
				{
					this.nextInfection.Delta(-dt, smi);
				}, UpdateRate.SIM_4000ms, false);
				this.running.infect.Enter(delegate(PlantBlightEvent.StatesInstance smi)
				{
					smi.InfectAPlant(false);
				}).GoTo(this.running.waiting);
				this.finished.DoNotification((PlantBlightEvent.StatesInstance smi) => this.CreateSuccessNotification(smi, this.GenerateEventPopupData(smi))).ReturnSuccess();
			}

			// Token: 0x0600B671 RID: 46705 RVA: 0x003CB9B4 File Offset: 0x003C9BB4
			public override EventInfoData GenerateEventPopupData(PlantBlightEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				string value = smi.gameplayEvent.targetPlantPrefab.ToTag().ProperName();
				eventInfoData.location = GAMEPLAY_EVENTS.LOCATIONS.COLONY_WIDE;
				eventInfoData.whenDescription = GAMEPLAY_EVENTS.TIMES.NOW;
				eventInfoData.SetTextParameter("plant", value);
				return eventInfoData;
			}

			// Token: 0x0600B672 RID: 46706 RVA: 0x003CBA2C File Offset: 0x003C9C2C
			private Notification CreateSuccessNotification(PlantBlightEvent.StatesInstance smi, EventInfoData eventInfoData)
			{
				string plantName = smi.gameplayEvent.targetPlantPrefab.ToTag().ProperName();
				return new Notification(GAMEPLAY_EVENTS.EVENT_TYPES.PLANT_BLIGHT.SUCCESS.Replace("{plant}", plantName), NotificationType.Neutral, (List<Notification> list, object data) => GAMEPLAY_EVENTS.EVENT_TYPES.PLANT_BLIGHT.SUCCESS_TOOLTIP.Replace("{plant}", plantName), null, true, 0f, null, null, null, true, false, false);
			}

			// Token: 0x0600B673 RID: 46707 RVA: 0x003CBA90 File Offset: 0x003C9C90
			private bool NoBlightedPlants(PlantBlightEvent.StatesInstance smi, object obj)
			{
				GameObject gameObject = (GameObject)obj;
				if (!gameObject.HasTag(GameTags.Blighted))
				{
					return true;
				}
				foreach (Crop crop in Components.Crops.Items.FindAll((Crop p) => p.name == smi.gameplayEvent.targetPlantPrefab))
				{
					if (!(gameObject.gameObject == crop.gameObject) && crop.HasTag(GameTags.Blighted))
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x04009E7C RID: 40572
			public GameStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State planning;

			// Token: 0x04009E7D RID: 40573
			public PlantBlightEvent.States.RunningStates running;

			// Token: 0x04009E7E RID: 40574
			public GameStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State finished;

			// Token: 0x04009E7F RID: 40575
			public StateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.Signal doFinish;

			// Token: 0x04009E80 RID: 40576
			public StateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.FloatParameter nextInfection;

			// Token: 0x02003528 RID: 13608
			public class RunningStates : GameStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400D791 RID: 55185
				public GameStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State waiting;

				// Token: 0x0400D792 RID: 55186
				public GameStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State infect;
			}
		}

		// Token: 0x02002359 RID: 9049
		public class StatesInstance : GameplayEventStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, PlantBlightEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600B678 RID: 46712 RVA: 0x003CBB78 File Offset: 0x003C9D78
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, PlantBlightEvent blightEvent) : base(master, eventInstance, blightEvent)
			{
				this.startTime = Time.time;
			}

			// Token: 0x0600B679 RID: 46713 RVA: 0x003CBB90 File Offset: 0x003C9D90
			public void InfectAPlant(bool initialInfection)
			{
				if (Time.time - this.startTime > base.smi.gameplayEvent.infectionDuration)
				{
					base.sm.doFinish.Trigger(base.smi);
					return;
				}
				List<Crop> list = Components.Crops.Items.FindAll((Crop p) => p.name == base.smi.gameplayEvent.targetPlantPrefab);
				if (list.Count == 0)
				{
					base.sm.doFinish.Trigger(base.smi);
					return;
				}
				if (list.Count > 0)
				{
					List<Crop> list2 = new List<Crop>();
					List<Crop> list3 = new List<Crop>();
					foreach (Crop crop in list)
					{
						if (crop.HasTag(GameTags.Blighted))
						{
							list2.Add(crop);
						}
						else
						{
							list3.Add(crop);
						}
					}
					if (list2.Count == 0)
					{
						if (initialInfection)
						{
							Crop crop2 = list3[UnityEngine.Random.Range(0, list3.Count)];
							global::Debug.Log("Blighting a random plant", crop2);
							crop2.GetComponent<BlightVulnerable>().MakeBlighted();
						}
						else
						{
							base.sm.doFinish.Trigger(base.smi);
						}
					}
					else if (list3.Count > 0)
					{
						Crop crop3 = list2[UnityEngine.Random.Range(0, list2.Count)];
						global::Debug.Log("Spreading blight from a plant", crop3);
						list3.Shuffle<Crop>();
						foreach (Crop crop4 in list3)
						{
							if ((crop4.transform.GetPosition() - crop3.transform.GetPosition()).magnitude < 6f)
							{
								crop4.GetComponent<BlightVulnerable>().MakeBlighted();
								break;
							}
						}
					}
				}
				base.sm.nextInfection.Set(base.smi.gameplayEvent.incubationDuration, this, false);
			}

			// Token: 0x04009E81 RID: 40577
			[Serialize]
			private float startTime;
		}
	}
}
