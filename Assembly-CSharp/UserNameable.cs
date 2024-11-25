using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000B41 RID: 2881
[AddComponentMenu("KMonoBehaviour/scripts/UserNameable")]
public class UserNameable : KMonoBehaviour
{
	// Token: 0x06005613 RID: 22035 RVA: 0x001EC935 File Offset: 0x001EAB35
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (string.IsNullOrEmpty(this.savedName))
		{
			this.SetName(base.gameObject.GetProperName());
			return;
		}
		this.SetName(this.savedName);
	}

	// Token: 0x06005614 RID: 22036 RVA: 0x001EC968 File Offset: 0x001EAB68
	public void SetName(string name)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		base.name = name;
		if (component != null)
		{
			component.SetName(name);
		}
		base.gameObject.name = name;
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
		if (base.GetComponent<CommandModule>() != null)
		{
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(base.GetComponent<LaunchConditionManager>()).SetRocketName(name);
		}
		else if (base.GetComponent<Clustercraft>() != null)
		{
			ClusterNameDisplayScreen.Instance.UpdateName(base.GetComponent<Clustercraft>());
		}
		this.savedName = name;
		base.Trigger(1102426921, name);
	}

	// Token: 0x04003862 RID: 14434
	[Serialize]
	public string savedName = "";
}
