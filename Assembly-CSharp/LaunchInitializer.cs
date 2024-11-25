using System;
using System.IO;
using System.Threading;
using UnityEngine;

// Token: 0x0200093A RID: 2362
public class LaunchInitializer : MonoBehaviour
{
	// Token: 0x060044AD RID: 17581 RVA: 0x00186BF6 File Offset: 0x00184DF6
	public static string BuildPrefix()
	{
		return LaunchInitializer.BUILD_PREFIX;
	}

	// Token: 0x060044AE RID: 17582 RVA: 0x00186BFD File Offset: 0x00184DFD
	public static int UpdateNumber()
	{
		return 53;
	}

	// Token: 0x060044AF RID: 17583 RVA: 0x00186C04 File Offset: 0x00184E04
	private void Update()
	{
		if (this.numWaitFrames > Time.renderedFrameCount)
		{
			return;
		}
		if (!DistributionPlatform.Initialized)
		{
			if (!SystemInfo.SupportsTextureFormat(TextureFormat.RGBAFloat))
			{
				global::Debug.LogError("Machine does not support RGBAFloat32");
			}
			GraphicsOptionsScreen.SetSettingsFromPrefs();
			Util.ApplyInvariantCultureToThread(Thread.CurrentThread);
			global::Debug.Log("Date: " + System.DateTime.Now.ToString());
			global::Debug.Log("Build: " + BuildWatermark.GetBuildText() + " (release)");
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			KPlayerPrefs.instance.Load();
			DistributionPlatform.Initialize();
		}
		if (!DistributionPlatform.Inst.IsDLCStatusReady())
		{
			return;
		}
		global::Debug.Log("DistributionPlatform initialized.");
		DebugUtil.LogArgs(new object[]
		{
			DebugUtil.LINE
		});
		global::Debug.Log("Build: " + BuildWatermark.GetBuildText() + " (release)");
		DebugUtil.LogArgs(new object[]
		{
			DebugUtil.LINE
		});
		DebugUtil.LogArgs(new object[]
		{
			"DLC Information"
		});
		foreach (string text in DlcManager.GetOwnedDLCIds())
		{
			global::Debug.Log(string.Format("- {0} loaded: {1}", text, DlcManager.IsContentSubscribed(text)));
		}
		DebugUtil.LogArgs(new object[]
		{
			DebugUtil.LINE
		});
		KFMOD.Initialize();
		for (int i = 0; i < this.SpawnPrefabs.Length; i++)
		{
			GameObject gameObject = this.SpawnPrefabs[i];
			if (gameObject != null)
			{
				Util.KInstantiate(gameObject, base.gameObject, null);
			}
		}
		LaunchInitializer.DeleteLingeringFiles();
		base.enabled = false;
	}

	// Token: 0x060044B0 RID: 17584 RVA: 0x00186DB4 File Offset: 0x00184FB4
	private static void DeleteLingeringFiles()
	{
		string[] array = new string[]
		{
			"fmod.log",
			"load_stats_0.json",
			"OxygenNotIncluded_Data/output_log.txt"
		};
		string directoryName = Path.GetDirectoryName(Application.dataPath);
		foreach (string path in array)
		{
			string path2 = Path.Combine(directoryName, path);
			try
			{
				if (File.Exists(path2))
				{
					File.Delete(path2);
				}
			}
			catch (Exception obj)
			{
				global::Debug.LogWarning(obj);
			}
		}
	}

	// Token: 0x04002CE7 RID: 11495
	private const string PREFIX = "U";

	// Token: 0x04002CE8 RID: 11496
	private const int UPDATE_NUMBER = 53;

	// Token: 0x04002CE9 RID: 11497
	private static readonly string BUILD_PREFIX = "U" + 53.ToString();

	// Token: 0x04002CEA RID: 11498
	public GameObject[] SpawnPrefabs;

	// Token: 0x04002CEB RID: 11499
	[SerializeField]
	private int numWaitFrames = 1;
}
