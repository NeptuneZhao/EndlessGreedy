using System;
using UnityEngine;

// Token: 0x02000642 RID: 1602
[AddComponentMenu("KMonoBehaviour/scripts/Achievements")]
public class Achievements : KMonoBehaviour
{
	// Token: 0x0600273A RID: 10042 RVA: 0x000DF6D5 File Offset: 0x000DD8D5
	public void Unlock(string id)
	{
		if (SteamAchievementService.Instance)
		{
			SteamAchievementService.Instance.Unlock(id);
		}
	}
}
