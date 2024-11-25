using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BE7 RID: 3047
[AddComponentMenu("KMonoBehaviour/scripts/BatteryUI")]
public class BatteryUI : KMonoBehaviour
{
	// Token: 0x06005CBC RID: 23740 RVA: 0x0021F4D4 File Offset: 0x0021D6D4
	private void Initialize()
	{
		if (this.unitLabel == null)
		{
			this.unitLabel = this.currentKJLabel.gameObject.GetComponentInChildrenOnly<LocText>();
		}
		if (this.sizeMap == null || this.sizeMap.Count == 0)
		{
			this.sizeMap = new Dictionary<float, float>();
			this.sizeMap.Add(20000f, 10f);
			this.sizeMap.Add(40000f, 25f);
			this.sizeMap.Add(60000f, 40f);
		}
	}

	// Token: 0x06005CBD RID: 23741 RVA: 0x0021F564 File Offset: 0x0021D764
	public void SetContent(Battery bat)
	{
		if (bat == null || bat.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			return;
		}
		base.gameObject.SetActive(true);
		this.Initialize();
		RectTransform component = this.batteryBG.GetComponent<RectTransform>();
		float num = 0f;
		foreach (KeyValuePair<float, float> keyValuePair in this.sizeMap)
		{
			if (bat.Capacity <= keyValuePair.Key)
			{
				num = keyValuePair.Value;
				break;
			}
		}
		this.batteryBG.sprite = ((bat.Capacity >= 40000f) ? this.bigBatteryBG : this.regularBatteryBG);
		float y = 25f;
		component.sizeDelta = new Vector2(num, y);
		BuildingEnabledButton component2 = bat.GetComponent<BuildingEnabledButton>();
		Color color;
		if (component2 != null && !component2.IsEnabled)
		{
			color = Color.gray;
		}
		else
		{
			color = ((bat.PercentFull >= bat.PreviousPercentFull) ? this.energyIncreaseColor : this.energyDecreaseColor);
		}
		this.batteryMeter.color = color;
		this.batteryBG.color = color;
		float num2 = this.batteryBG.GetComponent<RectTransform>().rect.height * bat.PercentFull;
		this.batteryMeter.GetComponent<RectTransform>().sizeDelta = new Vector2(num - 5.5f, num2 - 5.5f);
		color.a = 1f;
		if (this.currentKJLabel.color != color)
		{
			this.currentKJLabel.color = color;
			this.unitLabel.color = color;
		}
		this.currentKJLabel.text = bat.JoulesAvailable.ToString("F0");
	}

	// Token: 0x04003E06 RID: 15878
	[SerializeField]
	private LocText currentKJLabel;

	// Token: 0x04003E07 RID: 15879
	[SerializeField]
	private Image batteryBG;

	// Token: 0x04003E08 RID: 15880
	[SerializeField]
	private Image batteryMeter;

	// Token: 0x04003E09 RID: 15881
	[SerializeField]
	private Sprite regularBatteryBG;

	// Token: 0x04003E0A RID: 15882
	[SerializeField]
	private Sprite bigBatteryBG;

	// Token: 0x04003E0B RID: 15883
	[SerializeField]
	private Color energyIncreaseColor = Color.green;

	// Token: 0x04003E0C RID: 15884
	[SerializeField]
	private Color energyDecreaseColor = Color.red;

	// Token: 0x04003E0D RID: 15885
	private LocText unitLabel;

	// Token: 0x04003E0E RID: 15886
	private const float UIUnit = 10f;

	// Token: 0x04003E0F RID: 15887
	private Dictionary<float, float> sizeMap;
}
