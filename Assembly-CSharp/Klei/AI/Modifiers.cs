using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F70 RID: 3952
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/Modifiers")]
	public class Modifiers : KMonoBehaviour, ISaveLoadableDetails
	{
		// Token: 0x06007941 RID: 31041 RVA: 0x002FEEA8 File Offset: 0x002FD0A8
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.amounts = new Amounts(base.gameObject);
			this.sicknesses = new Sicknesses(base.gameObject);
			this.attributes = new Attributes(base.gameObject);
			foreach (string id in this.initialAmounts)
			{
				this.amounts.Add(new AmountInstance(Db.Get().Amounts.Get(id), base.gameObject));
			}
			foreach (string text in this.initialAttributes)
			{
				Attribute attribute = Db.Get().CritterAttributes.TryGet(text);
				if (attribute == null)
				{
					attribute = Db.Get().PlantAttributes.TryGet(text);
				}
				if (attribute == null)
				{
					attribute = Db.Get().Attributes.TryGet(text);
				}
				DebugUtil.Assert(attribute != null, "Couldn't find an attribute for id", text);
				this.attributes.Add(attribute);
			}
			Traits component = base.GetComponent<Traits>();
			if (this.initialTraits != null)
			{
				foreach (string id2 in this.initialTraits)
				{
					Trait trait = Db.Get().traits.Get(id2);
					component.Add(trait);
				}
			}
		}

		// Token: 0x06007942 RID: 31042 RVA: 0x002FF054 File Offset: 0x002FD254
		public float GetPreModifiedAttributeValue(Attribute attribute)
		{
			return AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
		}

		// Token: 0x06007943 RID: 31043 RVA: 0x002FF064 File Offset: 0x002FD264
		public string GetPreModifiedAttributeFormattedValue(Attribute attribute)
		{
			float totalValue = AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
			return attribute.formatter.GetFormattedValue(totalValue, attribute.formatter.DeltaTimeSlice);
		}

		// Token: 0x06007944 RID: 31044 RVA: 0x002FF098 File Offset: 0x002FD298
		public string GetPreModifiedAttributeDescription(Attribute attribute)
		{
			float totalValue = AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
			return string.Format(DUPLICANTS.ATTRIBUTES.VALUE, attribute.Name, attribute.formatter.GetFormattedValue(totalValue, GameUtil.TimeSlice.None));
		}

		// Token: 0x06007945 RID: 31045 RVA: 0x002FF0D5 File Offset: 0x002FD2D5
		public string GetPreModifiedAttributeToolTip(Attribute attribute)
		{
			return attribute.formatter.GetTooltip(attribute, this.GetPreModifiers(attribute), null);
		}

		// Token: 0x06007946 RID: 31046 RVA: 0x002FF0EC File Offset: 0x002FD2EC
		public List<AttributeModifier> GetPreModifiers(Attribute attribute)
		{
			List<AttributeModifier> list = new List<AttributeModifier>();
			foreach (string id in this.initialTraits)
			{
				foreach (AttributeModifier attributeModifier in Db.Get().traits.Get(id).SelfModifiers)
				{
					if (attributeModifier.AttributeId == attribute.Id)
					{
						list.Add(attributeModifier);
					}
				}
			}
			MutantPlant component = base.GetComponent<MutantPlant>();
			if (component != null && component.MutationIDs != null)
			{
				foreach (string id2 in component.MutationIDs)
				{
					foreach (AttributeModifier attributeModifier2 in Db.Get().PlantMutations.Get(id2).SelfModifiers)
					{
						if (attributeModifier2.AttributeId == attribute.Id)
						{
							list.Add(attributeModifier2);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06007947 RID: 31047 RVA: 0x002FF26C File Offset: 0x002FD46C
		public void Serialize(BinaryWriter writer)
		{
			this.OnSerialize(writer);
		}

		// Token: 0x06007948 RID: 31048 RVA: 0x002FF275 File Offset: 0x002FD475
		public void Deserialize(IReader reader)
		{
			this.OnDeserialize(reader);
		}

		// Token: 0x06007949 RID: 31049 RVA: 0x002FF27E File Offset: 0x002FD47E
		public virtual void OnSerialize(BinaryWriter writer)
		{
			this.amounts.Serialize(writer);
			this.sicknesses.Serialize(writer);
		}

		// Token: 0x0600794A RID: 31050 RVA: 0x002FF298 File Offset: 0x002FD498
		public virtual void OnDeserialize(IReader reader)
		{
			this.amounts.Deserialize(reader);
			this.sicknesses.Deserialize(reader);
		}

		// Token: 0x0600794B RID: 31051 RVA: 0x002FF2B2 File Offset: 0x002FD4B2
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			if (this.amounts != null)
			{
				this.amounts.Cleanup();
			}
		}

		// Token: 0x04005A92 RID: 23186
		public Amounts amounts;

		// Token: 0x04005A93 RID: 23187
		public Attributes attributes;

		// Token: 0x04005A94 RID: 23188
		public Sicknesses sicknesses;

		// Token: 0x04005A95 RID: 23189
		public List<string> initialTraits = new List<string>();

		// Token: 0x04005A96 RID: 23190
		public List<string> initialAmounts = new List<string>();

		// Token: 0x04005A97 RID: 23191
		public List<string> initialAttributes = new List<string>();
	}
}
