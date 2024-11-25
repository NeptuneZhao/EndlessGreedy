using System;
using System.Collections.Generic;
using ImGuiNET;
using STRINGS;

// Token: 0x02000633 RID: 1587
public class DevTool_StoryTrait_CritterManipulator : DevTool
{
	// Token: 0x0600270D RID: 9997 RVA: 0x000DE2D0 File Offset: 0x000DC4D0
	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.CollapsingHeader("Debug species lore unlock popup", ImGuiTreeNodeFlags.DefaultOpen))
		{
			this.Button_OpenSpecies(Tag.Invalid, "Unknown Species");
			ImGui.Separator();
			foreach (Tag species in this.GetCritterSpeciesTags())
			{
				this.Button_OpenSpecies(species, GravitasCreatureManipulatorConfig.GetNameForSpeciesTag(species).Unwrap());
			}
		}
	}

	// Token: 0x0600270E RID: 9998 RVA: 0x000DE350 File Offset: 0x000DC550
	public void Button_OpenSpecies(Tag species, string speciesName = null)
	{
		if (speciesName == null)
		{
			speciesName = species.Name;
		}
		else
		{
			speciesName = string.Format("\"{0}\" (ID: {1})", UI.StripLinkFormatting(speciesName), species);
		}
		if (ImGui.Button("Show popup for: " + speciesName))
		{
			GravitasCreatureManipulator.Instance.ShowLoreUnlockedPopup(species);
		}
	}

	// Token: 0x0600270F RID: 9999 RVA: 0x000DE390 File Offset: 0x000DC590
	public IEnumerable<Tag> GetCritterSpeciesTags()
	{
		yield return GameTags.Creatures.Species.HatchSpecies;
		yield return GameTags.Creatures.Species.LightBugSpecies;
		yield return GameTags.Creatures.Species.OilFloaterSpecies;
		yield return GameTags.Creatures.Species.DreckoSpecies;
		yield return GameTags.Creatures.Species.GlomSpecies;
		yield return GameTags.Creatures.Species.PuftSpecies;
		yield return GameTags.Creatures.Species.PacuSpecies;
		yield return GameTags.Creatures.Species.MooSpecies;
		yield return GameTags.Creatures.Species.MoleSpecies;
		yield return GameTags.Creatures.Species.SquirrelSpecies;
		yield return GameTags.Creatures.Species.CrabSpecies;
		yield return GameTags.Creatures.Species.DivergentSpecies;
		yield return GameTags.Creatures.Species.StaterpillarSpecies;
		yield return GameTags.Creatures.Species.BeetaSpecies;
		yield break;
	}
}
