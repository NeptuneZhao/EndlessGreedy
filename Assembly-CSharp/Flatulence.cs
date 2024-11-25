using System;
using TUNING;
using UnityEngine;

// Token: 0x020008B9 RID: 2233
[SkipSaveFileSerialization]
public class Flatulence : StateMachineComponent<Flatulence.StatesInstance>
{
	// Token: 0x06003E93 RID: 16019 RVA: 0x0015A88C File Offset: 0x00158A8C
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06003E94 RID: 16020 RVA: 0x0015A89C File Offset: 0x00158A9C
	private void Emit(object data)
	{
		GameObject gameObject = (GameObject)data;
		float value = Db.Get().Amounts.Temperature.Lookup(this).value;
		Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
		if (equippable != null)
		{
			equippable.GetComponent<Storage>().AddGasChunk(SimHashes.Methane, 0.1f, value, byte.MaxValue, 0, false, true);
		}
		else
		{
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
						minionIdentity.gameObject.GetSMI<ThoughtGraph.Instance>().AddThought(Db.Get().Thoughts.PutridOdour);
					}
				}
			}
			SimMessages.AddRemoveSubstance(Grid.PosToCell(gameObject.transform.GetPosition()), SimHashes.Methane, CellEventLogger.Instance.ElementConsumerSimUpdate, 0.1f, value, byte.MaxValue, 0, true, -1);
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("odor_fx_kanim", gameObject.transform.GetPosition(), gameObject.transform, true, Grid.SceneLayer.Front, false);
			kbatchedAnimController.Play(Flatulence.WorkLoopAnims, KAnim.PlayMode.Once);
			kbatchedAnimController.destroyOnAnimComplete = true;
		}
		GameObject gameObject2 = gameObject;
		bool flag = SoundEvent.ObjectIsSelectedAndVisible(gameObject2);
		Vector3 vector = gameObject2.transform.GetPosition();
		vector.z = 0f;
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

	// Token: 0x04002678 RID: 9848
	private const float EmitMass = 0.1f;

	// Token: 0x04002679 RID: 9849
	private const SimHashes EmitElement = SimHashes.Methane;

	// Token: 0x0400267A RID: 9850
	private const float EmissionRadius = 1.5f;

	// Token: 0x0400267B RID: 9851
	private const float MaxDistanceSq = 2.25f;

	// Token: 0x0400267C RID: 9852
	private static readonly HashedString[] WorkLoopAnims = new HashedString[]
	{
		"working_pre",
		"working_loop",
		"working_pst"
	};

	// Token: 0x020017B1 RID: 6065
	public class StatesInstance : GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.GameInstance
	{
		// Token: 0x0600965E RID: 38494 RVA: 0x00361D2B File Offset: 0x0035FF2B
		public StatesInstance(Flatulence master) : base(master)
		{
		}
	}

	// Token: 0x020017B2 RID: 6066
	public class States : GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence>
	{
		// Token: 0x0600965F RID: 38495 RVA: 0x00361D34 File Offset: 0x0035FF34
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Enter("ScheduleNextFart", delegate(Flatulence.StatesInstance smi)
			{
				smi.ScheduleGoTo(this.GetNewInterval(), this.emit);
			});
			this.emit.Enter("Fart", delegate(Flatulence.StatesInstance smi)
			{
				smi.master.Emit(smi.master.gameObject);
			}).ToggleExpression(Db.Get().Expressions.Relief, null).ScheduleGoTo(3f, this.idle);
		}

		// Token: 0x06009660 RID: 38496 RVA: 0x00361DCE File Offset: 0x0035FFCE
		private float GetNewInterval()
		{
			return Mathf.Min(Mathf.Max(Util.GaussianRandom(TRAITS.FLATULENCE_EMIT_INTERVAL_MAX - TRAITS.FLATULENCE_EMIT_INTERVAL_MIN, 1f), TRAITS.FLATULENCE_EMIT_INTERVAL_MIN), TRAITS.FLATULENCE_EMIT_INTERVAL_MAX);
		}

		// Token: 0x04007367 RID: 29543
		public GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.State idle;

		// Token: 0x04007368 RID: 29544
		public GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.State emit;
	}
}
