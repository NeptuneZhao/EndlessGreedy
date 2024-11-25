using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000B1D RID: 2845
[SkipSaveFileSerialization]
public class Stinky : StateMachineComponent<Stinky.StatesInstance>
{
	// Token: 0x060054A7 RID: 21671 RVA: 0x001E446D File Offset: 0x001E266D
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x060054A8 RID: 21672 RVA: 0x001E447C File Offset: 0x001E267C
	private void Emit(object data)
	{
		GameObject gameObject = (GameObject)data;
		Components.Cmps<MinionIdentity> liveMinionIdentities = Components.LiveMinionIdentities;
		Vector2 a = gameObject.transform.GetPosition();
		for (int i = 0; i < liveMinionIdentities.Count; i++)
		{
			MinionIdentity minionIdentity = liveMinionIdentities[i];
			if (minionIdentity.gameObject != gameObject.gameObject)
			{
				Vector2 b = minionIdentity.transform.GetPosition();
				if (Vector2.SqrMagnitude(a - b) <= 2.25f)
				{
					minionIdentity.Trigger(508119890, Strings.Get("STRINGS.DUPLICANTS.DISEASES.PUTRIDODOUR.CRINGE_EFFECT").String);
					minionIdentity.GetComponent<Effects>().Add("SmelledStinky", true);
					minionIdentity.gameObject.GetSMI<ThoughtGraph.Instance>().AddThought(Db.Get().Thoughts.PutridOdour);
				}
			}
		}
		int gameCell = Grid.PosToCell(gameObject.transform.GetPosition());
		float value = Db.Get().Amounts.Temperature.Lookup(this).value;
		SimMessages.AddRemoveSubstance(gameCell, SimHashes.ContaminatedOxygen, CellEventLogger.Instance.ElementConsumerSimUpdate, 0.0025000002f, value, byte.MaxValue, 0, true, -1);
		GameObject gameObject2 = gameObject;
		bool flag = SoundEvent.ObjectIsSelectedAndVisible(gameObject2);
		Vector3 vector = gameObject2.transform.GetPosition();
		float volume = 1f;
		if (flag)
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
			volume = SoundEvent.GetVolume(flag);
		}
		else
		{
			vector.z = 0f;
		}
		KFMOD.PlayOneShot(GlobalAssets.GetSound("Dupe_Flatulence", false), vector, volume);
	}

	// Token: 0x0400377E RID: 14206
	private const float EmitMass = 0.0025000002f;

	// Token: 0x0400377F RID: 14207
	private const SimHashes EmitElement = SimHashes.ContaminatedOxygen;

	// Token: 0x04003780 RID: 14208
	private const float EmissionRadius = 1.5f;

	// Token: 0x04003781 RID: 14209
	private const float MaxDistanceSq = 2.25f;

	// Token: 0x04003782 RID: 14210
	private KBatchedAnimController stinkyController;

	// Token: 0x04003783 RID: 14211
	private static readonly HashedString[] WorkLoopAnims = new HashedString[]
	{
		"working_pre",
		"working_loop",
		"working_pst"
	};

	// Token: 0x02001B69 RID: 7017
	public class StatesInstance : GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.GameInstance
	{
		// Token: 0x0600A363 RID: 41827 RVA: 0x00389B07 File Offset: 0x00387D07
		public StatesInstance(Stinky master) : base(master)
		{
		}
	}

	// Token: 0x02001B6A RID: 7018
	public class States : GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky>
	{
		// Token: 0x0600A364 RID: 41828 RVA: 0x00389B10 File Offset: 0x00387D10
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false).Enter(delegate(Stinky.StatesInstance smi)
			{
				KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("odor_fx_kanim", smi.master.gameObject.transform.GetPosition(), smi.master.gameObject.transform, true, Grid.SceneLayer.Front, false);
				kbatchedAnimController.Play(Stinky.WorkLoopAnims, KAnim.PlayMode.Once);
				smi.master.stinkyController = kbatchedAnimController;
			}).Update("StinkyFX", delegate(Stinky.StatesInstance smi, float dt)
			{
				if (smi.master.stinkyController != null)
				{
					smi.master.stinkyController.Play(Stinky.WorkLoopAnims, KAnim.PlayMode.Once);
				}
			}, UpdateRate.SIM_4000ms, false);
			this.idle.Enter("ScheduleNextFart", delegate(Stinky.StatesInstance smi)
			{
				smi.ScheduleGoTo(this.GetNewInterval(), this.emit);
			});
			this.emit.Enter("Fart", delegate(Stinky.StatesInstance smi)
			{
				smi.master.Emit(smi.master.gameObject);
			}).ToggleExpression(Db.Get().Expressions.Relief, null).ScheduleGoTo(3f, this.idle);
		}

		// Token: 0x0600A365 RID: 41829 RVA: 0x00389BF9 File Offset: 0x00387DF9
		private float GetNewInterval()
		{
			return Mathf.Min(Mathf.Max(Util.GaussianRandom(TRAITS.STINKY_EMIT_INTERVAL_MAX - TRAITS.STINKY_EMIT_INTERVAL_MIN, 1f), TRAITS.STINKY_EMIT_INTERVAL_MIN), TRAITS.STINKY_EMIT_INTERVAL_MAX);
		}

		// Token: 0x04007FA8 RID: 32680
		public GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State idle;

		// Token: 0x04007FA9 RID: 32681
		public GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State emit;
	}
}
