using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000AE0 RID: 2784
[SerializationConfig(MemberSerialization.OptIn)]
public class RocketEngineCluster : StateMachineComponent<RocketEngineCluster.StatesInstance>
{
	// Token: 0x060052B0 RID: 21168 RVA: 0x001DA3DC File Offset: 0x001D85DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.mainEngine)
		{
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new RequireAttachedComponent(base.gameObject.GetComponent<AttachableBuilding>(), typeof(IFuelTank), UI.STARMAP.COMPONENT.FUEL_TANK));
			if (this.requireOxidizer)
			{
				base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new RequireAttachedComponent(base.gameObject.GetComponent<AttachableBuilding>(), typeof(OxidizerTank), UI.STARMAP.COMPONENT.OXIDIZER_TANK));
			}
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionRocketHeight(this));
		}
	}

	// Token: 0x060052B1 RID: 21169 RVA: 0x001DA480 File Offset: 0x001D8680
	private void ConfigureFlameLight()
	{
		this.flameLight = base.gameObject.AddOrGet<Light2D>();
		this.flameLight.Color = Color.white;
		this.flameLight.overlayColour = LIGHT2D.LIGHTBUG_OVERLAYCOLOR;
		this.flameLight.Range = 10f;
		this.flameLight.Angle = 0f;
		this.flameLight.Direction = LIGHT2D.LIGHTBUG_DIRECTION;
		this.flameLight.Offset = LIGHT2D.LIGHTBUG_OFFSET;
		this.flameLight.shape = global::LightShape.Circle;
		this.flameLight.drawOverlay = true;
		this.flameLight.Lux = 80000;
		this.flameLight.emitter.RemoveFromGrid();
		base.gameObject.AddOrGet<LightSymbolTracker>().targetSymbol = base.GetComponent<KBatchedAnimController>().CurrentAnim.rootSymbol;
		this.flameLight.enabled = false;
	}

	// Token: 0x060052B2 RID: 21170 RVA: 0x001DA564 File Offset: 0x001D8764
	private void UpdateFlameLight(int cell)
	{
		base.smi.master.flameLight.RefreshShapeAndPosition();
		if (Grid.IsValidCell(cell))
		{
			if (!base.smi.master.flameLight.enabled && base.smi.timeinstate > 3f)
			{
				base.smi.master.flameLight.enabled = true;
				return;
			}
		}
		else
		{
			base.smi.master.flameLight.enabled = false;
		}
	}

	// Token: 0x060052B3 RID: 21171 RVA: 0x001DA5E5 File Offset: 0x001D87E5
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x04003695 RID: 13973
	public float exhaustEmitRate = 50f;

	// Token: 0x04003696 RID: 13974
	public float exhaustTemperature = 1500f;

	// Token: 0x04003697 RID: 13975
	public SpawnFXHashes explosionEffectHash;

	// Token: 0x04003698 RID: 13976
	public SimHashes exhaustElement = SimHashes.CarbonDioxide;

	// Token: 0x04003699 RID: 13977
	public Tag fuelTag;

	// Token: 0x0400369A RID: 13978
	public float efficiency = 1f;

	// Token: 0x0400369B RID: 13979
	public bool requireOxidizer = true;

	// Token: 0x0400369C RID: 13980
	public int maxModules = 32;

	// Token: 0x0400369D RID: 13981
	public int maxHeight;

	// Token: 0x0400369E RID: 13982
	public bool mainEngine = true;

	// Token: 0x0400369F RID: 13983
	public byte exhaustDiseaseIdx = byte.MaxValue;

	// Token: 0x040036A0 RID: 13984
	public int exhaustDiseaseCount;

	// Token: 0x040036A1 RID: 13985
	public bool emitRadiation;

	// Token: 0x040036A2 RID: 13986
	[MyCmpGet]
	private RadiationEmitter radiationEmitter;

	// Token: 0x040036A3 RID: 13987
	[MyCmpGet]
	private Generator powerGenerator;

	// Token: 0x040036A4 RID: 13988
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x040036A5 RID: 13989
	public Light2D flameLight;

	// Token: 0x02001B30 RID: 6960
	public class StatesInstance : GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.GameInstance
	{
		// Token: 0x0600A2C3 RID: 41667 RVA: 0x0038820B File Offset: 0x0038640B
		public StatesInstance(RocketEngineCluster smi) : base(smi)
		{
			if (smi.emitRadiation)
			{
				DebugUtil.Assert(smi.radiationEmitter != null, "emitRadiation enabled but no RadiationEmitter component");
				this.radiationEmissionBaseOffset = smi.radiationEmitter.emissionOffset;
			}
		}

		// Token: 0x0600A2C4 RID: 41668 RVA: 0x00388244 File Offset: 0x00386444
		public void BeginBurn()
		{
			if (base.smi.master.emitRadiation)
			{
				base.smi.master.radiationEmitter.SetEmitting(true);
			}
			LaunchPad currentPad = base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad;
			if (currentPad != null)
			{
				this.pad_cell = Grid.PosToCell(currentPad.gameObject.transform.GetPosition());
				if (base.smi.master.exhaustDiseaseIdx != 255)
				{
					currentPad.GetComponent<PrimaryElement>().AddDisease(base.smi.master.exhaustDiseaseIdx, base.smi.master.exhaustDiseaseCount, "rocket exhaust");
					return;
				}
			}
			else
			{
				global::Debug.LogWarning("RocketEngineCluster missing LaunchPad for burn.");
				this.pad_cell = Grid.InvalidCell;
			}
		}

		// Token: 0x0600A2C5 RID: 41669 RVA: 0x00388318 File Offset: 0x00386518
		public void DoBurn(float dt)
		{
			int num = Grid.PosToCell(base.smi.master.gameObject.transform.GetPosition() + base.smi.master.animController.Offset);
			if (Grid.AreCellsInSameWorld(num, this.pad_cell))
			{
				SimMessages.EmitMass(num, ElementLoader.GetElementIndex(base.smi.master.exhaustElement), dt * base.smi.master.exhaustEmitRate, base.smi.master.exhaustTemperature, base.smi.master.exhaustDiseaseIdx, base.smi.master.exhaustDiseaseCount, -1);
			}
			if (base.smi.master.emitRadiation)
			{
				Vector3 emissionOffset = base.smi.master.radiationEmitter.emissionOffset;
				base.smi.master.radiationEmitter.emissionOffset = base.smi.radiationEmissionBaseOffset + base.smi.master.animController.Offset;
				if (Grid.AreCellsInSameWorld(base.smi.master.radiationEmitter.GetEmissionCell(), this.pad_cell))
				{
					base.smi.master.radiationEmitter.Refresh();
				}
				else
				{
					base.smi.master.radiationEmitter.emissionOffset = emissionOffset;
					base.smi.master.radiationEmitter.SetEmitting(false);
				}
			}
			int num2 = 10;
			for (int i = 1; i < num2; i++)
			{
				int num3 = Grid.OffsetCell(num, -1, -i);
				int num4 = Grid.OffsetCell(num, 0, -i);
				int num5 = Grid.OffsetCell(num, 1, -i);
				if (Grid.AreCellsInSameWorld(num3, this.pad_cell))
				{
					if (base.smi.master.exhaustDiseaseIdx != 255)
					{
						SimMessages.ModifyDiseaseOnCell(num3, base.smi.master.exhaustDiseaseIdx, (int)((float)base.smi.master.exhaustDiseaseCount / ((float)i + 1f)));
					}
					SimMessages.ModifyEnergy(num3, base.smi.master.exhaustTemperature / (float)(i + 1), 3200f, SimMessages.EnergySourceID.Burner);
				}
				if (Grid.AreCellsInSameWorld(num4, this.pad_cell))
				{
					if (base.smi.master.exhaustDiseaseIdx != 255)
					{
						SimMessages.ModifyDiseaseOnCell(num4, base.smi.master.exhaustDiseaseIdx, (int)((float)base.smi.master.exhaustDiseaseCount / (float)i));
					}
					SimMessages.ModifyEnergy(num4, base.smi.master.exhaustTemperature / (float)i, 3200f, SimMessages.EnergySourceID.Burner);
				}
				if (Grid.AreCellsInSameWorld(num5, this.pad_cell))
				{
					if (base.smi.master.exhaustDiseaseIdx != 255)
					{
						SimMessages.ModifyDiseaseOnCell(num5, base.smi.master.exhaustDiseaseIdx, (int)((float)base.smi.master.exhaustDiseaseCount / ((float)i + 1f)));
					}
					SimMessages.ModifyEnergy(num5, base.smi.master.exhaustTemperature / (float)(i + 1), 3200f, SimMessages.EnergySourceID.Burner);
				}
			}
		}

		// Token: 0x0600A2C6 RID: 41670 RVA: 0x00388640 File Offset: 0x00386840
		public void EndBurn()
		{
			if (base.smi.master.emitRadiation)
			{
				base.smi.master.radiationEmitter.emissionOffset = base.smi.radiationEmissionBaseOffset;
				base.smi.master.radiationEmitter.SetEmitting(false);
			}
			this.pad_cell = Grid.InvalidCell;
		}

		// Token: 0x04007F07 RID: 32519
		public Vector3 radiationEmissionBaseOffset;

		// Token: 0x04007F08 RID: 32520
		private int pad_cell;
	}

	// Token: 0x02001B31 RID: 6961
	public class States : GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster>
	{
		// Token: 0x0600A2C7 RID: 41671 RVA: 0x003886A0 File Offset: 0x003868A0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.initializing.load;
			this.initializing.load.ScheduleGoTo(0f, this.initializing.decide);
			this.initializing.decide.Transition(this.space, new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsRocketInSpace), UpdateRate.SIM_200ms).Transition(this.burning, new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsRocketAirborne), UpdateRate.SIM_200ms).Transition(this.idle, new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsRocketGrounded), UpdateRate.SIM_200ms);
			this.idle.DefaultState(this.idle.grounded).EventTransition(GameHashes.RocketLaunched, this.burning_pre, null);
			this.idle.grounded.EventTransition(GameHashes.LaunchConditionChanged, this.idle.ready, new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsReadyToLaunch)).QueueAnim("grounded", true, null);
			this.idle.ready.EventTransition(GameHashes.LaunchConditionChanged, this.idle.grounded, GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Not(new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsReadyToLaunch))).PlayAnim("pre_ready_to_launch", KAnim.PlayMode.Once).QueueAnim("ready_to_launch", true, null).Exit(delegate(RocketEngineCluster.StatesInstance smi)
			{
				KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
				if (component != null)
				{
					component.Play("pst_ready_to_launch", KAnim.PlayMode.Once, 1f, 0f);
				}
			});
			this.burning_pre.PlayAnim("launch_pre").OnAnimQueueComplete(this.burning);
			this.burning.EventTransition(GameHashes.RocketLanded, this.burnComplete, null).PlayAnim("launch_loop", KAnim.PlayMode.Loop).Enter(delegate(RocketEngineCluster.StatesInstance smi)
			{
				smi.BeginBurn();
			}).Update(delegate(RocketEngineCluster.StatesInstance smi, float dt)
			{
				smi.DoBurn(dt);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(RocketEngineCluster.StatesInstance smi)
			{
				smi.EndBurn();
			}).TagTransition(GameTags.RocketInSpace, this.space, false);
			this.space.EventTransition(GameHashes.DoReturnRocket, this.burning, null);
			this.burnComplete.PlayAnim("launch_pst", KAnim.PlayMode.Loop).GoTo(this.idle);
		}

		// Token: 0x0600A2C8 RID: 41672 RVA: 0x003888F0 File Offset: 0x00386AF0
		private bool IsReadyToLaunch(RocketEngineCluster.StatesInstance smi)
		{
			return smi.GetComponent<RocketModuleCluster>().CraftInterface.CheckPreppedForLaunch();
		}

		// Token: 0x0600A2C9 RID: 41673 RVA: 0x00388902 File Offset: 0x00386B02
		public bool IsRocketAirborne(RocketEngineCluster.StatesInstance smi)
		{
			return smi.master.HasTag(GameTags.RocketNotOnGround) && !smi.master.HasTag(GameTags.RocketInSpace);
		}

		// Token: 0x0600A2CA RID: 41674 RVA: 0x0038892B File Offset: 0x00386B2B
		public bool IsRocketGrounded(RocketEngineCluster.StatesInstance smi)
		{
			return smi.master.HasTag(GameTags.RocketOnGround);
		}

		// Token: 0x0600A2CB RID: 41675 RVA: 0x0038893D File Offset: 0x00386B3D
		public bool IsRocketInSpace(RocketEngineCluster.StatesInstance smi)
		{
			return smi.master.HasTag(GameTags.RocketInSpace);
		}

		// Token: 0x04007F09 RID: 32521
		public RocketEngineCluster.States.InitializingStates initializing;

		// Token: 0x04007F0A RID: 32522
		public RocketEngineCluster.States.IdleStates idle;

		// Token: 0x04007F0B RID: 32523
		public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State burning_pre;

		// Token: 0x04007F0C RID: 32524
		public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State burning;

		// Token: 0x04007F0D RID: 32525
		public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State burnComplete;

		// Token: 0x04007F0E RID: 32526
		public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State space;

		// Token: 0x02002618 RID: 9752
		public class InitializingStates : GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State
		{
			// Token: 0x0400A98A RID: 43402
			public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State load;

			// Token: 0x0400A98B RID: 43403
			public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State decide;
		}

		// Token: 0x02002619 RID: 9753
		public class IdleStates : GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State
		{
			// Token: 0x0400A98C RID: 43404
			public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State grounded;

			// Token: 0x0400A98D RID: 43405
			public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State ready;
		}
	}
}
