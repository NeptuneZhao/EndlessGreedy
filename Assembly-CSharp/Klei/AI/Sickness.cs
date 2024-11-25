using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F4E RID: 3918
	[DebuggerDisplay("{base.Id}")]
	public abstract class Sickness : Resource
	{
		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x0600787F RID: 30847 RVA: 0x002FAACD File Offset: 0x002F8CCD
		public new string Name
		{
			get
			{
				return Strings.Get(this.name);
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06007880 RID: 30848 RVA: 0x002FAADF File Offset: 0x002F8CDF
		public float SicknessDuration
		{
			get
			{
				return this.sicknessDuration;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06007881 RID: 30849 RVA: 0x002FAAE7 File Offset: 0x002F8CE7
		public StringKey DescriptiveSymptoms
		{
			get
			{
				return this.descriptiveSymptoms;
			}
		}

		// Token: 0x06007882 RID: 30850 RVA: 0x002FAAF0 File Offset: 0x002F8CF0
		public Sickness(string id, Sickness.SicknessType type, Sickness.Severity severity, float immune_attack_strength, List<Sickness.InfectionVector> infection_vectors, float sickness_duration, string recovery_effect = null) : base(id, null, null)
		{
			this.name = new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".NAME");
			this.id = id;
			this.sicknessType = type;
			this.severity = severity;
			this.infectionVectors = infection_vectors;
			this.sicknessDuration = sickness_duration;
			this.recoveryEffect = recovery_effect;
			this.descriptiveSymptoms = new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".DESCRIPTIVE_SYMPTOMS");
			this.cureSpeedBase = new Attribute(id + "CureSpeed", false, Attribute.Display.Normal, false, 0f, null, null, null, null);
			this.cureSpeedBase.BaseValue = 1f;
			this.cureSpeedBase.SetFormatter(new ToPercentAttributeFormatter(1f, GameUtil.TimeSlice.None));
			Db.Get().Attributes.Add(this.cureSpeedBase);
		}

		// Token: 0x06007883 RID: 30851 RVA: 0x002FABEC File Offset: 0x002F8DEC
		public object[] Infect(GameObject go, SicknessInstance diseaseInstance, SicknessExposureInfo exposure_info)
		{
			object[] array = new object[this.components.Count];
			for (int i = 0; i < this.components.Count; i++)
			{
				array[i] = this.components[i].OnInfect(go, diseaseInstance);
			}
			return array;
		}

		// Token: 0x06007884 RID: 30852 RVA: 0x002FAC38 File Offset: 0x002F8E38
		public void Cure(GameObject go, object[] componentData)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				this.components[i].OnCure(go, componentData[i]);
			}
		}

		// Token: 0x06007885 RID: 30853 RVA: 0x002FAC70 File Offset: 0x002F8E70
		public List<Descriptor> GetSymptoms()
		{
			List<Descriptor> list = new List<Descriptor>();
			for (int i = 0; i < this.components.Count; i++)
			{
				List<Descriptor> symptoms = this.components[i].GetSymptoms();
				if (symptoms != null)
				{
					list.AddRange(symptoms);
				}
			}
			if (this.fatalityDuration > 0f)
			{
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.DEATH_SYMPTOM, GameUtil.GetFormattedCycles(this.fatalityDuration, "F1", false)), string.Format(DUPLICANTS.DISEASES.DEATH_SYMPTOM_TOOLTIP, GameUtil.GetFormattedCycles(this.fatalityDuration, "F1", false)), Descriptor.DescriptorType.SymptomAidable, false));
			}
			return list;
		}

		// Token: 0x06007886 RID: 30854 RVA: 0x002FAD10 File Offset: 0x002F8F10
		protected void AddSicknessComponent(Sickness.SicknessComponent cmp)
		{
			this.components.Add(cmp);
		}

		// Token: 0x06007887 RID: 30855 RVA: 0x002FAD20 File Offset: 0x002F8F20
		public T GetSicknessComponent<T>() where T : Sickness.SicknessComponent
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (this.components[i] is T)
				{
					return this.components[i] as T;
				}
			}
			return default(T);
		}

		// Token: 0x06007888 RID: 30856 RVA: 0x002FAD76 File Offset: 0x002F8F76
		public virtual List<Descriptor> GetSicknessSourceDescriptors()
		{
			return new List<Descriptor>();
		}

		// Token: 0x06007889 RID: 30857 RVA: 0x002FAD80 File Offset: 0x002F8F80
		public List<Descriptor> GetQualitativeDescriptors()
		{
			List<Descriptor> list = new List<Descriptor>();
			using (List<Sickness.InfectionVector>.Enumerator enumerator = this.infectionVectors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current)
					{
					case Sickness.InfectionVector.Contact:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SKINBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SKINBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					case Sickness.InfectionVector.Digestion:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.FOODBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.FOODBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					case Sickness.InfectionVector.Inhalation:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.AIRBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.AIRBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					case Sickness.InfectionVector.Exposure:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SUNBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SUNBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					}
				}
			}
			list.Add(new Descriptor(Strings.Get(this.descriptiveSymptoms), "", Descriptor.DescriptorType.Information, false));
			return list;
		}

		// Token: 0x04005A0A RID: 23050
		private StringKey name;

		// Token: 0x04005A0B RID: 23051
		private StringKey descriptiveSymptoms;

		// Token: 0x04005A0C RID: 23052
		private float sicknessDuration = 600f;

		// Token: 0x04005A0D RID: 23053
		public float fatalityDuration;

		// Token: 0x04005A0E RID: 23054
		public HashedString id;

		// Token: 0x04005A0F RID: 23055
		public Sickness.SicknessType sicknessType;

		// Token: 0x04005A10 RID: 23056
		public Sickness.Severity severity;

		// Token: 0x04005A11 RID: 23057
		public string recoveryEffect;

		// Token: 0x04005A12 RID: 23058
		public List<Sickness.InfectionVector> infectionVectors;

		// Token: 0x04005A13 RID: 23059
		private List<Sickness.SicknessComponent> components = new List<Sickness.SicknessComponent>();

		// Token: 0x04005A14 RID: 23060
		public Amount amount;

		// Token: 0x04005A15 RID: 23061
		public Attribute amountDeltaAttribute;

		// Token: 0x04005A16 RID: 23062
		public Attribute cureSpeedBase;

		// Token: 0x02002338 RID: 9016
		public abstract class SicknessComponent
		{
			// Token: 0x0600B5FA RID: 46586
			public abstract object OnInfect(GameObject go, SicknessInstance diseaseInstance);

			// Token: 0x0600B5FB RID: 46587
			public abstract void OnCure(GameObject go, object instance_data);

			// Token: 0x0600B5FC RID: 46588 RVA: 0x003C8DA7 File Offset: 0x003C6FA7
			public virtual List<Descriptor> GetSymptoms()
			{
				return null;
			}
		}

		// Token: 0x02002339 RID: 9017
		public enum InfectionVector
		{
			// Token: 0x04009E16 RID: 40470
			Contact,
			// Token: 0x04009E17 RID: 40471
			Digestion,
			// Token: 0x04009E18 RID: 40472
			Inhalation,
			// Token: 0x04009E19 RID: 40473
			Exposure
		}

		// Token: 0x0200233A RID: 9018
		public enum SicknessType
		{
			// Token: 0x04009E1B RID: 40475
			Pathogen,
			// Token: 0x04009E1C RID: 40476
			Ailment,
			// Token: 0x04009E1D RID: 40477
			Injury
		}

		// Token: 0x0200233B RID: 9019
		public enum Severity
		{
			// Token: 0x04009E1F RID: 40479
			Benign,
			// Token: 0x04009E20 RID: 40480
			Minor,
			// Token: 0x04009E21 RID: 40481
			Major,
			// Token: 0x04009E22 RID: 40482
			Critical
		}
	}
}
