using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000635 RID: 1589
public class UnityMouseCatcherUI
{
	// Token: 0x06002716 RID: 10006 RVA: 0x000DE538 File Offset: 0x000DC738
	public static Canvas ManifestCanvas()
	{
		if (UnityMouseCatcherUI.m_instance_canvas != null && UnityMouseCatcherUI.m_instance_canvas)
		{
			return UnityMouseCatcherUI.m_instance_canvas;
		}
		GameObject gameObject = new GameObject("UnityMouseCatcherUI Canvas");
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		Canvas canvas = gameObject.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.sortingOrder = 32767;
		canvas.pixelPerfect = false;
		UnityMouseCatcherUI.m_instance_canvas = canvas;
		gameObject.AddComponent<GraphicRaycaster>();
		GameObject gameObject2 = new GameObject("ImGui Consume Input", new Type[]
		{
			typeof(RectTransform)
		});
		gameObject2.transform.SetParent(gameObject.transform, false);
		RectTransform component = gameObject2.GetComponent<RectTransform>();
		component.anchorMin = Vector2.zero;
		component.anchorMax = Vector2.one;
		component.sizeDelta = Vector2.zero;
		component.anchoredPosition = Vector2.zero;
		Image image = gameObject2.AddComponent<Image>();
		image.sprite = Resources.Load<Sprite>("1x1_white");
		image.color = new Color(1f, 1f, 1f, 0f);
		image.raycastTarget = true;
		return UnityMouseCatcherUI.m_instance_canvas;
	}

	// Token: 0x06002717 RID: 10007 RVA: 0x000DE640 File Offset: 0x000DC840
	public static void SetEnabled(bool is_enabled)
	{
		Canvas canvas = UnityMouseCatcherUI.ManifestCanvas();
		if (canvas.gameObject.activeSelf != is_enabled)
		{
			canvas.gameObject.SetActive(is_enabled);
		}
		if (canvas.enabled != is_enabled)
		{
			canvas.enabled = is_enabled;
		}
	}

	// Token: 0x04001660 RID: 5728
	private static Canvas m_instance_canvas;
}
