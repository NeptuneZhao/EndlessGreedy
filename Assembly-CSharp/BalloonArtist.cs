using System;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using TUNING;

// Token: 0x0200047A RID: 1146
public class BalloonArtist : GameStateMachine<BalloonArtist, BalloonArtist.Instance>
{
	// Token: 0x060018C5 RID: 6341 RVA: 0x00083E28 File Offset: 0x00082028
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ParamTransition<int>(this.balloonsGivenOut, this.overjoyed.exitEarly, (BalloonArtist.Instance smi, int p) => p >= TRAITS.JOY_REACTIONS.BALLOON_ARTIST.NUM_BALLOONS_TO_GIVE).Exit(delegate(BalloonArtist.Instance smi)
		{
			smi.numBalloonsGiven = 0;
			this.balloonsGivenOut.Set(0, smi, false);
		});
		this.overjoyed.idle.Enter(delegate(BalloonArtist.Instance smi)
		{
			if (smi.IsRecTime())
			{
				smi.GoTo(this.overjoyed.balloon_stand);
			}
		}).ToggleStatusItem(Db.Get().DuplicantStatusItems.BalloonArtistPlanning, null).EventTransition(GameHashes.ScheduleBlocksChanged, this.overjoyed.balloon_stand, (BalloonArtist.Instance smi) => smi.IsRecTime());
		this.overjoyed.balloon_stand.ToggleStatusItem(Db.Get().DuplicantStatusItems.BalloonArtistHandingOut, null).EventTransition(GameHashes.ScheduleBlocksChanged, this.overjoyed.idle, (BalloonArtist.Instance smi) => !smi.IsRecTime()).ToggleChore((BalloonArtist.Instance smi) => new BalloonArtistChore(smi.master), this.overjoyed.idle);
		this.overjoyed.exitEarly.Enter(delegate(BalloonArtist.Instance smi)
		{
			smi.ExitJoyReactionEarly();
		});
	}

	// Token: 0x04000DBD RID: 3517
	public StateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.IntParameter balloonsGivenOut;

	// Token: 0x04000DBE RID: 3518
	public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000DBF RID: 3519
	public BalloonArtist.OverjoyedStates overjoyed;

	// Token: 0x02001245 RID: 4677
	public class OverjoyedStates : GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040062CD RID: 25293
		public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040062CE RID: 25294
		public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State balloon_stand;

		// Token: 0x040062CF RID: 25295
		public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State exitEarly;
	}

	// Token: 0x02001246 RID: 4678
	public new class Instance : GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600829D RID: 33437 RVA: 0x0031D14C File Offset: 0x0031B34C
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600829E RID: 33438 RVA: 0x0031D155 File Offset: 0x0031B355
		[OnDeserialized]
		private void OnDeserialized()
		{
			base.smi.sm.balloonsGivenOut.Set(this.numBalloonsGiven, base.smi, false);
		}

		// Token: 0x0600829F RID: 33439 RVA: 0x0031D17C File Offset: 0x0031B37C
		public void Internal_InitBalloons()
		{
			JoyResponseOutfitTarget joyResponseOutfitTarget = JoyResponseOutfitTarget.FromMinion(base.master.gameObject);
			if (!this.balloonSymbolIter.IsNullOrDestroyed())
			{
				if (this.balloonSymbolIter.facade.AndThen<string>((BalloonArtistFacadeResource f) => f.Id) == joyResponseOutfitTarget.ReadFacadeId())
				{
					return;
				}
			}
			this.balloonSymbolIter = joyResponseOutfitTarget.ReadFacadeId().AndThen<BalloonArtistFacadeResource>((string id) => Db.Get().Permits.BalloonArtistFacades.Get(id)).AndThen<BalloonOverrideSymbolIter>((BalloonArtistFacadeResource permit) => permit.GetSymbolIter()).UnwrapOr(new BalloonOverrideSymbolIter(Option.None), null);
			this.SetBalloonSymbolOverride(this.balloonSymbolIter.Current());
		}

		// Token: 0x060082A0 RID: 33440 RVA: 0x0031D269 File Offset: 0x0031B469
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x060082A1 RID: 33441 RVA: 0x0031D28C File Offset: 0x0031B48C
		public void SetBalloonSymbolOverride(BalloonOverrideSymbol balloonOverrideSymbol)
		{
			if (balloonOverrideSymbol.animFile.IsNone())
			{
				base.master.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", Assets.GetAnim("balloon_anim_kanim").GetData().build.GetSymbol("body"), 0);
				return;
			}
			base.master.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", balloonOverrideSymbol.symbol.Unwrap(), 0);
		}

		// Token: 0x060082A2 RID: 33442 RVA: 0x0031D314 File Offset: 0x0031B514
		public BalloonOverrideSymbol GetCurrentBalloonSymbolOverride()
		{
			return this.balloonSymbolIter.Current();
		}

		// Token: 0x060082A3 RID: 33443 RVA: 0x0031D321 File Offset: 0x0031B521
		public void ApplyNextBalloonSymbolOverride()
		{
			this.SetBalloonSymbolOverride(this.balloonSymbolIter.Next());
		}

		// Token: 0x060082A4 RID: 33444 RVA: 0x0031D334 File Offset: 0x0031B534
		public void GiveBalloon()
		{
			this.numBalloonsGiven++;
			base.smi.sm.balloonsGivenOut.Set(this.numBalloonsGiven, base.smi, false);
		}

		// Token: 0x060082A5 RID: 33445 RVA: 0x0031D368 File Offset: 0x0031B568
		public void ExitJoyReactionEarly()
		{
			JoyBehaviourMonitor.Instance smi = base.master.gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
			smi.sm.exitEarly.Trigger(smi);
		}

		// Token: 0x040062D0 RID: 25296
		[Serialize]
		public int numBalloonsGiven;

		// Token: 0x040062D1 RID: 25297
		[NonSerialized]
		private BalloonOverrideSymbolIter balloonSymbolIter;

		// Token: 0x040062D2 RID: 25298
		private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

		// Token: 0x040062D3 RID: 25299
		private const int TARGET_OVERRIDE_PRIORITY = 0;
	}
}
