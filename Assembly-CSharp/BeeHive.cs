using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000687 RID: 1671
public class BeeHive : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>
{
	// Token: 0x06002999 RID: 10649 RVA: 0x000EA9C8 File Offset: 0x000E8BC8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.enabled.grownStates;
		this.root.DoTutorial(Tutorial.TutorialMessages.TM_Radiation).Enter(delegate(BeeHive.StatesInstance smi)
		{
			AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(smi.gameObject);
			if (amountInstance != null)
			{
				amountInstance.hide = true;
			}
		}).EventHandler(GameHashes.Died, delegate(BeeHive.StatesInstance smi)
		{
			PrimaryElement component = smi.GetComponent<PrimaryElement>();
			Storage component2 = smi.GetComponent<Storage>();
			byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.id);
			component2.AddOre(SimHashes.NuclearWaste, BeeHiveTuning.WASTE_DROPPED_ON_DEATH, component.Temperature, index, BeeHiveTuning.GERMS_DROPPED_ON_DEATH, false, true);
			component2.DropAll(smi.master.transform.position, true, true, default(Vector3), true, null);
		});
		this.disabled.ToggleTag(GameTags.Creatures.Behaviours.DisableCreature).EventTransition(GameHashes.FoundationChanged, this.enabled, (BeeHive.StatesInstance smi) => !smi.IsDisabled()).EventTransition(GameHashes.EntombedChanged, this.enabled, (BeeHive.StatesInstance smi) => !smi.IsDisabled()).EventTransition(GameHashes.EnteredBreathableArea, this.enabled, (BeeHive.StatesInstance smi) => !smi.IsDisabled());
		this.enabled.EventTransition(GameHashes.FoundationChanged, this.disabled, (BeeHive.StatesInstance smi) => smi.IsDisabled()).EventTransition(GameHashes.EntombedChanged, this.disabled, (BeeHive.StatesInstance smi) => smi.IsDisabled()).EventTransition(GameHashes.Drowning, this.disabled, (BeeHive.StatesInstance smi) => smi.IsDisabled()).DefaultState(this.enabled.grownStates);
		this.enabled.growingStates.ParamTransition<float>(this.hiveGrowth, this.enabled.grownStates, (BeeHive.StatesInstance smi, float f) => f >= 1f).DefaultState(this.enabled.growingStates.idle);
		this.enabled.growingStates.idle.Update(delegate(BeeHive.StatesInstance smi, float dt)
		{
			smi.DeltaGrowth(dt / 600f / BeeHiveTuning.HIVE_GROWTH_TIME);
		}, UpdateRate.SIM_4000ms, false);
		this.enabled.grownStates.ParamTransition<float>(this.hiveGrowth, this.enabled.growingStates, (BeeHive.StatesInstance smi, float f) => f < 1f).DefaultState(this.enabled.grownStates.dayTime);
		this.enabled.grownStates.dayTime.EventTransition(GameHashes.Nighttime, (BeeHive.StatesInstance smi) => GameClock.Instance, this.enabled.grownStates.nightTime, (BeeHive.StatesInstance smi) => GameClock.Instance.IsNighttime());
		this.enabled.grownStates.nightTime.EventTransition(GameHashes.NewDay, (BeeHive.StatesInstance smi) => GameClock.Instance, this.enabled.grownStates.dayTime, (BeeHive.StatesInstance smi) => GameClock.Instance.GetTimeSinceStartOfCycle() <= 1f).Exit(delegate(BeeHive.StatesInstance smi)
		{
			if (!GameClock.Instance.IsNighttime())
			{
				smi.SpawnNewLarvaFromHive();
			}
		});
	}

	// Token: 0x040017FC RID: 6140
	public GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State disabled;

	// Token: 0x040017FD RID: 6141
	public BeeHive.EnabledStates enabled;

	// Token: 0x040017FE RID: 6142
	public StateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.FloatParameter hiveGrowth = new StateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.FloatParameter(1f);

	// Token: 0x0200146C RID: 5228
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040069B1 RID: 27057
		public string beePrefabID;

		// Token: 0x040069B2 RID: 27058
		public string larvaPrefabID;
	}

	// Token: 0x0200146D RID: 5229
	public class GrowingStates : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State
	{
		// Token: 0x040069B3 RID: 27059
		public GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State idle;
	}

	// Token: 0x0200146E RID: 5230
	public class GrownStates : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State
	{
		// Token: 0x040069B4 RID: 27060
		public GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State dayTime;

		// Token: 0x040069B5 RID: 27061
		public GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State nightTime;
	}

	// Token: 0x0200146F RID: 5231
	public class EnabledStates : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State
	{
		// Token: 0x040069B6 RID: 27062
		public BeeHive.GrowingStates growingStates;

		// Token: 0x040069B7 RID: 27063
		public BeeHive.GrownStates grownStates;
	}

	// Token: 0x02001470 RID: 5232
	public class StatesInstance : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.GameInstance
	{
		// Token: 0x06008A93 RID: 35475 RVA: 0x00334416 File Offset: 0x00332616
		public StatesInstance(IStateMachineTarget master, BeeHive.Def def) : base(master, def)
		{
			base.Subscribe(1119167081, new Action<object>(this.OnNewGameSpawn));
			Components.BeeHives.Add(this);
		}

		// Token: 0x06008A94 RID: 35476 RVA: 0x00334442 File Offset: 0x00332642
		public void SetUpNewHive()
		{
			base.sm.hiveGrowth.Set(0f, this, false);
		}

		// Token: 0x06008A95 RID: 35477 RVA: 0x0033445C File Offset: 0x0033265C
		protected override void OnCleanUp()
		{
			Components.BeeHives.Remove(this);
			base.OnCleanUp();
		}

		// Token: 0x06008A96 RID: 35478 RVA: 0x0033446F File Offset: 0x0033266F
		private void OnNewGameSpawn(object data)
		{
			this.NewGamePopulateHive();
		}

		// Token: 0x06008A97 RID: 35479 RVA: 0x00334478 File Offset: 0x00332678
		private void NewGamePopulateHive()
		{
			int num = 1;
			for (int i = 0; i < num; i++)
			{
				this.SpawnNewBeeFromHive();
			}
			num = 1;
			for (int j = 0; j < num; j++)
			{
				this.SpawnNewLarvaFromHive();
			}
		}

		// Token: 0x06008A98 RID: 35480 RVA: 0x003344AD File Offset: 0x003326AD
		public bool IsFullyGrown()
		{
			return base.sm.hiveGrowth.Get(this) >= 1f;
		}

		// Token: 0x06008A99 RID: 35481 RVA: 0x003344CC File Offset: 0x003326CC
		public void DeltaGrowth(float delta)
		{
			float num = base.sm.hiveGrowth.Get(this);
			num += delta;
			Mathf.Clamp01(num);
			base.sm.hiveGrowth.Set(num, this, false);
		}

		// Token: 0x06008A9A RID: 35482 RVA: 0x0033450A File Offset: 0x0033270A
		public void SpawnNewLarvaFromHive()
		{
			Util.KInstantiate(Assets.GetPrefab(base.def.larvaPrefabID), base.transform.GetPosition()).SetActive(true);
		}

		// Token: 0x06008A9B RID: 35483 RVA: 0x00334537 File Offset: 0x00332737
		public void SpawnNewBeeFromHive()
		{
			Util.KInstantiate(Assets.GetPrefab(base.def.beePrefabID), base.transform.GetPosition()).SetActive(true);
		}

		// Token: 0x06008A9C RID: 35484 RVA: 0x00334564 File Offset: 0x00332764
		public bool IsDisabled()
		{
			KPrefabID component = base.GetComponent<KPrefabID>();
			return component.HasTag(GameTags.Creatures.HasNoFoundation) || component.HasTag(GameTags.Entombed) || component.HasTag(GameTags.Creatures.Drowning);
		}
	}
}
