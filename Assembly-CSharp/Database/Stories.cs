using System;
using System.Collections.Generic;
using ProcGen;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E87 RID: 3719
	public class Stories : ResourceSet<Story>
	{
		// Token: 0x06007501 RID: 29953 RVA: 0x002DC564 File Offset: 0x002DA764
		public Stories(ResourceSet parent) : base("Stories", parent)
		{
			this.MegaBrainTank = base.Add(new Story("MegaBrainTank", "storytraits/MegaBrainTank", 0, 1, 43, "storytraits/mega_brain_tank").SetKeepsake("keepsake_megabrain"));
			this.CreatureManipulator = base.Add(new Story("CreatureManipulator", "storytraits/CritterManipulator", 1, 2, 43, "storytraits/creature_manipulator_retrofit").SetKeepsake("keepsake_crittermanipulator"));
			this.LonelyMinion = base.Add(new Story("LonelyMinion", "storytraits/LonelyMinion", 2, 3, 44, "storytraits/lonelyminion_retrofit").SetKeepsake("keepsake_lonelyminion"));
			this.FossilHunt = base.Add(new Story("FossilHunt", "storytraits/FossilHunt", 3, 4, 44, "storytraits/fossil_hunt_retrofit").SetKeepsake("keepsake_fossilhunt"));
			this.MorbRoverMaker = base.Add(new Story("MorbRoverMaker", "storytraits/MorbRoverMaker", 4, 5, 50, "storytraits/morb_rover_maker_retrofit").SetKeepsake("keepsake_morbrovermaker"));
			this.resources.Sort();
		}

		// Token: 0x06007502 RID: 29954 RVA: 0x002DC66E File Offset: 0x002DA86E
		public void AddStoryMod(Story mod)
		{
			mod.kleiUseOnlyCoordinateOrder = -1;
			base.Add(mod);
			this.resources.Sort();
		}

		// Token: 0x06007503 RID: 29955 RVA: 0x002DC68C File Offset: 0x002DA88C
		public int GetHighestCoordinate()
		{
			int num = 0;
			foreach (Story story in this.resources)
			{
				num = Mathf.Max(num, story.kleiUseOnlyCoordinateOrder);
			}
			return num;
		}

		// Token: 0x06007504 RID: 29956 RVA: 0x002DC6E8 File Offset: 0x002DA8E8
		public WorldTrait GetStoryTrait(string id, bool assertMissingTrait = false)
		{
			Story story = this.resources.Find((Story x) => x.Id == id);
			if (story != null)
			{
				return SettingsCache.GetCachedStoryTrait(story.worldgenStoryTraitKey, assertMissingTrait);
			}
			return null;
		}

		// Token: 0x06007505 RID: 29957 RVA: 0x002DC72C File Offset: 0x002DA92C
		public Story GetStoryFromStoryTrait(string storyTraitTemplate)
		{
			return this.resources.Find((Story x) => x.worldgenStoryTraitKey == storyTraitTemplate);
		}

		// Token: 0x06007506 RID: 29958 RVA: 0x002DC75D File Offset: 0x002DA95D
		public List<Story> GetStoriesSortedByCoordinateOrder()
		{
			List<Story> list = new List<Story>(this.resources);
			list.Sort((Story s1, Story s2) => s1.kleiUseOnlyCoordinateOrder.CompareTo(s2.kleiUseOnlyCoordinateOrder));
			return list;
		}

		// Token: 0x040054ED RID: 21741
		public Story MegaBrainTank;

		// Token: 0x040054EE RID: 21742
		public Story CreatureManipulator;

		// Token: 0x040054EF RID: 21743
		public Story LonelyMinion;

		// Token: 0x040054F0 RID: 21744
		public Story FossilHunt;

		// Token: 0x040054F1 RID: 21745
		public Story MorbRoverMaker;
	}
}
