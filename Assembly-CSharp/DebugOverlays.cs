using System;

// Token: 0x02000B8F RID: 2959
public class DebugOverlays : KScreen
{
	// Token: 0x170006BB RID: 1723
	// (get) Token: 0x06005956 RID: 22870 RVA: 0x002051A7 File Offset: 0x002033A7
	// (set) Token: 0x06005957 RID: 22871 RVA: 0x002051AE File Offset: 0x002033AE
	public static DebugOverlays instance { get; private set; }

	// Token: 0x06005958 RID: 22872 RVA: 0x002051B8 File Offset: 0x002033B8
	protected override void OnPrefabInit()
	{
		DebugOverlays.instance = this;
		KPopupMenu componentInChildren = base.GetComponentInChildren<KPopupMenu>();
		componentInChildren.SetOptions(new string[]
		{
			"None",
			"Rooms",
			"Lighting",
			"Style",
			"Flow"
		});
		KPopupMenu kpopupMenu = componentInChildren;
		kpopupMenu.OnSelect = (Action<string, int>)Delegate.Combine(kpopupMenu.OnSelect, new Action<string, int>(this.OnSelect));
		base.gameObject.SetActive(false);
	}

	// Token: 0x06005959 RID: 22873 RVA: 0x00205234 File Offset: 0x00203434
	private void OnSelect(string str, int index)
	{
		if (str == "None")
		{
			SimDebugView.Instance.SetMode(OverlayModes.None.ID);
			return;
		}
		if (str == "Flow")
		{
			SimDebugView.Instance.SetMode(SimDebugView.OverlayModes.Flow);
			return;
		}
		if (str == "Lighting")
		{
			SimDebugView.Instance.SetMode(OverlayModes.Light.ID);
			return;
		}
		if (!(str == "Rooms"))
		{
			Debug.LogError("Unknown debug view: " + str);
			return;
		}
		SimDebugView.Instance.SetMode(OverlayModes.Rooms.ID);
	}
}
