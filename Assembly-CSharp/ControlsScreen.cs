using System;
using UnityEngine.UI;

// Token: 0x02000C1D RID: 3101
public class ControlsScreen : KScreen
{
	// Token: 0x06005F1E RID: 24350 RVA: 0x00235720 File Offset: 0x00233920
	protected override void OnPrefabInit()
	{
		BindingEntry[] bindingEntries = GameInputMapping.GetBindingEntries();
		string text = "";
		foreach (BindingEntry bindingEntry in bindingEntries)
		{
			text += bindingEntry.mAction.ToString();
			text += ": ";
			text += bindingEntry.mKeyCode.ToString();
			text += "\n";
		}
		this.controlLabel.text = text;
	}

	// Token: 0x06005F1F RID: 24351 RVA: 0x002357A5 File Offset: 0x002339A5
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Help) || e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
		}
	}

	// Token: 0x04003FF5 RID: 16373
	public Text controlLabel;
}
