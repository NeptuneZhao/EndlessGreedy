using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C11 RID: 3089
public class CodexConversionPanel : CodexWidget<CodexConversionPanel>
{
	// Token: 0x06005EA9 RID: 24233 RVA: 0x00232AA4 File Offset: 0x00230CA4
	public CodexConversionPanel(string title, Tag ctag, float inputAmount, bool inputContinuous, Tag ptag, float outputAmount, bool outputContinuous, GameObject converter) : this(title, ctag, inputAmount, inputContinuous, null, ptag, outputAmount, outputContinuous, null, converter)
	{
	}

	// Token: 0x06005EAA RID: 24234 RVA: 0x00232AC8 File Offset: 0x00230CC8
	public CodexConversionPanel(string title, Tag ctag, float inputAmount, bool inputContinuous, Func<Tag, float, bool, string> input_customFormating, Tag ptag, float outputAmount, bool outputContinuous, Func<Tag, float, bool, string> output_customFormating, GameObject converter)
	{
		this.title = title;
		this.ins = new ElementUsage[]
		{
			new ElementUsage(ctag, inputAmount, inputContinuous, input_customFormating)
		};
		this.outs = new ElementUsage[]
		{
			new ElementUsage(ptag, outputAmount, outputContinuous, output_customFormating)
		};
		this.Converter = converter;
	}

	// Token: 0x06005EAB RID: 24235 RVA: 0x00232B20 File Offset: 0x00230D20
	public CodexConversionPanel(string title, ElementUsage[] ins, ElementUsage[] outs, GameObject converter)
	{
		this.title = title;
		this.ins = ((ins != null) ? ins : new ElementUsage[0]);
		this.outs = ((outs != null) ? outs : new ElementUsage[0]);
		this.Converter = converter;
	}

