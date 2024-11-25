using System;
using System.Collections;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000D26 RID: 3366
[AddComponentMenu("KMonoBehaviour/scripts/ResourceEntry")]
public class ResourceEntry : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISim4000ms
{
	// Token: 0x0600696E RID: 26990 RVA: 0x0027998C File Offset: 0x00277B8C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.QuantityLabel.color = this.AvailableColor;
		this.NameLabel.color = this.AvailableColor;
		this.button.onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x0600696F RID: 26991 RVA: 0x002799DD File Offset: 0x00277BDD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tooltip.OnToolTip = new Func<string>(this.OnToolTip);
		this.RefreshChart();
	}

	// Token: 0x06006970 RID: 26992 RVA: 0x00279A04 File Offset: 0x00277C04
	private void OnClick()
	{
		this.lastClickTime = Time.unscaledTime;
		if (this.cachedPickupables == null)
		{
			this.cachedPickupables = ClusterManager.Instance.activeWorld.worldInventory.CreatePickupablesList(this.Resource);
			base.StartCoroutine(this.ClearCachedPickupablesAfterThreshold());
		}
		if (this.cachedPickupables == null)
		{
			return;
		}
		Pickupable pickupable = null;
		for (int i = 0; i < this.cachedPickupables.Count; i++)
		{
			this.selectionIdx++;
			int index = this.selectionIdx % this.cachedPickupables.Count;
			pickupable = this.cachedPickupables[index];
			if (pickupable != null && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
			{
				break;
			}
		}
		if (pickupable != null)
		{
			Transform transform = pickupable.transform;
			if (pickupable.storage != null)
			{
				transform = pickupable.storage.transform;
			}
			SelectTool.Instance.SelectAndFocus(transform.transform.GetPosition(), transform.GetComponent<KSelectable>(), Vector3.zero);
			for (int j = 0; j < this.cachedPickupables.Count; j++)
			{
				Pickupable pickupable2 = this.cachedPickupables[j];
				if (pickupable2 != null)
				{
					KAnimControllerBase component = pickupable2.GetComponent<KAnimControllerBase>();
					if (component != null)
					{
						component.HighlightColour = this.HighlightColor;
					}
				}
			}
		}
	}

	// Token: 0x06006971 RID: 26993 RVA: 0x00279B60 File Offset: 0x00277D60
	private IEnumerator ClearCachedPickupablesAfterThreshold()
	{
		while (this.cachedPickupables != null && this.lastClickTime != 0f && Time.unscaledTime - this.lastClickTime < 10f)
		{
			yield return SequenceUtil.WaitForSeconds(1f);
		}
		this.cachedPickupables = null;
		yield break;
	}

	// Token: 0x06006972 RID: 26994 RVA: 0x00279B70 File Offset: 0x00277D70
	public void GetAmounts(EdiblesManager.FoodInfo food_info, bool doExtras, out float available, out float total, out float reserved)
	{
		available = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(this.Resource, false);
		total = (doExtras ? ClusterManager.Instance.activeWorld.worldInventory.GetTotalAmount(this.Resource, false) : 0f);
		reserved = (doExtras ? MaterialNeeds.GetAmount(this.Resource, ClusterManager.Instance.activeWorldId, false) : 0f);
		if (food_info != null)
		{
			available *= food_info.CaloriesPerUnit;
			total *= food_info.CaloriesPerUnit;
			reserved *= food_info.CaloriesPerUnit;
		}
	}

	// Token: 0x06006973 RID: 26995 RVA: 0x00279C10 File Offset: 0x00277E10
	private void GetAmounts(bool doExtras, out float available, out float total, out float reserved)
	{
		EdiblesManager.FoodInfo food_info = (this.Measure == GameUtil.MeasureUnit.kcal) ? EdiblesManager.GetFoodInfo(this.Resource.Name) : null;
		this.GetAmounts(food_info, doExtras, out available, out total, out reserved);
	}

	// Token: 0x06006974 RID: 26996 RVA: 0x00279C48 File Offset: 0x00277E48
	public void UpdateValue()
	{
		this.SetName(this.Resource.ProperName());
		bool allowInsufficientMaterialBuild = GenericGameSettings.instance.allowInsufficientMaterialBuild;
		float num;
		float num2;
		float num3;
		this.GetAmounts(allowInsufficientMaterialBuild, out num, out num2, out num3);
		if (this.currentQuantity != num)
		{
			this.currentQuantity = num;
			this.QuantityLabel.text = ResourceCategoryScreen.QuantityTextForMeasure(num, this.Measure);
		}
		Color color = this.AvailableColor;
		if (num3 > num2)
		{
			color = this.OverdrawnColor;
		}
		else if (num == 0f)
		{
			color = this.UnavailableColor;
		}
		if (this.QuantityLabel.color != color)
		{
			this.QuantityLabel.color = color;
		}
		if (this.NameLabel.color != color)
		{
			this.NameLabel.color = color;
		}
	}

	// Token: 0x06006975 RID: 26997 RVA: 0x00279D10 File Offset: 0x00277F10
	private string OnToolTip()
	{
		float quantity;
		float quantity2;
		float quantity3;
		this.GetAmounts(true, out quantity, out quantity2, out quantity3);
		string text = this.NameLabel.text + "\n";
		text += string.Format(UI.RESOURCESCREEN.AVAILABLE_TOOLTIP, ResourceCategoryScreen.QuantityTextForMeasure(quantity, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity3, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity2, this.Measure));
		float delta = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.Resource).GetDelta(150f);
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

	// Token: 0x06006976 RID: 26998 RVA: 0x00279E06 File Offset: 0x00278006
	public void SetName(string name)
	{
		this.NameLabel.text = name;
	}

	// Token: 0x06006977 RID: 26999 RVA: 0x00279E14 File Offset: 0x00278014
	public void SetTag(Tag t, GameUtil.MeasureUnit measure)
	{
		this.Resource = t;
		this.Measure = measure;
		this.cachedPickupables = null;
	}

	// Token: 0x06006978 RID: 27000 RVA: 0x00279E2C File Offset: 0x0027802C
	private void Hover(bool is_hovering)
	{
		if (ClusterManager.Instance.activeWorld.worldInventory == null)
		{
			return;
		}
		if (is_hovering)
		{
			this.Background.color = this.BackgroundHoverColor;
		}
		else
		{
			this.Background.color = new Color(0f, 0f, 0f, 0f);
		}
		ICollection<Pickupable> pickupables = ClusterManager.Instance.activeWorld.worldInventory.GetPickupables(this.Resource, false);
		if (pickupables == null)
		{
			return;
		}
		foreach (Pickupable pickupable in pickupables)
		{
			if (!(pickupable == null))
			{
				KAnimControllerBase component = pickupable.GetComponent<KAnimControllerBase>();
				if (!(component == null))
				{
					if (is_hovering)
					{
						component.HighlightColour = this.HighlightColor;
					}
					else
					{
						component.HighlightColour = Color.black;
					}
				}
			}
		}
	}

	// Token: 0x06006979 RID: 27001 RVA: 0x00279F20 File Offset: 0x00278120
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.Hover(true);
	}

	// Token: 0x0600697A RID: 27002 RVA: 0x00279F29 File Offset: 0x00278129
	public void OnPointerExit(PointerEventData eventData)
	{
		this.Hover(false);
	}

	// Token: 0x0600697B RID: 27003 RVA: 0x00279F34 File Offset: 0x00278134
	public void SetSprite(Tag t)
	{
		Element element = ElementLoader.FindElementByName(this.Resource.Name);
		if (element != null)
		{
			Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(element.substance.anim, "ui", false, "");
			if (uispriteFromMultiObjectAnim != null)
			{
				this.image.sprite = uispriteFromMultiObjectAnim;
			}
		}
	}

	// Token: 0x0600697C RID: 27004 RVA: 0x00279F86 File Offset: 0x00278186
	public void SetSprite(Sprite sprite)
	{
		this.image.sprite = sprite;
	}

	// Token: 0x0600697D RID: 27005 RVA: 0x00279F94 File Offset: 0x00278194
	public void Sim4000ms(float dt)
	{
		this.RefreshChart();
	}

	// Token: 0x0600697E RID: 27006 RVA: 0x00279F9C File Offset: 0x0027819C
	private void RefreshChart()
	{
		if (this.sparkChart != null)
		{
			ResourceTracker resourceStatistic = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.Resource);
			this.sparkChart.GetComponentInChildren<LineLayer>().RefreshLine(resourceStatistic.ChartableData(3000f), "resourceAmount");
			this.sparkChart.GetComponentInChildren<SparkLayer>().SetColor(Constants.NEUTRAL_COLOR);
		}
	}

	// Token: 0x040047B9 RID: 18361
	public Tag Resource;

	// Token: 0x040047BA RID: 18362
	public GameUtil.MeasureUnit Measure;

	// Token: 0x040047BB RID: 18363
	public LocText NameLabel;

	// Token: 0x040047BC RID: 18364
	public LocText QuantityLabel;

	// Token: 0x040047BD RID: 18365
	public Image image;

	// Token: 0x040047BE RID: 18366
	[SerializeField]
	private Color AvailableColor;

	// Token: 0x040047BF RID: 18367
	[SerializeField]
	private Color UnavailableColor;

	// Token: 0x040047C0 RID: 18368
	[SerializeField]
	private Color OverdrawnColor;

	// Token: 0x040047C1 RID: 18369
	[SerializeField]
	private Color HighlightColor;

	// Token: 0x040047C2 RID: 18370
	[SerializeField]
	private Color BackgroundHoverColor;

	// Token: 0x040047C3 RID: 18371
	[SerializeField]
	private Image Background;

	// Token: 0x040047C4 RID: 18372
	[MyCmpGet]
	private ToolTip tooltip;

	// Token: 0x040047C5 RID: 18373
	[MyCmpReq]
	private Button button;

	// Token: 0x040047C6 RID: 18374
	public GameObject sparkChart;

	// Token: 0x040047C7 RID: 18375
	private const float CLICK_RESET_TIME_THRESHOLD = 10f;

	// Token: 0x040047C8 RID: 18376
	private int selectionIdx;

	// Token: 0x040047C9 RID: 18377
	private float lastClickTime;

	// Token: 0x040047CA RID: 18378
	private List<Pickupable> cachedPickupables;

	// Token: 0x040047CB RID: 18379
	private float currentQuantity = float.MinValue;
}
