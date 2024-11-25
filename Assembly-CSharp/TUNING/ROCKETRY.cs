using System;
using UnityEngine;

namespace TUNING
{
	// Token: 0x02000F05 RID: 3845
	public class ROCKETRY
	{
		// Token: 0x06007733 RID: 30515 RVA: 0x002F4AD2 File Offset: 0x002F2CD2
		public static float MassFromPenaltyPercentage(float penaltyPercentage = 0.5f)
		{
			return -(1f / Mathf.Pow(penaltyPercentage - 1f, 5f));
		}

		// Token: 0x06007734 RID: 30516 RVA: 0x002F4AEC File Offset: 0x002F2CEC
		public static float CalculateMassWithPenalty(float realMass)
		{
			float b = Mathf.Pow(realMass / ROCKETRY.MASS_PENALTY_DIVISOR, ROCKETRY.MASS_PENALTY_EXPONENT);
			return Mathf.Max(realMass, b);
		}

		// Token: 0x04005855 RID: 22613
		public static float MISSION_DURATION_SCALE = 1800f;

		// Token: 0x04005856 RID: 22614
		public static float MASS_PENALTY_EXPONENT = 3.2f;

		// Token: 0x04005857 RID: 22615
		public static float MASS_PENALTY_DIVISOR = 300f;

		// Token: 0x04005858 RID: 22616
		public const float SELF_DESTRUCT_REFUND_FACTOR = 0.5f;

		// Token: 0x04005859 RID: 22617
		public static float CARGO_CAPACITY_SCALE = 10f;

		// Token: 0x0400585A RID: 22618
		public static float LIQUID_CARGO_BAY_CLUSTER_CAPACITY = 2700f;

		// Token: 0x0400585B RID: 22619
		public static float SOLID_CARGO_BAY_CLUSTER_CAPACITY = 2700f;

		// Token: 0x0400585C RID: 22620
		public static float GAS_CARGO_BAY_CLUSTER_CAPACITY = 1100f;

		// Token: 0x0400585D RID: 22621
		public const float ENTITIES_CARGO_BAY_CLUSTER_CAPACITY = 100f;

		// Token: 0x0400585E RID: 22622
		public static Vector2I ROCKET_INTERIOR_SIZE = new Vector2I(32, 32);

		// Token: 0x02001FF9 RID: 8185
		public class DESTINATION_RESEARCH
		{
			// Token: 0x0400919A RID: 37274
			public static int EVERGREEN = 10;

			// Token: 0x0400919B RID: 37275
			public static int BASIC = 50;

			// Token: 0x0400919C RID: 37276
			public static int HIGH = 150;
		}

		// Token: 0x02001FFA RID: 8186
		public class DESTINATION_ANALYSIS
		{
			// Token: 0x0400919D RID: 37277
			public static int DISCOVERED = 50;

			// Token: 0x0400919E RID: 37278
			public static int COMPLETE = 100;

			// Token: 0x0400919F RID: 37279
			public static float DEFAULT_CYCLES_PER_DISCOVERY = 0.5f;
		}

		// Token: 0x02001FFB RID: 8187
		public class DESTINATION_THRUST_COSTS
		{
			// Token: 0x040091A0 RID: 37280
			public static int LOW = 3;

			// Token: 0x040091A1 RID: 37281
			public static int MID = 5;

			// Token: 0x040091A2 RID: 37282
			public static int HIGH = 7;

			// Token: 0x040091A3 RID: 37283
			public static int VERY_HIGH = 9;
		}

		// Token: 0x02001FFC RID: 8188
		public class CLUSTER_FOW
		{
			// Token: 0x040091A4 RID: 37284
			public static float POINTS_TO_REVEAL = 100f;

			// Token: 0x040091A5 RID: 37285
			public static float DEFAULT_CYCLES_PER_REVEAL = 0.5f;
		}

		// Token: 0x02001FFD RID: 8189
		public class ENGINE_EFFICIENCY
		{
			// Token: 0x040091A6 RID: 37286
			public static float WEAK = 20f;

			// Token: 0x040091A7 RID: 37287
			public static float MEDIUM = 40f;

			// Token: 0x040091A8 RID: 37288
			public static float STRONG = 60f;

			// Token: 0x040091A9 RID: 37289
			public static float BOOSTER = 30f;
		}

