using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000AB8 RID: 2744
[SerializationConfig(MemberSerialization.OptIn)]
public class CargoLander : GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>
{
	// Token: 0x060050ED RID: 20717 RVA: 0x001D0EDC File Offset: 0x001CF0DC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.init;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.InitializeOperationalFlag(RocketModule.landedFlag, false).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.OnStorageChange, delegate(CargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		});
		this.init.ParamTransition<bool>(this.isLanded, this.grounded, GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.IsTrue).GoTo(this.stored);
		this.stored.TagTransition(GameTags.Stored, this.landing, true).EventHandler(GameHashes.JettisonedLander, delegate(CargoLander.StatesInstance smi)
		{
			smi.OnJettisoned();
		});
		this.landing.PlayAnim("landing", KAnim.PlayMode.Loop).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.ShowLandingPreview(true);
		}).Exit(delegate(CargoLander.StatesInstance smi)
		{
			smi.ShowLandingPreview(false);
		}).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.ResetAnimPosition();
		}).Update(delegate(CargoLander.StatesInstance smi, float dt)
		{
			smi.LandingUpdate(dt);
		}, UpdateRate.SIM_EVERY_TICK, false).Transition(this.land, (CargoLander.StatesInstance smi) => smi.flightAnimOffset <= 0f, UpdateRate.SIM_200ms);
		this.land.PlayAnim("grounded_pre").OnAnimQueueComplete(this.grounded);
		this.grounded.DefaultState(this.grounded.loaded).ToggleOperationalFlag(RocketModule.landedFlag).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.sm.isLanded.Set(true, smi, false);
		});
		this.grounded.loaded.PlayAnim("grounded").ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.IsFalse).OnSignal(this.emptyCargo, this.grounded.emptying).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.DoLand();
		});
		this.grounded.emptying.PlayAnim("deploying").TriggerOnEnter(GameHashes.JettisonCargo, null).OnAnimQueueComplete(this.grounded.empty);
		this.grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.grounded.loaded, GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.IsTrue);
	}

	// Token: 0x040035C0 RID: 13760
	public StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.BoolParameter hasCargo;

	// Token: 0x040035C1 RID: 13761
	public StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.Signal emptyCargo;

	// Token: 0x040035C2 RID: 13762
	public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State init;

	// Token: 0x040035C3 RID: 13763
	public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State stored;

	// Token: 0x040035C4 RID: 13764
	public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State landing;

	// Token: 0x040035C5 RID: 13765
	public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State land;

	// Token: 0x040035C6 RID: 13766
	public CargoLander.CrashedStates grounded;

	// Token: 0x040035C7 RID: 13767
	public StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.BoolParameter isLanded = new StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.BoolParameter(false);

	// Token: 0x02001AF7 RID: 6903
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007E46 RID: 32326
		public Tag previewTag;

		// Token: 0x04007E47 RID: 32327
		public bool deployOnLanding = true;
	}

	// Token: 0x02001AF8 RID: 6904
	public class CrashedStates : GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State
	{
		// Token: 0x04007E48 RID: 32328
		public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State loaded;

		// Token: 0x04007E49 RID: 32329
		public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State emptying;

		// Token: 0x04007E4A RID: 32330
		public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State empty;
	}

	// Token: 0x02001AF9 RID: 6905
	public class StatesInstance : GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.GameInstance
	{
		// Token: 0x0600A1C4 RID: 41412 RVA: 0x00383EE8 File Offset: 0x003820E8
		public StatesInstance(IStateMachineTarget master, CargoLander.Def def) : base(master, def)
		{
		}

		// Token: 0x0600A1C5 RID: 41413 RVA: 0x00383F34 File Offset: 0x00382134
		public void ResetAnimPosition()
		{
			base.GetComponent<KBatchedAnimController>().Offset = Vector3.up * this.flightAnimOffset;
		}

		// Token: 0x0600A1C6 RID: 41414 RVA: 0x00383F51 File Offset: 0x00382151
		public void OnJettisoned()
		{
			this.flightAnimOffset = 50f;
		}

		// Token: 0x0600A1C7 RID: 41415 RVA: 0x00383F60 File Offset: 0x00382160
		public void ShowLandingPreview(bool show)
		{
			if (show)
			{
				this.landingPreview = Util.KInstantiate(Assets.GetPrefab(base.def.previewTag), base.transform.GetPosition(), Quaternion.identity, base.gameObject, null, true, 0);
				this.landingPreview.SetActive(true);
				return;
			}
			this.landingPreview.DeleteObject();
			this.landingPreview = null;
		}

		// Token: 0x0600A1C8 RID: 41416 RVA: 0x00383FC4 File Offset: 0x003821C4
		public void LandingUpdate(float dt)
		{
			this.flightAnimOffset = Mathf.Max(this.flightAnimOffset - dt * this.topSpeed, 0f);
			this.ResetAnimPosition();
			int num = Grid.PosToCell(base.gameObject.transform.GetPosition() + new Vector3(0f, this.flightAnimOffset, 0f));
			if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == base.gameObject.GetMyWorldId())
			{
				SimMessages.EmitMass(num, ElementLoader.GetElementIndex(this.exhaustElement), dt * this.exhaustEmitRate, this.exhaustTemperature, 0, 0, -1);
			}
		}

		// Token: 0x0600A1C9 RID: 41417 RVA: 0x00384064 File Offset: 0x00382264
		public void DoLand()
		{
			base.smi.master.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
			OccupyArea component = base.smi.GetComponent<OccupyArea>();
			if (component != null)
			{
				component.ApplyToCells = true;
			}
			if (base.def.deployOnLanding && this.CheckIfLoaded())
			{
				base.sm.emptyCargo.Trigger(this);
			}
			base.smi.master.gameObject.Trigger(1591811118, this);
		}

		// Token: 0x0600A1CA RID: 41418 RVA: 0x003840E8 File Offset: 0x003822E8
		public bool CheckIfLoaded()
		{
			bool flag = false;
			MinionStorage component = base.GetComponent<MinionStorage>();
			if (component != null)
			{
				flag |= (component.GetStoredMinionInfo().Count > 0);
			}
			Storage component2 = base.GetComponent<Storage>();
			if (component2 != null && !component2.IsEmpty())
			{
				flag = true;
			}
			if (flag != base.sm.hasCargo.Get(this))
			{
				base.sm.hasCargo.Set(flag, this, false);
			}
			return flag;
		}

		// Token: 0x04007E4B RID: 32331
		[Serialize]
		public float flightAnimOffset = 50f;

		// Token: 0x04007E4C RID: 32332
		public float exhaustEmitRate = 2f;

		// Token: 0x04007E4D RID: 32333
		public float exhaustTemperature = 1000f;

		// Token: 0x04007E4E RID: 32334
		public SimHashes exhaustElement = SimHashes.CarbonDioxide;

		// Token: 0x04007E4F RID: 32335
		public float topSpeed = 5f;

		// Token: 0x04007E50 RID: 32336
		private GameObject landingPreview;
	}
}
