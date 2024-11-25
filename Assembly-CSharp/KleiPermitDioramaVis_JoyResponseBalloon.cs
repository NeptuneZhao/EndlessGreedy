using System;
using System.Linq;
using Database;
using UnityEngine;

// Token: 0x02000C92 RID: 3218
public class KleiPermitDioramaVis_JoyResponseBalloon : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060062D4 RID: 25300 RVA: 0x0024DA15 File Offset: 0x0024BC15
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060062D5 RID: 25301 RVA: 0x0024DA20 File Offset: 0x0024BC20
	public void ConfigureSetup()
	{
		this.minionUI.transform.localScale = Vector3.one * 0.7f;
		this.minionUI.transform.localPosition = new Vector3(this.minionUI.transform.localPosition.x - 73f, this.minionUI.transform.localPosition.y - 152f + 8f, this.minionUI.transform.localPosition.z);
	}

	// Token: 0x060062D6 RID: 25302 RVA: 0x0024DAB2 File Offset: 0x0024BCB2
	public void ConfigureWith(PermitResource permit)
	{
		this.ConfigureWith(Option.Some<BalloonArtistFacadeResource>((BalloonArtistFacadeResource)permit));
	}

	// Token: 0x060062D7 RID: 25303 RVA: 0x0024DAC8 File Offset: 0x0024BCC8
	public void ConfigureWith(Option<BalloonArtistFacadeResource> permit)
	{
		KleiPermitDioramaVis_JoyResponseBalloon.<>c__DisplayClass10_0 CS$<>8__locals1 = new KleiPermitDioramaVis_JoyResponseBalloon.<>c__DisplayClass10_0();
		CS$<>8__locals1.permit = permit;
		KBatchedAnimController component = this.minionUI.SpawnedAvatar.GetComponent<KBatchedAnimController>();
		CS$<>8__locals1.minionSymbolOverrider = this.minionUI.SpawnedAvatar.GetComponent<SymbolOverrideController>();
		this.minionUI.SetMinion(this.specificPersonality.UnwrapOrElse(() => (from p in Db.Get().Personalities.GetAll(true, true)
		where p.joyTrait == "BalloonArtist"
		select p).GetRandom<Personality>(), null));
		if (!this.didAddAnims)
		{
			this.didAddAnims = true;
			component.AddAnimOverrides(Assets.GetAnim("anim_interacts_balloon_artist_kanim"), 0f);
		}
		component.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
		CS$<>8__locals1.<ConfigureWith>g__DisplayNextBalloon|3();
		Updater[] array = new Updater[2];
		array[0] = Updater.WaitForSeconds(1.3f);
		int num = 1;
		Func<Updater>[] array2 = new Func<Updater>[2];
		array2[0] = (() => Updater.WaitForSeconds(1.618f));
		array2[1] = (() => Updater.Do(new System.Action(base.<ConfigureWith>g__DisplayNextBalloon|3)));
		array[num] = Updater.Loop(array2);
		this.QueueUpdater(Updater.Series(array));
	}

	// Token: 0x060062D8 RID: 25304 RVA: 0x0024DC09 File Offset: 0x0024BE09
	public void SetMinion(Personality personality)
	{
		this.specificPersonality = personality;
		if (base.gameObject.activeInHierarchy)
		{
			this.minionUI.SetMinion(personality);
		}
	}

	// Token: 0x060062D9 RID: 25305 RVA: 0x0024DC30 File Offset: 0x0024BE30
	private void QueueUpdater(Updater updater)
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.RunUpdater(updater);
			return;
		}
		this.updaterToRunOnStart = updater;
	}

	// Token: 0x060062DA RID: 25306 RVA: 0x0024DC53 File Offset: 0x0024BE53
	private void RunUpdater(Updater updater)
	{
		if (this.updaterRoutine != null)
		{
			base.StopCoroutine(this.updaterRoutine);
			this.updaterRoutine = null;
		}
		this.updaterRoutine = base.StartCoroutine(updater);
	}

	// Token: 0x060062DB RID: 25307 RVA: 0x0024DC82 File Offset: 0x0024BE82
	private void OnEnable()
	{
		if (this.updaterToRunOnStart.IsSome())
		{
			this.RunUpdater(this.updaterToRunOnStart.Unwrap());
			this.updaterToRunOnStart = Option.None;
		}
	}

	// Token: 0x0400430E RID: 17166
	private const int FRAMES_TO_MAKE_BALLOON_IN_ANIM = 39;

	// Token: 0x0400430F RID: 17167
	private const float SECONDS_TO_MAKE_BALLOON_IN_ANIM = 1.3f;

	// Token: 0x04004310 RID: 17168
	private const float SECONDS_BETWEEN_BALLOONS = 1.618f;

	// Token: 0x04004311 RID: 17169
	[SerializeField]
	private UIMinion minionUI;

	// Token: 0x04004312 RID: 17170
	private bool didAddAnims;

	// Token: 0x04004313 RID: 17171
	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	// Token: 0x04004314 RID: 17172
	private const int TARGET_OVERRIDE_PRIORITY = 0;

	// Token: 0x04004315 RID: 17173
	private Option<Personality> specificPersonality;

	// Token: 0x04004316 RID: 17174
	private Option<PermitResource> lastConfiguredPermit;

	// Token: 0x04004317 RID: 17175
	private Option<Updater> updaterToRunOnStart;

	// Token: 0x04004318 RID: 17176
	private Coroutine updaterRoutine;
}
