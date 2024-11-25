using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F5F RID: 3935
	public class CreatureSpawnEvent : GameplayEvent<CreatureSpawnEvent.StatesInstance>
	{
		// Token: 0x060078F4 RID: 30964 RVA: 0x002FDFFC File Offset: 0x002FC1FC
		public CreatureSpawnEvent() : base("HatchSpawnEvent", 0, 0)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.CREATURE_SPAWN.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.CREATURE_SPAWN.DESCRIPTION;
		}

		// Token: 0x060078F5 RID: 30965 RVA: 0x002FE02B File Offset: 0x002FC22B
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new CreatureSpawnEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x04005A52 RID: 23122
		public const string ID = "HatchSpawnEvent";

		// Token: 0x04005A53 RID: 23123
		public const float UPDATE_TIME = 4f;

		// Token: 0x04005A54 RID: 23124
		public const float NUM_TO_SPAWN = 10f;

		// Token: 0x04005A55 RID: 23125
		public const float duration = 40f;

		// Token: 0x04005A56 RID: 23126
		public static List<string> CreatureSpawnEventIDs = new List<string>
		{
			"Hatch",
			"Squirrel",
			"Puft",
			"Crab",
			"Drecko",
			"Mole",
			"LightBug",
			"Pacu"
		};

		// Token: 0x0200234B RID: 9035
		public class StatesInstance : GameplayEventStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, CreatureSpawnEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600B627 RID: 46631 RVA: 0x003C9C48 File Offset: 0x003C7E48
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, CreatureSpawnEvent creatureEvent) : base(master, eventInstance, creatureEvent)
			{
			}

			// Token: 0x0600B628 RID: 46632 RVA: 0x003C9C5E File Offset: 0x003C7E5E
			private void PickCreatureToSpawn()
			{
				this.creatureID = CreatureSpawnEvent.CreatureSpawnEventIDs.GetRandom<string>();
			}

			// Token: 0x0600B629 RID: 46633 RVA: 0x003C9C70 File Offset: 0x003C7E70
			private void PickSpawnLocations()
			{
				Vector3 position = Components.Telepads.Items.GetRandom<Telepad>().transform.GetPosition();
				int num = 100;
				ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
				GameScenePartitioner.Instance.GatherEntries((int)position.x - num / 2, (int)position.y - num / 2, num, num, GameScenePartitioner.Instance.plants, pooledList);
				foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
				{
					KPrefabID kprefabID = (KPrefabID)scenePartitionerEntry.obj;
					if (!kprefabID.GetComponent<TreeBud>())
					{
						base.smi.spawnPositions.Add(kprefabID.transform.GetPosition());
					}
				}
				pooledList.Recycle();
			}

			// Token: 0x0600B62A RID: 46634 RVA: 0x003C9D44 File Offset: 0x003C7F44
			public void InitializeEvent()
			{
				this.PickCreatureToSpawn();
				this.PickSpawnLocations();
			}

			// Token: 0x0600B62B RID: 46635 RVA: 0x003C9D52 File Offset: 0x003C7F52
			public void EndEvent()
			{
				this.creatureID = null;
				this.spawnPositions.Clear();
			}

			// Token: 0x0600B62C RID: 46636 RVA: 0x003C9D68 File Offset: 0x003C7F68
			public void SpawnCreature()
			{
				if (this.spawnPositions.Count > 0)
				{
					Vector3 random = this.spawnPositions.GetRandom<Vector3>();
					Util.KInstantiate(Assets.GetPrefab(this.creatureID), random).SetActive(true);
				}
			}

			// Token: 0x04009E4D RID: 40525
			[Serialize]
			private List<Vector3> spawnPositions = new List<Vector3>();

			// Token: 0x04009E4E RID: 40526
			[Serialize]
			private string creatureID;
		}

		// Token: 0x0200234C RID: 9036
		public class States : GameplayEventStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, CreatureSpawnEvent>
		{
			// Token: 0x0600B62D RID: 46637 RVA: 0x003C9DAC File Offset: 0x003C7FAC
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.initialize_event;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.initialize_event.Enter(delegate(CreatureSpawnEvent.StatesInstance smi)
				{
					smi.InitializeEvent();
					smi.GoTo(this.spawn_season);
				});
				this.start.DoNothing();
				this.spawn_season.Update(delegate(CreatureSpawnEvent.StatesInstance smi, float dt)
				{
					smi.SpawnCreature();
				}, UpdateRate.SIM_4000ms, false).Exit(delegate(CreatureSpawnEvent.StatesInstance smi)
				{
					smi.EndEvent();
				});
			}

			// Token: 0x0600B62E RID: 46638 RVA: 0x003C9E40 File Offset: 0x003C8040
			public override EventInfoData GenerateEventPopupData(CreatureSpawnEvent.StatesInstance smi)
			{
				return new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName)
				{
					location = GAMEPLAY_EVENTS.LOCATIONS.PRINTING_POD,
					whenDescription = GAMEPLAY_EVENTS.TIMES.NOW
				};
			}

			// Token: 0x04009E4F RID: 40527
			public GameStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State initialize_event;

			// Token: 0x04009E50 RID: 40528
			public GameStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State spawn_season;

			// Token: 0x04009E51 RID: 40529
			public GameStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State start;
		}
	}
}
