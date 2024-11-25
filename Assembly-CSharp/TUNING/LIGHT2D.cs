using System;
using UnityEngine;

namespace TUNING
{
	// Token: 0x02000EFF RID: 3839
	public class LIGHT2D
	{
		// Token: 0x040057D4 RID: 22484
		public const int SUNLIGHT_MAX_DEFAULT = 80000;

		// Token: 0x040057D5 RID: 22485
		public static readonly Color LIGHT_BLUE = new Color(0.38f, 0.61f, 1f, 1f);

		// Token: 0x040057D6 RID: 22486
		public static readonly Color LIGHT_PURPLE = new Color(0.9f, 0.4f, 0.74f, 1f);

		// Token: 0x040057D7 RID: 22487
		public static readonly Color LIGHT_PINK = new Color(0.9f, 0.4f, 0.6f, 1f);

		// Token: 0x040057D8 RID: 22488
		public static readonly Color LIGHT_YELLOW = new Color(0.57f, 0.55f, 0.44f, 1f);

		// Token: 0x040057D9 RID: 22489
		public static readonly Color LIGHT_OVERLAY = new Color(0.56f, 0.56f, 0.56f, 1f);

		// Token: 0x040057DA RID: 22490
		public static readonly Vector2 DEFAULT_DIRECTION = new Vector2(0f, -1f);

		// Token: 0x040057DB RID: 22491
		public const int FLOORLAMP_LUX = 1000;

		// Token: 0x040057DC RID: 22492
		public const float FLOORLAMP_RANGE = 4f;

		// Token: 0x040057DD RID: 22493
		public const float FLOORLAMP_ANGLE = 0f;

		// Token: 0x040057DE RID: 22494
		public const global::LightShape FLOORLAMP_SHAPE = global::LightShape.Circle;

		// Token: 0x040057DF RID: 22495
		public static readonly Color FLOORLAMP_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x040057E0 RID: 22496
		public static readonly Color FLOORLAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x040057E1 RID: 22497
		public static readonly Vector2 FLOORLAMP_OFFSET = new Vector2(0.05f, 1.5f);

		// Token: 0x040057E2 RID: 22498
		public static readonly Vector2 FLOORLAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x040057E3 RID: 22499
		public const float CEILINGLIGHT_RANGE = 8f;

		// Token: 0x040057E4 RID: 22500
		public const float CEILINGLIGHT_ANGLE = 2.6f;

		// Token: 0x040057E5 RID: 22501
		public const global::LightShape CEILINGLIGHT_SHAPE = global::LightShape.Cone;

		// Token: 0x040057E6 RID: 22502
		public static readonly Color CEILINGLIGHT_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x040057E7 RID: 22503
		public static readonly Color CEILINGLIGHT_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x040057E8 RID: 22504
		public static readonly Vector2 CEILINGLIGHT_OFFSET = new Vector2(0.05f, 0.65f);

		// Token: 0x040057E9 RID: 22505
		public static readonly Vector2 CEILINGLIGHT_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x040057EA RID: 22506
		public const int CEILINGLIGHT_LUX = 1800;

		// Token: 0x040057EB RID: 22507
		public static readonly int SUNLAMP_LUX = (int)((float)BeachChairConfig.TAN_LUX * 4f);

		// Token: 0x040057EC RID: 22508
		public const float SUNLAMP_RANGE = 16f;

		// Token: 0x040057ED RID: 22509
		public const float SUNLAMP_ANGLE = 5.2f;

		// Token: 0x040057EE RID: 22510
		public const global::LightShape SUNLAMP_SHAPE = global::LightShape.Cone;

		// Token: 0x040057EF RID: 22511
		public static readonly Color SUNLAMP_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x040057F0 RID: 22512
		public static readonly Color SUNLAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x040057F1 RID: 22513
		public static readonly Vector2 SUNLAMP_OFFSET = new Vector2(0f, 3.5f);

		// Token: 0x040057F2 RID: 22514
		public static readonly Vector2 SUNLAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x040057F3 RID: 22515
		public const int MERCURYCEILINGLIGHT_LUX = 60000;

		// Token: 0x040057F4 RID: 22516
		public const float MERCURYCEILINGLIGHT_RANGE = 8f;

		// Token: 0x040057F5 RID: 22517
		public const float MERCURYCEILINGLIGHT_ANGLE = 2.6f;

		// Token: 0x040057F6 RID: 22518
		public const float MERCURYCEILINGLIGHT_FALLOFFRATE = 0.4f;

		// Token: 0x040057F7 RID: 22519
		public const int MERCURYCEILINGLIGHT_WIDTH = 3;

		// Token: 0x040057F8 RID: 22520
		public const global::LightShape MERCURYCEILINGLIGHT_SHAPE = global::LightShape.Quad;

