using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F64 RID: 3940
	public class MeteorShowerEvent : GameplayEvent<MeteorShowerEvent.StatesInstance>
	{
		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06007906 RID: 30982 RVA: 0x002FE526 File Offset: 0x002FC726
		public bool canStarTravel
		{
			get
			{
				return this.clusterMapMeteorShowerID != null && DlcManager.FeatureClusterSpaceEnabled();
			}
		}

		// Token: 0x06007907 RID: 30983 RVA: 0x002FE537 File Offset: 0x002FC737
		public string GetClusterMapMeteorShowerID()
		{
			return this.clusterMapMeteorShowerID;
		}

		// Token: 0x06007908 RID: 30984 RVA: 0x002FE53F File Offset: 0x002FC73F
		public List<MeteorShowerEvent.BombardmentInfo> GetMeteorsInfo()
		{
			return new List<MeteorShowerEvent.BombardmentInfo>(this.bombardmentInfo);
		}

		// Token: 0x06007909 RID: 30985 RVA: 0x002FE54C File Offset: 0x002FC74C
		public MeteorShowerEvent(string id, float duration, float secondsPerMeteor, MathUtil.MinMax secondsBombardmentOff = default(MathUtil.MinMax), MathUtil.MinMax secondsBombardmentOn = default(MathUtil.MinMax), string clusterMapMeteorShowerID = null, bool affectedByDifficulty = true) : base(id, 0, 0)
		{
			this.allowMultipleEventInstances = true;
			this.clusterMapMeteorShowerID = clusterMapMeteorShowerID;
			this.duration = duration;
			this.secondsPerMeteor = secondsPerMeteor;
			this.secondsBombardmentOff = secondsBombardmentOff;
			this.secondsBombardmentOn = secondsBombardmentOn;
			this.affectedByDifficulty = affectedByDifficulty;
			this.bombardmentInfo = new List<MeteorShowerEvent.BombardmentInfo>();
			this.tags.Add(GameTags.SpaceDanger);
		}

		// Token: 0x0600790A RID: 30986 RVA: 0x002FE5C4 File Offset: 0x002FC7C4
		public MeteorShowerEvent AddMeteor(string prefab, float weight)
		{
			this.bombardmentInfo.Add(new MeteorShowerEvent.BombardmentInfo
			{
				prefab = prefab,
				weight = weight
			});
			return this;
		}

		// Token: 0x0600790B RID: 30987 RVA: 0x002FE5F6 File Offset: 0x002FC7F6
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new MeteorShowerEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0600790C RID: 30988 RVA: 0x002FE600 File Offset: 0x002FC800
		public override bool IsAllowed()
		{
			return base.IsAllowed() && (!this.affectedByDifficulty || CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers).id != "ClearSkies");
		}

		// Token: 0x04005A72 RID: 23154
		private List<MeteorShowerEvent.BombardmentInfo> bombardmentInfo;

		// Token: 0x04005A73 RID: 23155
		private MathUtil.MinMax secondsBombardmentOff;

		// Token: 0x04005A74 RID: 23156
		private MathUtil.MinMax secondsBombardmentOn;

		// Token: 0x04005A75 RID: 23157
		private float secondsPerMeteor = 0.33f;

		// Token: 0x04005A76 RID: 23158
		private float duration;

		// Token: 0x04005A77 RID: 23159
		private string clusterMapMeteorShowerID;

		// Token: 0x04005A78 RID: 23160
		private bool affectedByDifficulty = true;

		// Token: 0x02002353 RID: 9043
		public struct BombardmentInfo
		{
			// Token: 0x04009E61 RID: 40545
			public string prefab;

			// Token: 0x04009E62 RID: 40546
			public float weight;
		}

		// Token: 0x02002354 RID: 9044
		public class States : GameplayEventStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, MeteorShowerEvent>
		{
			// Token: 0x0600B644 RID: 46660 RVA: 0x003CA4C0 File Offset: 0x003C86C0
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				base.InitializeStates(out default_state);
				default_state = this.planning;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.planning.Enter(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					this.runTimeRemaining.Set(smi.gameplayEvent.duration, smi, false);
					this.bombardTimeRemaining.Set(smi.GetBombardOnTime(), smi, false);
					this.snoozeTimeRemaining.Set(smi.GetBombardOffTime(), smi, false);
					if (smi.gameplayEvent.canStarTravel && smi.clusterTravelDuration > 0f)
					{
						smi.GoTo(smi.sm.starMap);
						return;
					}
					smi.GoTo(smi.sm.running);
				});
				this.starMap.Enter(new StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State.Callback(MeteorShowerEvent.States.CreateClusterMapMeteorShower)).DefaultState(this.starMap.travelling);
				this.starMap.travelling.OnSignal(this.OnClusterMapDestinationReached, this.starMap.arrive);
				this.starMap.arrive.GoTo(this.running.bombarding);
				this.running.DefaultState(this.running.snoozing).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					this.runTimeRemaining.Delta(-dt, smi);
				}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.runTimeRemaining, this.finished, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero);
				this.running.bombarding.Enter(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					MeteorShowerEvent.States.TriggerMeteorGlobalEvent(smi, GameHashes.MeteorShowerBombardStateBegins);
				}).Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					MeteorShowerEvent.States.TriggerMeteorGlobalEvent(smi, GameHashes.MeteorShowerBombardStateEnds);
				}).Enter(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					smi.StartBackgroundEffects();
				}).Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					smi.StopBackgroundEffects();
				}).Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					this.bombardTimeRemaining.Set(smi.GetBombardOnTime(), smi, false);
				}).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					this.bombardTimeRemaining.Delta(-dt, smi);
				}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.bombardTimeRemaining, this.running.snoozing, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					smi.Bombarding(dt);
				}, UpdateRate.SIM_200ms, false);
				this.running.snoozing.Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					this.snoozeTimeRemaining.Set(smi.GetBombardOffTime(), smi, false);
				}).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					this.snoozeTimeRemaining.Delta(-dt, smi);
				}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.snoozeTimeRemaining, this.running.bombarding, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero);
				this.finished.ReturnSuccess();
			}

			// Token: 0x0600B645 RID: 46661 RVA: 0x003CA6F9 File Offset: 0x003C88F9
			public static void TriggerMeteorGlobalEvent(MeteorShowerEvent.StatesInstance smi, GameHashes hash)
			{
				Game.Instance.Trigger((int)hash, smi.eventInstance.worldId);
			}

			// Token: 0x0600B646 RID: 46662 RVA: 0x003CA718 File Offset: 0x003C8918
			public static void CreateClusterMapMeteorShower(MeteorShowerEvent.StatesInstance smi)
			{
				if (smi.sm.clusterMapMeteorShower.Get(smi) == null)
				{
					GameObject prefab = Assets.GetPrefab(smi.gameplayEvent.clusterMapMeteorShowerID.ToTag());
					float arrivalTime = smi.eventInstance.eventStartTime * 600f + smi.clusterTravelDuration;
					AxialI randomCellAtEdgeOfUniverse = ClusterGrid.Instance.GetRandomCellAtEdgeOfUniverse();
					GameObject gameObject = Util.KInstantiate(prefab, null, null);
					gameObject.GetComponent<ClusterMapMeteorShowerVisualizer>().SetInitialLocation(randomCellAtEdgeOfUniverse);
					ClusterMapMeteorShower.Def def = gameObject.AddOrGetDef<ClusterMapMeteorShower.Def>();
					def.destinationWorldID = smi.eventInstance.worldId;
					def.arrivalTime = arrivalTime;
					gameObject.SetActive(true);
					smi.sm.clusterMapMeteorShower.Set(gameObject, smi, false);
				}
				GameObject go = smi.sm.clusterMapMeteorShower.Get(smi);
				go.GetDef<ClusterMapMeteorShower.Def>();
				go.Subscribe(1796608350, new Action<object>(smi.OnClusterMapDestinationReached));
			}

			// Token: 0x04009E63 RID: 40547
			public MeteorShowerEvent.States.ClusterMapStates starMap;

			// Token: 0x04009E64 RID: 40548
			public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State planning;

			// Token: 0x04009E65 RID: 40549
			public MeteorShowerEvent.States.RunningStates running;

			// Token: 0x04009E66 RID: 40550
			public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State finished;

			// Token: 0x04009E67 RID: 40551
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.TargetParameter clusterMapMeteorShower;

			// Token: 0x04009E68 RID: 40552
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter runTimeRemaining;

			// Token: 0x04009E69 RID: 40553
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter bombardTimeRemaining;

			// Token: 0x04009E6A RID: 40554
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter snoozeTimeRemaining;

			// Token: 0x04009E6B RID: 40555
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.Signal OnClusterMapDestinationReached;

			// Token: 0x0200351F RID: 13599
			public class ClusterMapStates : GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400D779 RID: 55161
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State travelling;

				// Token: 0x0400D77A RID: 55162
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State arrive;
			}

			// Token: 0x02003520 RID: 13600
			public class RunningStates : GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400D77B RID: 55163
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State bombarding;

				// Token: 0x0400D77C RID: 55164
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State snoozing;
			}
		}

		// Token: 0x02002355 RID: 9045
		public class StatesInstance : GameplayEventStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, MeteorShowerEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600B64E RID: 46670 RVA: 0x003CA8EA File Offset: 0x003C8AEA
			public float GetSleepTimerValue()
			{
				return Mathf.Clamp(GameplayEventManager.Instance.GetSleepTimer(this.gameplayEvent) - GameUtil.GetCurrentTimeInCycles(), 0f, float.MaxValue);
			}

			// Token: 0x0600B64F RID: 46671 RVA: 0x003CA914 File Offset: 0x003C8B14
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, MeteorShowerEvent meteorShowerEvent) : base(master, eventInstance, meteorShowerEvent)
			{
				this.world = ClusterManager.Instance.GetWorld(this.m_worldId);
				this.difficultyLevel = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
				this.m_worldId = eventInstance.worldId;
				Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
			}

			// Token: 0x0600B650 RID: 46672 RVA: 0x003CA988 File Offset: 0x003C8B88
			public void OnClusterMapDestinationReached(object obj)
			{
				base.smi.sm.OnClusterMapDestinationReached.Trigger(this);
			}

			// Token: 0x0600B651 RID: 46673 RVA: 0x003CA9A0 File Offset: 0x003C8BA0
			private void OnActiveWorldChanged(object data)
			{
				int first = ((global::Tuple<int, int>)data).first;
				if (this.activeMeteorBackground != null)
				{
					this.activeMeteorBackground.GetComponent<ParticleSystemRenderer>().enabled = (first == this.m_worldId);
				}
			}

			// Token: 0x0600B652 RID: 46674 RVA: 0x003CA9E0 File Offset: 0x003C8BE0
			public override void StopSM(string reason)
			{
				this.StopBackgroundEffects();
				base.StopSM(reason);
			}

			// Token: 0x0600B653 RID: 46675 RVA: 0x003CA9EF File Offset: 0x003C8BEF
			protected override void OnCleanUp()
			{
				Game.Instance.Unsubscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
				this.DestroyClusterMapMeteorShowerObject();
				base.OnCleanUp();
			}

			// Token: 0x0600B654 RID: 46676 RVA: 0x003CAA18 File Offset: 0x003C8C18
			private void DestroyClusterMapMeteorShowerObject()
			{
				if (base.sm.clusterMapMeteorShower.Get(this) != null)
				{
					ClusterMapMeteorShower.Instance smi = base.sm.clusterMapMeteorShower.Get(this).GetSMI<ClusterMapMeteorShower.Instance>();
					if (smi != null)
					{
						smi.StopSM("Event is being aborted");
						Util.KDestroyGameObject(smi.gameObject);
					}
				}
			}

			// Token: 0x0600B655 RID: 46677 RVA: 0x003CAA70 File Offset: 0x003C8C70
			public void StartBackgroundEffects()
			{
				if (this.activeMeteorBackground == null)
				{
					this.activeMeteorBackground = Util.KInstantiate(EffectPrefabs.Instance.MeteorBackground, null, null);
					float x = (this.world.maximumBounds.x + this.world.minimumBounds.x) / 2f;
					float y = this.world.maximumBounds.y;
					float z = 25f;
					this.activeMeteorBackground.transform.SetPosition(new Vector3(x, y, z));
					this.activeMeteorBackground.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
				}
			}

			// Token: 0x0600B656 RID: 46678 RVA: 0x003CAB24 File Offset: 0x003C8D24
			public void StopBackgroundEffects()
			{
				if (this.activeMeteorBackground != null)
				{
					ParticleSystem component = this.activeMeteorBackground.GetComponent<ParticleSystem>();
					component.main.stopAction = ParticleSystemStopAction.Destroy;
					component.Stop();
					if (!component.IsAlive())
					{
						UnityEngine.Object.Destroy(this.activeMeteorBackground);
					}
					this.activeMeteorBackground = null;
				}
			}

			// Token: 0x0600B657 RID: 46679 RVA: 0x003CAB78 File Offset: 0x003C8D78
			public float TimeUntilNextShower()
			{
				if (base.IsInsideState(base.sm.running.bombarding))
				{
					return 0f;
				}
				if (!base.IsInsideState(base.sm.starMap))
				{
					return base.sm.snoozeTimeRemaining.Get(this);
				}
				float num = base.smi.eventInstance.eventStartTime * 600f + base.smi.clusterTravelDuration - GameUtil.GetCurrentTimeInCycles() * 600f;
				if (num >= 0f)
				{
					return num;
				}
				return 0f;
			}

			// Token: 0x0600B658 RID: 46680 RVA: 0x003CAC08 File Offset: 0x003C8E08
			public void Bombarding(float dt)
			{
				this.nextMeteorTime -= dt;
				while (this.nextMeteorTime < 0f)
				{
					if (this.GetSleepTimerValue() <= 0f)
					{
						this.DoBombardment(this.gameplayEvent.bombardmentInfo);
					}
					this.nextMeteorTime += this.GetNextMeteorTime();
				}
			}

			// Token: 0x0600B659 RID: 46681 RVA: 0x003CAC68 File Offset: 0x003C8E68
			private void DoBombardment(List<MeteorShowerEvent.BombardmentInfo> bombardment_info)
			{
				float num = 0f;
				foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo in bombardment_info)
				{
					num += bombardmentInfo.weight;
				}
				num = UnityEngine.Random.Range(0f, num);
				MeteorShowerEvent.BombardmentInfo bombardmentInfo2 = bombardment_info[0];
				int num2 = 0;
				while (num - bombardmentInfo2.weight > 0f)
				{
					num -= bombardmentInfo2.weight;
					bombardmentInfo2 = bombardment_info[++num2];
				}
				Game.Instance.Trigger(-84771526, null);
				this.SpawnBombard(bombardmentInfo2.prefab);
			}

			// Token: 0x0600B65A RID: 46682 RVA: 0x003CAD1C File Offset: 0x003C8F1C
			private GameObject SpawnBombard(string prefab)
			{
				WorldContainer worldContainer = ClusterManager.Instance.GetWorld(this.m_worldId);
				float x = (float)(worldContainer.Width - 1) * UnityEngine.Random.value + (float)worldContainer.WorldOffset.x;
				float y = (float)(worldContainer.Height + worldContainer.WorldOffset.y - 1);
				float layerZ = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
				Vector3 position = new Vector3(x, y, layerZ);
				GameObject prefab2 = Assets.GetPrefab(prefab);
				if (prefab2 == null)
				{
					return null;
				}
				GameObject gameObject = Util.KInstantiate(prefab2, position, Quaternion.identity, null, null, true, 0);
				Comet component = gameObject.GetComponent<Comet>();
				if (component != null)
				{
					component.spawnWithOffset = true;
				}
				gameObject.SetActive(true);
				return gameObject;
			}

			// Token: 0x0600B65B RID: 46683 RVA: 0x003CADCB File Offset: 0x003C8FCB
			public float BombardTimeRemaining()
			{
				return Mathf.Min(base.sm.bombardTimeRemaining.Get(this), base.sm.runTimeRemaining.Get(this));
			}

			// Token: 0x0600B65C RID: 46684 RVA: 0x003CADF4 File Offset: 0x003C8FF4
			public float GetBombardOffTime()
			{
				float num = this.gameplayEvent.secondsBombardmentOff.Get();
				if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
				{
					string id = this.difficultyLevel.id;
					if (!(id == "Infrequent"))
					{
						if (!(id == "Intense"))
						{
							if (id == "Doomed")
							{
								num *= 0.5f;
							}
						}
						else
						{
							num *= 1f;
						}
					}
					else
					{
						num *= 1f;
					}
				}
				return num;
			}

			// Token: 0x0600B65D RID: 46685 RVA: 0x003CAE7C File Offset: 0x003C907C
			public float GetBombardOnTime()
			{
				float num = this.gameplayEvent.secondsBombardmentOn.Get();
				if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
				{
					string id = this.difficultyLevel.id;
					if (!(id == "Infrequent"))
					{
						if (!(id == "Intense"))
						{
							if (id == "Doomed")
							{
								num *= 1f;
							}
						}
						else
						{
							num *= 1f;
						}
					}
					else
					{
						num *= 1f;
					}
				}
				return num;
			}

			// Token: 0x0600B65E RID: 46686 RVA: 0x003CAF04 File Offset: 0x003C9104
			private float GetNextMeteorTime()
			{
				float num = this.gameplayEvent.secondsPerMeteor;
				num *= 256f / (float)this.world.Width;
				if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
				{
					string id = this.difficultyLevel.id;
					if (!(id == "Infrequent"))
					{
						if (!(id == "Intense"))
						{
							if (id == "Doomed")
							{
								num *= 0.5f;
							}
						}
						else
						{
							num *= 0.8f;
						}
					}
					else
					{
						num *= 1.5f;
					}
				}
				return num;
			}

			// Token: 0x04009E6C RID: 40556
			public GameObject activeMeteorBackground;

			// Token: 0x04009E6D RID: 40557
			[Serialize]
			public float clusterTravelDuration = -1f;

			// Token: 0x04009E6E RID: 40558
			[Serialize]
			private float nextMeteorTime;

			// Token: 0x04009E6F RID: 40559
			[Serialize]
			private int m_worldId;

			// Token: 0x04009E70 RID: 40560
			private WorldContainer world;

			// Token: 0x04009E71 RID: 40561
			private SettingLevel difficultyLevel;
		}
	}
}
