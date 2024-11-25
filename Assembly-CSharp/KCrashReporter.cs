using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Klei;
using KMod;
using Newtonsoft.Json;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Token: 0x02000937 RID: 2359
public class KCrashReporter : MonoBehaviour
{
	// Token: 0x1400001C RID: 28
	// (add) Token: 0x0600447D RID: 17533 RVA: 0x00185C00 File Offset: 0x00183E00
	// (remove) Token: 0x0600447E RID: 17534 RVA: 0x00185C34 File Offset: 0x00183E34
	public static event Action<bool> onCrashReported;

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x0600447F RID: 17535 RVA: 0x00185C68 File Offset: 0x00183E68
	// (remove) Token: 0x06004480 RID: 17536 RVA: 0x00185C9C File Offset: 0x00183E9C
	public static event Action<float> onCrashUploadProgress;

	// Token: 0x170004EC RID: 1260
	// (get) Token: 0x06004481 RID: 17537 RVA: 0x00185CCF File Offset: 0x00183ECF
	// (set) Token: 0x06004482 RID: 17538 RVA: 0x00185CD6 File Offset: 0x00183ED6
	public static bool hasReportedError { get; private set; }

	// Token: 0x06004483 RID: 17539 RVA: 0x00185CE0 File Offset: 0x00183EE0
	private void OnEnable()
	{
		KCrashReporter.dataRoot = Application.dataPath;
		Application.logMessageReceived += this.HandleLog;
		KCrashReporter.ignoreAll = true;
		string path = Path.Combine(KCrashReporter.dataRoot, "hashes.json");
		if (File.Exists(path))
		{
			StringBuilder stringBuilder = new StringBuilder();
			MD5 md = MD5.Create();
			Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
			if (dictionary.Count > 0)
			{
				bool flag = true;
				foreach (KeyValuePair<string, string> keyValuePair in dictionary)
				{
					string key = keyValuePair.Key;
					string value = keyValuePair.Value;
					stringBuilder.Length = 0;
					using (FileStream fileStream = new FileStream(Path.Combine(KCrashReporter.dataRoot, key), FileMode.Open, FileAccess.Read))
					{
						foreach (byte b in md.ComputeHash(fileStream))
						{
							stringBuilder.AppendFormat("{0:x2}", b);
						}
						if (stringBuilder.ToString() != value)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					KCrashReporter.ignoreAll = false;
				}
			}
			else
			{
				KCrashReporter.ignoreAll = false;
			}
		}
		else
		{
			KCrashReporter.ignoreAll = false;
		}
		if (KCrashReporter.ignoreAll)
		{
			global::Debug.Log("Ignoring crash due to mismatched hashes.json entries.");
		}
		if (File.Exists("ignorekcrashreporter.txt"))
		{
			KCrashReporter.ignoreAll = true;
			global::Debug.Log("Ignoring crash due to ignorekcrashreporter.txt");
		}
		if (Application.isEditor && !GenericGameSettings.instance.enableEditorCrashReporting)
		{
			KCrashReporter.terminateOnError = false;
		}
	}

	// Token: 0x06004484 RID: 17540 RVA: 0x00185E88 File Offset: 0x00184088
	private void OnDisable()
	{
		Application.logMessageReceived -= this.HandleLog;
	}

	// Token: 0x06004485 RID: 17541 RVA: 0x00185E9C File Offset: 0x0018409C
	private void HandleLog(string msg, string stack_trace, LogType type)
	{
		if ((KCrashReporter.logCount += 1U) == 10000000U)
		{
			DebugUtil.DevLogError("Turning off logging to avoid increasing the file to an unreasonable size, please review the logs as they probably contain spam");
			global::Debug.DisableLogging();
		}
		if (KCrashReporter.ignoreAll)
		{
			return;
		}
		if (msg != null && msg.StartsWith(DebugUtil.START_CALLSTACK))
		{
			string text = msg;
			msg = text.Substring(text.IndexOf(DebugUtil.END_CALLSTACK, StringComparison.Ordinal) + DebugUtil.END_CALLSTACK.Length);
			stack_trace = text.Substring(DebugUtil.START_CALLSTACK.Length, text.IndexOf(DebugUtil.END_CALLSTACK, StringComparison.Ordinal) - DebugUtil.START_CALLSTACK.Length);
		}
		if (Array.IndexOf<string>(KCrashReporter.IgnoreStrings, msg) != -1)
		{
			return;
		}
		if (msg != null && msg.StartsWith("<RI.Hid>"))
		{
			return;
		}
		if (msg != null && msg.StartsWith("Failed to load cursor"))
		{
			return;
		}
		if (msg != null && msg.StartsWith("Failed to save a temporary cursor"))
		{
			return;
		}
		if (type == LogType.Exception)
		{
			RestartWarning.ShouldWarn = true;
		}
		if (this.errorScreen == null && (type == LogType.Exception || type == LogType.Error))
		{
			if (KCrashReporter.terminateOnError && KCrashReporter.hasCrash)
			{
				return;
			}
			if (SpeedControlScreen.Instance != null)
			{
				SpeedControlScreen.Instance.Pause(true, true);
			}
			string text2 = msg;
			string text3 = stack_trace;
			if (string.IsNullOrEmpty(text3))
			{
				text3 = new StackTrace(5, true).ToString();
			}
			if (App.isLoading)
			{
				if (!SceneInitializerLoader.deferred_error.IsValid)
				{
					SceneInitializerLoader.deferred_error = new SceneInitializerLoader.DeferredError
					{
						msg = text2,
						stack_trace = text3
					};
					return;
				}
			}
			else
			{
				this.ShowDialog(text2, text3);
			}
		}
	}

	// Token: 0x06004486 RID: 17542 RVA: 0x00186010 File Offset: 0x00184210
	public bool ShowDialog(string error, string stack_trace)
	{
		if (this.errorScreen != null)
		{
			return false;
		}
		GameObject gameObject = GameObject.Find(KCrashReporter.error_canvas_name);
		if (gameObject == null)
		{
			gameObject = new GameObject();
			gameObject.name = KCrashReporter.error_canvas_name;
			Canvas canvas = gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
			canvas.sortingOrder = 32767;
			gameObject.AddComponent<GraphicRaycaster>();
		}
		this.errorScreen = UnityEngine.Object.Instantiate<GameObject>(this.reportErrorPrefab, Vector3.zero, Quaternion.identity);
		this.errorScreen.transform.SetParent(gameObject.transform, false);
		ReportErrorDialog errorDialog = this.errorScreen.GetComponentInChildren<ReportErrorDialog>();
		string stackTrace = error + "\n\n" + stack_trace;
		KCrashReporter.hasCrash = true;
		if (Global.Instance != null && Global.Instance.modManager != null && Global.Instance.modManager.HasCrashableMods())
		{
			Exception ex = DebugUtil.RetrieveLastExceptionLogged();
			StackTrace stackTrace2 = (ex != null) ? new StackTrace(ex) : new StackTrace(5, true);
			Global.Instance.modManager.SearchForModsInStackTrace(stackTrace2);
			Global.Instance.modManager.SearchForModsInStackTrace(stack_trace);
			errorDialog.PopupDisableModsDialog(stackTrace, new System.Action(this.OnQuitToDesktop), (Global.Instance.modManager.IsInDevMode() || !KCrashReporter.terminateOnError) ? new System.Action(this.OnCloseErrorDialog) : null);
		}
		else
		{
			errorDialog.PopupSubmitErrorDialog(stackTrace, delegate
			{
				KCrashReporter.ReportError(error, stack_trace, this.confirmDialogPrefab, this.errorScreen, errorDialog.UserMessage(), true, null, null);
			}, new System.Action(this.OnQuitToDesktop), KCrashReporter.terminateOnError ? null : new System.Action(this.OnCloseErrorDialog));
		}
		return true;
	}

	// Token: 0x06004487 RID: 17543 RVA: 0x001861E1 File Offset: 0x001843E1
	private void OnCloseErrorDialog()
	{
		UnityEngine.Object.Destroy(this.errorScreen);
		this.errorScreen = null;
		KCrashReporter.hasCrash = false;
		if (SpeedControlScreen.Instance != null)
		{
			SpeedControlScreen.Instance.Unpause(true);
		}
	}

	// Token: 0x06004488 RID: 17544 RVA: 0x00186213 File Offset: 0x00184413
	private void OnQuitToDesktop()
	{
		App.Quit();
	}

	// Token: 0x06004489 RID: 17545 RVA: 0x0018621C File Offset: 0x0018441C
	private static string GetUserID()
	{
		if (DistributionPlatform.Initialized)
		{
			string[] array = new string[5];
			array[0] = DistributionPlatform.Inst.Name;
			array[1] = "ID_";
			array[2] = DistributionPlatform.Inst.LocalUser.Name;
			array[3] = "_";
			int num = 4;
			DistributionPlatform.UserId id = DistributionPlatform.Inst.LocalUser.Id;
			array[num] = ((id != null) ? id.ToString() : null);
			return string.Concat(array);
		}
		return "LocalUser_" + Environment.UserName;
	}

	// Token: 0x0600448A RID: 17546 RVA: 0x00186298 File Offset: 0x00184498
	private static string GetLogContents()
	{
		string path = Util.LogFilePath();
		if (File.Exists(path))
		{
			using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using (StreamReader streamReader = new StreamReader(fileStream))
				{
					return streamReader.ReadToEnd();
				}
			}
		}
		return "";
	}

