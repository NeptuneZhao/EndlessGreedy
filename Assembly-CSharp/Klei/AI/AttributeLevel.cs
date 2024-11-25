using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F43 RID: 3907
	public class AttributeLevel
	{
		// Token: 0x0600782B RID: 30763 RVA: 0x002F845C File Offset: 0x002F665C
		public AttributeLevel(AttributeInstance attribute)
		{
			this.notification = new Notification(MISC.NOTIFICATIONS.LEVELUP.NAME, NotificationType.Good, new Func<List<Notification>, object, string>(AttributeLevel.OnLevelUpTooltip), null, true, 0f, null, null, null, true, false, false);
			this.attribute = attribute;
		}

		// Token: 0x0600782C RID: 30764 RVA: 0x002F84A5 File Offset: 0x002F66A5
		public int GetLevel()
		{
			return this.level;
		}

		// Token: 0x0600782D RID: 30765 RVA: 0x002F84B0 File Offset: 0x002F66B0
		public void Apply(AttributeLevels levels)
		{
			Attributes attributes = levels.GetAttributes();
			if (this.modifier != null)
			{
				attributes.Remove(this.modifier);
				this.modifier = null;
			}
			this.modifier = new AttributeModifier(this.attribute.Id, (float)this.GetLevel(), DUPLICANTS.MODIFIERS.SKILLLEVEL.NAME, false, false, true);
			attributes.Add(this.modifier);
		}

		// Token: 0x0600782E RID: 30766 RVA: 0x002F8515 File Offset: 0x002F6715
		public void SetExperience(float experience)
		{
			this.experience = experience;
		}

		// Token: 0x0600782F RID: 30767 RVA: 0x002F851E File Offset: 0x002F671E
		public void SetLevel(int level)
		{
			this.level = level;
		}

		// Token: 0x06007830 RID: 30768 RVA: 0x002F8528 File Offset: 0x002F6728
		public float GetExperienceForNextLevel()
		{
			float num = Mathf.Pow((float)this.level / (float)DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL, DUPLICANTSTATS.ATTRIBUTE_LEVELING.EXPERIENCE_LEVEL_POWER) * (float)DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE * 600f;
			return Mathf.Pow(((float)this.level + 1f) / (float)DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL, DUPLICANTSTATS.ATTRIBUTE_LEVELING.EXPERIENCE_LEVEL_POWER) * (float)DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE * 600f - num;
		}

		// Token: 0x06007831 RID: 30769 RVA: 0x002F8588 File Offset: 0x002F6788
		public float GetPercentComplete()
		{
			return this.experience / this.GetExperienceForNextLevel();
		}

		// Token: 0x06007832 RID: 30770 RVA: 0x002F8598 File Offset: 0x002F6798
		public void LevelUp(AttributeLevels levels)
		{
			this.level++;
			this.experience = 0f;
			this.Apply(levels);
			this.experience = 0f;
			if (PopFXManager.Instance != null)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, this.attribute.modifier.Name, levels.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
			}
			levels.GetComponent<Notifier>().Add(this.notification, string.Format(MISC.NOTIFICATIONS.LEVELUP.SUFFIX, this.attribute.modifier.Name, this.level));
			StateMachine.Instance instance = new UpgradeFX.Instance(levels.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, -0.1f));
			ReportManager.Instance.ReportValue(ReportManager.ReportType.LevelUp, 1f, levels.GetProperName(), null);
			instance.StartSM();
			levels.Trigger(-110704193, this.attribute.Id);
		}

		// Token: 0x06007833 RID: 30771 RVA: 0x002F86B0 File Offset: 0x002F68B0
		public bool AddExperience(AttributeLevels levels, float experience)
		{
			if (this.level >= DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL)
			{
				return false;
			}
			this.experience += experience;
			this.experience = Mathf.Max(0f, this.experience);
			if (this.experience >= this.GetExperienceForNextLevel())
			{
				this.LevelUp(levels);
				return true;
			}
			return false;
		}

		// Token: 0x06007834 RID: 30772 RVA: 0x002F8708 File Offset: 0x002F6908
		private static string OnLevelUpTooltip(List<Notification> notifications, object data)
		{
			return MISC.NOTIFICATIONS.LEVELUP.TOOLTIP + notifications.ReduceMessages(false);
		}

		// Token: 0x040059DA RID: 23002
		public float experience;

		// Token: 0x040059DB RID: 23003
		public int level;

		// Token: 0x040059DC RID: 23004
		public AttributeInstance attribute;

		// Token: 0x040059DD RID: 23005
		public AttributeModifier modifier;

		// Token: 0x040059DE RID: 23006
		public Notification notification;
	}
}
