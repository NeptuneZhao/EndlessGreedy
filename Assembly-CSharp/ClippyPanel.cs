using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DF2 RID: 3570
public class ClippyPanel : KScreen
{
	// Token: 0x06007159 RID: 29017 RVA: 0x002ADFFC File Offset: 0x002AC1FC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600715A RID: 29018 RVA: 0x002AE004 File Offset: 0x002AC204
	protected override void OnActivate()
	{
		base.OnActivate();
		SpeedControlScreen.Instance.Pause(true, false);
		Game.Instance.Trigger(1634669191, null);
	}

	// Token: 0x0600715B RID: 29019 RVA: 0x002AE028 File Offset: 0x002AC228
	public void OnOk()
	{
		SpeedControlScreen.Instance.Unpause(true);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04004E05 RID: 19973
	public Text title;

	// Token: 0x04004E06 RID: 19974
	public Text detailText;

	// Token: 0x04004E07 RID: 19975
	public Text flavorText;

	// Token: 0x04004E08 RID: 19976
	public Image topicIcon;

	// Token: 0x04004E09 RID: 19977
	private KButton okButton;
}