	// Token: 0x06005EAC RID: 24236 RVA: 0x00232B5C File Offset: 0x00230D5C
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		this.label = component.GetReference<LocText>("Title");
		this.materialPrefab = component.GetReference<RectTransform>("MaterialPrefab").gameObject;
		this.fabricatorPrefab = component.GetReference<RectTransform>("FabricatorPrefab").gameObject;
		this.ingredientsContainer = component.GetReference<RectTransform>("IngredientsContainer").gameObject;
		this.resultsContainer = component.GetReference<RectTransform>("ResultsContainer").gameObject;
		this.fabricatorContainer = component.GetReference<RectTransform>("FabricatorContainer").gameObject;
		this.arrow1 = component.GetReference<RectTransform>("Arrow1").gameObject;
		this.arrow2 = component.GetReference<RectTransform>("Arrow2").gameObject;
		this.ClearPanel();
		this.ConfigureConversion();
	}

	// Token: 0x06005EAD RID: 24237 RVA: 0x00232C28 File Offset: 0x00230E28
	private global::Tuple<Sprite, Color> GetUISprite(Tag tag)
	{
		if (ElementLoader.GetElement(tag) != null)
		{
			return Def.GetUISprite(ElementLoader.GetElement(tag), "ui", false);
		}
		if (Assets.GetPrefab(tag) != null)
		{
			return Def.GetUISprite(Assets.GetPrefab(tag), "ui", false);
		}
		if (Assets.GetSprite(tag.Name) != null)
		{
			return new global::Tuple<Sprite, Color>(Assets.GetSprite(tag.Name), Color.white);
		}
		return null;
	}

	// Token: 0x06005EAE RID: 24238 RVA: 0x00232CA8 File Offset: 0x00230EA8
	private void ConfigureConversion()
	{
		this.label.text = this.title;
		bool active = false;
		ElementUsage[] array = this.ins;
		for (int i = 0; i < array.Length; i++)
		{
			ElementUsage elementUsage = array[i];
			Tag tag = elementUsage.tag;
			if (!(tag == Tag.Invalid))
			{
				float amount = elementUsage.amount;
				active = true;
				HierarchyReferences component = Util.KInstantiateUI(this.materialPrefab, this.ingredientsContainer, true).GetComponent<HierarchyReferences>();
				global::Tuple<Sprite, Color> uisprite = this.GetUISprite(tag);
				if (uisprite != null)
				{
					component.GetReference<Image>("Icon").sprite = uisprite.first;
					component.GetReference<Image>("Icon").color = uisprite.second;
				}
				GameUtil.TimeSlice timeSlice = elementUsage.continuous ? GameUtil.TimeSlice.PerCycle : GameUtil.TimeSlice.None;
				component.GetReference<LocText>("Amount").text = ((elementUsage.customFormating == null) ? GameUtil.GetFormattedByTag(tag, amount, timeSlice) : elementUsage.customFormating(tag, amount, elementUsage.continuous));
				component.GetReference<LocText>("Amount").color = Color.black;
				string text = tag.ProperName();
				GameObject prefab = Assets.GetPrefab(tag);
				if (prefab && prefab.GetComponent<Edible>() != null)
				{
					text = text + "\n    • " + string.Format(UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(prefab.GetComponent<Edible>().GetQuality()));
				}
				component.GetReference<ToolTip>("Tooltip").toolTip = text;
				component.GetReference<KButton>("Button").onClick += delegate()
				{
					ManagementMenu.Instance.codexScreen.ChangeArticle(UI.ExtractLinkID(tag.ProperName()), false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
				};
			}
		}
		this.arrow1.SetActive(active);
		string name = this.Converter.PrefabID().Name;
		HierarchyReferences component2 = Util.KInstantiateUI(this.fabricatorPrefab, this.fabricatorContainer, true).GetComponent<HierarchyReferences>();
		global::Tuple<Sprite, Color> uisprite2 = Def.GetUISprite(name, "ui", false);
		component2.GetReference<Image>("Icon").sprite = uisprite2.first;
		component2.GetReference<Image>("Icon").color = uisprite2.second;
		component2.GetReference<ToolTip>("Tooltip").toolTip = this.Converter.GetProperName();
		component2.GetReference<KButton>("Button").onClick += delegate()
		{
			ManagementMenu.Instance.codexScreen.ChangeArticle(UI.ExtractLinkID(this.Converter.GetProperName()), false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		};
		bool active2 = false;
		array = this.outs;
		for (int i = 0; i < array.Length; i++)
		{
			ElementUsage elementUsage2 = array[i];
			Tag tag = elementUsage2.tag;
			if (!(tag == Tag.Invalid))
			{
				float amount2 = elementUsage2.amount;
				active2 = true;
				HierarchyReferences component3 = Util.KInstantiateUI(this.materialPrefab, this.resultsContainer, true).GetComponent<HierarchyReferences>();
				global::Tuple<Sprite, Color> uisprite3 = this.GetUISprite(tag);
				if (uisprite3 != null)
				{
					component3.GetReference<Image>("Icon").sprite = uisprite3.first;
					component3.GetReference<Image>("Icon").color = uisprite3.second;
				}
				GameUtil.TimeSlice timeSlice2 = elementUsage2.continuous ? GameUtil.TimeSlice.PerCycle : GameUtil.TimeSlice.None;
				component3.GetReference<LocText>("Amount").text = ((elementUsage2.customFormating == null) ? GameUtil.GetFormattedByTag(tag, amount2, timeSlice2) : elementUsage2.customFormating(tag, amount2, elementUsage2.continuous));
				component3.GetReference<LocText>("Amount").color = Color.black;
				string text2 = tag.ProperName();
				GameObject prefab2 = Assets.GetPrefab(tag);
				if (prefab2 && prefab2.GetComponent<Edible>() != null)
				{
					text2 = text2 + "\n    • " + string.Format(UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(prefab2.GetComponent<Edible>().GetQuality()));
				}
				component3.GetReference<ToolTip>("Tooltip").toolTip = text2;
				component3.GetReference<KButton>("Button").onClick += delegate()
				{
					ManagementMenu.Instance.codexScreen.ChangeArticle(UI.ExtractLinkID(tag.ProperName()), false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
				};
			}
		}
		this.arrow2.SetActive(active2);
	}

	// Token: 0x06005EAF RID: 24239 RVA: 0x002330F4 File Offset: 0x002312F4
	private void ClearPanel()
	{
		foreach (object obj in this.ingredientsContainer.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj).gameObject);
		}
		foreach (object obj2 in this.resultsContainer.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj2).gameObject);
		}
		foreach (object obj3 in this.fabricatorContainer.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj3).gameObject);
		}
	}

	// Token: 0x04003F46 RID: 16198
	private LocText label;

	// Token: 0x04003F47 RID: 16199
	private GameObject materialPrefab;

	// Token: 0x04003F48 RID: 16200
	private GameObject fabricatorPrefab;

	// Token: 0x04003F49 RID: 16201
	private GameObject ingredientsContainer;

	// Token: 0x04003F4A RID: 16202
	private GameObject resultsContainer;

	// Token: 0x04003F4B RID: 16203
	private GameObject fabricatorContainer;

	// Token: 0x04003F4C RID: 16204
	private GameObject arrow1;

	// Token: 0x04003F4D RID: 16205
	private GameObject arrow2;

	// Token: 0x04003F4E RID: 16206
	private string title;

	// Token: 0x04003F4F RID: 16207
	private ElementUsage[] ins;

	// Token: 0x04003F50 RID: 16208
	private ElementUsage[] outs;

	// Token: 0x04003F51 RID: 16209
	private GameObject Converter;
}