	// Token: 0x0600448B RID: 17547 RVA: 0x00186304 File Offset: 0x00184504
	public static void ReportDevNotification(string notification_name, string stack_trace, string details = "", bool includeSaveFile = false, string[] extraCategories = null)
	{
		if (KCrashReporter.previouslyReportedDevNotifications == null)
		{
			KCrashReporter.previouslyReportedDevNotifications = new HashSet<int>();
		}
		details = notification_name + " - " + details;
		global::Debug.Log(details);
		int hashValue = new HashedString(notification_name).HashValue;
		bool hasReportedError = KCrashReporter.hasReportedError;
		if (!KCrashReporter.previouslyReportedDevNotifications.Contains(hashValue))
		{
			KCrashReporter.previouslyReportedDevNotifications.Add(hashValue);
			if (extraCategories != null)
			{
				Array.Resize<string>(ref extraCategories, extraCategories.Length + 1);
				extraCategories[extraCategories.Length - 1] = KCrashReporter.CRASH_CATEGORY.DEVNOTIFICATION;
			}
			else
			{
				extraCategories = new string[]
				{
					KCrashReporter.CRASH_CATEGORY.DEVNOTIFICATION
				};
			}
			KCrashReporter.ReportError("DevNotification: " + notification_name, stack_trace, null, null, details, includeSaveFile, extraCategories, null);
		}
		KCrashReporter.hasReportedError = hasReportedError;
	}

