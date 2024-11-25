using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C0D RID: 3085
public class CodexConfigurableConsumerRecipePanel : CodexWidget<CodexConfigurableConsumerRecipePanel>
{
	// Token: 0x06005E93 RID: 24211 RVA: 0x002318E9 File Offset: 0x0022FAE9
	public CodexConfigurableConsumerRecipePanel(IConfigurableConsumerOption data)
	{
		this.data = data;
	}

	// Token: 0x06005E94 RID: 24212 RVA: 0x002318F8 File Offset: 0x0022FAF8
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		this.title = component.GetReference<LocText>("Title");
		this.result_description = component.GetReference<LocText>("ResultDescription");
		this.resultIcon = component.GetReference<Image>("ResultIcon");
		this.ingredient_original = component.GetReference<RectTransform>("IngredientPrefab").gameObject;
		this.ingredient_original.SetActive(false);
		CodexText codexText = new CodexText();
		LocText reference = this.ingredient_original.GetComponent<HierarchyReferences>().GetReference<LocText>("Name");
		codexText.ConfigureLabel(reference, textStyles);
		this.Clear();
		if (this.data != null)
		{
			this.title.text = this.data.GetName();
			this.result_description.text = this.data.GetDescription();
			this.result_description.color = Color.black;
			this.resultIcon.sprite = this.data.GetIcon();
			IConfigurableConsumerIngredient[] ingredients = this.data.GetIngredients();
			this._ingredientRows = new GameObject[ingredients.Length];
			for (int i = 0; i < this._ingredientRows.Length; i++)
			{
				this._ingredientRows[i] = this.CreateIngredientRow(ingredients[i]);
			}
		}
	}

	// Token: 0x06005E95 RID: 24213 RVA: 0x00231A24 File Offset: 0x0022FC24
	public GameObject CreateIngredientRow(IConfigurableConsumerIngredient ingredient)
	{
		Tag[] idsets = ingredient.GetIDSets();
		if (this.ingredient_original != null && idsets.Length != 0)
		{
			GameObject gameObject = Util.KInstantiateUI(this.ingredient_original, this.ingredient_original.transform.parent.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(idsets[0], "ui", false);
			component.GetReference<Image>("Icon").sprite = uisprite.first;
			component.GetReference<Image>("Icon").color = uisprite.second;
			component.GetReference<LocText>("Name").text = idsets[0].ProperName();
			component.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(idsets[0], ingredient.GetAmount(), GameUtil.TimeSlice.None);
			component.GetReference<LocText>("Amount").color = Color.black;
			return gameObject;
		}
		return null;
	}

	// Token: 0x06005E96 RID: 24214 RVA: 0x00231B10 File Offset: 0x0022FD10
	public void Clear()
	{
		if (this._ingredientRows != null)
		{
			for (int i = 0; i < this._ingredientRows.Length; i++)
			{
				UnityEngine.Object.Destroy(this._ingredientRows[i]);
			}
			this._ingredientRows = null;
		}
	}

	// Token: 0x04003F2B RID: 16171
	private LocText title;

	// Token: 0x04003F2C RID: 16172
	private LocText result_description;

	// Token: 0x04003F2D RID: 16173
	private Image resultIcon;

	// Token: 0x04003F2E RID: 16174
	private GameObject ingredient_original;

	// Token: 0x04003F2F RID: 16175
	private IConfigurableConsumerOption data;

	// Token: 0x04003F30 RID: 16176
	private GameObject[] _ingredientRows;
}
