using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000CC7 RID: 3271
public class GenericMessage : Message
{
	// Token: 0x0600652C RID: 25900 RVA: 0x0025D8F2 File Offset: 0x0025BAF2
	public GenericMessage(string _title, string _body, string _tooltip, KMonoBehaviour click_focus = null)
	{
		this.title = _title;
		this.body = _body;
		this.tooltip = _tooltip;
		this.clickFocus.Set(click_focus);
	}

	// Token: 0x0600652D RID: 25901 RVA: 0x0025D927 File Offset: 0x0025BB27
	public GenericMessage()
	{
	}

	// Token: 0x0600652E RID: 25902 RVA: 0x0025D93A File Offset: 0x0025BB3A
	public override string GetSound()
	{
		return null;
	}

	// Token: 0x0600652F RID: 25903 RVA: 0x0025D93D File Offset: 0x0025BB3D
	public override string GetMessageBody()
	{
		return this.body;
	}

	// Token: 0x06006530 RID: 25904 RVA: 0x0025D945 File Offset: 0x0025BB45
	public override string GetTooltip()
	{
		return this.tooltip;
	}

	// Token: 0x06006531 RID: 25905 RVA: 0x0025D94D File Offset: 0x0025BB4D
	public override string GetTitle()
	{
		return this.title;
	}

	// Token: 0x06006532 RID: 25906 RVA: 0x0025D958 File Offset: 0x0025BB58
	public override void OnClick()
	{
		KMonoBehaviour kmonoBehaviour = this.clickFocus.Get();
		if (kmonoBehaviour == null)
		{
			return;
		}
		Transform transform = kmonoBehaviour.transform;
		if (transform == null)
		{
			return;
		}
		Vector3 position = transform.GetPosition();
		position.z = -40f;
		CameraController.Instance.SetTargetPos(position, 8f, true);
		if (transform.GetComponent<KSelectable>() != null)
		{
			SelectTool.Instance.Select(transform.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x04004477 RID: 17527
	[Serialize]
	private string title;

	// Token: 0x04004478 RID: 17528
	[Serialize]
	private string tooltip;

	// Token: 0x04004479 RID: 17529
	[Serialize]
	private string body;

	// Token: 0x0400447A RID: 17530
	[Serialize]
	private Ref<KMonoBehaviour> clickFocus = new Ref<KMonoBehaviour>();
}
