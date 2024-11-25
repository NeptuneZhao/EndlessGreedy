using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200052E RID: 1326
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Automatable")]
public class Automatable : KMonoBehaviour
{
	// Token: 0x06001DD6 RID: 7638 RVA: 0x000A5785 File Offset: 0x000A3985
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Automatable>(-905833192, Automatable.OnCopySettingsDelegate);
	}

	// Token: 0x06001DD7 RID: 7639 RVA: 0x000A57A0 File Offset: 0x000A39A0
	private void OnCopySettings(object data)
	{
		Automatable component = ((GameObject)data).GetComponent<Automatable>();
		if (component != null)
		{
			this.automationOnly = component.automationOnly;
		}
	}

	// Token: 0x06001DD8 RID: 7640 RVA: 0x000A57CE File Offset: 0x000A39CE
	public bool GetAutomationOnly()
	{
		return this.automationOnly;
	}

	// Token: 0x06001DD9 RID: 7641 RVA: 0x000A57D6 File Offset: 0x000A39D6
	public void SetAutomationOnly(bool only)
	{
		this.automationOnly = only;
	}

	// Token: 0x06001DDA RID: 7642 RVA: 0x000A57DF File Offset: 0x000A39DF
	public bool AllowedByAutomation(bool is_transfer_arm)
	{
		return !this.GetAutomationOnly() || is_transfer_arm;
	}

	// Token: 0x040010C4 RID: 4292
	[Serialize]
	private bool automationOnly = true;

	// Token: 0x040010C5 RID: 4293
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040010C6 RID: 4294
	private static readonly EventSystem.IntraObjectHandler<Automatable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Automatable>(delegate(Automatable component, object data)
	{
		component.OnCopySettings(data);
	});
}
