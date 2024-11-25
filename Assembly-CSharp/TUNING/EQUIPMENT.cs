using System;

namespace TUNING
{
	// Token: 0x02000F02 RID: 3842
	public class EQUIPMENT
	{
		// Token: 0x02001FF1 RID: 8177
		public class TOYS
		{
			// Token: 0x0400914A RID: 37194
			public static string SLOT = "Toy";

			// Token: 0x0400914B RID: 37195
			public static float BALLOON_MASS = 1f;
		}

		// Token: 0x02001FF2 RID: 8178
		public class ATTRIBUTE_MOD_IDS
		{
			// Token: 0x0400914C RID: 37196
			public static string DECOR = "Decor";

			// Token: 0x0400914D RID: 37197
			public static string INSULATION = "Insulation";

			// Token: 0x0400914E RID: 37198
			public static string ATHLETICS = "Athletics";

			// Token: 0x0400914F RID: 37199
			public static string DIGGING = "Digging";

			// Token: 0x04009150 RID: 37200
			public static string MAX_UNDERWATER_TRAVELCOST = "MaxUnderwaterTravelCost";

			// Token: 0x04009151 RID: 37201
			public static string THERMAL_CONDUCTIVITY_BARRIER = "ThermalConductivityBarrier";
		}

		// Token: 0x02001FF3 RID: 8179
		public class TOOLS
		{
			// Token: 0x04009152 RID: 37202
			public static string TOOLSLOT = "Multitool";

			// Token: 0x04009153 RID: 37203
			public static string TOOLFABRICATOR = "MultitoolWorkbench";

			// Token: 0x04009154 RID: 37204
			public static string TOOL_ANIM = "constructor_gun_kanim";
		}

		// Token: 0x02001FF4 RID: 8180
		public class CLOTHING
		{
			// Token: 0x04009155 RID: 37205
			public static string SLOT = "Outfit";
		}

		// Token: 0x02001FF5 RID: 8181
		public class SUITS
		{
			// Token: 0x04009156 RID: 37206
			public static string SLOT = "Suit";

			// Token: 0x04009157 RID: 37207
			public static string FABRICATOR = "SuitFabricator";

			// Token: 0x04009158 RID: 37208
			public static string ANIM = "clothing_kanim";

			// Token: 0x04009159 RID: 37209
			public static string SNAPON = "snapTo_neck";

			// Token: 0x0400915A RID: 37210
			public static float SUIT_DURABILITY_SKILL_BONUS = 0.25f;

			// Token: 0x0400915B RID: 37211
			public static int OXYMASK_FABTIME = 20;

			// Token: 0x0400915C RID: 37212
			public static int ATMOSUIT_FABTIME = 40;

			// Token: 0x0400915D RID: 37213
			public static int ATMOSUIT_INSULATION = 50;

			// Token: 0x0400915E RID: 37214
			public static int ATMOSUIT_ATHLETICS = -6;

			// Token: 0x0400915F RID: 37215
			public static float ATMOSUIT_THERMAL_CONDUCTIVITY_BARRIER = 0.2f;

			// Token: 0x04009160 RID: 37216
			public static int ATMOSUIT_DIGGING = 10;

			// Token: 0x04009161 RID: 37217
			public static int ATMOSUIT_CONSTRUCTION = 10;

			// Token: 0x04009162 RID: 37218
			public static float ATMOSUIT_BLADDER = -0.18333334f;

			// Token: 0x04009163 RID: 37219
			public static int ATMOSUIT_MASS = 200;

			// Token: 0x04009164 RID: 37220
			public static int ATMOSUIT_SCALDING = 1000;

			// Token: 0x04009165 RID: 37221
			public static int ATMOSUIT_SCOLDING = -1000;

			// Token: 0x04009166 RID: 37222
			public static float ATMOSUIT_DECAY = -0.1f;

			// Token: 0x04009167 RID: 37223
			public static float LEADSUIT_THERMAL_CONDUCTIVITY_BARRIER = 0.3f;

			// Token: 0x04009168 RID: 37224
			public static int LEADSUIT_SCALDING = 1000;

			// Token: 0x04009169 RID: 37225
			public static int LEADSUIT_SCOLDING = -1000;

			// Token: 0x0400916A RID: 37226
			public static int LEADSUIT_INSULATION = 50;

			// Token: 0x0400916B RID: 37227
			public static int LEADSUIT_STRENGTH = 10;

			// Token: 0x0400916C RID: 37228
			public static int LEADSUIT_ATHLETICS = -8;

			// Token: 0x0400916D RID: 37229
			public static float LEADSUIT_RADIATION_SHIELDING = 0.66f;