		// Token: 0x040057F9 RID: 22521
		public static readonly Color MERCURYCEILINGLIGHT_LUX_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x040057FA RID: 22522
		public static readonly Color MERCURYCEILINGLIGHT_COLOR = LIGHT2D.LIGHT_PINK;

		// Token: 0x040057FB RID: 22523
		public static readonly Vector2 MERCURYCEILINGLIGHT_OFFSET = new Vector2(0.05f, 0.65f);

		// Token: 0x040057FC RID: 22524
		public static readonly Vector2 MERCURYCEILINGLIGHT_DIRECTIONVECTOR = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x040057FD RID: 22525
		public const DiscreteShadowCaster.Direction MERCURYCEILINGLIGHT_DIRECTION = DiscreteShadowCaster.Direction.South;

		// Token: 0x040057FE RID: 22526
		public static readonly Color LIGHT_PREVIEW_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x040057FF RID: 22527
		public const float HEADQUARTERS_RANGE = 5f;

		// Token: 0x04005800 RID: 22528
		public const global::LightShape HEADQUARTERS_SHAPE = global::LightShape.Circle;

		// Token: 0x04005801 RID: 22529
		public static readonly Color HEADQUARTERS_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005802 RID: 22530
		public static readonly Color HEADQUARTERS_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005803 RID: 22531
		public static readonly Vector2 HEADQUARTERS_OFFSET = new Vector2(0.5f, 3f);

		// Token: 0x04005804 RID: 22532
		public static readonly Vector2 EXOBASE_HEADQUARTERS_OFFSET = new Vector2(0f, 2.5f);

		// Token: 0x04005805 RID: 22533
		public const float POI_TECH_UNLOCK_RANGE = 5f;

		// Token: 0x04005806 RID: 22534
		public const float POI_TECH_UNLOCK_ANGLE = 2.6f;

		// Token: 0x04005807 RID: 22535
		public const global::LightShape POI_TECH_UNLOCK_SHAPE = global::LightShape.Cone;

		// Token: 0x04005808 RID: 22536
		public static readonly Color POI_TECH_UNLOCK_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005809 RID: 22537
		public static readonly Color POI_TECH_UNLOCK_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x0400580A RID: 22538
		public static readonly Vector2 POI_TECH_UNLOCK_OFFSET = new Vector2(0f, 3.4f);

		// Token: 0x0400580B RID: 22539
		public const int POI_TECH_UNLOCK_LUX = 1800;

		// Token: 0x0400580C RID: 22540
		public static readonly Vector2 POI_TECH_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x0400580D RID: 22541
		public const float ENGINE_RANGE = 10f;

		// Token: 0x0400580E RID: 22542
		public const global::LightShape ENGINE_SHAPE = global::LightShape.Circle;

		// Token: 0x0400580F RID: 22543
		public const int ENGINE_LUX = 80000;

		// Token: 0x04005810 RID: 22544
		public const float WALLLIGHT_RANGE = 4f;

		// Token: 0x04005811 RID: 22545
		public const float WALLLIGHT_ANGLE = 0f;

		// Token: 0x04005812 RID: 22546
		public const global::LightShape WALLLIGHT_SHAPE = global::LightShape.Circle;

		// Token: 0x04005813 RID: 22547
		public static readonly Color WALLLIGHT_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005814 RID: 22548
		public static readonly Color WALLLIGHT_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005815 RID: 22549
		public static readonly Vector2 WALLLIGHT_OFFSET = new Vector2(0f, 0.5f);

		// Token: 0x04005816 RID: 22550
		public static readonly Vector2 WALLLIGHT_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005817 RID: 22551
		public const float LIGHTBUG_RANGE = 5f;

		// Token: 0x04005818 RID: 22552
		public const float LIGHTBUG_ANGLE = 0f;

		// Token: 0x04005819 RID: 22553
		public const global::LightShape LIGHTBUG_SHAPE = global::LightShape.Circle;

		// Token: 0x0400581A RID: 22554
		public const int LIGHTBUG_LUX = 1800;

		// Token: 0x0400581B RID: 22555
		public static readonly Color LIGHTBUG_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x0400581C RID: 22556
		public static readonly Color LIGHTBUG_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x0400581D RID: 22557
		public static readonly Color LIGHTBUG_COLOR_ORANGE = new Color(0.5686275f, 0.48235294f, 0.4392157f, 1f);

		// Token: 0x0400581E RID: 22558
		public static readonly Color LIGHTBUG_COLOR_PURPLE = new Color(0.49019608f, 0.4392157f, 0.5686275f, 1f);

		// Token: 0x0400581F RID: 22559
		public static readonly Color LIGHTBUG_COLOR_PINK = new Color(0.5686275f, 0.4392157f, 0.5686275f, 1f);

