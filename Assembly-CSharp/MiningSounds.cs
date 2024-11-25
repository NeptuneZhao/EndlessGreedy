using System;
using FMODUnity;
using UnityEngine;

// Token: 0x0200058B RID: 1419
[AddComponentMenu("KMonoBehaviour/scripts/MiningSounds")]
public class MiningSounds : KMonoBehaviour
{
	// Token: 0x060020FC RID: 8444 RVA: 0x000B8B03 File Offset: 0x000B6D03
	protected override void OnPrefabInit()
	{
		base.Subscribe<MiningSounds>(-1762453998, MiningSounds.OnStartMiningSoundDelegate);
		base.Subscribe<MiningSounds>(939543986, MiningSounds.OnStopMiningSoundDelegate);
	}

	// Token: 0x060020FD RID: 8445 RVA: 0x000B8B28 File Offset: 0x000B6D28
	private void OnStartMiningSound(object data)
	{
		if (this.miningSound == null)
		{
			Element element = data as Element;
			if (element != null)
			{
				string text = element.substance.GetMiningSound();
				if (text == null || text == "")
				{
					return;
				}
				text = "Mine_" + text;
				string sound = GlobalAssets.GetSound(text, false);
				this.miningSoundEvent = RuntimeManager.PathToEventReference(sound);
				if (!this.miningSoundEvent.IsNull)
				{
					this.loopingSounds.StartSound(this.miningSoundEvent);
				}
			}
		}
	}

	// Token: 0x060020FE RID: 8446 RVA: 0x000B8BA9 File Offset: 0x000B6DA9
	private void OnStopMiningSound(object data)
	{
		if (!this.miningSoundEvent.IsNull)
		{
			this.loopingSounds.StopSound(this.miningSoundEvent);
			this.miningSound = null;
		}
	}

	// Token: 0x060020FF RID: 8447 RVA: 0x000B8BD0 File Offset: 0x000B6DD0
	public void SetPercentComplete(float progress)
	{
		if (!this.miningSoundEvent.IsNull)
		{
			this.loopingSounds.SetParameter(this.miningSoundEvent, MiningSounds.HASH_PERCENTCOMPLETE, progress);
		}
	}

	// Token: 0x0400127A RID: 4730
	private static HashedString HASH_PERCENTCOMPLETE = "percentComplete";

	// Token: 0x0400127B RID: 4731
	[MyCmpGet]
	private LoopingSounds loopingSounds;

	// Token: 0x0400127C RID: 4732
	private FMODAsset miningSound;

	// Token: 0x0400127D RID: 4733
	private EventReference miningSoundEvent;

	// Token: 0x0400127E RID: 4734
	private static readonly EventSystem.IntraObjectHandler<MiningSounds> OnStartMiningSoundDelegate = new EventSystem.IntraObjectHandler<MiningSounds>(delegate(MiningSounds component, object data)
	{
		component.OnStartMiningSound(data);
	});

	// Token: 0x0400127F RID: 4735
	private static readonly EventSystem.IntraObjectHandler<MiningSounds> OnStopMiningSoundDelegate = new EventSystem.IntraObjectHandler<MiningSounds>(delegate(MiningSounds component, object data)
	{
		component.OnStopMiningSound(data);
	});
}
