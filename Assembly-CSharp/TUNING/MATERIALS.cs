using System;
using System.Linq;
using STRINGS;

namespace TUNING
{
	// Token: 0x02000EE9 RID: 3817
	public class MATERIALS
	{
		// Token: 0x060076F5 RID: 30453 RVA: 0x002EC74C File Offset: 0x002EA94C
		public static string GetMaterialString(string materialCategory)
		{
			string[] array = materialCategory.Split('&', StringSplitOptions.None);
			string result;
			if (array.Length == 1)
			{
				result = UI.FormatAsLink(Strings.Get("STRINGS.MISC.TAGS." + materialCategory.ToUpper()), materialCategory);
			}
			else
			{
				result = string.Join(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.PREPARED_SEPARATOR, from s in array
				select UI.FormatAsLink(Strings.Get("STRINGS.MISC.TAGS." + s.ToUpper()), s));
			}
			return result;
		}

		// Token: 0x040056C9 RID: 22217
		public const string METAL = "Metal";

		// Token: 0x040056CA RID: 22218
		public const string REFINED_METAL = "RefinedMetal";

		// Token: 0x040056CB RID: 22219
		public const string GLASS = "Glass";

		// Token: 0x040056CC RID: 22220
		public const string TRANSPARENT = "Transparent";

		// Token: 0x040056CD RID: 22221
		public const string PLASTIC = "Plastic";

		// Token: 0x040056CE RID: 22222
		public const string BUILDABLERAW = "BuildableRaw";

		// Token: 0x040056CF RID: 22223
		public const string PRECIOUSROCK = "PreciousRock";

		// Token: 0x040056D0 RID: 22224
		public const string WOOD = "BuildingWood";

		// Token: 0x040056D1 RID: 22225
		public const string BUILDINGFIBER = "BuildingFiber";

		// Token: 0x040056D2 RID: 22226
		public const string LEAD = "Lead";

		// Token: 0x040056D3 RID: 22227
		public const string INSULATOR = "Insulator";

		// Token: 0x040056D4 RID: 22228
		public static readonly string[] ALL_METALS = new string[]
		{
			"Metal"
		};

		// Token: 0x040056D5 RID: 22229
		public static readonly string[] RAW_METALS = new string[]
		{
			"Metal"
		};

		// Token: 0x040056D6 RID: 22230
		public static readonly string[] REFINED_METALS = new string[]
		{
			"RefinedMetal"
		};

		// Token: 0x040056D7 RID: 22231
		public static readonly string[] ALLOYS = new string[]
		{
			"Alloy"
		};

		// Token: 0x040056D8 RID: 22232
		public static readonly string[] ALL_MINERALS = new string[]
		{
			"BuildableRaw"
		};

		// Token: 0x040056D9 RID: 22233
		public static readonly string[] RAW_MINERALS = new string[]
		{
			"BuildableRaw"
		};

		// Token: 0x040056DA RID: 22234
		public static readonly string[] RAW_MINERALS_OR_METALS = new string[]
		{
			"BuildableRaw&Metal"
		};

		// Token: 0x040056DB RID: 22235
		public static readonly string[] RAW_MINERALS_OR_WOOD = new string[]
		{
			"BuildableRaw&" + GameTags.BuildingWood.ToString()
		};

		// Token: 0x040056DC RID: 22236
		public static readonly string[] WOODS = new string[]
		{
			"BuildingWood"
		};

		// Token: 0x040056DD RID: 22237
		public static readonly string[] REFINED_MINERALS = new string[]
		{
			"BuildableProcessed"
		};

		// Token: 0x040056DE RID: 22238
		public static readonly string[] PRECIOUS_ROCKS = new string[]
		{
			"PreciousRock"
		};

		// Token: 0x040056DF RID: 22239
		public static readonly string[] FARMABLE = new string[]
		{
			"Farmable"
		};

		// Token: 0x040056E0 RID: 22240
		public static readonly string[] EXTRUDABLE = new string[]
		{
			"Extrudable"
		};

		// Token: 0x040056E1 RID: 22241
		public static readonly string[] PLUMBABLE = new string[]
		{
			"Plumbable"
		};

		// Token: 0x040056E2 RID: 22242
		public static readonly string[] PLUMBABLE_OR_METALS = new string[]
		{
			"Plumbable&Metal"
		};

		// Token: 0x040056E3 RID: 22243
		public static readonly string[] PLASTICS = new string[]
		{
			"Plastic"
		};

		// Token: 0x040056E4 RID: 22244
		public static readonly string[] GLASSES = new string[]
		{
			"Glass"
		};

		// Token: 0x040056E5 RID: 22245
		public static readonly string[] TRANSPARENTS = new string[]
		{
			"Transparent"
		};

		// Token: 0x040056E6 RID: 22246
		public static readonly string[] BUILDING_FIBER = new string[]
		{
			"BuildingFiber"
		};

		// Token: 0x040056E7 RID: 22247
		public static readonly string[] ANY_BUILDABLE = new string[]
		{
			"BuildableAny"
		};

		// Token: 0x040056E8 RID: 22248
		public static readonly string[] FLYING_CRITTER_FOOD = new string[]
		{
			"FlyingCritterEdible"
		};

		// Token: 0x040056E9 RID: 22249
		public static readonly string[] RADIATION_CONTAINMENT = new string[]
		{
			"Metal",
			"Lead"
		};
	}
}
