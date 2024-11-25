using System;

namespace TUNING
{
	// Token: 0x02000EFE RID: 3838
	public class RADIATION
	{
		// Token: 0x040057D0 RID: 22480
		public const float GERM_RAD_SCALE = 0.01f;

		// Token: 0x040057D1 RID: 22481
		public const float STANDARD_DAILY_RECOVERY = 100f;

		// Token: 0x040057D2 RID: 22482
		public const float EXTRA_VOMIT_RECOVERY = 20f;

		// Token: 0x040057D3 RID: 22483
		public const float REACT_THRESHOLD = 133f;

		// Token: 0x02001FEE RID: 8174
		public class STANDARD_EMITTER
		{
			// Token: 0x04009137 RID: 37175
			public const float STEADY_PULSE_RATE = 0.2f;

			// Token: 0x04009138 RID: 37176
			public const float DOUBLE_SPEED_PULSE_RATE = 0.1f;

			// Token: 0x04009139 RID: 37177
			public const float RADIUS_SCALE = 1f;
		}

		// Token: 0x02001FEF RID: 8175
		public class RADIATION_PER_SECOND
		{
			// Token: 0x0400913A RID: 37178
			public const float TRIVIAL = 60f;

			// Token: 0x0400913B RID: 37179
			public const float VERY_LOW = 120f;

			// Token: 0x0400913C RID: 37180
			public const float LOW = 240f;

			// Token: 0x0400913D RID: 37181
			public const float MODERATE = 600f;

			// Token: 0x0400913E RID: 37182
			public const float HIGH = 1800f;

			// Token: 0x0400913F RID: 37183
			public const float VERY_HIGH = 4800f;

			// Token: 0x04009140 RID: 37184
			public const int EXTREME = 9600;
		}

		// Token: 0x02001FF0 RID: 8176
		public class RADIATION_CONSTANT_RADS_PER_CYCLE
		{
			// Token: 0x04009141 RID: 37185
			public const float LESS_THAN_TRIVIAL = 60f;

			// Token: 0x04009142 RID: 37186
			public const float TRIVIAL = 120f;

			// Token: 0x04009143 RID: 37187
			public const float VERY_LOW = 240f;

			// Token: 0x04009144 RID: 37188
			public const float LOW = 480f;

			// Token: 0x04009145 RID: 37189
			public const float MODERATE = 1200f;

			// Token: 0x04009146 RID: 37190
			public const float MODERATE_PLUS = 2400f;

			// Token: 0x04009147 RID: 37191
			public const float HIGH = 3600f;

			// Token: 0x04009148 RID: 37192
			public const float VERY_HIGH = 8400f;

			// Token: 0x04009149 RID: 37193
			public const int EXTREME = 16800;
		}
	}
}