		// Token: 0x02001FFE RID: 8190
		public class ROCKET_HEIGHT
		{
			// Token: 0x040091AA RID: 37290
			public static int VERY_SHORT = 10;

			// Token: 0x040091AB RID: 37291
			public static int SHORT = 16;

			// Token: 0x040091AC RID: 37292
			public static int MEDIUM = 20;

			// Token: 0x040091AD RID: 37293
			public static int TALL = 25;

			// Token: 0x040091AE RID: 37294
			public static int VERY_TALL = 35;

			// Token: 0x040091AF RID: 37295
			public static int MAX_MODULE_STACK_HEIGHT = ROCKETRY.ROCKET_HEIGHT.VERY_TALL - 5;
		}

		// Token: 0x02001FFF RID: 8191
		public class OXIDIZER_EFFICIENCY
		{
			// Token: 0x040091B0 RID: 37296
			public static float VERY_LOW = 0.334f;

			// Token: 0x040091B1 RID: 37297
			public static float LOW = 1f;

			// Token: 0x040091B2 RID: 37298
			public static float HIGH = 1.33f;
		}

		// Token: 0x02002000 RID: 8192
		public class DLC1_OXIDIZER_EFFICIENCY
		{
			// Token: 0x040091B3 RID: 37299
			public static float VERY_LOW = 1f;

			// Token: 0x040091B4 RID: 37300
			public static float LOW = 2f;

			// Token: 0x040091B5 RID: 37301
			public static float HIGH = 4f;
		}

		// Token: 0x02002001 RID: 8193
		public class CARGO_CONTAINER_MASS
		{
			// Token: 0x040091B6 RID: 37302
			public static float STATIC_MASS = 1000f;

			// Token: 0x040091B7 RID: 37303
			public static float PAYLOAD_MASS = 1000f;
		}

		// Token: 0x02002002 RID: 8194
		public class BURDEN
		{
			// Token: 0x040091B8 RID: 37304
			public static int INSIGNIFICANT = 1;

			// Token: 0x040091B9 RID: 37305
			public static int MINOR = 2;

			// Token: 0x040091BA RID: 37306
			public static int MINOR_PLUS = 3;

			// Token: 0x040091BB RID: 37307
			public static int MODERATE = 4;

			// Token: 0x040091BC RID: 37308
			public static int MODERATE_PLUS = 5;

			// Token: 0x040091BD RID: 37309
			public static int MAJOR = 6;

			// Token: 0x040091BE RID: 37310
			public static int MAJOR_PLUS = 7;

			// Token: 0x040091BF RID: 37311
			public static int MEGA = 9;

			// Token: 0x040091C0 RID: 37312
			public static int MONUMENTAL = 15;
		}

		// Token: 0x02002003 RID: 8195
		public class ENGINE_POWER
		{
			// Token: 0x040091C1 RID: 37313
			public static int EARLY_WEAK = 16;

			// Token: 0x040091C2 RID: 37314
			public static int EARLY_STRONG = 23;

			// Token: 0x040091C3 RID: 37315
			public static int MID_VERY_STRONG = 48;

			// Token: 0x040091C4 RID: 37316
			public static int MID_STRONG = 31;

			// Token: 0x040091C5 RID: 37317
			public static int MID_WEAK = 27;

			// Token: 0x040091C6 RID: 37318
			public static int LATE_STRONG = 34;

			// Token: 0x040091C7 RID: 37319
			public static int LATE_VERY_STRONG = 55;
		}

		// Token: 0x02002004 RID: 8196
		public class FUEL_COST_PER_DISTANCE
		{
			// Token: 0x040091C8 RID: 37320
			public static float VERY_LOW = 0.033333335f;

			// Token: 0x040091C9 RID: 37321
			public static float LOW = 0.0375f;

			// Token: 0x040091CA RID: 37322
			public static float MEDIUM = 0.075f;

			// Token: 0x040091CB RID: 37323
			public static float HIGH = 0.09375f;

			// Token: 0x040091CC RID: 37324
			public static float VERY_HIGH = 0.15f;

			// Token: 0x040091CD RID: 37325
			public static float GAS_VERY_LOW = 0.025f;

			// Token: 0x040091CE RID: 37326
			public static float GAS_LOW = 0.027777778f;

			// Token: 0x040091CF RID: 37327
			public static float GAS_HIGH = 0.041666668f;

			// Token: 0x040091D0 RID: 37328
			public static float PARTICLES = 0.33333334f;
		}
	}
}
