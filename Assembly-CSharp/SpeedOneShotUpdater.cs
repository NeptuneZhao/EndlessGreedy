using System;

// Token: 0x02000DCA RID: 3530
public class SpeedOneShotUpdater : OneShotSoundParameterUpdater
{
	// Token: 0x0600700D RID: 28685 RVA: 0x002A425D File Offset: 0x002A245D
	public SpeedOneShotUpdater() : base("Speed")
	{
	}

	// Token: 0x0600700E RID: 28686 RVA: 0x002A426F File Offset: 0x002A246F
	public override void Play(OneShotSoundParameterUpdater.Sound sound)
	{
		sound.ev.setParameterByID(sound.description.GetParameterId(base.parameter), SpeedLoopingSoundUpdater.GetSpeedParameterValue(), false);
	}
}
