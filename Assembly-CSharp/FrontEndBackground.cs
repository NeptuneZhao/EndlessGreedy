using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C49 RID: 3145
public class FrontEndBackground : UIDupeRandomizer
{
	// Token: 0x060060A0 RID: 24736 RVA: 0x0023F3F8 File Offset: 0x0023D5F8
	protected override void Start()
	{
		this.tuning = TuningData<FrontEndBackground.Tuning>.Get();
		base.Start();
		for (int i = 0; i < this.anims.Length; i++)
		{
			int minionIndex = i;
			KBatchedAnimController kbatchedAnimController = this.anims[i].minions[0];
			if (kbatchedAnimController.gameObject.activeInHierarchy)
			{
				kbatchedAnimController.onAnimComplete += delegate(HashedString name)
				{
					this.WaitForABit(minionIndex, name);
				};
				this.WaitForABit(i, HashedString.Invalid);
			}
		}
		this.dreckoController = base.transform.GetChild(0).Find("startmenu_drecko").GetComponent<KBatchedAnimController>();
		if (this.dreckoController.gameObject.activeInHierarchy)
		{
			this.dreckoController.enabled = false;
			this.nextDreckoTime = UnityEngine.Random.Range(this.tuning.minFirstDreckoInterval, this.tuning.maxFirstDreckoInterval) + Time.unscaledTime;
		}
	}

	// Token: 0x060060A1 RID: 24737 RVA: 0x0023F4E6 File Offset: 0x0023D6E6
	protected override void Update()
	{
		base.Update();
		this.UpdateDrecko();
	}

	// Token: 0x060060A2 RID: 24738 RVA: 0x0023F4F4 File Offset: 0x0023D6F4
	private void UpdateDrecko()
	{
		if (this.dreckoController.gameObject.activeInHierarchy && Time.unscaledTime > this.nextDreckoTime)
		{
			this.dreckoController.enabled = true;
			this.dreckoController.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
			this.nextDreckoTime = UnityEngine.Random.Range(this.tuning.minDreckoInterval, this.tuning.maxDreckoInterval) + Time.unscaledTime;
		}
	}

	// Token: 0x060060A3 RID: 24739 RVA: 0x0023F573 File Offset: 0x0023D773
	private void WaitForABit(int minion_idx, HashedString name)
	{
		base.StartCoroutine(this.WaitForTime(minion_idx));
	}

	// Token: 0x060060A4 RID: 24740 RVA: 0x0023F583 File Offset: 0x0023D783
	private IEnumerator WaitForTime(int minion_idx)
	{
		this.anims[minion_idx].lastWaitTime = UnityEngine.Random.Range(this.anims[minion_idx].minSecondsBetweenAction, this.anims[minion_idx].maxSecondsBetweenAction);
		yield return new WaitForSecondsRealtime(this.anims[minion_idx].lastWaitTime);
		base.GetNewBody(minion_idx);
		using (List<KBatchedAnimController>.Enumerator enumerator = this.anims[minion_idx].minions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KBatchedAnimController kbatchedAnimController = enumerator.Current;
				kbatchedAnimController.ClearQueue();
				kbatchedAnimController.Play(this.anims[minion_idx].anim_name, KAnim.PlayMode.Once, 1f, 0f);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x04004156 RID: 16726
	private KBatchedAnimController dreckoController;

	// Token: 0x04004157 RID: 16727
	private float nextDreckoTime;

	// Token: 0x04004158 RID: 16728
	private FrontEndBackground.Tuning tuning;

	// Token: 0x02001D32 RID: 7474
	public class Tuning : TuningData<FrontEndBackground.Tuning>
	{
		// Token: 0x04008651 RID: 34385
		public float minDreckoInterval;

		// Token: 0x04008652 RID: 34386
		public float maxDreckoInterval;

		// Token: 0x04008653 RID: 34387
		public float minFirstDreckoInterval;

		// Token: 0x04008654 RID: 34388
		public float maxFirstDreckoInterval;
	}
}
