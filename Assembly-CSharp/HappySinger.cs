using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200047C RID: 1148
public class HappySinger : GameStateMachine<HappySinger, HappySinger.Instance>
{
	// Token: 0x060018CE RID: 6350 RVA: 0x00084234 File Offset: 0x00082434
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.DefaultState(this.overjoyed.idle).TagTransition(GameTags.Overjoyed, this.neutral, true).ToggleEffect("IsJoySinger").ToggleLoopingSound(this.soundPath, null, true, true, true).ToggleAnims("anim_loco_singer_kanim", 0f).ToggleAnims("anim_idle_singer_kanim", 0f).EventHandler(GameHashes.TagsChanged, delegate(HappySinger.Instance smi, object obj)
		{
			smi.musicParticleFX.SetActive(!smi.HasTag(GameTags.Asleep));
		}).Enter(delegate(HappySinger.Instance smi)
		{
			smi.musicParticleFX = Util.KInstantiate(EffectPrefabs.Instance.HappySingerFX, smi.master.transform.GetPosition() + this.offset);
			smi.musicParticleFX.transform.SetParent(smi.master.transform);
			smi.CreatePasserbyReactable();
			smi.musicParticleFX.SetActive(!smi.HasTag(GameTags.Asleep));
		}).Update(delegate(HappySinger.Instance smi, float dt)
		{
			if (!smi.GetSpeechMonitor().IsPlayingSpeech() && SpeechMonitor.IsAllowedToPlaySpeech(smi.gameObject))
			{
				smi.GetSpeechMonitor().PlaySpeech(Db.Get().Thoughts.CatchyTune.speechPrefix, Db.Get().Thoughts.CatchyTune.sound);
			}
		}, UpdateRate.SIM_1000ms, false).Exit(delegate(HappySinger.Instance smi)
		{
			Util.KDestroyGameObject(smi.musicParticleFX);
			smi.ClearPasserbyReactable();
			smi.musicParticleFX.SetActive(false);
		});
	}

	// Token: 0x04000DC4 RID: 3524
	private Vector3 offset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x04000DC5 RID: 3525
	public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000DC6 RID: 3526
	public HappySinger.OverjoyedStates overjoyed;

	// Token: 0x04000DC7 RID: 3527
	public string soundPath = GlobalAssets.GetSound("DupeSinging_NotesFX_LP", false);

	// Token: 0x0200124B RID: 4683
	public class OverjoyedStates : GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040062E3 RID: 25315
		public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040062E4 RID: 25316
		public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State moving;
	}

	// Token: 0x0200124C RID: 4684
	public new class Instance : GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060082B9 RID: 33465 RVA: 0x0031D494 File Offset: 0x0031B694
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060082BA RID: 33466 RVA: 0x0031D4A0 File Offset: 0x0031B6A0
		public void CreatePasserbyReactable()
		{
			if (this.passerbyReactable == null)
			{
				EmoteReactable emoteReactable = new EmoteReactable(base.gameObject, "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, 0f, 600f, float.PositiveInfinity, 0f);
				Emote sing = Db.Get().Emotes.Minion.Sing;
				emoteReactable.SetEmote(sing).SetThought(Db.Get().Thoughts.CatchyTune).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor));
				emoteReactable.RegisterEmoteStepCallbacks("react", new Action<GameObject>(this.AddReactionEffect), null);
				this.passerbyReactable = emoteReactable;
			}
		}

		// Token: 0x060082BB RID: 33467 RVA: 0x0031D55A File Offset: 0x0031B75A
		public SpeechMonitor.Instance GetSpeechMonitor()
		{
			if (this.speechMonitor == null)
			{
				this.speechMonitor = base.master.gameObject.GetSMI<SpeechMonitor.Instance>();
			}
			return this.speechMonitor;
		}

		// Token: 0x060082BC RID: 33468 RVA: 0x0031D580 File Offset: 0x0031B780
		private void AddReactionEffect(GameObject reactor)
		{
			reactor.Trigger(-1278274506, null);
		}

		// Token: 0x060082BD RID: 33469 RVA: 0x0031D58E File Offset: 0x0031B78E
		private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
		{
			return transition.end == NavType.Floor;
		}

		// Token: 0x060082BE RID: 33470 RVA: 0x0031D599 File Offset: 0x0031B799
		public void ClearPasserbyReactable()
		{
			if (this.passerbyReactable != null)
			{
				this.passerbyReactable.Cleanup();
				this.passerbyReactable = null;
			}
		}

		// Token: 0x040062E5 RID: 25317
		private Reactable passerbyReactable;

		// Token: 0x040062E6 RID: 25318
		public GameObject musicParticleFX;

		// Token: 0x040062E7 RID: 25319
		public SpeechMonitor.Instance speechMonitor;
	}
}
