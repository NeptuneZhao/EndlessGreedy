using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200000C RID: 12
public class DragMe : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
{
	// Token: 0x0600002B RID: 43 RVA: 0x00002B24 File Offset: 0x00000D24
	public void OnBeginDrag(PointerEventData eventData)
	{
		Canvas canvas = DragMe.FindInParents<Canvas>(base.gameObject);
		if (canvas == null)
		{
			return;
		}
		this.m_DraggingIcon = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
		GraphicRaycaster component = this.m_DraggingIcon.GetComponent<GraphicRaycaster>();
		if (component != null)
		{
			component.enabled = false;
		}
		this.m_DraggingIcon.name = "dragObj";
		this.m_DraggingIcon.transform.SetParent(canvas.transform, false);
		this.m_DraggingIcon.transform.SetAsLastSibling();
		this.m_DraggingIcon.GetComponent<RectTransform>().pivot = Vector2.zero;
		if (this.dragOnSurfaces)
		{
			this.m_DraggingPlane = (base.transform as RectTransform);
		}
		else
		{
			this.m_DraggingPlane = (canvas.transform as RectTransform);
		}
		this.SetDraggedPosition(eventData);
		this.listener.OnBeginDrag(eventData.position);
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002C04 File Offset: 0x00000E04
	public void OnDrag(PointerEventData data)
	{
		if (this.m_DraggingIcon != null)
		{
			this.SetDraggedPosition(data);
		}
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002C1C File Offset: 0x00000E1C
	private void SetDraggedPosition(PointerEventData data)
	{
		if (this.dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
		{
			this.m_DraggingPlane = (data.pointerEnter.transform as RectTransform);
		}
		RectTransform component = this.m_DraggingIcon.GetComponent<RectTransform>();
		Vector3 position;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.m_DraggingPlane, data.position, data.pressEventCamera, out position))
		{
			component.position = position;
			component.rotation = this.m_DraggingPlane.rotation;
		}
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002CAC File Offset: 0x00000EAC
	public void OnEndDrag(PointerEventData eventData)
	{
		this.listener.OnEndDrag(eventData.position);
		if (this.m_DraggingIcon != null)
		{
			UnityEngine.Object.Destroy(this.m_DraggingIcon);
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002CD8 File Offset: 0x00000ED8
	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return default(T);
		}
		T t = default(T);
		Transform parent = go.transform.parent;
		while (parent != null && t == null)
		{
			t = parent.gameObject.GetComponent<T>();
			parent = parent.parent;
		}
		return t;
	}

	// Token: 0x04000031 RID: 49
	public bool dragOnSurfaces = true;

	// Token: 0x04000032 RID: 50
	private GameObject m_DraggingIcon;

	// Token: 0x04000033 RID: 51
	private RectTransform m_DraggingPlane;

	// Token: 0x04000034 RID: 52
	public DragMe.IDragListener listener;

	// Token: 0x02000F90 RID: 3984
	public interface IDragListener
	{
		// Token: 0x060079EF RID: 31215
		void OnBeginDrag(Vector2 position);

		// Token: 0x060079F0 RID: 31216
		void OnEndDrag(Vector2 position);
	}
}