	// Token: 0x0600448C RID: 17548 RVA: 0x001863B4 File Offset: 0x001845B4
	public static void ReportError(string msg, string stack_trace, ConfirmDialogScreen confirm_prefab, GameObject confirm_parent, string userMessage = "", bool includeSaveFile = true, string[] extraCategories = null, string[] extraFiles = null)
	{
		if (KPrivacyPrefs.instance.disableDataCollection)
		{
			return;
		}
		if (KCrashReporter.ignoreAll)
		{
			return;
		}
		global::Debug.Log("Reporting error.\n");
		if (msg != null)
		{
			global::Debug.Log(msg);
		}
		if (stack_trace != null)
		{
			global::Debug.Log(stack_trace);
		}
		KCrashReporter.hasReportedError = true;
		if (string.IsNullOrEmpty(msg))
		{
			msg = "No message";
		}
		Match match = KCrashReporter.failedToLoadModuleRegEx.Match(msg);
		if (match.Success)
		{
			string path = match.Groups[1].ToString();
			string text = match.Groups[2].ToString();
			string fileName = Path.GetFileName(path);
			msg = string.Concat(new string[]
			{
				"Failed to load '",
				fileName,
				"' with error '",
				text,
				"'."
			});
		}
		if (string.IsNullOrEmpty(stack_trace))
		{
			string buildText = BuildWatermark.GetBuildText();
			stack_trace = string.Format("No stack trace {0}\n\n{1}", buildText, msg);
		}
		List<string> list = new List<string>();
		if (KCrashReporter.debugWasUsed)
		{
			list.Add("(Debug Used)");
		}
		if (KCrashReporter.haveActiveMods)
		{
			list.Add("(Mods Active)");
		}
		list.Add(msg);
		string[] array = new string[]
		{
			"Debug:LogError",
			"UnityEngine.Debug",
			"Output:LogError",
			"DebugUtil:Assert",
			"System.Array",
			"System.Collections",
			"KCrashReporter.Assert",
			"No stack trace."
		};
		foreach (string text2 in stack_trace.Split('\n', StringSplitOptions.None))
		{
			if (list.Count >= 5)
			{
				break;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				bool flag = false;
				foreach (string value in array)
				{
					if (text2.StartsWith(value))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(text2);
				}
			}
		}
		if (userMessage == UI.CRASHSCREEN.BODY.text || userMessage.IsNullOrWhiteSpace())
		{
			userMessage = "";
		}
		else
		{
			userMessage = "[" + BuildWatermark.GetBuildText() + "] " + userMessage;
		}
		userMessage = userMessage.Replace(stack_trace, "");
		KCrashReporter.Error error = new KCrashReporter.Error();
		if (extraCategories != null)
		{
			error.categories.AddRange(extraCategories);
		}
		error.callstack = stack_trace;
		if (KCrashReporter.disableDeduping)
		{
			error.callstack = error.callstack + "\n" + Guid.NewGuid().ToString();
		}
		error.fullstack = string.Format("{0}\n\n{1}", msg, stack_trace);
		error.summaryline = string.Join("\n", list.ToArray());
		error.userMessage = userMessage;
		List<string> list2 = new List<string>();
		if (includeSaveFile && KCrashReporter.MOST_RECENT_SAVEFILE != null)
		{
			list2.Add(KCrashReporter.MOST_RECENT_SAVEFILE);
			error.saveFilename = Path.GetFileName(KCrashReporter.MOST_RECENT_SAVEFILE);
		}
		if (extraFiles != null)
		{
			foreach (string text3 in extraFiles)
			{
				list2.Add(text3);
				error.extraFilenames.Add(Path.GetFileName(text3));
			}
		}
		string jsonString = JsonConvert.SerializeObject(error);
		byte[] archiveData = KCrashReporter.CreateArchiveZip(KCrashReporter.GetLogContents(), list2);
		System.Action successCallback = delegate()
		{
			if (confirm_prefab != null && confirm_parent != null)
			{
				((ConfirmDialogScreen)KScreenManager.Instance.StartScreen(confirm_prefab.gameObject, confirm_parent)).PopupConfirmDialog(UI.CRASHSCREEN.REPORTEDERROR_SUCCESS, null, null, null, null, null, null, null, null);
			}
		};
		Action<long> failureCallback = delegate(long errorCode)
		{
			if (confirm_prefab != null && confirm_parent != null)
			{
				string text4 = (errorCode == 413L) ? UI.CRASHSCREEN.REPORTEDERROR_FAILURE_TOO_LARGE : UI.CRASHSCREEN.REPORTEDERROR_FAILURE;
				((ConfirmDialogScreen)KScreenManager.Instance.StartScreen(confirm_prefab.gameObject, confirm_parent)).PopupConfirmDialog(text4, null, null, null, null, null, null, null, null);
			}
		};
		KCrashReporter.pendingCrash = new KCrashReporter.PendingCrash
		{
			jsonString = jsonString,
			archiveData = archiveData,
			successCallback = successCallback,
			failureCallback = failureCallback
		};
	}