			// Token: 0x0400916E RID: 37230
			public static int AQUASUIT_FABTIME = EQUIPMENT.SUITS.ATMOSUIT_FABTIME;

			// Token: 0x0400916F RID: 37231
			public static int AQUASUIT_INSULATION = 0;

			// Token: 0x04009170 RID: 37232
			public static int AQUASUIT_ATHLETICS = EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS;

			// Token: 0x04009171 RID: 37233
			public static int AQUASUIT_MASS = EQUIPMENT.SUITS.ATMOSUIT_MASS;

			// Token: 0x04009172 RID: 37234
			public static int AQUASUIT_UNDERWATER_TRAVELCOST = 6;

			// Token: 0x04009173 RID: 37235
			public static int TEMPERATURESUIT_FABTIME = EQUIPMENT.SUITS.ATMOSUIT_FABTIME;

			// Token: 0x04009174 RID: 37236
			public static float TEMPERATURESUIT_INSULATION = 0.2f;

			// Token: 0x04009175 RID: 37237
			public static int TEMPERATURESUIT_ATHLETICS = EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS;

			// Token: 0x04009176 RID: 37238
			public static int TEMPERATURESUIT_MASS = EQUIPMENT.SUITS.ATMOSUIT_MASS;

			// Token: 0x04009177 RID: 37239
			public const int OXYGEN_MASK_MASS = 15;

			// Token: 0x04009178 RID: 37240
			public static int OXYGEN_MASK_ATHLETICS = -2;

			// Token: 0x04009179 RID: 37241
			public static float OXYGEN_MASK_DECAY = -0.2f;

			// Token: 0x0400917A RID: 37242
			public static float INDESTRUCTIBLE_DURABILITY_MOD = 0f;

			// Token: 0x0400917B RID: 37243
			public static float REINFORCED_DURABILITY_MOD = 0.5f;

			// Token: 0x0400917C RID: 37244
			public static float FLIMSY_DURABILITY_MOD = 1.5f;

			// Token: 0x0400917D RID: 37245
			public static float THREADBARE_DURABILITY_MOD = 2f;

			// Token: 0x0400917E RID: 37246
			public static float MINIMUM_USABLE_SUIT_CHARGE = 0.95f;
		}

		// Token: 0x02001FF6 RID: 8182
		public class VESTS
		{
			// Token: 0x0400917F RID: 37247
			public static string SLOT = "Suit";

			// Token: 0x04009180 RID: 37248
			public static string FABRICATOR = "ClothingFabricator";

			// Token: 0x04009181 RID: 37249
			public static string SNAPON0 = "snapTo_body";

			// Token: 0x04009182 RID: 37250
			public static string SNAPON1 = "snapTo_arm";

			// Token: 0x04009183 RID: 37251
			public static string WARM_VEST_ANIM0 = "body_shirt_hot_shearling_kanim";

			// Token: 0x04009184 RID: 37252
			public static string WARM_VEST_ICON0 = "shirt_hot_shearling_kanim";

			// Token: 0x04009185 RID: 37253
			public static float WARM_VEST_FABTIME = 180f;

			// Token: 0x04009186 RID: 37254
			public static float WARM_VEST_INSULATION = 0.01f;

			// Token: 0x04009187 RID: 37255
			public static int WARM_VEST_MASS = 4;

			// Token: 0x04009188 RID: 37256
			public static float COOL_VEST_FABTIME = EQUIPMENT.VESTS.WARM_VEST_FABTIME;

			// Token: 0x04009189 RID: 37257
			public static float COOL_VEST_INSULATION = 0.01f;

			// Token: 0x0400918A RID: 37258
			public static int COOL_VEST_MASS = EQUIPMENT.VESTS.WARM_VEST_MASS;

			// Token: 0x0400918B RID: 37259
			public static float FUNKY_VEST_FABTIME = EQUIPMENT.VESTS.WARM_VEST_FABTIME;

			// Token: 0x0400918C RID: 37260
			public static float FUNKY_VEST_DECOR = 1f;

			// Token: 0x0400918D RID: 37261
			public static int FUNKY_VEST_MASS = EQUIPMENT.VESTS.WARM_VEST_MASS;

			// Token: 0x0400918E RID: 37262
			public static float CUSTOM_CLOTHING_FABTIME = 180f;

			// Token: 0x0400918F RID: 37263
			public static float CUSTOM_ATMOSUIT_FABTIME = 15f;

			// Token: 0x04009190 RID: 37264
			public static int CUSTOM_CLOTHING_MASS = EQUIPMENT.VESTS.WARM_VEST_MASS + 3;
		}
	}
}
