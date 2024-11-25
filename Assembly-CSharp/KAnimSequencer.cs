using System;
using KSerialization;
using UnityEngine;

// Token: 0x020004E2 RID: 1250
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimSequencer")]
public class KAnimSequencer : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06001B71 RID: 7025 RVA: 0x0008FB09 File Offset: 0x0008DD09
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.kbac = base.GetComponent<KBatchedAnimController>();
		this.mb = base.GetComponent<MinionBrain>();
		if (this.autoRun)
		{
			this.PlaySequence();
		}
	}

	// Token: 0x06001B72 RID: 7026 RVA: 0x0008FB37 File Offset: 0x0008DD37
	public void Reset()
	{
		this.currentIndex = 0;
	}

	// Token: 0x06001B73 RID: 7027 RVA: 0x0008FB40 File Offset: 0x0008DD40
	public void PlaySequence()
	{
		if (this.sequence != null && this.sequence.Length != 0)
		{
			if (this.mb != null)
			{
				this.mb.Suspend("AnimSequencer");
			}
			this.kbac.onAnimComplete += this.PlayNext;
			this.PlayNext(null);
		}
	}

	// Token: 0x06001B74 RID: 7028 RVA: 0x0008FBA0 File Offset: 0x0008DDA0
	private void PlayNext(HashedString name)
	{
		if (this.sequence.Length > this.currentIndex)
		{
			this.kbac.Play(new HashedString(this.sequence[this.currentIndex].anim), this.sequence[this.currentIndex].mode, this.sequence[this.currentIndex].speed, 0f);
			this.currentIndex++;
			return;
		}
		this.kbac.onAnimComplete -= this.PlayNext;
		if (this.mb != null)
		{
			this.mb.Resume("AnimSequencer");
		}
	}

	// Token: 0x04000F84 RID: 3972
	[Serialize]
	public bool autoRun;

	// Token: 0x04000F85 RID: 3973
	[Serialize]
	public KAnimSequencer.KAnimSequence[] sequence = new KAnimSequencer.KAnimSequence[0];

	// Token: 0x04000F86 RID: 3974
	private int currentIndex;

	// Token: 0x04000F87 RID: 3975
	private KBatchedAnimController kbac;

	// Token: 0x04000F88 RID: 3976
	private MinionBrain mb;

	// Token: 0x020012BA RID: 4794
	[SerializationConfig(MemberSerialization.OptOut)]
	[Serializable]
	public class KAnimSequence
	{
		// Token: 0x04006425 RID: 25637
		public string anim;

		// Token: 0x04006426 RID: 25638
		public float speed = 1f;

		// Token: 0x04006427 RID: 25639
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}
}
