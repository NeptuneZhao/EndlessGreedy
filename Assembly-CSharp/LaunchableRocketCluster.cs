using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000AD3 RID: 2771
[SerializationConfig(MemberSerialization.OptIn)]
public class LaunchableRocketCluster : StateMachineComponent<LaunchableRocketCluster.StatesInstance>, ILaunchableRocket
{
	// Token: 0x17000624 RID: 1572
	// (get) Token: 0x06005249 RID: 21065 RVA: 0x001D81F4 File Offset: 0x001D63F4
	public IList<Ref<RocketModuleCluster>> parts
	{
		get
		{
			return base.GetComponent<RocketModuleCluster>().CraftInterface.ClusterModules;
		}
	}

	// Token: 0x17000625 RID: 1573
	// (get) Token: 0x0600524A RID: 21066 RVA: 0x001D8206 File Offset: 0x001D6406
	// (set) Token: 0x0600524B RID: 21067 RVA: 0x001D820E File Offset: 0x001D640E
	public bool isLanding { get; private set; }

	// Token: 0x17000626 RID: 1574
	// (get) Token: 0x0600524C RID: 21068 RVA: 0x001D8217 File Offset: 0x001D6417
	// (set) Token: 0x0600524D RID: 21069 RVA: 0x001D821F File Offset: 0x001D641F
	public float rocketSpeed { get; private set; }

	// Token: 0x17000627 RID: 1575
	// (get) Token: 0x0600524E RID: 21070 RVA: 0x001D8228 File Offset: 0x001D6428
	public LaunchableRocketRegisterType registerType
	{
		get
		{
			return LaunchableRocketRegisterType.Clustercraft;
		}
	}