	// Token: 0x0600448D RID: 17549 RVA: 0x0018672F File Offset: 0x0018492F
	private static IEnumerator SubmitCrashAsync(string jsonString, byte[] archiveData, System.Action successCallback, Action<long> failureCallback)
	{
		bool success = false;
		Uri uri = new Uri("https://games-feedback.klei.com/submit");
		List<IMultipartFormSection> list = new List<IMultipartFormSection>
		{
			new MultipartFormDataSection("metadata", jsonString),
			new MultipartFormFileSection("archiveFile", archiveData, "Archive.zip", "application/octet-stream")
		};
		if (KleiAccount.KleiToken != null)
		{
			list.Add(new MultipartFormDataSection("loginToken", KleiAccount.KleiToken));
		}
		using (UnityWebRequest w = UnityWebRequest.Post(uri, list))
		{
			w.SendWebRequest();
			while (!w.isDone)
			{
				yield return null;
				if (KCrashReporter.onCrashUploadProgress != null)
				{
					KCrashReporter.onCrashUploadProgress(w.uploadProgress);
				}
			}
			if (w.result == UnityWebRequest.Result.Success)
			{
				UnityEngine.Debug.Log("Submitted crash!");
				if (successCallback != null)
				{
					successCallback();
				}
				success = true;
			}
			else
			{
				UnityEngine.Debug.Log("CrashReporter: Could not submit crash " + w.result.ToString());
				if (failureCallback != null)
				{
					failureCallback(w.responseCode);
				}
			}
		}
		UnityWebRequest w = null;
		if (KCrashReporter.onCrashReported != null)
		{
			KCrashReporter.onCrashReported(success);
		}
		yield break;
		yield break;
	}

