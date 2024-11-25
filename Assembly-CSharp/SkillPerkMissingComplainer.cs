using System;
using UnityEngine;

// Token: 0x020005BB RID: 1467
[AddComponentMenu("KMonoBehaviour/scripts/SkillPerkMissingComplainer")]
public class SkillPerkMissingComplainer : KMonoBehaviour
{
	// Token: 0x060022FF RID: 8959 RVA: 0x000C331B File Offset: 0x000C151B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			this.skillUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x06002300 RID: 8960 RVA: 0x000C3359 File Offset: 0x000C1559
	protected override void OnCleanUp()
	{
		if (this.skillUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillUpdateHandle);
		}
		base.OnCleanUp();
	}

	// Token: 0x06002301 RID: 8961 RVA: 0x000C337C File Offset: 0x000C157C
	protected virtual void UpdateStatusItem(object data = null)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			return;
		}
		bool flag = MinionResume.AnyMinionHasPerk(this.requiredSkillPerk, this.GetMyWorldId());
		if (!flag && this.workStatusItemHandle == Guid.Empty)
		{
			this.workStatusItemHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.ColonyLacksRequiredSkillPerk, this.requiredSkillPerk);
			return;
		}
		if (flag && this.workStatusItemHandle != Guid.Empty)
		{
			component.RemoveStatusItem(this.workStatusItemHandle, false);
			this.workStatusItemHandle = Guid.Empty;
		}
	}

	// Token: 0x040013DC RID: 5084
	public string requiredSkillPerk;

	// Token: 0x040013DD RID: 5085
	private int skillUpdateHandle = -1;

	// Token: 0x040013DE RID: 5086
	private Guid workStatusItemHandle;
}
