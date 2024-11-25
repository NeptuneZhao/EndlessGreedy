using System;
using UnityEngine;

// Token: 0x02000A7C RID: 2684
public class SceneInitializerLoader : MonoBehaviour
{
	// Token: 0x06004EA0 RID: 20128 RVA: 0x001C4CC8 File Offset: 0x001C2EC8
	private void Awake()
	{
		Camera[] array = UnityEngine.Object.FindObjectsOfType<Camera>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		KMonoBehaviour.isLoadingScene = false;
		Singleton<StateMachineManager>.Instance.Clear();
		Util.KInstantiate(this.sceneInitializer, null, null);
		if (SceneInitializerLoader.ReportDeferredError != null && SceneInitializerLoader.deferred_error.IsValid)
		{
			SceneInitializerLoader.ReportDeferredError(SceneInitializerLoader.deferred_error);
			SceneInitializerLoader.deferred_error = default(SceneInitializerLoader.DeferredError);
		}
	}

	// Token: 0x0400343D RID: 13373
	public SceneInitializer sceneInitializer;

	// Token: 0x0400343E RID: 13374
	public static SceneInitializerLoader.DeferredError deferred_error;

	// Token: 0x0400343F RID: 13375
	public static SceneInitializerLoader.DeferredErrorDelegate ReportDeferredError;

	// Token: 0x02001AAF RID: 6831
	public struct DeferredError
	{
		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x0600A0E1 RID: 41185 RVA: 0x003813A2 File Offset: 0x0037F5A2
		public bool IsValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.msg);
			}
		}

		// Token: 0x04007D55 RID: 32085
		public string msg;

		// Token: 0x04007D56 RID: 32086
		public string stack_trace;
	}

	// Token: 0x02001AB0 RID: 6832
	// (Invoke) Token: 0x0600A0E3 RID: 41187
	public delegate void DeferredErrorDelegate(SceneInitializerLoader.DeferredError deferred_error);
}
