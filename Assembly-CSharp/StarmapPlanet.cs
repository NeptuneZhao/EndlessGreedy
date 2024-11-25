using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DCD RID: 3533
[AddComponentMenu("KMonoBehaviour/scripts/StarmapPlanet")]
public class StarmapPlanet : KMonoBehaviour
{
	// Token: 0x0600701C RID: 28700 RVA: 0x002A4590 File Offset: 0x002A2790
	public void SetSprite(Sprite sprite, Color color)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.image.sprite = sprite;
			starmapPlanetVisualizer.image.color = color;
		}
	}

	// Token: 0x0600701D RID: 28701 RVA: 0x002A45F4 File Offset: 0x002A27F4
	public void SetFillAmount(float amount)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.image.fillAmount = amount;
		}
	}

	// Token: 0x0600701E RID: 28702 RVA: 0x002A464C File Offset: 0x002A284C
	public void SetUnknownBGActive(bool active, Color color)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.unknownBG.gameObject.SetActive(active);
			starmapPlanetVisualizer.unknownBG.color = color;
		}
	}

	// Token: 0x0600701F RID: 28703 RVA: 0x002A46B4 File Offset: 0x002A28B4
	public void SetSelectionActive(bool active)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.selection.gameObject.SetActive(active);
		}
	}

	// Token: 0x06007020 RID: 28704 RVA: 0x002A4710 File Offset: 0x002A2910
	public void SetAnalysisActive(bool active)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.analysisSelection.SetActive(active);
		}
	}

	// Token: 0x06007021 RID: 28705 RVA: 0x002A4768 File Offset: 0x002A2968
	public void SetLabel(string text)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.label.text = text;
			this.ShowLabel(false);
		}
	}

	// Token: 0x06007022 RID: 28706 RVA: 0x002A47C8 File Offset: 0x002A29C8
	public void ShowLabel(bool show)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.label.gameObject.SetActive(show);
		}
	}

	// Token: 0x06007023 RID: 28707 RVA: 0x002A4824 File Offset: 0x002A2A24
	public void SetOnClick(System.Action del)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.button.onClick = del;
		}
	}

	// Token: 0x06007024 RID: 28708 RVA: 0x002A487C File Offset: 0x002A2A7C
	public void SetOnEnter(System.Action del)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.button.onEnter = del;
		}
	}

	// Token: 0x06007025 RID: 28709 RVA: 0x002A48D4 File Offset: 0x002A2AD4
	public void SetOnExit(System.Action del)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.button.onExit = del;
		}
	}

	// Token: 0x06007026 RID: 28710 RVA: 0x002A492C File Offset: 0x002A2B2C
	public void AnimateSelector(float time)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.selection.anchoredPosition = new Vector2(0f, 25f + Mathf.Sin(time * 4f) * 5f);
		}
	}

	// Token: 0x06007027 RID: 28711 RVA: 0x002A49A4 File Offset: 0x002A2BA4
	public void ShowAsCurrentRocketDestination(bool show)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			RectTransform rectTransform = starmapPlanetVisualizer.rocketIconContainer.rectTransform();
			if (rectTransform.childCount > 0)
			{
				rectTransform.GetChild(rectTransform.childCount - 1).GetComponent<HierarchyReferences>().GetReference<Image>("fg").color = (show ? new Color(0.11764706f, 0.8627451f, 0.3137255f) : Color.white);
			}
		}
	}

	// Token: 0x06007028 RID: 28712 RVA: 0x002A4A44 File Offset: 0x002A2C44
	public void SetRocketIcons(int numRockets, GameObject iconPrefab)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			RectTransform rectTransform = starmapPlanetVisualizer.rocketIconContainer.rectTransform();
			for (int i = rectTransform.childCount; i < numRockets; i++)
			{
				Util.KInstantiateUI(iconPrefab, starmapPlanetVisualizer.rocketIconContainer, true);
			}
			for (int j = rectTransform.childCount; j > numRockets; j--)
			{
				UnityEngine.Object.Destroy(rectTransform.GetChild(j - 1).gameObject);
			}
			int num = 0;
			foreach (object obj in rectTransform)
			{
				((RectTransform)obj).anchoredPosition = new Vector2((float)num * -10f, 0f);
				num++;
			}
		}
	}

	// Token: 0x04004CD7 RID: 19671
	public List<StarmapPlanetVisualizer> visualizers;
}
