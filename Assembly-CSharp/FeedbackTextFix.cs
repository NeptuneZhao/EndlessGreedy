using System;
using Steamworks;
using UnityEngine;

// Token: 0x02000C47 RID: 3143
public class FeedbackTextFix : MonoBehaviour
{
	// Token: 0x0600609A RID: 24730 RVA: 0x0023F338 File Offset: 0x0023D538
	private void Awake()
	{
		if (!DistributionPlatform.Initialized || !SteamUtils.IsSteamRunningOnSteamDeck())
		{
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		this.locText.key = this.newKey;
	}

	// Token: 0x04004151 RID: 16721
	public string newKey;

	// Token: 0x04004152 RID: 16722
	public LocText locText;
}
