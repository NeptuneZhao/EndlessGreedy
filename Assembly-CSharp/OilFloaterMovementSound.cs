using System;

// Token: 0x020000AA RID: 170
internal class OilFloaterMovementSound : KMonoBehaviour
{
	// Token: 0x0600030F RID: 783 RVA: 0x00018910 File Offset: 0x00016B10
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.sound = GlobalAssets.GetSound(this.sound, false);
		base.Subscribe<OilFloaterMovementSound>(1027377649, OilFloaterMovementSound.OnObjectMovementStateChangedDelegate);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged), "OilFloaterMovementSound");
	}

	// Token: 0x06000310 RID: 784 RVA: 0x00018968 File Offset: 0x00016B68
	private void OnObjectMovementStateChanged(object data)
	{
		GameHashes gameHashes = (GameHashes)data;
		this.isMoving = (gameHashes == GameHashes.ObjectMovementWakeUp);
		this.UpdateSound();
	}

	// Token: 0x06000311 RID: 785 RVA: 0x00018990 File Offset: 0x00016B90
	private void OnCellChanged()
	{
		this.UpdateSound();
	}

	// Token: 0x06000312 RID: 786 RVA: 0x00018998 File Offset: 0x00016B98
	private void UpdateSound()
	{
		bool flag = this.isMoving && base.GetComponent<Navigator>().CurrentNavType != NavType.Swim;
		if (flag == this.isPlayingSound)
		{
			return;
		}
		LoopingSounds component = base.GetComponent<LoopingSounds>();
		if (flag)
		{
			component.StartSound(this.sound);
		}
		else
		{
			component.StopSound(this.sound);
		}
		this.isPlayingSound = flag;
	}

	// Token: 0x06000313 RID: 787 RVA: 0x000189F8 File Offset: 0x00016BF8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged));
	}

	// Token: 0x0400021E RID: 542
	public string sound;

	// Token: 0x0400021F RID: 543
	public bool isPlayingSound;

	// Token: 0x04000220 RID: 544
	public bool isMoving;

	// Token: 0x04000221 RID: 545
	private static readonly EventSystem.IntraObjectHandler<OilFloaterMovementSound> OnObjectMovementStateChangedDelegate = new EventSystem.IntraObjectHandler<OilFloaterMovementSound>(delegate(OilFloaterMovementSound component, object data)
	{
		component.OnObjectMovementStateChanged(data);
	});
}
