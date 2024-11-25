using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000A89 RID: 2697
public class MaterialsAvailable : SelectModuleCondition
{
	// Token: 0x06004F35 RID: 20277 RVA: 0x001C7D0A File Offset: 0x001C5F0A
	public override bool IgnoreInSanboxMode()
	{
		return true;
	}

	// Token: 0x06004F36 RID: 20278 RVA: 0x001C7D0D File Offset: 0x001C5F0D
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		return existingModule == null || ProductInfoScreen.MaterialsMet(selectedPart.CraftRecipe);
	}

	// Token: 0x06004F37 RID: 20279 RVA: 0x001C7D28 File Offset: 0x001C5F28
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MATERIALS_AVAILABLE.COMPLETE;
		}
		string text = UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MATERIALS_AVAILABLE.FAILED;
		foreach (Recipe.Ingredient ingredient in selectedPart.CraftRecipe.Ingredients)
		{
			string str = "\n" + string.Format("{0}{1}: {2}", "    • ", ingredient.tag.ProperName(), GameUtil.GetFormattedMass(ingredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			text += str;
		}
		return text;
	}
}
