using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200047E RID: 1150
public class SparkleStreaker : GameStateMachine<SparkleStreaker, SparkleStreaker.Instance>
{
	// Token: 0x060018D5 RID: 6357 RVA: 0x00084604 File Offset: 0x00082804
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.DefaultState(this.overjoyed.idle).TagTransition(GameTags.Overjoyed, this.neutral, true).ToggleEffect("IsSparkleStreaker").ToggleLoopingSound(this.soundPath, null, true, true, true).Enter(delegate(SparkleStreaker.Instance smi)
		{
			smi.sparkleStreakFX = Util.KInstantiate(EffectPrefabs.Instance.SparkleStreakFX, smi.master.transform.GetPosition() + this.offset);
			smi.sparkleStreakFX.transform.SetParent(smi.master.transform);
			smi.sparkleStreakFX.SetActive(true);
			smi.CreatePasserbyReactable();
		}).Exit(delegate(SparkleStreaker.Instance smi)
		{
			Util.KDestroyGameObject(smi.sparkleStreakFX);
			smi.ClearPasserbyReactable();
		});
		this.overjoyed.idle.Enter(delegate(SparkleStreaker.Instance smi)
		{
			smi.SetSparkleSoundParam(0f);
		}).EventTransition(GameHashes.ObjectMovementStateChanged, this.overjoyed.moving, (SparkleStreaker.Instance smi) => smi.IsMoving());
		this.overjoyed.moving.Enter(delegate(SparkleStreaker.Instance smi)
		{
			smi.SetSparkleSoundParam(1f);
		}).EventTransition(GameHashes.ObjectMovementStateChanged, this.overjoyed.idle, (SparkleStreaker.Instance smi) => !smi.IsMoving());
	}

	// Token: 0x04000DCB RID: 3531
	private Vector3 offset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x04000DCC RID: 3532
	public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000DCD RID: 3533
	public SparkleStreaker.OverjoyedStates overjoyed;

	// Token: 0x04000DCE RID: 3534
	public string soundPath = GlobalAssets.GetSound("SparkleStreaker_lp", false);

	// Token: 0x04000DCF RID: 3535
	public HashedString SPARKLE_STREAKER_MOVING_PARAMETER = "sparkleStreaker_moving";

	// Token: 0x02001251 RID: 4689
	public class OverjoyedStates : GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040062F5 RID: 25333
		public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040062F6 RID: 25334
		public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State moving;
	}

	// Token: 0x02001252 RID: 4690
	public new class Instance : GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060082D0 RID: 33488 RVA: 0x0031D710 File Offset: 0x0031B910
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060082D1 RID: 33489 RVA: 0x0031D71C File Offset: 0x0031B91C
		public void CreatePasserbyReactable()
		{
			if (this.passerbyReactable == null)
			{
				EmoteReactable emoteReactable = new EmoteReactable(base.gameObject, "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, 0f, 600f, float.PositiveInfinity, 0f);
				Emote clapCheer = Db.Get().Emotes.Minion.ClapCheer;
				emoteReactable.SetEmote(clapCheer).SetThought(Db.Get().Thoughts.Happy).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor));
				emoteReactable.RegisterEmoteStepCallbacks("clapcheer_pre", new Action<GameObject>(this.AddReactionEffect), null);
				this.passerbyReactable = emoteReactable;
			}
		}

		// Token: 0x060082D2 RID: 33490 RVA: 0x0031D7D6 File Offset: 0x0031B9D6
		private void AddReactionEffect(GameObject reactor)
		{
			reactor.GetComponent<Effects>().Add("SawSparkleStreaker", true);
		}

		// Token: 0x060082D3 RID: 33491 RVA: 0x0031D7EA File Offset: 0x0031B9EA
		private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
		{
			return transition.end == NavType.Floor;
		}

		// Token: 0x060082D4 RID: 33492 RVA: 0x0031D7F5 File Offset: 0x0031B9F5
		public void ClearPasserbyReactable()
		{
			if (this.passerbyReactable != null)
			{
				this.passerbyReactable.Cleanup();
				this.passerbyReactable = null;
			}
		}

		// Token: 0x060082D5 RID: 33493 RVA: 0x0031D811 File Offset: 0x0031BA11
		public bool IsMoving()
		{
			return base.smi.master.GetComponent<Navigator>().IsMoving();
		}

		// Token: 0x060082D6 RID: 33494 RVA: 0x0031D828 File Offset: 0x0031BA28
		public void SetSparkleSoundParam(float val)
		{
			base.GetComponent<LoopingSounds>().SetParameter(GlobalAssets.GetSound("SparkleStreaker_lp", false), "sparkleStreaker_moving", val);
		}

		// Token: 0x040062F7 RID: 25335
		private Reactable passerbyReactable;

		// Token: 0x040062F8 RID: 25336
		public GameObject sparkleStreakFX;
	}
}
