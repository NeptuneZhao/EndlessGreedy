using System;
using UnityEngine;

// Token: 0x02000433 RID: 1075
public static class ChoreHelpers
{
	// Token: 0x060016E3 RID: 5859 RVA: 0x0007B991 File Offset: 0x00079B91
	public static GameObject CreateLocator(string name, Vector3 pos)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(ApproachableLocator.ID), null, null);
		gameObject.name = name;
		gameObject.transform.SetPosition(pos);
		gameObject.gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x060016E4 RID: 5860 RVA: 0x0007B9C9 File Offset: 0x00079BC9
	public static GameObject CreateSleepLocator(Vector3 pos)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(SleepLocator.ID), null, null);
		gameObject.name = "SLeepLocator";
		gameObject.transform.SetPosition(pos);
		gameObject.gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x060016E5 RID: 5861 RVA: 0x0007BA05 File Offset: 0x00079C05
	public static void DestroyLocator(GameObject locator)
	{
		if (locator != null)
		{
			locator.gameObject.DeleteObject();
		}
	}
}
