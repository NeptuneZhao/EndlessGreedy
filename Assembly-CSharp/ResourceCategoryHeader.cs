using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000D24 RID: 3364
[AddComponentMenu("KMonoBehaviour/scripts/ResourceCategoryHeader")]
public class ResourceCategoryHeader : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISim4000ms
{
	// Token: 0x06006954 RID: 26964 RVA: 0x00278D38 File Offset: 0x00276F38
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.EntryContainer.SetParent(base.transform.parent);
		this.EntryContainer.SetSiblingIndex(base.transform.GetSiblingIndex() + 1);
		this.EntryContainer.localScale = Vector3.one;
		this.mButton = base.GetComponent<Button>();
		this.mButton.onClick.AddListener(delegate()
		{
			this.ToggleOpen(true);
		});
		this.SetInteractable(this.anyDiscovered);
		this.SetActiveColor(false);
	}

	// Token: 0x06006955 RID: 26965 RVA: 0x00278DC4 File Offset: 0x00276FC4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tooltip.OnToolTip = new Func<string>(this.OnTooltip);
		this.UpdateContents();
		this.RefreshChart();
	}

	// Token: 0x06006956 RID: 26966 RVA: 0x00278DEF File Offset: 0x00276FEF
	private void SetInteractable(bool state)
	{
		if (!state)
		{
			this.SetOpen(false);
			this.expandArrow.SetDisabled();
			return;
		}
		if (!this.IsOpen)
		{
			this.expandArrow.SetInactive();
			return;
		}
		this.expandArrow.SetActive();
	}

	// Token: 0x06006957 RID: 26967 RVA: 0x00278E28 File Offset: 0x00277028
	private void SetActiveColor(bool state)
	{
		if (state)
		{
			this.elements.QuantityText.color = this.TextColor_Interactable;
			this.elements.LabelText.color = this.TextColor_Interactable;
			this.expandArrow.ActiveColour = this.TextColor_Interactable;
			this.expandArrow.InactiveColour = this.TextColor_Interactable;
			this.expandArrow.TargetImage.color = this.TextColor_Interactable;
			return;
		}
		this.elements.LabelText.color = this.TextColor_NonInteractable;
		this.elements.QuantityText.color = this.TextColor_NonInteractable;
		this.expandArrow.ActiveColour = this.TextColor_NonInteractable;
		this.expandArrow.InactiveColour = this.TextColor_NonInteractable;
		this.expandArrow.TargetImage.color = this.TextColor_NonInteractable;
	}

	// Token: 0x06006958 RID: 26968 RVA: 0x00278F04 File Offset: 0x00277104
	public void SetTag(Tag t, GameUtil.MeasureUnit measure)
	{
		this.ResourceCategoryTag = t;
		this.Measure = measure;
		this.elements.LabelText.text = t.ProperName();
		if (SaveGame.Instance.expandedResourceTags.Contains(this.ResourceCategoryTag))
		{
			this.anyDiscovered = true;
			this.ToggleOpen(false);
		}
	}

	// Token: 0x06006959 RID: 26969 RVA: 0x00278F5C File Offset: 0x0027715C
	private void ToggleOpen(bool play_sound)
	{
		if (!this.anyDiscovered)
		{
			if (play_sound)
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
			}
			return;
		}
		if (!this.IsOpen)
		{
			if (play_sound)
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
			}
			this.SetOpen(true);
			this.elements.LabelText.fontSize = (float)this.maximizedFontSize;
			this.elements.QuantityText.fontSize = (float)this.maximizedFontSize;
			return;
		}
		if (play_sound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		}
		this.SetOpen(false);
		this.elements.LabelText.fontSize = (float)this.minimizedFontSize;
		this.elements.QuantityText.fontSize = (float)this.minimizedFontSize;
	}

	// Token: 0x0600695A RID: 26970 RVA: 0x00279020 File Offset: 0x00277220
	private void Hover(bool is_hovering)
	{
		this.Background.color = (is_hovering ? this.BackgroundHoverColor : new Color(0f, 0f, 0f, 0f));
		ICollection<Pickupable> collection = null;
		if (ClusterManager.Instance.activeWorld.worldInventory != null)
		{
			collection = ClusterManager.Instance.activeWorld.worldInventory.GetPickupables(this.ResourceCategoryTag, false);
		}
		if (collection == null)
		{
			return;
		}
		foreach (Pickupable pickupable in collection)
		{
			if (!(pickupable == null))
			{
				KAnimControllerBase component = pickupable.GetComponent<KAnimControllerBase>();
				if (!(component == null))
				{
					component.HighlightColour = (is_hovering ? this.highlightColour : Color.black);
				}
			}
		}
	}

	// Token: 0x0600695B RID: 26971 RVA: 0x00279100 File Offset: 0x00277300
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.Hover(true);
	}

	// Token: 0x0600695C RID: 26972 RVA: 0x00279109 File Offset: 0x00277309
	public void OnPointerExit(PointerEventData eventData)
	{
		this.Hover(false);
	}

	// Token: 0x0600695D RID: 26973 RVA: 0x00279114 File Offset: 0x00277314
	public void SetOpen(bool open)
	{
		this.IsOpen = open;
		if (open)
		{
			this.expandArrow.SetActive();
			if (!SaveGame.Instance.expandedResourceTags.Contains(this.ResourceCategoryTag))
			{
				SaveGame.Instance.expandedResourceTags.Add(this.ResourceCategoryTag);
			}
		}
		else
		{
			this.expandArrow.SetInactive();
			SaveGame.Instance.expandedResourceTags.Remove(this.ResourceCategoryTag);
		}
		this.EntryContainer.gameObject.SetActive(this.IsOpen);
	}

	// Token: 0x0600695E RID: 26974 RVA: 0x0027919C File Offset: 0x0027739C
	private void GetAmounts(bool doExtras, out float available, out float total, out float reserved)
	{
		available = 0f;
		total = 0f;
		reserved = 0f;
		HashSet<Tag> hashSet = null;
		if (!DiscoveredResources.Instance.TryGetDiscoveredResourcesFromTag(this.ResourceCategoryTag, out hashSet))
		{
			return;
		}
		ListPool<Tag, ResourceCategoryHeader>.PooledList pooledList = ListPool<Tag, ResourceCategoryHeader>.Allocate();
		foreach (Tag tag in hashSet)
		{
			EdiblesManager.FoodInfo foodInfo = null;
			if (this.Measure == GameUtil.MeasureUnit.kcal)
			{
				foodInfo = EdiblesManager.GetFoodInfo(tag.Name);
				if (foodInfo == null)
				{
					pooledList.Add(tag);
					continue;
				}
			}
			this.anyDiscovered = true;
			ResourceEntry resourceEntry = null;
			if (!this.ResourcesDiscovered.TryGetValue(tag, out resourceEntry))
			{
				resourceEntry = this.NewResourceEntry(tag, this.Measure);
				this.ResourcesDiscovered.Add(tag, resourceEntry);
			}
			float num;
			float num2;
			float num3;
			resourceEntry.GetAmounts(foodInfo, doExtras, out num, out num2, out num3);
			available += num;
			total += num2;
			reserved += num3;
		}
		foreach (Tag item in pooledList)
		{
			hashSet.Remove(item);
		}
		pooledList.Recycle();
	}

	// Token: 0x0600695F RID: 26975 RVA: 0x002792E8 File Offset: 0x002774E8
	public void UpdateContents()
	{
		float num;
		float num2;
		float num3;
		this.GetAmounts(false, out num, out num2, out num3);
		if (num != this.cachedAvailable || num2 != this.cachedTotal || num3 != this.cachedReserved)
		{
			if (this.quantityString == null || this.currentQuantity != num)
			{
				switch (this.Measure)
				{
				case GameUtil.MeasureUnit.mass:
					this.quantityString = GameUtil.GetFormattedMass(num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
					break;
				case GameUtil.MeasureUnit.kcal:
					this.quantityString = GameUtil.GetFormattedCalories(num, GameUtil.TimeSlice.None, true);
					break;
				case GameUtil.MeasureUnit.quantity:
					this.quantityString = num.ToString();
					break;
				}
				this.elements.QuantityText.text = this.quantityString;
				this.currentQuantity = num;
			}
			this.cachedAvailable = num;
			this.cachedTotal = num2;
			this.cachedReserved = num3;
		}
		foreach (KeyValuePair<Tag, ResourceEntry> keyValuePair in this.ResourcesDiscovered)
		{
			keyValuePair.Value.UpdateValue();
		}
		this.SetActiveColor(num > 0f);
		this.SetInteractable(this.anyDiscovered);
	}

	// Token: 0x06006960 RID: 26976 RVA: 0x00279418 File Offset: 0x00277618
	private string OnTooltip()
	{
		float quantity;
		float quantity2;
		float quantity3;
		this.GetAmounts(true, out quantity, out quantity2, out quantity3);
		string text = this.elements.LabelText.text + "\n";
		text += string.Format(UI.RESOURCESCREEN.AVAILABLE_TOOLTIP, ResourceCategoryScreen.QuantityTextForMeasure(quantity, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity3, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity2, this.Measure));
		float delta = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.ResourceCategoryTag).GetDelta(150f);
		if (delta != 0f)
		{
			text = text + "\n\n" + string.Format(UI.RESOURCESCREEN.TREND_TOOLTIP, (delta > 0f) ? UI.RESOURCESCREEN.INCREASING_STR : UI.RESOURCESCREEN.DECREASING_STR, GameUtil.GetFormattedMass(Mathf.Abs(delta), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		else
		{
			text = text + "\n\n" + UI.RESOURCESCREEN.TREND_TOOLTIP_NO_CHANGE;
		}
		return text;
	}

	// Token: 0x06006961 RID: 26977 RVA: 0x00279513 File Offset: 0x00277713
	private ResourceEntry NewResourceEntry(Tag resourceTag, GameUtil.MeasureUnit measure)
	{
		ResourceEntry component = Util.KInstantiateUI(this.Prefab_ResourceEntry, this.EntryContainer.gameObject, true).GetComponent<ResourceEntry>();
		component.SetTag(resourceTag, measure);
		return component;
	}

	// Token: 0x06006962 RID: 26978 RVA: 0x00279539 File Offset: 0x00277739
	public void Sim4000ms(float dt)
	{
		this.RefreshChart();
	}

	// Token: 0x06006963 RID: 26979 RVA: 0x00279544 File Offset: 0x00277744
	private void RefreshChart()
	{
		if (this.sparkChart != null)
		{
			ResourceTracker resourceStatistic = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.ResourceCategoryTag);
			this.sparkChart.GetComponentInChildren<LineLayer>().RefreshLine(resourceStatistic.ChartableData(3000f), "resourceAmount");
			this.sparkChart.GetComponentInChildren<SparkLayer>().SetColor(Constants.NEUTRAL_COLOR);
		}
	}

	// Token: 0x04004795 RID: 18325
	public GameObject Prefab_ResourceEntry;

	// Token: 0x04004796 RID: 18326
	public Transform EntryContainer;

	// Token: 0x04004797 RID: 18327
	public Tag ResourceCategoryTag;

	// Token: 0x04004798 RID: 18328
	public GameUtil.MeasureUnit Measure;

	// Token: 0x04004799 RID: 18329
	public bool IsOpen;

	// Token: 0x0400479A RID: 18330
	public ImageToggleState expandArrow;

	// Token: 0x0400479B RID: 18331
	private Button mButton;

	// Token: 0x0400479C RID: 18332
	public Dictionary<Tag, ResourceEntry> ResourcesDiscovered = new Dictionary<Tag, ResourceEntry>();

	// Token: 0x0400479D RID: 18333
	public ResourceCategoryHeader.ElementReferences elements;

	// Token: 0x0400479E RID: 18334
	public Color TextColor_Interactable;

	// Token: 0x0400479F RID: 18335
	public Color TextColor_NonInteractable;

	// Token: 0x040047A0 RID: 18336
	private string quantityString;

	// Token: 0x040047A1 RID: 18337
	private float currentQuantity;

	// Token: 0x040047A2 RID: 18338
	private bool anyDiscovered;

	// Token: 0x040047A3 RID: 18339
	public const float chartHistoryLength = 3000f;

	// Token: 0x040047A4 RID: 18340
	[MyCmpGet]
	private ToolTip tooltip;

	// Token: 0x040047A5 RID: 18341
	[SerializeField]
	private int minimizedFontSize;

	// Token: 0x040047A6 RID: 18342
	[SerializeField]
	private int maximizedFontSize;

	// Token: 0x040047A7 RID: 18343
	[SerializeField]
	private Color highlightColour;

	// Token: 0x040047A8 RID: 18344
	[SerializeField]
	private Color BackgroundHoverColor;

	// Token: 0x040047A9 RID: 18345
	[SerializeField]
	private Image Background;

	// Token: 0x040047AA RID: 18346
	public GameObject sparkChart;

	// Token: 0x040047AB RID: 18347
	private float cachedAvailable = float.MinValue;

	// Token: 0x040047AC RID: 18348
	private float cachedTotal = float.MinValue;

	// Token: 0x040047AD RID: 18349
	private float cachedReserved = float.MinValue;

	// Token: 0x02001E4E RID: 7758
	[Serializable]
	public struct ElementReferences
	{
		// Token: 0x04008A11 RID: 35345
		public LocText LabelText;

		// Token: 0x04008A12 RID: 35346
		public LocText QuantityText;
	}
}
