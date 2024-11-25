using System;
using UnityEngine;

// Token: 0x020004D5 RID: 1237
[Serializable]
public class AnimEvent
{
	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06001AAE RID: 6830 RVA: 0x0008C5CF File Offset: 0x0008A7CF
	// (set) Token: 0x06001AAF RID: 6831 RVA: 0x0008C5D7 File Offset: 0x0008A7D7
	[SerializeField]
	public string name { get; private set; }

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x06001AB0 RID: 6832 RVA: 0x0008C5E0 File Offset: 0x0008A7E0
	// (set) Token: 0x06001AB1 RID: 6833 RVA: 0x0008C5E8 File Offset: 0x0008A7E8
	[SerializeField]
	public string file { get; private set; }

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x06001AB2 RID: 6834 RVA: 0x0008C5F1 File Offset: 0x0008A7F1
	// (set) Token: 0x06001AB3 RID: 6835 RVA: 0x0008C5F9 File Offset: 0x0008A7F9
	[SerializeField]
	public int frame { get; private set; }

	// Token: 0x06001AB4 RID: 6836 RVA: 0x0008C602 File Offset: 0x0008A802
	public AnimEvent()
	{
	}

	// Token: 0x06001AB5 RID: 6837 RVA: 0x0008C60C File Offset: 0x0008A80C
	public AnimEvent(string file, string name, int frame)
	{
		this.file = ((file == "") ? null : file);
		if (this.file != null)
		{
			this.fileHash = new KAnimHashedString(this.file);
		}
		this.name = name;
		this.frame = frame;
	}

	// Token: 0x06001AB6 RID: 6838 RVA: 0x0008C660 File Offset: 0x0008A860
	public void Play(AnimEventManager.EventPlayerData behaviour)
	{
		if (this.IsFilteredOut(behaviour))
		{
			return;
		}
		if (behaviour.previousFrame < behaviour.currentFrame)
		{
			if (behaviour.previousFrame < this.frame && behaviour.currentFrame >= this.frame)
			{
				this.OnPlay(behaviour);
				return;
			}
		}
		else if (behaviour.previousFrame > behaviour.currentFrame && (behaviour.previousFrame < this.frame || this.frame <= behaviour.currentFrame))
		{
			this.OnPlay(behaviour);
		}
	}

	// Token: 0x06001AB7 RID: 6839 RVA: 0x0008C6E2 File Offset: 0x0008A8E2
	private void DebugAnimEvent(string ev_name, AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x06001AB8 RID: 6840 RVA: 0x0008C6E4 File Offset: 0x0008A8E4
	public virtual void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x06001AB9 RID: 6841 RVA: 0x0008C6E6 File Offset: 0x0008A8E6
	public virtual void OnUpdate(AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x0008C6E8 File Offset: 0x0008A8E8
	public virtual void Stop(AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x06001ABB RID: 6843 RVA: 0x0008C6EA File Offset: 0x0008A8EA
	protected bool IsFilteredOut(AnimEventManager.EventPlayerData behaviour)
	{
		return this.file != null && !behaviour.controller.HasAnimationFile(this.fileHash);
	}

	// Token: 0x04000F22 RID: 3874
	[SerializeField]
	private KAnimHashedString fileHash;

	// Token: 0x04000F24 RID: 3876
	public bool OnExit;
}
