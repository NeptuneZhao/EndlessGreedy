using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000D25 RID: 3365
public class ResourceCategoryScreen : KScreen
{
	// Token: 0x06006966 RID: 26982 RVA: 0x002795EC File Offset: 0x002777EC
	public static void DestroyInstance()
	{
		ResourceCategoryScreen.Instance = null;
	}

	// Token: 0x06006967 RID: 26983 RVA: 0x002795F4 File Offset: 0x002777F4
	protected override void OnActivate()
	{
		base.OnActivate();
		ResourceCategoryScreen.Instance = this;
		base.ConsumeMouseScroll = true;
		MultiToggle hiderButton = this.HiderButton;
		hiderButton.onClick = (System.Action)Delegate.Combine(hiderButton.onClick, new System.Action(this.OnHiderClick));
		this.OnHiderClick();
		this.CreateTagSetHeaders(GameTags.MaterialCategories, GameUtil.MeasureUnit.mass);
		this.CreateTagSetHeaders(GameTags.CalorieCategories, GameUtil.MeasureUnit.kcal);
		this.CreateTagSetHeaders(GameTags.UnitCategories, GameUtil.MeasureUnit.quantity);
		if (!this.DisplayedCategories.ContainsKey(GameTags.Miscellaneous))
		{
			ResourceCategoryHeader value = this.NewCategoryHeader(GameTags.Miscellaneous, GameUtil.MeasureUnit.mass);
			this.DisplayedCategories.Add(GameTags.Miscellaneous, value);
		}
		this.DisplayedCategoryKeys = this.DisplayedCategories.Keys.ToArray<Tag>();
	}

	// Token: 0x06006968 RID: 26984 RVA: 0x002796AC File Offset: 0x002778AC
	private void CreateTagSetHeaders(IEnumerable<Tag> set, GameUtil.MeasureUnit measure)
	{
		foreach (Tag tag in set)
		{
			ResourceCategoryHeader value = this.NewCategoryHeader(tag, measure);
			this.DisplayedCategories.Add(tag, value);
		}
	}

	// Token: 0x06006969 RID: 26985 RVA: 0x00279704 File Offset: 0x00277904
	private void OnHiderClick()
	{
		this.HiderButton.NextState();
		if (this.HiderButton.CurrentState == 0)
		{
			this.targetContentHideHeight = 0f;
			return;
		}
		this.targetContentHideHeight = Mathf.Min(((float)Screen.height - this.maxHeightPadding) / GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>().GetCanvasScale(), this.CategoryContainer.rectTransform().rect.height);
	}

	// Token: 0x0600696A RID: 26986 RVA: 0x0027977C File Offset: 0x0027797C
	private void Update()
	{
		if (ClusterManager.Instance.activeWorld.worldInventory == null)
		{
			return;
		}
		if (this.HideTarget.minHeight != this.targetContentHideHeight)
		{
			float num = this.HideTarget.minHeight;
			float num2 = this.targetContentHideHeight - num;
			num2 = Mathf.Clamp(num2 * this.HideSpeedFactor * Time.unscaledDeltaTime, (num2 > 0f) ? (-num2) : num2, (num2 > 0f) ? num2 : (-num2));
			num += num2;
			this.HideTarget.minHeight = num;
		}
		for (int i = 0; i < 1; i++)
		{
			Tag tag = this.DisplayedCategoryKeys[this.categoryUpdatePacer];
			ResourceCategoryHeader resourceCategoryHeader = this.DisplayedCategories[tag];
			if (DiscoveredResources.Instance.IsDiscovered(tag) && !resourceCategoryHeader.gameObject.activeInHierarchy)
			{
				resourceCategoryHeader.gameObject.SetActive(true);
			}
			resourceCategoryHeader.UpdateContents();
			this.categoryUpdatePacer = (this.categoryUpdatePacer + 1) % this.DisplayedCategoryKeys.Length;
		}
		if (this.HiderButton.CurrentState != 0)
		{
			this.targetContentHideHeight = Mathf.Min(((float)Screen.height - this.maxHeightPadding) / GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>().GetCanvasScale(), this.CategoryContainer.rectTransform().rect.height);
		}
		if (MeterScreen.Instance != null && !MeterScreen.Instance.StartValuesSet)
		{
			MeterScreen.Instance.InitializeValues();
		}
	}

	// Token: 0x0600696B RID: 26987 RVA: 0x002798EF File Offset: 0x00277AEF
	private ResourceCategoryHeader NewCategoryHeader(Tag categoryTag, GameUtil.MeasureUnit measure)
	{
		GameObject gameObject = Util.KInstantiateUI(this.Prefab_CategoryBar, this.CategoryContainer.gameObject, false);
		gameObject.name = "CategoryHeader_" + categoryTag.Name;
		ResourceCategoryHeader component = gameObject.GetComponent<ResourceCategoryHeader>();
		component.SetTag(categoryTag, measure);
		return component;
	}

	// Token: 0x0600696C RID: 26988 RVA: 0x0027992C File Offset: 0x00277B2C
	public static string QuantityTextForMeasure(float quantity, GameUtil.MeasureUnit measure)
	{
		switch (measure)
		{
		case GameUtil.MeasureUnit.mass:
			return GameUtil.GetFormattedMass(quantity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		case GameUtil.MeasureUnit.kcal:
			return GameUtil.GetFormattedCalories(quantity, GameUtil.TimeSlice.None, true);
		}
		return quantity.ToString();
	}

	// Token: 0x040047AE RID: 18350
	public static ResourceCategoryScreen Instance;

	// Token: 0x040047AF RID: 18351
	public GameObject Prefab_CategoryBar;

	// Token: 0x040047B0 RID: 18352
	public Transform CategoryContainer;

	// Token: 0x040047B1 RID: 18353
	public MultiToggle HiderButton;

	// Token: 0x040047B2 RID: 18354
	public KLayoutElement HideTarget;

	// Token: 0x040047B3 RID: 18355
	private float HideSpeedFactor = 12f;

	// Token: 0x040047B4 RID: 18356
	private float maxHeightPadding = 480f;

	// Token: 0x040047B5 RID: 18357
	private float targetContentHideHeight;

	// Token: 0x040047B6 RID: 18358
	public Dictionary<Tag, ResourceCategoryHeader> DisplayedCategories = new Dictionary<Tag, ResourceCategoryHeader>();

	// Token: 0x040047B7 RID: 18359
	private Tag[] DisplayedCategoryKeys;

	// Token: 0x040047B8 RID: 18360
	private int categoryUpdatePacer;
}
