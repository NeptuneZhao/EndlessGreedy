using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C71 RID: 3185
[AddComponentMenu("KMonoBehaviour/scripts/KCanvasScaler")]
public class KCanvasScaler : KMonoBehaviour
{
	// Token: 0x060061CD RID: 25037 RVA: 0x002484DC File Offset: 0x002466DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (KPlayerPrefs.HasKey(KCanvasScaler.UIScalePrefKey))
		{
			this.SetUserScale(KPlayerPrefs.GetFloat(KCanvasScaler.UIScalePrefKey) / 100f);
		}
		else
		{
			this.SetUserScale(1f);
		}
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
	}

	// Token: 0x060061CE RID: 25038 RVA: 0x00248544 File Offset: 0x00246744
	private void OnResize()
	{
		this.SetUserScale(this.userScale);
	}

	// Token: 0x060061CF RID: 25039 RVA: 0x00248552 File Offset: 0x00246752
	public void SetUserScale(float scale)
	{
		if (this.canvasScaler == null)
		{
			this.canvasScaler = base.GetComponent<CanvasScaler>();
		}
		this.userScale = scale;
		this.canvasScaler.scaleFactor = this.GetCanvasScale();
	}

	// Token: 0x060061D0 RID: 25040 RVA: 0x00248586 File Offset: 0x00246786
	public float GetUserScale()
	{
		return this.userScale;
	}

	// Token: 0x060061D1 RID: 25041 RVA: 0x0024858E File Offset: 0x0024678E
	public float GetCanvasScale()
	{
		return this.userScale * this.ScreenRelativeScale();
	}

	// Token: 0x060061D2 RID: 25042 RVA: 0x002485A0 File Offset: 0x002467A0
	private float ScreenRelativeScale()
	{
		float dpi = Screen.dpi;
		Camera x = Camera.main;
		if (x == null)
		{
			x = UnityEngine.Object.FindObjectOfType<Camera>();
		}
		x != null;
		if ((float)Screen.height <= this.scaleSteps[0].maxRes_y || (float)Screen.width / (float)Screen.height < 1.6777778f)
		{
			return this.scaleSteps[0].scale;
		}
		if ((float)Screen.height > this.scaleSteps[this.scaleSteps.Length - 1].maxRes_y)
		{
			return this.scaleSteps[this.scaleSteps.Length - 1].scale;
		}
		for (int i = 0; i < this.scaleSteps.Length; i++)
		{
			if ((float)Screen.height > this.scaleSteps[i].maxRes_y && (float)Screen.height <= this.scaleSteps[i + 1].maxRes_y)
			{
				float t = ((float)Screen.height - this.scaleSteps[i].maxRes_y) / (this.scaleSteps[i + 1].maxRes_y - this.scaleSteps[i].maxRes_y);
				return Mathf.Lerp(this.scaleSteps[i].scale, this.scaleSteps[i + 1].scale, t);
			}
		}
		return 1f;
	}

	// Token: 0x04004252 RID: 16978
	[MyCmpReq]
	private CanvasScaler canvasScaler;

	// Token: 0x04004253 RID: 16979
	public static string UIScalePrefKey = "UIScalePref";

	// Token: 0x04004254 RID: 16980
	private float userScale = 1f;

	// Token: 0x04004255 RID: 16981
	[Range(0.75f, 2f)]
	private KCanvasScaler.ScaleStep[] scaleSteps = new KCanvasScaler.ScaleStep[]
	{
		new KCanvasScaler.ScaleStep(720f, 0.86f),
		new KCanvasScaler.ScaleStep(1080f, 1f),
		new KCanvasScaler.ScaleStep(2160f, 1.33f)
	};

	// Token: 0x02001D55 RID: 7509
	[Serializable]
	public struct ScaleStep
	{
		// Token: 0x0600A854 RID: 43092 RVA: 0x0039C6E5 File Offset: 0x0039A8E5
		public ScaleStep(float maxRes_y, float scale)
		{
			this.maxRes_y = maxRes_y;
			this.scale = scale;
		}

		// Token: 0x040086FA RID: 34554
		public float scale;

		// Token: 0x040086FB RID: 34555
		public float maxRes_y;
	}
}