	// Token: 0x0600448E RID: 17550 RVA: 0x00186754 File Offset: 0x00184954
	public static void ReportBug(string msg, GameObject confirmParent)
	{
		string stack_trace = "Bug Report From: " + KCrashReporter.GetUserID() + " at " + System.DateTime.Now.ToString();
		KCrashReporter.ReportError(msg, stack_trace, ScreenPrefabs.Instance.ConfirmDialogScreen, confirmParent, "", true, null, null);
	}

	// Token: 0x0600448F RID: 17551 RVA: 0x001867A0 File Offset: 0x001849A0
	public static void Assert(bool condition, string message, string[] extraCategories = null)
	{
		if (!condition && !KCrashReporter.hasReportedError)
		{
			StackTrace stackTrace = new StackTrace(1, true);
			KCrashReporter.ReportError("ASSERT: " + message, stackTrace.ToString(), null, null, null, true, extraCategories, null);
		}
	}

	// Token: 0x06004490 RID: 17552 RVA: 0x001867DC File Offset: 0x001849DC
	public static void ReportSimDLLCrash(string msg, string stack_trace, string dmp_filename)
	{
		if (KCrashReporter.hasReportedError)
		{
			return;
		}
		KCrashReporter.ReportError(msg, stack_trace, null, null, "", true, new string[]
		{
			KCrashReporter.CRASH_CATEGORY.SIM
		}, new string[]
		{
			dmp_filename
		});
	}

	// Token: 0x06004491 RID: 17553 RVA: 0x00186818 File Offset: 0x00184A18
	private static byte[] CreateArchiveZip(string log, List<string> files)
	{
		byte[] result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
			{
				if (files != null)
				{
					foreach (string text in files)
					{
						try
						{
							if (!File.Exists(text))
							{
								UnityEngine.Debug.Log("CrashReporter: file does not exist to include: " + text);
							}
							else
							{
								using (Stream stream = zipArchive.CreateEntry(Path.GetFileName(text), System.IO.Compression.CompressionLevel.Fastest).Open())
								{
									byte[] array = File.ReadAllBytes(text);
									stream.Write(array, 0, array.Length);
								}
							}
						}
						catch (Exception ex)
						{
							string str = "CrashReporter: Could not add file '";
							string str2 = text;
							string str3 = "' to archive: ";
							Exception ex2 = ex;
							UnityEngine.Debug.Log(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
						}
					}
					using (Stream stream2 = zipArchive.CreateEntry("Player.log", System.IO.Compression.CompressionLevel.Fastest).Open())
					{
						byte[] bytes = Encoding.UTF8.GetBytes(log);
						stream2.Write(bytes, 0, bytes.Length);
					}
				}
			}
			result = memoryStream.ToArray();
		}
		return result;
	}

	// Token: 0x06004492 RID: 17554 RVA: 0x001869D8 File Offset: 0x00184BD8
	private void Update()
	{
		if (KCrashReporter.pendingCrash != null)
		{
			KCrashReporter.PendingCrash pendingCrash = KCrashReporter.pendingCrash;
			KCrashReporter.pendingCrash = null;
			global::Debug.Log("Submitting crash...");
			base.StartCoroutine(KCrashReporter.SubmitCrashAsync(pendingCrash.jsonString, pendingCrash.archiveData, pendingCrash.successCallback, pendingCrash.failureCallback));
		}
	}

	// Token: 0x04002CCE RID: 11470
	public static string MOST_RECENT_SAVEFILE = null;

	// Token: 0x04002CCF RID: 11471
	public const string CRASH_REPORTER_SERVER = "https://games-feedback.klei.com";

	// Token: 0x04002CD0 RID: 11472
	public const uint MAX_LOGS = 10000000U;

	// Token: 0x04002CD3 RID: 11475
	public static bool ignoreAll = false;

	// Token: 0x04002CD4 RID: 11476
	public static bool debugWasUsed = false;

	// Token: 0x04002CD5 RID: 11477
	public static bool haveActiveMods = false;

	// Token: 0x04002CD6 RID: 11478
	public static uint logCount = 0U;

	// Token: 0x04002CD7 RID: 11479
	public static string error_canvas_name = "ErrorCanvas";

