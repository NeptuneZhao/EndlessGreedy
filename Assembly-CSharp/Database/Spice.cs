using System;
using Klei.AI;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E80 RID: 3712
	public class Spice : Resource
	{
		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x060074E7 RID: 29927 RVA: 0x002DBDAF File Offset: 0x002D9FAF
		// (set) Token: 0x060074E8 RID: 29928 RVA: 0x002DBDB7 File Offset: 0x002D9FB7
		public AttributeModifier StatBonus { get; private set; }

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x060074E9 RID: 29929 RVA: 0x002DBDC0 File Offset: 0x002D9FC0
		// (set) Token: 0x060074EA RID: 29930 RVA: 0x002DBDC8 File Offset: 0x002D9FC8
		public AttributeModifier FoodModifier { get; private set; }

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x060074EB RID: 29931 RVA: 0x002DBDD1 File Offset: 0x002D9FD1
		// (set) Token: 0x060074EC RID: 29932 RVA: 0x002DBDD9 File Offset: 0x002D9FD9
		public AttributeModifier CalorieModifier { get; private set; }

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x060074ED RID: 29933 RVA: 0x002DBDE2 File Offset: 0x002D9FE2
		// (set) Token: 0x060074EE RID: 29934 RVA: 0x002DBDEA File Offset: 0x002D9FEA
		public Color PrimaryColor { get; private set; }

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x060074EF RID: 29935 RVA: 0x002DBDF3 File Offset: 0x002D9FF3
		// (set) Token: 0x060074F0 RID: 29936 RVA: 0x002DBDFB File Offset: 0x002D9FFB
		public Color SecondaryColor { get; private set; }

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x060074F2 RID: 29938 RVA: 0x002DBE0D File Offset: 0x002DA00D
		// (set) Token: 0x060074F1 RID: 29937 RVA: 0x002DBE04 File Offset: 0x002DA004
		public string Image { get; private set; }

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x060074F4 RID: 29940 RVA: 0x002DBE1E File Offset: 0x002DA01E
		// (set) Token: 0x060074F3 RID: 29939 RVA: 0x002DBE15 File Offset: 0x002DA015
		public string[] DlcIds { get; private set; } = DlcManager.AVAILABLE_ALL_VERSIONS;

		// Token: 0x060074F5 RID: 29941 RVA: 0x002DBE28 File Offset: 0x002DA028
		public Spice(ResourceSet parent, string id, Spice.Ingredient[] ingredients, Color primaryColor, Color secondaryColor, AttributeModifier foodMod = null, AttributeModifier statBonus = null, string imageName = "unknown", string[] dlcID = null) : base(id, parent, null)
		{
			if (dlcID != null)
			{
				this.DlcIds = dlcID;
			}
			this.StatBonus = statBonus;
			this.FoodModifier = foodMod;
			this.Ingredients = ingredients;
			this.Image = imageName;
			this.PrimaryColor = primaryColor;
			this.SecondaryColor = secondaryColor;
			for (int i = 0; i < this.Ingredients.Length; i++)
			{
				this.TotalKG += this.Ingredients[i].AmountKG;
			}
		}

		// Token: 0x040054C8 RID: 21704
		public readonly Spice.Ingredient[] Ingredients;

		// Token: 0x040054C9 RID: 21705
		public readonly float TotalKG;

		// Token: 0x02001F78 RID: 8056
		public class Ingredient : IConfigurableConsumerIngredient
		{
			// Token: 0x0600AF45 RID: 44869 RVA: 0x003B10D7 File Offset: 0x003AF2D7
			public float GetAmount()
			{
				return this.AmountKG;
			}

			// Token: 0x0600AF46 RID: 44870 RVA: 0x003B10DF File Offset: 0x003AF2DF
			public Tag[] GetIDSets()
			{
				return this.IngredientSet;
			}

			// Token: 0x04008EB9 RID: 36537
			public Tag[] IngredientSet;

			// Token: 0x04008EBA RID: 36538
			public float AmountKG;
		}
	}
}
