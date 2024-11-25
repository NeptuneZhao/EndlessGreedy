using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000D3A RID: 3386
public class SelectableTextStyler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x06006A7B RID: 27259 RVA: 0x00281C45 File Offset: 0x0027FE45
	private void Start()
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
	}

	// Token: 0x06006A7C RID: 27260 RVA: 0x00281C54 File Offset: 0x0027FE54
	private void SetState(SelectableTextStyler.State state, SelectableTextStyler.HoverState hover_state)
	{
		if (state == SelectableTextStyler.State.Normal)
		{
			if (hover_state != SelectableTextStyler.HoverState.Normal)
			{
				if (hover_state == SelectableTextStyler.HoverState.Hovered)
				{
					this.target.textStyleSetting = this.normalHovered;
				}
			}
			else
			{
				this.target.textStyleSetting = this.normalNormal;
			}
		}
		this.target.ApplySettings();
	}

	// Token: 0x06006A7D RID: 27261 RVA: 0x00281C91 File Offset: 0x0027FE91
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Hovered);
	}

	// Token: 0x06006A7E RID: 27262 RVA: 0x00281CA0 File Offset: 0x0027FEA0
	public void OnPointerExit(PointerEventData eventData)
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
	}

	// Token: 0x06006A7F RID: 27263 RVA: 0x00281CAF File Offset: 0x0027FEAF
	public void OnPointerClick(PointerEventData eventData)
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
	}

	// Token: 0x04004892 RID: 18578
	[SerializeField]
	private LocText target;

	// Token: 0x04004893 RID: 18579
	[SerializeField]
	private SelectableTextStyler.State state;

	// Token: 0x04004894 RID: 18580
	[SerializeField]
	private TextStyleSetting normalNormal;

	// Token: 0x04004895 RID: 18581
	[SerializeField]
	private TextStyleSetting normalHovered;

	// Token: 0x02001E75 RID: 7797
	public enum State
	{
		// Token: 0x04008AA7 RID: 35495
		Normal
	}

	// Token: 0x02001E76 RID: 7798
	public enum HoverState
	{
		// Token: 0x04008AA9 RID: 35497
		Normal,
		// Token: 0x04008AAA RID: 35498
		Hovered
	}
}
