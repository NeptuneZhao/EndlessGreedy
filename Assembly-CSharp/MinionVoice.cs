using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000E RID: 14
public readonly struct MinionVoice
{
	// Token: 0x06000038 RID: 56 RVA: 0x000038D0 File Offset: 0x00001AD0
	public MinionVoice(int voiceIndex)
	{
		this.voiceIndex = voiceIndex;
		this.voiceId = (voiceIndex + 1).ToString("D2");
		this.isValid = true;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00003901 File Offset: 0x00001B01
	public static MinionVoice ByPersonality(Personality personality)
	{
		return MinionVoice.ByPersonality(personality.Id);
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00003910 File Offset: 0x00001B10
	public static MinionVoice ByPersonality(string personalityId)
	{
		if (personalityId == "JORGE")
		{
			return new MinionVoice(-2);
		}
		if (personalityId == "MEEP")
		{
			return new MinionVoice(2);
		}
		MinionVoice minionVoice;
		if (!MinionVoice.personalityVoiceMap.TryGetValue(personalityId, out minionVoice))
		{
			minionVoice = MinionVoice.Random();
			MinionVoice.personalityVoiceMap.Add(personalityId, minionVoice);
		}
		return minionVoice;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00003968 File Offset: 0x00001B68
	public static MinionVoice Random()
	{
		return new MinionVoice(UnityEngine.Random.Range(0, 4));
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00003978 File Offset: 0x00001B78
	public static Option<MinionVoice> ByObject(UnityEngine.Object unityObject)
	{
		GameObject gameObject = unityObject as GameObject;
		GameObject gameObject2;
		if (gameObject != null)
		{
			gameObject2 = gameObject;
		}
		else
		{
			Component component = unityObject as Component;
			if (component != null)
			{
				gameObject2 = component.gameObject;
			}
			else
			{
				gameObject2 = null;
			}
		}
		if (gameObject2.IsNullOrDestroyed())
		{
			return Option.None;
		}
		MinionVoiceProviderMB componentInParent = gameObject2.GetComponentInParent<MinionVoiceProviderMB>();
		if (componentInParent.IsNullOrDestroyed())
		{
			return Option.None;
		}
		return componentInParent.voice;
	}

	// Token: 0x0600003D RID: 61 RVA: 0x000039DC File Offset: 0x00001BDC
	public string GetSoundAssetName(string localName)
	{
		global::Debug.Assert(this.isValid);
		string d = localName;
		if (localName.Contains(":"))
		{
			d = localName.Split(':', StringSplitOptions.None)[0];
		}
		return StringFormatter.Combine("DupVoc_", this.voiceId, "_", d);
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00003A25 File Offset: 0x00001C25
	public string GetSoundPath(string localName)
	{
		return GlobalAssets.GetSound(this.GetSoundAssetName(localName), true);
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00003A34 File Offset: 0x00001C34
	public void PlaySoundUI(string localName)
	{
		global::Debug.Assert(this.isValid);
		string soundPath = this.GetSoundPath(localName);
		try
		{
			if (SoundListenerController.Instance == null)
			{
				KFMOD.PlayUISound(soundPath);
			}
			else
			{
				KFMOD.PlayOneShot(soundPath, SoundListenerController.Instance.transform.GetPosition(), 1f);
			}
		}
		catch
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"AUDIOERROR: Missing [" + soundPath + "]"
			});
		}
	}

	// Token: 0x04000039 RID: 57
	public readonly int voiceIndex;

	// Token: 0x0400003A RID: 58
	public readonly string voiceId;

	// Token: 0x0400003B RID: 59
	public readonly bool isValid;

	// Token: 0x0400003C RID: 60
	private static Dictionary<string, MinionVoice> personalityVoiceMap = new Dictionary<string, MinionVoice>();
}
