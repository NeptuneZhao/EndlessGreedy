using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BC6 RID: 3014
public class AlertVignette : KMonoBehaviour
{
	// Token: 0x06005BDA RID: 23514 RVA: 0x00219251 File Offset: 0x00217451
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06005BDB RID: 23515 RVA: 0x0021925C File Offset: 0x0021745C
	private void Update()
	{
		Color color = this.image.color;
		if (ClusterManager.Instance.GetWorld(this.worldID) == null)
		{
			color = Color.clear;
			this.image.color = color;
			return;
		}
		if (ClusterManager.Instance.GetWorld(this.worldID).IsRedAlert())
		{
			if (color.r != Vignette.Instance.redAlertColor.r || color.g != Vignette.Instance.redAlertColor.g || color.b != Vignette.Instance.redAlertColor.b)
			{
				color = Vignette.Instance.redAlertColor;
			}
		}
		else if (ClusterManager.Instance.GetWorld(this.worldID).IsYellowAlert())
		{
			if (color.r != Vignette.Instance.yellowAlertColor.r || color.g != Vignette.Instance.yellowAlertColor.g || color.b != Vignette.Instance.yellowAlertColor.b)
			{
				color = Vignette.Instance.yellowAlertColor;
			}
		}
		else
		{
			color = Color.clear;
		}
		if (color != Color.clear)
		{
			color.a = 0.2f + (0.5f + Mathf.Sin(Time.unscaledTime * 4f - 1f) / 2f) * 0.5f;
		}
		if (this.image.color != color)
		{
			this.image.color = color;
		}
	}

	// Token: 0x04003D7D RID: 15741
	public Image image;

	// Token: 0x04003D7E RID: 15742
	public int worldID;
}