	// Token: 0x17000628 RID: 1576
	// (get) Token: 0x0600524F RID: 21071 RVA: 0x001D822B File Offset: 0x001D642B
	public GameObject LaunchableGameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x06005250 RID: 21072 RVA: 0x001D8233 File Offset: 0x001D6433
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06005251 RID: 21073 RVA: 0x001D8248 File Offset: 0x001D6448
	public List<GameObject> GetEngines()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (Ref<RocketModuleCluster> @ref in this.parts)
		{
			if (@ref.Get().GetComponent<RocketEngineCluster>())
			{
				list.Add(@ref.Get().gameObject);
			}
		}
		return list;
	}

	// Token: 0x06005252 RID: 21074 RVA: 0x001D82B8 File Offset: 0x001D64B8
	private int GetRocketHeight()
	{
		int num = 0;
		foreach (Ref<RocketModuleCluster> @ref in this.parts)
		{
			num += @ref.Get().GetComponent<Building>().Def.HeightInCells;
		}
		return num;
	}

	// Token: 0x06005253 RID: 21075 RVA: 0x001D831C File Offset: 0x001D651C
	private float InitialFlightAnimOffsetForLanding()
	{
		int num = Grid.PosToCell(base.gameObject);
		return ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]).maximumBounds.y - base.gameObject.transform.GetPosition().y + (float)this.GetRocketHeight() + 100f;
	}

	// Token: 0x04003647 RID: 13895
	[Serialize]
	private int takeOffLocation;

	// Token: 0x0400364A RID: 13898
	private GameObject soundSpeakerObject;

	// Token: 0x02001B1D RID: 6941
	public class StatesInstance : GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.GameInstance
	{
		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x0600A262 RID: 41570 RVA: 0x003862E8 File Offset: 0x003844E8
		private float heightLaunchSpeedRatio
		{
			get
			{
				return Mathf.Pow((float)base.master.GetRocketHeight(), TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().heightSpeedPower) * TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().heightSpeedFactor;
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x0600A263 RID: 41571 RVA: 0x00386310 File Offset: 0x00384510
		// (set) Token: 0x0600A264 RID: 41572 RVA: 0x00386323 File Offset: 0x00384523
		public float DistanceAboveGround
		{
			get
			{
				return base.sm.distanceAboveGround.Get(this);
			}
			set
			{
				base.sm.distanceAboveGround.Set(value, this, false);
			}
		}

		// Token: 0x0600A265 RID: 41573 RVA: 0x00386339 File Offset: 0x00384539
		public StatesInstance(LaunchableRocketCluster master) : base(master)
		{
			this.takeoffAccelPowerInv = 1f / TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower;
		}

		// Token: 0x0600A266 RID: 41574 RVA: 0x00386358 File Offset: 0x00384558
		public void SetMissionState(Spacecraft.MissionState state)
		{
			global::Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(base.master.GetComponent<LaunchConditionManager>()).SetState(state);
		}

		// Token: 0x0600A267 RID: 41575 RVA: 0x00386382 File Offset: 0x00384582
		public Spacecraft.MissionState GetMissionState()
		{
			global::Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
			return SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(base.master.GetComponent<LaunchConditionManager>()).state;
		}

		// Token: 0x0600A268 RID: 41576 RVA: 0x003863AB File Offset: 0x003845AB
		public bool IsGrounded()
		{
			return base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.Grounded;
		}

		// Token: 0x0600A269 RID: 41577 RVA: 0x003863D0 File Offset: 0x003845D0
		public bool IsNotSpaceBound()
		{
			Clustercraft component = base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			return component.Status == Clustercraft.CraftStatus.Grounded || component.Status == Clustercraft.CraftStatus.Landing;
		}

		// Token: 0x0600A26A RID: 41578 RVA: 0x0038640C File Offset: 0x0038460C
		public bool IsNotGroundBound()
		{
			Clustercraft component = base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			return component.Status == Clustercraft.CraftStatus.Launching || component.Status == Clustercraft.CraftStatus.InFlight;
		}

		// Token: 0x0600A26B RID: 41579 RVA: 0x00386448 File Offset: 0x00384648
		public void SetupLaunch()
		{
			base.master.isLanding = false;
			base.master.rocketSpeed = 0f;
			base.sm.warmupTimeRemaining.Set(5f, this, false);
			base.sm.distanceAboveGround.Set(0f, this, false);
			if (base.master.soundSpeakerObject == null)
			{
				base.master.soundSpeakerObject = new GameObject("rocketSpeaker");
				base.master.soundSpeakerObject.transform.SetParent(base.master.gameObject.transform);
			}
			foreach (Ref<RocketModuleCluster> @ref in base.master.parts)
			{
				if (@ref != null)
				{
					base.master.takeOffLocation = Grid.PosToCell(base.master.gameObject);
					@ref.Get().Trigger(-1277991738, base.master.gameObject);
				}
			}
			CraftModuleInterface craftInterface = base.master.GetComponent<RocketModuleCluster>().CraftInterface;
			if (craftInterface != null)
			{
				craftInterface.Trigger(-1277991738, base.master.gameObject);
				WorldContainer component = craftInterface.GetComponent<WorldContainer>();
				if (component != null)
				{
					List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(component.id, false);
					MinionMigrationEventArgs minionMigrationEventArgs = new MinionMigrationEventArgs
					{
						prevWorldId = component.id,
						targetWorldId = component.id
					};
					foreach (MinionIdentity minionId in worldItems)
					{
						minionMigrationEventArgs.minionId = minionId;
						Game.Instance.Trigger(586301400, minionMigrationEventArgs);
					}
				}
			}
			Game.Instance.Trigger(-1277991738, base.gameObject);
			this.constantVelocityPhase_maxSpeed = 0f;
		}

		// Token: 0x0600A26C RID: 41580 RVA: 0x0038664C File Offset: 0x0038484C
		public void LaunchLoop(float dt)
		{
			base.master.isLanding = false;
			if (this.constantVelocityPhase_maxSpeed == 0f)
			{
				float num = Mathf.Pow((Mathf.Pow(TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance, this.takeoffAccelPowerInv) * this.heightLaunchSpeedRatio - 0.033f) / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				this.constantVelocityPhase_maxSpeed = (TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance - num) / 0.033f;
			}
			if (base.sm.warmupTimeRemaining.Get(this) > 0f)
			{
				base.sm.warmupTimeRemaining.Delta(-dt, this);
			}
			else if (this.DistanceAboveGround < TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance)
			{
				float num2 = Mathf.Pow(this.DistanceAboveGround, this.takeoffAccelPowerInv) * this.heightLaunchSpeedRatio;
				num2 += dt;
				this.DistanceAboveGround = Mathf.Pow(num2 / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				float num3 = Mathf.Pow((num2 - 0.033f) / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				base.master.rocketSpeed = (this.DistanceAboveGround - num3) / 0.033f;
			}
			else
			{
				base.master.rocketSpeed = this.constantVelocityPhase_maxSpeed;
				this.DistanceAboveGround += base.master.rocketSpeed * dt;
			}
			this.UpdateSoundSpeakerObject();
			if (this.UpdatePartsAnimPositionsAndDamage(true) == 0)
			{
				base.smi.GoTo(base.sm.not_grounded.space);
			}
		}

		// Token: 0x0600A26D RID: 41581 RVA: 0x003867D0 File Offset: 0x003849D0
		public void FinalizeLaunch()
		{
			base.master.rocketSpeed = 0f;
			this.DistanceAboveGround = base.sm.distanceToSpace.Get(base.smi);
			foreach (Ref<RocketModuleCluster> @ref in base.master.parts)
			{
				if (@ref != null && !(@ref.Get() == null))
				{
					RocketModuleCluster rocketModuleCluster = @ref.Get();
					rocketModuleCluster.GetComponent<KBatchedAnimController>().Offset = Vector3.up * this.DistanceAboveGround;
					rocketModuleCluster.GetComponent<KBatchedAnimController>().enabled = false;
					rocketModuleCluster.GetComponent<RocketModule>().MoveToSpace();
				}
			}
			base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().SetCraftStatus(Clustercraft.CraftStatus.InFlight);
		}

		// Token: 0x0600A26E RID: 41582 RVA: 0x003868B0 File Offset: 0x00384AB0
		public void SetupLanding()
		{
			float distanceAboveGround = base.master.InitialFlightAnimOffsetForLanding();
			this.DistanceAboveGround = distanceAboveGround;
			base.sm.warmupTimeRemaining.Set(2f, this, false);
			base.master.isLanding = true;
			base.master.rocketSpeed = 0f;
			this.constantVelocityPhase_maxSpeed = 0f;
		}

		// Token: 0x0600A26F RID: 41583 RVA: 0x00386910 File Offset: 0x00384B10
		public void LandingLoop(float dt)
		{
			base.master.isLanding = true;
			if (this.constantVelocityPhase_maxSpeed == 0f)
			{
				float num = Mathf.Pow((Mathf.Pow(TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance, this.takeoffAccelPowerInv) * this.heightLaunchSpeedRatio - 0.033f) / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				this.constantVelocityPhase_maxSpeed = (num - TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance) / 0.033f;
			}
			if (this.DistanceAboveGround > TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance)
			{
				base.master.rocketSpeed = this.constantVelocityPhase_maxSpeed;
				this.DistanceAboveGround += base.master.rocketSpeed * dt;
			}
			else if (this.DistanceAboveGround > 0.0025f)
			{
				float num2 = Mathf.Pow(this.DistanceAboveGround, this.takeoffAccelPowerInv) * this.heightLaunchSpeedRatio;
				num2 -= dt;
				this.DistanceAboveGround = Mathf.Pow(num2 / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				float num3 = Mathf.Pow((num2 - 0.033f) / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				base.master.rocketSpeed = (this.DistanceAboveGround - num3) / 0.033f;
			}
			else if (base.sm.warmupTimeRemaining.Get(this) > 0f)
			{
				base.sm.warmupTimeRemaining.Delta(-dt, this);
				this.DistanceAboveGround = 0f;
			}
			this.UpdateSoundSpeakerObject();
			this.UpdatePartsAnimPositionsAndDamage(true);
		}

		// Token: 0x0600A270 RID: 41584 RVA: 0x00386A90 File Offset: 0x00384C90
		public void FinalizeLanding()
		{
			base.GetComponent<KSelectable>().IsSelectable = true;
			base.master.rocketSpeed = 0f;
			this.DistanceAboveGround = 0f;
			foreach (Ref<RocketModuleCluster> @ref in base.smi.master.parts)
			{
				if (@ref != null && !(@ref.Get() == null))
				{
					@ref.Get().GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
				}
			}
			base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().SetCraftStatus(Clustercraft.CraftStatus.Grounded);
		}

		// Token: 0x0600A271 RID: 41585 RVA: 0x00386B50 File Offset: 0x00384D50
		private void UpdateSoundSpeakerObject()
		{
			if (base.master.soundSpeakerObject == null)
			{
				base.master.soundSpeakerObject = new GameObject("rocketSpeaker");
				base.master.soundSpeakerObject.transform.SetParent(base.gameObject.transform);
			}
			base.master.soundSpeakerObject.transform.SetLocalPosition(this.DistanceAboveGround * Vector3.up);
		}

		// Token: 0x0600A272 RID: 41586 RVA: 0x00386BCC File Offset: 0x00384DCC
		public int UpdatePartsAnimPositionsAndDamage(bool doDamage = true)
		{
			int myWorldId = base.gameObject.GetMyWorldId();
			if (myWorldId == -1)
			{
				return 0;
			}
			LaunchPad currentPad = base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad;
			if (currentPad != null)
			{
				myWorldId = currentPad.GetMyWorldId();
			}
			int num = 0;
			foreach (Ref<RocketModuleCluster> @ref in base.master.parts)
			{
				if (@ref != null)
				{
					RocketModuleCluster rocketModuleCluster = @ref.Get();
					KBatchedAnimController component = rocketModuleCluster.GetComponent<KBatchedAnimController>();
					component.Offset = Vector3.up * this.DistanceAboveGround;
					Vector3 positionIncludingOffset = component.PositionIncludingOffset;
					int num2 = Grid.PosToCell(component.transform.GetPosition());
					bool flag = Grid.IsValidCell(num2);
					bool flag2 = flag && (int)Grid.WorldIdx[num2] == myWorldId;
					if (component.enabled != flag2)
					{
						component.enabled = flag2;
					}
					if (doDamage && flag)
					{
						num++;
						LaunchableRocketCluster.States.DoWorldDamage(rocketModuleCluster.gameObject, positionIncludingOffset, myWorldId);
					}
				}
			}
			return num;
		}

		// Token: 0x04007ED5 RID: 32469
		private float takeoffAccelPowerInv;

		// Token: 0x04007ED6 RID: 32470
		private float constantVelocityPhase_maxSpeed;

		// Token: 0x02002613 RID: 9747
		public class Tuning : TuningData<LaunchableRocketCluster.StatesInstance.Tuning>
		{
			// Token: 0x0400A96E RID: 43374
			public float takeoffAccelPower = 4f;

			// Token: 0x0400A96F RID: 43375
			public float maxAccelerationDistance = 25f;

			// Token: 0x0400A970 RID: 43376
			public float warmupTime = 5f;

			// Token: 0x0400A971 RID: 43377
			public float heightSpeedPower = 0.5f;

			// Token: 0x0400A972 RID: 43378
			public float heightSpeedFactor = 4f;

			// Token: 0x0400A973 RID: 43379
			public int maxAccelHeight = 40;
		}
	}

	// Token: 0x02001B1E RID: 6942
	public class States : GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster>
	{
		// Token: 0x0600A273 RID: 41587 RVA: 0x00386CF4 File Offset: 0x00384EF4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grounded;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.grounded.EventTransition(GameHashes.DoLaunchRocket, this.not_grounded.launch_setup, null).EnterTransition(this.not_grounded.launch_loop, (LaunchableRocketCluster.StatesInstance smi) => smi.IsNotGroundBound()).Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.FinalizeLanding();
			});
			this.not_grounded.launch_setup.Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.SetupLaunch();
				this.distanceToSpace.Set((float)ConditionFlightPathIsClear.PadTopEdgeDistanceToOutOfScreenEdge(smi.master.gameObject.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad.gameObject), smi, false);
				smi.GoTo(this.not_grounded.launch_loop);
			});
			this.not_grounded.launch_loop.EventTransition(GameHashes.DoReturnRocket, this.not_grounded.landing_setup, null).Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.UpdatePartsAnimPositionsAndDamage(false);
			}).Update(delegate(LaunchableRocketCluster.StatesInstance smi, float dt)
			{
				smi.LaunchLoop(dt);
			}, UpdateRate.SIM_EVERY_TICK, false).ParamTransition<float>(this.distanceAboveGround, this.not_grounded.launch_pst, (LaunchableRocketCluster.StatesInstance smi, float p) => p >= this.distanceToSpace.Get(smi)).TriggerOnEnter(GameHashes.StartRocketLaunch, null).Exit(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				WorldContainer myWorld = smi.gameObject.GetMyWorld();
				if (myWorld != null)
				{
					myWorld.RevealSurface();
				}
			});
			this.not_grounded.launch_pst.ScheduleGoTo(0f, this.not_grounded.space);
			this.not_grounded.space.EnterTransition(this.not_grounded.landing_setup, (LaunchableRocketCluster.StatesInstance smi) => smi.IsNotSpaceBound()).EventTransition(GameHashes.DoReturnRocket, this.not_grounded.landing_setup, null).Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.FinalizeLaunch();
			});
			this.not_grounded.landing_setup.Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.SetupLanding();
				smi.GoTo(this.not_grounded.landing_loop);
			});
			this.not_grounded.landing_loop.Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.UpdatePartsAnimPositionsAndDamage(false);
			}).Update(delegate(LaunchableRocketCluster.StatesInstance smi, float dt)
			{
				smi.LandingLoop(dt);
			}, UpdateRate.SIM_EVERY_TICK, false).ParamTransition<float>(this.distanceAboveGround, this.not_grounded.land, new StateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.Parameter<float>.Callback(this.IsFullyLanded<float>)).ParamTransition<float>(this.warmupTimeRemaining, this.not_grounded.land, new StateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.Parameter<float>.Callback(this.IsFullyLanded<float>));
			this.not_grounded.land.TriggerOnEnter(GameHashes.RocketTouchDown, null).Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				foreach (Ref<RocketModuleCluster> @ref in smi.master.parts)
				{
					if (@ref != null && !(@ref.Get() == null))
					{
						@ref.Get().Trigger(-887025858, smi.gameObject);
					}
				}
				CraftModuleInterface craftInterface = smi.master.GetComponent<RocketModuleCluster>().CraftInterface;
				if (craftInterface != null)
				{
					craftInterface.Trigger(-887025858, smi.gameObject);
					WorldContainer component = craftInterface.GetComponent<WorldContainer>();
					if (component != null)
					{
						List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(component.id, false);
						MinionMigrationEventArgs minionMigrationEventArgs = new MinionMigrationEventArgs
						{
							prevWorldId = component.id,
							targetWorldId = component.id
						};
						foreach (MinionIdentity minionId in worldItems)
						{
							minionMigrationEventArgs.minionId = minionId;
							Game.Instance.Trigger(586301400, minionMigrationEventArgs);
						}
					}
				}
				Game.Instance.Trigger(-887025858, smi.gameObject);
				if (craftInterface != null)
				{
					PassengerRocketModule passengerModule = craftInterface.GetPassengerModule();
					if (passengerModule != null)
					{
						passengerModule.RemovePassengersOnOtherWorlds();
					}
				}
				smi.GoTo(this.grounded);
			});
		}

		// Token: 0x0600A274 RID: 41588 RVA: 0x00386FCF File Offset: 0x003851CF
		public bool IsFullyLanded<T>(LaunchableRocketCluster.StatesInstance smi, T p)
		{
			return this.distanceAboveGround.Get(smi) <= 0.0025f && this.warmupTimeRemaining.Get(smi) <= 0f;
		}

		// Token: 0x0600A275 RID: 41589 RVA: 0x00386FFC File Offset: 0x003851FC
		public static void DoWorldDamage(GameObject part, Vector3 apparentPosition, int actualWorld)
		{
			OccupyArea component = part.GetComponent<OccupyArea>();
			component.UpdateOccupiedArea();
			foreach (CellOffset offset in component.OccupiedCellsOffsets)
			{
				int num = Grid.OffsetCell(Grid.PosToCell(apparentPosition), offset);
				if (Grid.IsValidCell(num) && Grid.WorldIdx[num] == Grid.WorldIdx[actualWorld])
				{
					if (Grid.Solid[num])
					{
						WorldDamage.Instance.ApplyDamage(num, 10000f, num, BUILDINGS.DAMAGESOURCES.ROCKET, UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.ROCKET);
					}
					else if (Grid.FakeFloor[num])
					{
						GameObject gameObject = Grid.Objects[num, 39];
						if (gameObject != null && gameObject.HasTag(GameTags.GantryExtended))
						{
							BuildingHP component2 = gameObject.GetComponent<BuildingHP>();
							if (component2 != null)
							{
								gameObject.Trigger(-794517298, new BuildingHP.DamageSourceInfo
								{
									damage = component2.MaxHitPoints,
									source = BUILDINGS.DAMAGESOURCES.ROCKET,
									popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.ROCKET
								});
							}
						}
					}
				}
			}
		}

		// Token: 0x04007ED7 RID: 32471
		public StateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.FloatParameter warmupTimeRemaining;

		// Token: 0x04007ED8 RID: 32472
		public StateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.FloatParameter distanceAboveGround;

		// Token: 0x04007ED9 RID: 32473
		public StateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.FloatParameter distanceToSpace;

		// Token: 0x04007EDA RID: 32474
		public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State grounded;

		// Token: 0x04007EDB RID: 32475
		public LaunchableRocketCluster.States.NotGroundedStates not_grounded;

		// Token: 0x02002614 RID: 9748
		public class NotGroundedStates : GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State
		{
			// Token: 0x0400A974 RID: 43380
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State launch_setup;

			// Token: 0x0400A975 RID: 43381
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State launch_loop;

			// Token: 0x0400A976 RID: 43382
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State launch_pst;

			// Token: 0x0400A977 RID: 43383
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State space;

			// Token: 0x0400A978 RID: 43384
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State landing_setup;

			// Token: 0x0400A979 RID: 43385
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State landing_loop;

			// Token: 0x0400A97A RID: 43386
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State land;
		}
	}
}
