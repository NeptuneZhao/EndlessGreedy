using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A51 RID: 2641
public class ResearchTypes
{
	// Token: 0x06004CA0 RID: 19616 RVA: 0x001B5E5C File Offset: 0x001B405C
	public ResearchTypes()
	{
		ResearchType item = new ResearchType("basic", RESEARCH.TYPES.ALPHA.NAME, RESEARCH.TYPES.ALPHA.DESC, Assets.GetSprite("research_type_alpha_icon"), new Color(0.59607846f, 0.6666667f, 0.9137255f), new Recipe.Ingredient[]
		{
			new Recipe.Ingredient("Dirt".ToTag(), 100f)
		}, 600f, "research_center_kanim", new string[]
		{
			"ResearchCenter"
		}, RESEARCH.TYPES.ALPHA.RECIPEDESC);
		this.Types.Add(item);
		ResearchType item2 = new ResearchType("advanced", RESEARCH.TYPES.BETA.NAME, RESEARCH.TYPES.BETA.DESC, Assets.GetSprite("research_type_beta_icon"), new Color(0.6f, 0.38431373f, 0.5686275f), new Recipe.Ingredient[]
		{
			new Recipe.Ingredient("Water".ToTag(), 25f)
		}, 1200f, "research_center_kanim", new string[]
		{
			"AdvancedResearchCenter"
		}, RESEARCH.TYPES.BETA.RECIPEDESC);
		this.Types.Add(item2);
		ResearchType item3 = new ResearchType("space", RESEARCH.TYPES.GAMMA.NAME, RESEARCH.TYPES.GAMMA.DESC, Assets.GetSprite("research_type_gamma_icon"), new Color32(240, 141, 44, byte.MaxValue), null, 2400f, "research_center_kanim", new string[]
		{
			"CosmicResearchCenter"
		}, RESEARCH.TYPES.GAMMA.RECIPEDESC);
		this.Types.Add(item3);
		ResearchType item4 = new ResearchType("nuclear", RESEARCH.TYPES.DELTA.NAME, RESEARCH.TYPES.DELTA.DESC, Assets.GetSprite("research_type_delta_icon"), new Color32(231, 210, 17, byte.MaxValue), null, 2400f, "research_center_kanim", new string[]
		{
			"NuclearResearchCenter"
		}, RESEARCH.TYPES.DELTA.RECIPEDESC);
		this.Types.Add(item4);
		ResearchType item5 = new ResearchType("orbital", RESEARCH.TYPES.ORBITAL.NAME, RESEARCH.TYPES.ORBITAL.DESC, Assets.GetSprite("research_type_orbital_icon"), new Color32(240, 141, 44, byte.MaxValue), null, 2400f, "research_center_kanim", new string[]
		{
			"OrbitalResearchCenter",
			"DLC1CosmicResearchCenter"
		}, RESEARCH.TYPES.ORBITAL.RECIPEDESC);
		this.Types.Add(item5);
	}

	// Token: 0x06004CA1 RID: 19617 RVA: 0x001B6120 File Offset: 0x001B4320
	public ResearchType GetResearchType(string id)
	{
		foreach (ResearchType researchType in this.Types)
		{
			if (id == researchType.id)
			{
				return researchType;
			}
		}
		global::Debug.LogWarning(string.Format("No research with type id {0} found", id));
		return null;
	}

	// Token: 0x040032F3 RID: 13043
	public List<ResearchType> Types = new List<ResearchType>();

	// Token: 0x02001A59 RID: 6745
	public class ID
	{
		// Token: 0x04007C1F RID: 31775
		public const string BASIC = "basic";

		// Token: 0x04007C20 RID: 31776
		public const string ADVANCED = "advanced";

		// Token: 0x04007C21 RID: 31777
		public const string SPACE = "space";

		// Token: 0x04007C22 RID: 31778
		public const string NUCLEAR = "nuclear";

		// Token: 0x04007C23 RID: 31779
		public const string ORBITAL = "orbital";
	}
}
