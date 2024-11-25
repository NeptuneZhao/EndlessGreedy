using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D27 RID: 3367
public class ResourceRemainingDisplayScreen : KScreen
{
	// Token: 0x06006980 RID: 27008 RVA: 0x0027A01A File Offset: 0x0027821A
	public static void DestroyInstance()
	{
		ResourceRemainingDisplayScreen.instance = null;
	}

	// Token: 0x06006981 RID: 27009 RVA: 0x0027A022 File Offset: 0x00278222
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Activate();
		ResourceRemainingDisplayScreen.instance = this;
		this.dispayPrefab.SetActive(false);
	}

	// Token: 0x06006982 RID: 27010 RVA: 0x0027A042 File Offset: 0x00278242
	public void ActivateDisplay(GameObject target)
	{
		this.numberOfPendingConstructions = 0;
		this.dispayPrefab.SetActive(true);
	}

	// Token: 0x06006983 RID: 27011 RVA: 0x0027A057 File Offset: 0x00278257
	public void DeactivateDisplay()
	{
		this.dispayPrefab.SetActive(false);
	}

	// Token: 0x06006984 RID: 27012 RVA: 0x0027A068 File Offset: 0x00278268
	public void SetResources(IList<Tag> _selected_elements, Recipe recipe)
	{
		this.selected_elements.Clear();
		foreach (Tag item in _selected_elements)
		{
			this.selected_elements.Add(item);
		}
		this.currentRecipe = recipe;
		global::Debug.Assert(this.selected_elements.Count == recipe.Ingredients.Count, string.Format("{0} Mismatch number of selected elements {1} and recipe requirements {2}", recipe.Name, this.selected_elements.Count, recipe.Ingredients.Count));
	}

	// Token: 0x06006985 RID: 27013 RVA: 0x0027A114 File Offset: 0x00278314
	public void SetNumberOfPendingConstructions(int number)
	{
		this.numberOfPendingConstructions = number;
	}

	// Token: 0x06006986 RID: 27014 RVA: 0x0027A120 File Offset: 0x00278320
	public void Update()
	{
		if (!this.dispayPrefab.activeSelf)
		{
			return;
		}
		if (base.canvas != null)
		{
			if (this.rect == null)
			{
				this.rect = base.GetComponent<RectTransform>();
			}
			this.rect.anchoredPosition = base.WorldToScreen(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
		}
		if (this.displayedConstructionCostMultiplier == this.numberOfPendingConstructions)
		{
			this.label.text = "";
			return;
		}
		this.displayedConstructionCostMultiplier = this.numberOfPendingConstructions;
	}

	// Token: 0x06006987 RID: 27015 RVA: 0x0027A1B0 File Offset: 0x002783B0
	public string GetString()
	{
		string text = "";
		if (this.selected_elements != null && this.currentRecipe != null)
		{
			for (int i = 0; i < this.currentRecipe.Ingredients.Count; i++)
			{
				Tag tag = this.selected_elements[i];
				float num = this.currentRecipe.Ingredients[i].amount * (float)this.numberOfPendingConstructions;
				float num2 = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag, true);
				num2 -= num;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				string text2 = tag.ProperName();
				if (MaterialSelector.DeprioritizeAutoSelectElementList.Contains(tag) && MaterialSelector.GetValidMaterials(this.currentRecipe.Ingredients[i].tag, false).Count > 1)
				{
					text2 = string.Concat(new string[]
					{
						"<b>",
						UIConstants.ColorPrefixYellow,
						text2,
						UIConstants.ColorSuffix,
						"</b>"
					});
				}
				text = string.Concat(new string[]
				{
					text,
					text2,
					": ",
					GameUtil.GetFormattedMass(num2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"),
					" / ",
					GameUtil.GetFormattedMass(this.currentRecipe.Ingredients[i].amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
				});
				if (i < this.selected_elements.Count - 1)
				{
					text += "\n";
				}
			}
		}
		return text;
	}

	// Token: 0x040047CC RID: 18380
	public static ResourceRemainingDisplayScreen instance;

	// Token: 0x040047CD RID: 18381
	public GameObject dispayPrefab;

	// Token: 0x040047CE RID: 18382
	public LocText label;

	// Token: 0x040047CF RID: 18383
	private Recipe currentRecipe;

	// Token: 0x040047D0 RID: 18384
	private List<Tag> selected_elements = new List<Tag>();

	// Token: 0x040047D1 RID: 18385
	private int numberOfPendingConstructions;

	// Token: 0x040047D2 RID: 18386
	private int displayedConstructionCostMultiplier;

	// Token: 0x040047D3 RID: 18387
	private RectTransform rect;
}
