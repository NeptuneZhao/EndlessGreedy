using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C81 RID: 3201
public static class KleiItemsStatusRefresher
{
	// Token: 0x0600627D RID: 25213 RVA: 0x0024C36C File Offset: 0x0024A56C
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void Initialize()
	{
		KleiItems.AddInventoryRefreshCallback(new KleiItems.InventoryRefreshCallback(KleiItemsStatusRefresher.OnRefreshResponseFromServer));
	}

	// Token: 0x0600627E RID: 25214 RVA: 0x0024C380 File Offset: 0x0024A580
	private static void OnRefreshResponseFromServer()
	{
		foreach (KleiItemsStatusRefresher.UIListener uilistener in KleiItemsStatusRefresher.listeners)
		{
			uilistener.Internal_RefreshUI();
		}
	}

	// Token: 0x0600627F RID: 25215 RVA: 0x0024C3D0 File Offset: 0x0024A5D0
	public static void Refresh()
	{
		foreach (KleiItemsStatusRefresher.UIListener uilistener in KleiItemsStatusRefresher.listeners)
		{
			uilistener.Internal_RefreshUI();
		}
	}

	// Token: 0x06006280 RID: 25216 RVA: 0x0024C420 File Offset: 0x0024A620
	public static KleiItemsStatusRefresher.UIListener AddOrGetListener(Component component)
	{
		return KleiItemsStatusRefresher.AddOrGetListener(component.gameObject);
	}

	// Token: 0x06006281 RID: 25217 RVA: 0x0024C42D File Offset: 0x0024A62D
	public static KleiItemsStatusRefresher.UIListener AddOrGetListener(GameObject onGameObject)
	{
		return onGameObject.AddOrGet<KleiItemsStatusRefresher.UIListener>();
	}

	// Token: 0x040042D8 RID: 17112
	public static HashSet<KleiItemsStatusRefresher.UIListener> listeners = new HashSet<KleiItemsStatusRefresher.UIListener>();

	// Token: 0x02001D6F RID: 7535
	public class UIListener : MonoBehaviour
	{
		// Token: 0x0600A8A2 RID: 43170 RVA: 0x0039DC50 File Offset: 0x0039BE50
		public void Internal_RefreshUI()
		{
			if (this.refreshUIFn != null)
			{
				this.refreshUIFn();
			}
		}

		// Token: 0x0600A8A3 RID: 43171 RVA: 0x0039DC65 File Offset: 0x0039BE65
		public void OnRefreshUI(System.Action fn)
		{
			this.refreshUIFn = fn;
		}

		// Token: 0x0600A8A4 RID: 43172 RVA: 0x0039DC6E File Offset: 0x0039BE6E
		private void OnEnable()
		{
			KleiItemsStatusRefresher.listeners.Add(this);
		}

		// Token: 0x0600A8A5 RID: 43173 RVA: 0x0039DC7C File Offset: 0x0039BE7C
		private void OnDisable()
		{
			KleiItemsStatusRefresher.listeners.Remove(this);
		}

		// Token: 0x04008758 RID: 34648
		private System.Action refreshUIFn;
	}
}
