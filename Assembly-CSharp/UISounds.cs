using System;
using UnityEngine;

// Token: 0x02000B3B RID: 2875
[AddComponentMenu("KMonoBehaviour/scripts/UISounds")]
public class UISounds : KMonoBehaviour
{
	// Token: 0x17000667 RID: 1639
	// (get) Token: 0x060055CC RID: 21964 RVA: 0x001EA47A File Offset: 0x001E867A
	// (set) Token: 0x060055CD RID: 21965 RVA: 0x001EA481 File Offset: 0x001E8681
	public static UISounds Instance { get; private set; }

	// Token: 0x060055CE RID: 21966 RVA: 0x001EA489 File Offset: 0x001E8689
	public static void DestroyInstance()
	{
		UISounds.Instance = null;
	}

	// Token: 0x060055CF RID: 21967 RVA: 0x001EA491 File Offset: 0x001E8691
	protected override void OnPrefabInit()
	{
		UISounds.Instance = this;
	}

	// Token: 0x060055D0 RID: 21968 RVA: 0x001EA499 File Offset: 0x001E8699
	public static void PlaySound(UISounds.Sound sound)
	{
		UISounds.Instance.PlaySoundInternal(sound);
	}

	// Token: 0x060055D1 RID: 21969 RVA: 0x001EA4A8 File Offset: 0x001E86A8
	private void PlaySoundInternal(UISounds.Sound sound)
	{
		for (int i = 0; i < this.soundData.Length; i++)
		{
			if (this.soundData[i].sound == sound)
			{
				if (this.logSounds)
				{
					DebugUtil.LogArgs(new object[]
					{
						"Play sound",
						this.soundData[i].name
					});
				}
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.soundData[i].name, false));
			}
		}
	}

	// Token: 0x04003837 RID: 14391
	[SerializeField]
	private bool logSounds;

	// Token: 0x04003838 RID: 14392
	[SerializeField]
	private UISounds.SoundData[] soundData;

	// Token: 0x02001B90 RID: 7056
	public enum Sound
	{
		// Token: 0x0400800E RID: 32782
		NegativeNotification,
		// Token: 0x0400800F RID: 32783
		PositiveNotification,
		// Token: 0x04008010 RID: 32784
		Select,
		// Token: 0x04008011 RID: 32785
		Negative,
		// Token: 0x04008012 RID: 32786
		Back,
		// Token: 0x04008013 RID: 32787
		ClickObject,
		// Token: 0x04008014 RID: 32788
		HUD_Mouseover,
		// Token: 0x04008015 RID: 32789
		Object_Mouseover,
		// Token: 0x04008016 RID: 32790
		ClickHUD,
		// Token: 0x04008017 RID: 32791
		Object_AutoSelected
	}

	// Token: 0x02001B91 RID: 7057
	[Serializable]
	private struct SoundData
	{
		// Token: 0x04008018 RID: 32792
		public string name;

		// Token: 0x04008019 RID: 32793
		public UISounds.Sound sound;
	}
}
