using System;
using UnityEngine;

// Token: 0x02000B79 RID: 2937
public class Infrared : MonoBehaviour
{
	// Token: 0x0600582F RID: 22575 RVA: 0x001FDCA2 File Offset: 0x001FBEA2
	public static void DestroyInstance()
	{
		Infrared.Instance = null;
	}

	// Token: 0x06005830 RID: 22576 RVA: 0x001FDCAA File Offset: 0x001FBEAA
	private void Awake()
	{
		Infrared.temperatureParametersId = Shader.PropertyToID("_TemperatureParameters");
		Infrared.Instance = this;
		this.OnResize();
		this.UpdateState();
	}

	// Token: 0x06005831 RID: 22577 RVA: 0x001FDCCD File Offset: 0x001FBECD
	private void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, this.minionTexture);
		Graphics.Blit(source, dest);
	}

	// Token: 0x06005832 RID: 22578 RVA: 0x001FDCE4 File Offset: 0x001FBEE4
	private void OnResize()
	{
		if (this.minionTexture != null)
		{
			this.minionTexture.DestroyRenderTexture();
		}
		if (this.cameraTexture != null)
		{
			this.cameraTexture.DestroyRenderTexture();
		}
		int num = 2;
		this.minionTexture = new RenderTexture(Screen.width / num, Screen.height / num, 0, RenderTextureFormat.ARGB32);
		this.cameraTexture = new RenderTexture(Screen.width / num, Screen.height / num, 0, RenderTextureFormat.ARGB32);
		base.GetComponent<Camera>().targetTexture = this.cameraTexture;
	}

	// Token: 0x06005833 RID: 22579 RVA: 0x001FDD6C File Offset: 0x001FBF6C
	public void SetMode(Infrared.Mode mode)
	{
		Vector4 zero;
		if (mode != Infrared.Mode.Disabled)
		{
			if (mode != Infrared.Mode.Disease)
			{
				zero = new Vector4(1f, 0f, 0f, 0f);
			}
			else
			{
				zero = new Vector4(1f, 0f, 0f, 0f);
				GameComps.InfraredVisualizers.ClearOverlayColour();
			}
		}
		else
		{
			zero = Vector4.zero;
		}
		Shader.SetGlobalVector("_ColouredOverlayParameters", zero);
		this.mode = mode;
		this.UpdateState();
	}

	// Token: 0x06005834 RID: 22580 RVA: 0x001FDDE4 File Offset: 0x001FBFE4
	private void UpdateState()
	{
		base.gameObject.SetActive(this.mode > Infrared.Mode.Disabled);
		if (base.gameObject.activeSelf)
		{
			this.Update();
		}
	}

	// Token: 0x06005835 RID: 22581 RVA: 0x001FDE10 File Offset: 0x001FC010
	private void Update()
	{
		switch (this.mode)
		{
		case Infrared.Mode.Disabled:
			break;
		case Infrared.Mode.Infrared:
			GameComps.InfraredVisualizers.UpdateTemperature();
			return;
		case Infrared.Mode.Disease:
			GameComps.DiseaseContainers.UpdateOverlayColours();
			break;
		default:
			return;
		}
	}

	// Token: 0x040039A6 RID: 14758
	private RenderTexture minionTexture;

	// Token: 0x040039A7 RID: 14759
	private RenderTexture cameraTexture;

	// Token: 0x040039A8 RID: 14760
	private Infrared.Mode mode;

	// Token: 0x040039A9 RID: 14761
	public static int temperatureParametersId;

	// Token: 0x040039AA RID: 14762
	public static Infrared Instance;

	// Token: 0x02001BDC RID: 7132
	public enum Mode
	{
		// Token: 0x040080EC RID: 33004
		Disabled,
		// Token: 0x040080ED RID: 33005
		Infrared,
		// Token: 0x040080EE RID: 33006
		Disease
	}
}