		// Token: 0x04005820 RID: 22560
		public static readonly Color LIGHTBUG_COLOR_BLUE = new Color(0.4392157f, 0.4862745f, 0.5686275f, 1f);

		// Token: 0x04005821 RID: 22561
		public static readonly Color LIGHTBUG_COLOR_CRYSTAL = new Color(0.5137255f, 0.6666667f, 0.6666667f, 1f);

		// Token: 0x04005822 RID: 22562
		public static readonly Color LIGHTBUG_COLOR_GREEN = new Color(0.43137255f, 1f, 0.53333336f, 1f);

		// Token: 0x04005823 RID: 22563
		public const int MAJORFOSSILDIGSITE_LAMP_LUX = 1000;

		// Token: 0x04005824 RID: 22564
		public const float MAJORFOSSILDIGSITE_LAMP_RANGE = 3f;

		// Token: 0x04005825 RID: 22565
		public static readonly Vector2 MAJORFOSSILDIGSITE_LAMP_OFFSET = new Vector2(-0.15f, 2.35f);

		// Token: 0x04005826 RID: 22566
		public static readonly Vector2 LIGHTBUG_OFFSET = new Vector2(0.05f, 0.25f);

		// Token: 0x04005827 RID: 22567
		public static readonly Vector2 LIGHTBUG_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005828 RID: 22568
		public const int PLASMALAMP_LUX = 666;

		// Token: 0x04005829 RID: 22569
		public const float PLASMALAMP_RANGE = 2f;

		// Token: 0x0400582A RID: 22570
		public const float PLASMALAMP_ANGLE = 0f;

		// Token: 0x0400582B RID: 22571
		public const global::LightShape PLASMALAMP_SHAPE = global::LightShape.Circle;

		// Token: 0x0400582C RID: 22572
		public static readonly Color PLASMALAMP_COLOR = LIGHT2D.LIGHT_PURPLE;

		// Token: 0x0400582D RID: 22573
		public static readonly Color PLASMALAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x0400582E RID: 22574
		public static readonly Vector2 PLASMALAMP_OFFSET = new Vector2(0.05f, 0.5f);

		// Token: 0x0400582F RID: 22575
		public static readonly Vector2 PLASMALAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005830 RID: 22576
		public const int MAGMALAMP_LUX = 666;

		// Token: 0x04005831 RID: 22577
		public const float MAGMALAMP_RANGE = 2f;

		// Token: 0x04005832 RID: 22578
		public const float MAGMALAMP_ANGLE = 0f;

		// Token: 0x04005833 RID: 22579
		public const global::LightShape MAGMALAMP_SHAPE = global::LightShape.Cone;

		// Token: 0x04005834 RID: 22580
		public static readonly Color MAGMALAMP_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005835 RID: 22581
		public static readonly Color MAGMALAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005836 RID: 22582
		public static readonly Vector2 MAGMALAMP_OFFSET = new Vector2(0.05f, 0.33f);

		// Token: 0x04005837 RID: 22583
		public static readonly Vector2 MAGMALAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005838 RID: 22584
		public const int BIOLUMROCK_LUX = 666;

		// Token: 0x04005839 RID: 22585
		public const float BIOLUMROCK_RANGE = 2f;

		// Token: 0x0400583A RID: 22586
		public const float BIOLUMROCK_ANGLE = 0f;

		// Token: 0x0400583B RID: 22587
		public const global::LightShape BIOLUMROCK_SHAPE = global::LightShape.Cone;

		// Token: 0x0400583C RID: 22588
		public static readonly Color BIOLUMROCK_COLOR = LIGHT2D.LIGHT_BLUE;

		// Token: 0x0400583D RID: 22589
		public static readonly Color BIOLUMROCK_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x0400583E RID: 22590
		public static readonly Vector2 BIOLUMROCK_OFFSET = new Vector2(0.05f, 0.33f);

		// Token: 0x0400583F RID: 22591
		public static readonly Vector2 BIOLUMROCK_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005840 RID: 22592
		public const float PINKROCK_RANGE = 2f;

		// Token: 0x04005841 RID: 22593
		public const float PINKROCK_ANGLE = 0f;

		// Token: 0x04005842 RID: 22594
		public const global::LightShape PINKROCK_SHAPE = global::LightShape.Circle;

		// Token: 0x04005843 RID: 22595
		public static readonly Color PINKROCK_COLOR = LIGHT2D.LIGHT_PINK;

		// Token: 0x04005844 RID: 22596
		public static readonly Color PINKROCK_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005845 RID: 22597
		public static readonly Vector2 PINKROCK_OFFSET = new Vector2(0.05f, 0.33f);

		// Token: 0x04005846 RID: 22598
		public static readonly Vector2 PINKROCK_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;
	}
}
