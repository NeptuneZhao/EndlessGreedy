using System;

// Token: 0x020006DE RID: 1758
public class GeoTunerSwitchGeyserWorkable : Workable
{
	// Token: 0x06002CA3 RID: 11427 RVA: 0x000FACC8 File Offset: 0x000F8EC8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
	}

	// Token: 0x06002CA4 RID: 11428 RVA: 0x000FACFC File Offset: 0x000F8EFC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(3f);
	}

	// Token: 0x040019C5 RID: 6597
	private const string animName = "anim_use_remote_kanim";
}
