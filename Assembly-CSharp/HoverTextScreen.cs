using System;
using UnityEngine;

// Token: 0x02000B99 RID: 2969
public class HoverTextScreen : KScreen
{
	// Token: 0x06005996 RID: 22934 RVA: 0x0020687B File Offset: 0x00204A7B
	public static void DestroyInstance()
	{
		HoverTextScreen.Instance = null;
	}

	// Token: 0x06005997 RID: 22935 RVA: 0x00206883 File Offset: 0x00204A83
	protected override void OnActivate()
	{
		base.OnActivate();
		HoverTextScreen.Instance = this;
		this.drawer = new HoverTextDrawer(this.skin.skin, base.GetComponent<RectTransform>());
	}

	// Token: 0x06005998 RID: 22936 RVA: 0x002068B0 File Offset: 0x00204AB0
	public HoverTextDrawer BeginDrawing()
	{
		Vector2 zero = Vector2.zero;
		Vector2 screenPoint = KInputManager.GetMousePos();
		RectTransform rectTransform = base.transform.parent as RectTransform;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, base.transform.parent.GetComponent<Canvas>().worldCamera, out zero);
		zero.x += rectTransform.sizeDelta.x / 2f;
		zero.y -= rectTransform.sizeDelta.y / 2f;
		this.drawer.BeginDrawing(zero);
		return this.drawer;
	}

	// Token: 0x06005999 RID: 22937 RVA: 0x00206948 File Offset: 0x00204B48
	private void Update()
	{
		bool enabled = PlayerController.Instance.ActiveTool.ShowHoverUI();
		this.drawer.SetEnabled(enabled);
	}

	// Token: 0x0600599A RID: 22938 RVA: 0x00206974 File Offset: 0x00204B74
	public Sprite GetSprite(string byName)
	{
		foreach (Sprite sprite in this.HoverIcons)
		{
			if (sprite != null && sprite.name == byName)
			{
				return sprite;
			}
		}
		global::Debug.LogWarning("No icon named " + byName + " was found on HoverTextScreen.prefab");
		return null;
	}

	// Token: 0x0600599B RID: 22939 RVA: 0x002069C9 File Offset: 0x00204BC9
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.drawer.Cleanup();
	}

	// Token: 0x04003AEA RID: 15082
	[SerializeField]
	private HoverTextSkin skin;

	// Token: 0x04003AEB RID: 15083
	public Sprite[] HoverIcons;

	// Token: 0x04003AEC RID: 15084
	public HoverTextDrawer drawer;

	// Token: 0x04003AED RID: 15085
	public static HoverTextScreen Instance;
}
