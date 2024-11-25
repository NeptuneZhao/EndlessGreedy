using System;

// Token: 0x02000505 RID: 1285
internal abstract class UserVolumeOneShotUpdater : OneShotSoundParameterUpdater
{
	// Token: 0x06001CAD RID: 7341 RVA: 0x00096F2C File Offset: 0x0009512C
	public UserVolumeOneShotUpdater(string parameter, string player_pref) : base(parameter)
	{
		this.playerPref = player_pref;
	}

	// Token: 0x06001CAE RID: 7342 RVA: 0x00096F44 File Offset: 0x00095144
	public override void Play(OneShotSoundParameterUpdater.Sound sound)
	{
		if (!string.IsNullOrEmpty(this.playerPref))
		{
			float @float = KPlayerPrefs.GetFloat(this.playerPref);
			sound.ev.setParameterByID(sound.description.GetParameterId(base.parameter), @float, false);
		}
	}

	// Token: 0x0400102E RID: 4142
	private string playerPref;
}