	// Token: 0x04002CD8 RID: 11480
	public static bool disableDeduping = false;

	// Token: 0x04002CDA RID: 11482
	public static bool hasCrash = false;

	// Token: 0x04002CDB RID: 11483
	private static readonly Regex failedToLoadModuleRegEx = new Regex("^Failed to load '(.*?)' with error (.*)", RegexOptions.Multiline);

	// Token: 0x04002CDC RID: 11484
	[SerializeField]
	private LoadScreen loadScreenPrefab;

	// Token: 0x04002CDD RID: 11485
	[SerializeField]
	private GameObject reportErrorPrefab;

	// Token: 0x04002CDE RID: 11486
	[SerializeField]
	private ConfirmDialogScreen confirmDialogPrefab;

	// Token: 0x04002CDF RID: 11487
	private GameObject errorScreen;

	// Token: 0x04002CE0 RID: 11488
	public static bool terminateOnError = true;

	// Token: 0x04002CE1 RID: 11489
	private static string dataRoot;

	// Token: 0x04002CE2 RID: 11490
	private static readonly string[] IgnoreStrings = new string[]
	{
		"Releasing render texture whose render buffer is set as Camera's target buffer with Camera.SetTargetBuffers!",
		"The profiler has run out of samples for this frame. This frame will be skipped. Increase the sample limit using Profiler.maxNumberOfSamplesPerFrame",
		"Trying to add Text (LocText) for graphic rebuild while we are already inside a graphic rebuild loop. This is not supported.",
		"Texture has out of range width / height",
		"<I> Failed to get cursor position:\r\nSuccess.\r\n"
	};

	// Token: 0x04002CE3 RID: 11491
	private static HashSet<int> previouslyReportedDevNotifications;

	// Token: 0x04002CE4 RID: 11492
	private static KCrashReporter.PendingCrash pendingCrash;

	// Token: 0x02001895 RID: 6293
	public class CRASH_CATEGORY
	{
		// Token: 0x04007691 RID: 30353
		public static string DEVNOTIFICATION = "DevNotification";

		// Token: 0x04007692 RID: 30354
		public static string VANILLA = "Vanilla";

		// Token: 0x04007693 RID: 30355
		public static string SPACEDOUT = "SpacedOut";

		// Token: 0x04007694 RID: 30356
		public static string MODDED = "Modded";

		// Token: 0x04007695 RID: 30357
		public static string DEBUGUSED = "DebugUsed";

		// Token: 0x04007696 RID: 30358
		public static string SANDBOX = "Sandbox";

		// Token: 0x04007697 RID: 30359
		public static string STEAMDECK = "SteamDeck";

		// Token: 0x04007698 RID: 30360
		public static string SIM = "SimDll";

		// Token: 0x04007699 RID: 30361
		public static string FILEIO = "FileIO";

		// Token: 0x0400769A RID: 30362
		public static string MODSYSTEM = "ModSystem";

		// Token: 0x0400769B RID: 30363
		public static string WORLDGENFAILURE = "WorldgenFailure";
	}

	// Token: 0x02001896 RID: 6294
	private class Error
	{
		// Token: 0x060098F8 RID: 39160 RVA: 0x00369588 File Offset: 0x00367788
		public Error()
		{
			this.userName = KCrashReporter.GetUserID();
			this.platform = Util.GetOperatingSystem();
			this.InitDefaultCategories();
			this.InitSku();
			this.InitSlackSummary();
			if (DistributionPlatform.Inst.Initialized)
			{
				string a;
				bool flag = !SteamApps.GetCurrentBetaName(out a, 100);
				this.branch = a;
				if (a == "public_playtest")
				{
					this.branch = "public_testing";
				}
				if (flag || (a == "public_testing" && !UnityEngine.Debug.isDebugBuild))
				{
					this.branch = "default";
				}
			}
		}

