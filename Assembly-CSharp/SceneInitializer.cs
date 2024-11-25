using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000A7B RID: 2683
public class SceneInitializer : MonoBehaviour
{
	// Token: 0x170005AD RID: 1453
	// (get) Token: 0x06004E98 RID: 20120 RVA: 0x001C4B14 File Offset: 0x001C2D14
	// (set) Token: 0x06004E99 RID: 20121 RVA: 0x001C4B1B File Offset: 0x001C2D1B
	public static SceneInitializer Instance { get; private set; }

	// Token: 0x06004E9A RID: 20122 RVA: 0x001C4B24 File Offset: 0x001C2D24
	private void Awake()
	{
		Localization.SwapToLocalizedFont();
		string environmentVariable = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
		string text = Application.dataPath + Path.DirectorySeparatorChar.ToString() + "Plugins";
		if (!environmentVariable.Contains(text))
		{
			Environment.SetEnvironmentVariable("PATH", environmentVariable + Path.PathSeparator.ToString() + text, EnvironmentVariableTarget.Process);
		}
		SceneInitializer.Instance = this;
		this.PreLoadPrefabs();
	}

	// Token: 0x06004E9B RID: 20123 RVA: 0x001C4B93 File Offset: 0x001C2D93
	private void OnDestroy()
	{
		SceneInitializer.Instance = null;
	}

	// Token: 0x06004E9C RID: 20124 RVA: 0x001C4B9C File Offset: 0x001C2D9C
	private void PreLoadPrefabs()
	{
		foreach (GameObject gameObject in this.preloadPrefabs)
		{
			if (gameObject != null)
			{
				Util.KInstantiate(gameObject, gameObject.transform.GetPosition(), Quaternion.identity, base.gameObject, null, true, 0);
			}
		}
	}

	// Token: 0x06004E9D RID: 20125 RVA: 0x001C4C14 File Offset: 0x001C2E14
	public void NewSaveGamePrefab()
	{
		if (this.prefab_NewSaveGame != null && SaveGame.Instance == null)
		{
			Util.KInstantiate(this.prefab_NewSaveGame, base.gameObject, null);
		}
	}

	// Token: 0x06004E9E RID: 20126 RVA: 0x001C4C44 File Offset: 0x001C2E44
	public void PostLoadPrefabs()
	{
		foreach (GameObject gameObject in this.prefabs)
		{
			if (gameObject != null)
			{
				Util.KInstantiate(gameObject, base.gameObject, null);
			}
		}
	}

	// Token: 0x04003437 RID: 13367
	public const int MAXDEPTH = -30000;

	// Token: 0x04003438 RID: 13368
	public const int SCREENDEPTH = -1000;

	// Token: 0x0400343A RID: 13370
	public GameObject prefab_NewSaveGame;

	// Token: 0x0400343B RID: 13371
	public List<GameObject> preloadPrefabs = new List<GameObject>();

	// Token: 0x0400343C RID: 13372
	public List<GameObject> prefabs = new List<GameObject>();
}
