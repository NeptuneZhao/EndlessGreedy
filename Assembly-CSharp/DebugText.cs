using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000833 RID: 2099
[AddComponentMenu("KMonoBehaviour/scripts/DebugText")]
public class DebugText : KMonoBehaviour
{
	// Token: 0x06003A5B RID: 14939 RVA: 0x0013F6AE File Offset: 0x0013D8AE
	public static void DestroyInstance()
	{
		DebugText.Instance = null;
	}

	// Token: 0x06003A5C RID: 14940 RVA: 0x0013F6B6 File Offset: 0x0013D8B6
	protected override void OnPrefabInit()
	{
		DebugText.Instance = this;
	}

	// Token: 0x06003A5D RID: 14941 RVA: 0x0013F6C0 File Offset: 0x0013D8C0
	public void Draw(string text, Vector3 pos, Color color)
	{
		DebugText.Entry item = new DebugText.Entry
		{
			text = text,
			pos = pos,
			color = color
		};
		this.entries.Add(item);
	}

	// Token: 0x06003A5E RID: 14942 RVA: 0x0013F6FC File Offset: 0x0013D8FC
	private void LateUpdate()
	{
		foreach (Text text in this.texts)
		{
			UnityEngine.Object.Destroy(text.gameObject);
		}
		this.texts.Clear();
		foreach (DebugText.Entry entry in this.entries)
		{
			GameObject gameObject = new GameObject();
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.GetComponent<RectTransform>());
			gameObject.transform.SetPosition(entry.pos);
			rectTransform.localScale = new Vector3(0.02f, 0.02f, 1f);
			Text text2 = gameObject.AddComponent<Text>();
			text2.font = Assets.DebugFont;
			text2.text = entry.text;
			text2.color = entry.color;
			text2.horizontalOverflow = HorizontalWrapMode.Overflow;
			text2.verticalOverflow = VerticalWrapMode.Overflow;
			text2.alignment = TextAnchor.MiddleCenter;
			this.texts.Add(text2);
		}
		this.entries.Clear();
	}

	// Token: 0x04002318 RID: 8984
	public static DebugText Instance;

	// Token: 0x04002319 RID: 8985
	private List<DebugText.Entry> entries = new List<DebugText.Entry>();

	// Token: 0x0400231A RID: 8986
	private List<Text> texts = new List<Text>();

	// Token: 0x02001755 RID: 5973
	private struct Entry
	{
		// Token: 0x04007279 RID: 29305
		public string text;

		// Token: 0x0400727A RID: 29306
		public Vector3 pos;

		// Token: 0x0400727B RID: 29307
		public Color color;
	}
}
