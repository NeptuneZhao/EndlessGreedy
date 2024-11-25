using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020009A1 RID: 2465
public class SpeechMonitor : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>
{
	// Token: 0x060047CA RID: 18378 RVA: 0x0019B02C File Offset: 0x0019922C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Enter(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.CreateMouth)).Exit(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.DestroyMouth));
		this.satisfied.DoNothing();
		this.talking.Enter(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.BeginTalking)).Update(new Action<SpeechMonitor.Instance, float>(SpeechMonitor.UpdateTalking), UpdateRate.RENDER_EVERY_TICK, false).Target(this.mouth).OnAnimQueueComplete(this.satisfied).Exit(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.EndTalking));
	}

	// Token: 0x060047CB RID: 18379 RVA: 0x0019B0C8 File Offset: 0x001992C8
	private static void CreateMouth(SpeechMonitor.Instance smi)
	{
		smi.mouth = global::Util.KInstantiate(Assets.GetPrefab(MouthAnimation.ID), null, null).GetComponent<KBatchedAnimController>();
		smi.mouth.gameObject.SetActive(true);
		smi.sm.mouth.Set(smi.mouth.gameObject, smi, false);
		smi.SetMouthId();
	}

	// Token: 0x060047CC RID: 18380 RVA: 0x0019B12B File Offset: 0x0019932B
	private static void DestroyMouth(SpeechMonitor.Instance smi)
	{
		if (smi.mouth != null)
		{
			global::Util.KDestroyGameObject(smi.mouth);
			smi.mouth = null;
		}
	}

	// Token: 0x060047CD RID: 18381 RVA: 0x0019B150 File Offset: 0x00199350
	private static string GetRandomSpeechAnim(SpeechMonitor.Instance smi)
	{
		return smi.speechPrefix + UnityEngine.Random.Range(1, TuningData<SpeechMonitor.Tuning>.Get().speechCount).ToString() + smi.mouthId;
	}

	// Token: 0x060047CE RID: 18382 RVA: 0x0019B188 File Offset: 0x00199388
	public static bool IsAllowedToPlaySpeech(GameObject go)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		if (component.HasTag(GameTags.Dead) || component.HasTag(GameTags.Incapacitated))
		{
			return false;
		}
		KBatchedAnimController component2 = go.GetComponent<KBatchedAnimController>();
		KAnim.Anim currentAnim = component2.GetCurrentAnim();
		return currentAnim == null || (GameAudioSheets.Get().IsAnimAllowedToPlaySpeech(currentAnim) && SpeechMonitor.CanOverrideHead(component2));
	}

	// Token: 0x060047CF RID: 18383 RVA: 0x0019B1E0 File Offset: 0x001993E0
	private static bool CanOverrideHead(KBatchedAnimController kbac)
	{
		bool result = true;
		KAnim.Anim currentAnim = kbac.GetCurrentAnim();
		if (currentAnim == null)
		{
			result = false;
		}
		else if (currentAnim.animFile.name != SpeechMonitor.GENERIC_CONVO_ANIM_NAME)
		{
			int currentFrameIndex = kbac.GetCurrentFrameIndex();
			KAnim.Anim.Frame frame;
			if (currentFrameIndex <= 0)
			{
				result = false;
			}
			else if (KAnimBatchManager.Instance().GetBatchGroupData(currentAnim.animFile.animBatchTag).TryGetFrame(currentFrameIndex, out frame) && frame.hasHead)
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x060047D0 RID: 18384 RVA: 0x0019B254 File Offset: 0x00199454
	public static void BeginTalking(SpeechMonitor.Instance smi)
	{
		smi.ev.clearHandle();
		if (smi.voiceEvent != null)
		{
			smi.ev = VoiceSoundEvent.PlayVoice(smi.voiceEvent, smi.GetComponent<KBatchedAnimController>(), 0f, false, false);
		}
		if (smi.ev.isValid())
		{
			smi.mouth.Play(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
		}
		else
		{
			smi.mouth.Play(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
		}
		SpeechMonitor.UpdateTalking(smi, 0f);
	}

	// Token: 0x060047D1 RID: 18385 RVA: 0x0019B375 File Offset: 0x00199575
	public static void EndTalking(SpeechMonitor.Instance smi)
	{
		smi.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, 3);
	}

	// Token: 0x060047D2 RID: 18386 RVA: 0x0019B38C File Offset: 0x0019958C
	public static KAnim.Anim.FrameElement GetFirstFrameElement(KBatchedAnimController controller)
	{
		KAnim.Anim.FrameElement result = default(KAnim.Anim.FrameElement);
		result.symbol = HashedString.Invalid;
		int currentFrameIndex = controller.GetCurrentFrameIndex();
		KAnimBatch batch = controller.GetBatch();
		if (currentFrameIndex == -1 || batch == null)
		{
			return result;
		}
		KAnim.Anim.Frame frame;
		if (!controller.GetBatch().group.data.TryGetFrame(currentFrameIndex, out frame))
		{
			return result;
		}
		for (int i = 0; i < frame.numElements; i++)
		{
			int num = frame.firstElementIdx + i;
			if (num < batch.group.data.frameElements.Count)
			{
				KAnim.Anim.FrameElement frameElement = batch.group.data.frameElements[num];
				if (!(frameElement.symbol == HashedString.Invalid))
				{
					result = frameElement;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060047D3 RID: 18387 RVA: 0x0019B450 File Offset: 0x00199650
	public static void UpdateTalking(SpeechMonitor.Instance smi, float dt)
	{
		if (smi.ev.isValid())
		{
			PLAYBACK_STATE playback_STATE;
			smi.ev.getPlaybackState(out playback_STATE);
			if (playback_STATE == PLAYBACK_STATE.STOPPING || playback_STATE == PLAYBACK_STATE.STOPPED)
			{
				smi.GoTo(smi.sm.satisfied);
				smi.ev.clearHandle();
				return;
			}
		}
		KAnim.Anim.FrameElement firstFrameElement = SpeechMonitor.GetFirstFrameElement(smi.mouth);
		if (firstFrameElement.symbol == HashedString.Invalid)
		{
			return;
		}
		smi.Get<SymbolOverrideController>().AddSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol), 3);
	}

	// Token: 0x04002EF9 RID: 12025
	public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State satisfied;

	// Token: 0x04002EFA RID: 12026
	public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State talking;

	// Token: 0x04002EFB RID: 12027
	public static string PREFIX_SAD = "sad";

	// Token: 0x04002EFC RID: 12028
	public static string PREFIX_HAPPY = "happy";

	// Token: 0x04002EFD RID: 12029
	public static string PREFIX_SINGER = "sing";

	// Token: 0x04002EFE RID: 12030
	public StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.TargetParameter mouth;

	// Token: 0x04002EFF RID: 12031
	private static HashedString HASH_SNAPTO_MOUTH = "snapto_mouth";

	// Token: 0x04002F00 RID: 12032
	private static HashedString GENERIC_CONVO_ANIM_NAME = new HashedString("anim_generic_convo_kanim");

	// Token: 0x02001984 RID: 6532
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001985 RID: 6533
	public class Tuning : TuningData<SpeechMonitor.Tuning>
	{
		// Token: 0x040079B1 RID: 31153
		public float randomSpeechIntervalMin;

		// Token: 0x040079B2 RID: 31154
		public float randomSpeechIntervalMax;

		// Token: 0x040079B3 RID: 31155
		public int speechCount;
	}

	// Token: 0x02001986 RID: 6534
	public new class Instance : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.GameInstance
	{
		// Token: 0x06009CF1 RID: 40177 RVA: 0x00373999 File Offset: 0x00371B99
		public Instance(IStateMachineTarget master, SpeechMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06009CF2 RID: 40178 RVA: 0x003739AE File Offset: 0x00371BAE
		public bool IsPlayingSpeech()
		{
			return base.IsInsideState(base.sm.talking);
		}

		// Token: 0x06009CF3 RID: 40179 RVA: 0x003739C1 File Offset: 0x00371BC1
		public void PlaySpeech(string speech_prefix, string voice_event)
		{
			this.speechPrefix = speech_prefix;
			this.voiceEvent = voice_event;
			this.GoTo(base.sm.talking);
		}

		// Token: 0x06009CF4 RID: 40180 RVA: 0x003739E4 File Offset: 0x00371BE4
		public void DrawMouth()
		{
			KAnim.Anim.FrameElement firstFrameElement = SpeechMonitor.GetFirstFrameElement(base.smi.mouth);
			if (firstFrameElement.symbol == HashedString.Invalid)
			{
				return;
			}
			KAnim.Build.Symbol symbol = base.smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol);
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			base.GetComponent<SymbolOverrideController>().AddSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, base.smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol), 3);
			KAnim.Build.Symbol symbol2 = KAnimBatchManager.Instance().GetBatchGroupData(component.batchGroupID).GetSymbol(SpeechMonitor.HASH_SNAPTO_MOUTH);
			KAnim.Build.SymbolFrameInstance symbolFrameInstance = KAnimBatchManager.Instance().GetBatchGroupData(symbol.build.batchTag).symbolFrameInstances[symbol.firstFrameIdx + firstFrameElement.frame];
			symbolFrameInstance.buildImageIdx = base.GetComponent<SymbolOverrideController>().GetAtlasIdx(symbol.build.GetTexture(0));
			component.SetSymbolOverride(symbol2.firstFrameIdx, ref symbolFrameInstance);
		}

		// Token: 0x06009CF5 RID: 40181 RVA: 0x00373AF8 File Offset: 0x00371CF8
		public void SetMouthId()
		{
			if (base.smi.Get<Accessorizer>().GetAccessory(Db.Get().AccessorySlots.Mouth).Id.Contains("006"))
			{
				base.smi.mouthId = "_006";
			}
		}

		// Token: 0x040079B4 RID: 31156
		public KBatchedAnimController mouth;

		// Token: 0x040079B5 RID: 31157
		public string speechPrefix = "happy";

		// Token: 0x040079B6 RID: 31158
		public string voiceEvent;

		// Token: 0x040079B7 RID: 31159
		public EventInstance ev;

		// Token: 0x040079B8 RID: 31160
		public string mouthId;
	}
}
