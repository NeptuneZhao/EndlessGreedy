﻿using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000EB9 RID: 3769
	public class RevealAsteriod : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075EA RID: 30186 RVA: 0x002E2015 File Offset: 0x002E0215
		public RevealAsteriod(float percentToReveal)
		{
			this.percentToReveal = percentToReveal;
		}

		// Token: 0x060075EB RID: 30187 RVA: 0x002E2024 File Offset: 0x002E0224
		public override bool Success()
		{
			this.amountRevealed = 0f;
			float num = 0f;
			WorldContainer startWorld = ClusterManager.Instance.GetStartWorld();
			Vector2 minimumBounds = startWorld.minimumBounds;
			Vector2 maximumBounds = startWorld.maximumBounds;
			int num2 = (int)minimumBounds.x;
			while ((float)num2 <= maximumBounds.x)
			{
				int num3 = (int)minimumBounds.y;
				while ((float)num3 <= maximumBounds.y)
				{
					if (Grid.Visible[Grid.PosToCell(new Vector2((float)num2, (float)num3))] > 0)
					{
						num += 1f;
					}
					num3++;
				}
				num2++;
			}
			this.amountRevealed = num / (float)(startWorld.Width * startWorld.Height);
			return this.amountRevealed > this.percentToReveal;
		}

		// Token: 0x060075EC RID: 30188 RVA: 0x002E20D8 File Offset: 0x002E02D8
		public void Deserialize(IReader reader)
		{
			this.percentToReveal = reader.ReadSingle();
		}

		// Token: 0x060075ED RID: 30189 RVA: 0x002E20E6 File Offset: 0x002E02E6
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.REVEALED, this.amountRevealed * 100f, this.percentToReveal * 100f);
		}

		// Token: 0x04005578 RID: 21880
		private float percentToReveal;

		// Token: 0x04005579 RID: 21881
		private float amountRevealed;
	}
}
