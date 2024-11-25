using System;
using UnityEngine;

// Token: 0x02000D9B RID: 3483
public class RocketRestrictionSideScreen : SideScreenContent
{
	// Token: 0x06006DE0 RID: 28128 RVA: 0x0029514F File Offset: 0x0029334F
	protected override void OnSpawn()
	{
		this.unrestrictedButton.onClick += this.ClickNone;
		this.spaceRestrictedButton.onClick += this.ClickSpace;
	}

	// Token: 0x06006DE1 RID: 28129 RVA: 0x0029517F File Offset: 0x0029337F
	public override int GetSideScreenSortOrder()
	{
		return 0;
	}

	// Token: 0x06006DE2 RID: 28130 RVA: 0x00295182 File Offset: 0x00293382
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<RocketControlStation.StatesInstance>() != null;
	}

	// Token: 0x06006DE3 RID: 28131 RVA: 0x0029518D File Offset: 0x0029338D
	public override void SetTarget(GameObject new_target)
	{
		this.controlStation = new_target.GetComponent<RocketControlStation>();
		this.controlStationLogicSubHandle = this.controlStation.Subscribe(1861523068, new Action<object>(this.UpdateButtonStates));
		this.UpdateButtonStates(null);
	}

	// Token: 0x06006DE4 RID: 28132 RVA: 0x002951C4 File Offset: 0x002933C4
	public override void ClearTarget()
	{
		if (this.controlStationLogicSubHandle != -1 && this.controlStation != null)
		{
			this.controlStation.Unsubscribe(this.controlStationLogicSubHandle);
			this.controlStationLogicSubHandle = -1;
		}
		this.controlStation = null;
	}

	// Token: 0x06006DE5 RID: 28133 RVA: 0x002951FC File Offset: 0x002933FC
	private void UpdateButtonStates(object data = null)
	{
		bool flag = this.controlStation.IsLogicInputConnected();
		if (!flag)
		{
			this.unrestrictedButton.isOn = !this.controlStation.RestrictWhenGrounded;
			this.spaceRestrictedButton.isOn = this.controlStation.RestrictWhenGrounded;
		}
		this.unrestrictedButton.gameObject.SetActive(!flag);
		this.spaceRestrictedButton.gameObject.SetActive(!flag);
		this.automationControlled.gameObject.SetActive(flag);
	}

	// Token: 0x06006DE6 RID: 28134 RVA: 0x00295280 File Offset: 0x00293480
	private void ClickNone()
	{
		this.controlStation.RestrictWhenGrounded = false;
		this.UpdateButtonStates(null);
	}

	// Token: 0x06006DE7 RID: 28135 RVA: 0x00295295 File Offset: 0x00293495
	private void ClickSpace()
	{
		this.controlStation.RestrictWhenGrounded = true;
		this.UpdateButtonStates(null);
	}

	// Token: 0x04004AFB RID: 19195
	private RocketControlStation controlStation;

	// Token: 0x04004AFC RID: 19196
	[Header("Buttons")]
	public KToggle unrestrictedButton;

	// Token: 0x04004AFD RID: 19197
	public KToggle spaceRestrictedButton;

	// Token: 0x04004AFE RID: 19198
	public GameObject automationControlled;

	// Token: 0x04004AFF RID: 19199
	private int controlStationLogicSubHandle = -1;
}
