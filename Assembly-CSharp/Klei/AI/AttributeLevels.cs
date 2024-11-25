using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F44 RID: 3908
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/AttributeLevels")]
	public class AttributeLevels : KMonoBehaviour, ISaveLoadable
	{
		// Token: 0x06007835 RID: 30773 RVA: 0x002F8720 File Offset: 0x002F6920
		public IEnumerator<AttributeLevel> GetEnumerator()
		{
			return this.levels.GetEnumerator();
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06007836 RID: 30774 RVA: 0x002F8732 File Offset: 0x002F6932
		// (set) Token: 0x06007837 RID: 30775 RVA: 0x002F873A File Offset: 0x002F693A
		public AttributeLevels.LevelSaveLoad[] SaveLoadLevels
		{
			get
			{
				return this.saveLoadLevels;
			}
			set
			{
				this.saveLoadLevels = value;
			}
		}

		// Token: 0x06007838 RID: 30776 RVA: 0x002F8744 File Offset: 0x002F6944
		protected override void OnPrefabInit()
		{
			foreach (AttributeInstance attributeInstance in this.GetAttributes())
			{
				if (attributeInstance.Attribute.IsTrainable)
				{
					AttributeLevel attributeLevel = new AttributeLevel(attributeInstance);
					this.levels.Add(attributeLevel);
					attributeLevel.Apply(this);
				}
			}
		}

		// Token: 0x06007839 RID: 30777 RVA: 0x002F87B4 File Offset: 0x002F69B4
		[OnSerializing]
		public void OnSerializing()
		{
			this.saveLoadLevels = new AttributeLevels.LevelSaveLoad[this.levels.Count];
			for (int i = 0; i < this.levels.Count; i++)
			{
				this.saveLoadLevels[i].attributeId = this.levels[i].attribute.Attribute.Id;
				this.saveLoadLevels[i].experience = this.levels[i].experience;
				this.saveLoadLevels[i].level = this.levels[i].level;
			}
		}

		// Token: 0x0600783A RID: 30778 RVA: 0x002F8860 File Offset: 0x002F6A60
		[OnDeserialized]
		public void OnDeserialized()
		{
			foreach (AttributeLevels.LevelSaveLoad levelSaveLoad in this.saveLoadLevels)
			{
				this.SetExperience(levelSaveLoad.attributeId, levelSaveLoad.experience);
				this.SetLevel(levelSaveLoad.attributeId, levelSaveLoad.level);
			}
		}

		// Token: 0x0600783B RID: 30779 RVA: 0x002F88B0 File Offset: 0x002F6AB0
		public int GetLevel(Attribute attribute)
		{
			foreach (AttributeLevel attributeLevel in this.levels)
			{
				if (attribute == attributeLevel.attribute.Attribute)
				{
					return attributeLevel.GetLevel();
				}
			}
			return 1;
		}

		// Token: 0x0600783C RID: 30780 RVA: 0x002F8918 File Offset: 0x002F6B18
		public AttributeLevel GetAttributeLevel(string attribute_id)
		{
			foreach (AttributeLevel attributeLevel in this.levels)
			{
				if (attributeLevel.attribute.Attribute.Id == attribute_id)
				{
					return attributeLevel;
				}
			}
			return null;
		}

		// Token: 0x0600783D RID: 30781 RVA: 0x002F8984 File Offset: 0x002F6B84
		public bool AddExperience(string attribute_id, float time_spent, float multiplier)
		{
			AttributeLevel attributeLevel = this.GetAttributeLevel(attribute_id);
			if (attributeLevel == null)
			{
				global::Debug.LogWarning(attribute_id + " has no level.");
				return false;
			}
			time_spent *= multiplier;
			AttributeConverterInstance attributeConverterInstance = Db.Get().AttributeConverters.TrainingSpeed.Lookup(this);
			if (attributeConverterInstance != null)
			{
				float num = attributeConverterInstance.Evaluate();
				time_spent += time_spent * num;
			}
			bool result = attributeLevel.AddExperience(this, time_spent);
			attributeLevel.Apply(this);
			return result;
		}

		// Token: 0x0600783E RID: 30782 RVA: 0x002F89EC File Offset: 0x002F6BEC
		public void SetLevel(string attribute_id, int level)
		{
			AttributeLevel attributeLevel = this.GetAttributeLevel(attribute_id);
			if (attributeLevel != null)
			{
				attributeLevel.SetLevel(level);
				attributeLevel.Apply(this);
			}
		}

		// Token: 0x0600783F RID: 30783 RVA: 0x002F8A14 File Offset: 0x002F6C14
		public void SetExperience(string attribute_id, float experience)
		{
			AttributeLevel attributeLevel = this.GetAttributeLevel(attribute_id);
			if (attributeLevel != null)
			{
				attributeLevel.SetExperience(experience);
				attributeLevel.Apply(this);
			}
		}

		// Token: 0x06007840 RID: 30784 RVA: 0x002F8A3A File Offset: 0x002F6C3A
		public float GetPercentComplete(string attribute_id)
		{
			return this.GetAttributeLevel(attribute_id).GetPercentComplete();
		}

		// Token: 0x06007841 RID: 30785 RVA: 0x002F8A48 File Offset: 0x002F6C48
		public int GetMaxLevel()
		{
			int num = 0;
			foreach (AttributeLevel attributeLevel in this)
			{
				if (attributeLevel.GetLevel() > num)
				{
					num = attributeLevel.GetLevel();
				}
			}
			return num;
		}

		// Token: 0x040059DF RID: 23007
		private List<AttributeLevel> levels = new List<AttributeLevel>();

		// Token: 0x040059E0 RID: 23008
		[Serialize]
		private AttributeLevels.LevelSaveLoad[] saveLoadLevels = new AttributeLevels.LevelSaveLoad[0];

		// Token: 0x02002336 RID: 9014
		[Serializable]
		public struct LevelSaveLoad
		{
			// Token: 0x04009E0E RID: 40462
			public string attributeId;

			// Token: 0x04009E0F RID: 40463
			public float experience;

			// Token: 0x04009E10 RID: 40464
			public int level;
		}
	}
}
