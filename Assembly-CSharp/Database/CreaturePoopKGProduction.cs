using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EBA RID: 3770
	public class CreaturePoopKGProduction : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075EE RID: 30190 RVA: 0x002E2119 File Offset: 0x002E0319
		public CreaturePoopKGProduction(Tag poopElement, float amountToPoop)
		{
			this.poopElement = poopElement;
			this.amountToPoop = amountToPoop;
		}

		// Token: 0x060075EF RID: 30191 RVA: 0x002E2130 File Offset: 0x002E0330
		public override bool Success()
		{
			return Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(this.poopElement) && Game.Instance.savedInfo.creaturePoopAmount[this.poopElement] >= this.amountToPoop;
		}

		// Token: 0x060075F0 RID: 30192 RVA: 0x002E2180 File Offset: 0x002E0380
		public void Deserialize(IReader reader)
		{
			this.amountToPoop = reader.ReadSingle();
			string name = reader.ReadKleiString();
			this.poopElement = new Tag(name);
		}

		// Token: 0x060075F1 RID: 30193 RVA: 0x002E21AC File Offset: 0x002E03AC
		public override string GetProgress(bool complete)
		{
			float num = 0f;
			Game.Instance.savedInfo.creaturePoopAmount.TryGetValue(this.poopElement, out num);
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.POOP_PRODUCTION, GameUtil.GetFormattedMass(complete ? this.amountToPoop : num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"), GameUtil.GetFormattedMass(this.amountToPoop, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"));
		}

		// Token: 0x0400557A RID: 21882
		private Tag poopElement;

		// Token: 0x0400557B RID: 21883
		private float amountToPoop;
	}
}