		// Token: 0x060098F9 RID: 39161 RVA: 0x003696D4 File Offset: 0x003678D4
		private void InitDefaultCategories()
		{
			if (DlcManager.IsPureVanilla())
			{
				this.categories.Add(KCrashReporter.CRASH_CATEGORY.VANILLA);
			}
			if (DlcManager.IsExpansion1Active())
			{
				this.categories.Add(KCrashReporter.CRASH_CATEGORY.SPACEDOUT);
			}
			foreach (string text in DlcManager.GetActiveDLCIds())
			{
				if (!(text == "EXPANSION1_ID"))
				{
					this.categories.Add(text);
				}
			}
			if (KCrashReporter.debugWasUsed)
			{
				this.categories.Add(KCrashReporter.CRASH_CATEGORY.DEBUGUSED);
			}
			if (KCrashReporter.haveActiveMods)
			{
				this.categories.Add(KCrashReporter.CRASH_CATEGORY.MODDED);
			}
			if (SaveGame.Instance != null && SaveGame.Instance.sandboxEnabled)
			{
				this.categories.Add(KCrashReporter.CRASH_CATEGORY.SANDBOX);
			}
			if (DistributionPlatform.Inst.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
			{
				this.categories.Add(KCrashReporter.CRASH_CATEGORY.STEAMDECK);
			}
		}

		// Token: 0x060098FA RID: 39162 RVA: 0x003697E0 File Offset: 0x003679E0
		private void InitSku()
		{
			this.sku = "steam";
			if (DistributionPlatform.Inst.Initialized)
			{
				string a;
				bool flag = !SteamApps.GetCurrentBetaName(out a, 100);
				if (a == "public_testing" || a == "preview" || a == "public_playtest" || a == "playtest")
				{
					if (UnityEngine.Debug.isDebugBuild)
					{
						this.sku = "steam-public-testing";
					}
					else
					{
						this.sku = "steam-release";
					}
				}
				if (flag || a == "release")
				{
					this.sku = "steam-release";
				}
			}
		}

		// Token: 0x060098FB RID: 39163 RVA: 0x00369880 File Offset: 0x00367A80
		private void InitSlackSummary()
		{
			string buildText = BuildWatermark.GetBuildText();
			string text = (GameClock.Instance != null) ? string.Format(" - Cycle {0}", GameClock.Instance.GetCycle() + 1) : "";
			int num;
			if (!(Global.Instance != null) || Global.Instance.modManager == null)
			{
				num = 0;
			}
			else
			{
				num = Global.Instance.modManager.mods.Count((Mod x) => x.IsEnabledForActiveDlc());
			}
			int num2 = num;
			string text2 = (num2 > 0) ? string.Format(" - {0} active mods", num2) : "";
			this.slackSummary = string.Concat(new string[]
			{
				buildText,
				" ",
				this.platform,
				text,
				text2
			});
		}

		// Token: 0x0400769C RID: 30364
		public string game = "ONI";

		// Token: 0x0400769D RID: 30365
		public string userName;

		// Token: 0x0400769E RID: 30366
		public string platform;

		// Token: 0x0400769F RID: 30367
		public string version = LaunchInitializer.BuildPrefix();

		// Token: 0x040076A0 RID: 30368
		public string branch = "default";

		// Token: 0x040076A1 RID: 30369
		public string sku = "";

		// Token: 0x040076A2 RID: 30370
		public int build = 642695;

		// Token: 0x040076A3 RID: 30371
		public string callstack = "";

		// Token: 0x040076A4 RID: 30372
		public string fullstack = "";

		// Token: 0x040076A5 RID: 30373
		public string summaryline = "";

		// Token: 0x040076A6 RID: 30374
		public string userMessage = "";

		// Token: 0x040076A7 RID: 30375
		public List<string> categories = new List<string>();

		// Token: 0x040076A8 RID: 30376
		public string slackSummary;

		// Token: 0x040076A9 RID: 30377
		public string logFilename = "Player.log";

		// Token: 0x040076AA RID: 30378
		public string saveFilename = "";

		// Token: 0x040076AB RID: 30379
		public string screenshotFilename = "";

		// Token: 0x040076AC RID: 30380
		public List<string> extraFilenames = new List<string>();

		// Token: 0x040076AD RID: 30381
		public string title = "";

		// Token: 0x040076AE RID: 30382
		public bool isServer;

		// Token: 0x040076AF RID: 30383
		public bool isDedicated;

		// Token: 0x040076B0 RID: 30384
		public bool isError = true;

		// Token: 0x040076B1 RID: 30385
		public string emote = "";
	}

	// Token: 0x02001897 RID: 6295
	public class PendingCrash
	{
		// Token: 0x040076B2 RID: 30386
		public string jsonString;

		// Token: 0x040076B3 RID: 30387
		public byte[] archiveData;

		// Token: 0x040076B4 RID: 30388
		public System.Action successCallback;

		// Token: 0x040076B5 RID: 30389
		public Action<long> failureCallback;
	}
}
