using System;
using Klei.AI;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E81 RID: 3713
	public class Spices : ResourceSet<Spice>
	{
		// Token: 0x060074F6 RID: 29942 RVA: 0x002DBEB4 File Offset: 0x002DA0B4
		public Spices(ResourceSet parent) : base("Spices", parent)
		{
			this.PreservingSpice = new Spice(this, "PRESERVING_SPICE", new Spice.Ingredient[]
			{
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						"BasicSingleHarvestPlantSeed"
					},
					AmountKG = 0.1f
				},
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						SimHashes.Salt.CreateTag()
					},
					AmountKG = 3f
				}
			}, new Color(0.961f, 0.827f, 0.29f), Color.white, new AttributeModifier("RotDelta", 0.5f, "Spices", false, false, true), null, "spice_recipe1", null);
			this.PilotingSpice = new Spice(this, "PILOTING_SPICE", new Spice.Ingredient[]
			{
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						"MushroomSeed"
					},
					AmountKG = 0.1f
				},
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						SimHashes.Sucrose.CreateTag()
					},
					AmountKG = 3f
				}
			}, new Color(0.039f, 0.725f, 0.831f), Color.white, null, new AttributeModifier("SpaceNavigation", 3f, "Spices", false, false, true), "spice_recipe2", DlcManager.AVAILABLE_EXPANSION1_ONLY);
			this.StrengthSpice = new Spice(this, "STRENGTH_SPICE", new Spice.Ingredient[]
			{
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						"SeaLettuceSeed"
					},
					AmountKG = 0.1f
				},
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						SimHashes.Iron.CreateTag()
					},
					AmountKG = 3f
				}
			}, new Color(0.588f, 0.278f, 0.788f), Color.white, null, new AttributeModifier("Strength", 3f, "Spices", false, false, true), "spice_recipe3", null);
			this.MachinerySpice = new Spice(this, "MACHINERY_SPICE", new Spice.Ingredient[]
			{
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						"PrickleFlowerSeed"
					},
					AmountKG = 0.1f
				},
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						SimHashes.SlimeMold.CreateTag()
					},
					AmountKG = 3f
				}
			}, new Color(0.788f, 0.443f, 0.792f), Color.white, null, new AttributeModifier("Machinery", 3f, "Spices", false, false, true), "spice_recipe4", null);
		}

		// Token: 0x040054CC RID: 21708
		public Spice PreservingSpice;

		// Token: 0x040054CD RID: 21709
		public Spice PilotingSpice;

		// Token: 0x040054CE RID: 21710
		public Spice StrengthSpice;

		// Token: 0x040054CF RID: 21711
		public Spice MachinerySpice;
	}
}
