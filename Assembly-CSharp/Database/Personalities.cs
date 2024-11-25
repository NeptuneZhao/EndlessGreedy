using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02000E73 RID: 3699
	public class Personalities : ResourceSet<Personality>
	{
		// Token: 0x060074BE RID: 29886 RVA: 0x002D8438 File Offset: 0x002D6638
		public Personalities()
		{
			foreach (Personalities.PersonalityInfo personalityInfo in AsyncLoadManager<IGlobalAsyncLoader>.AsyncLoader<Personalities.PersonalityLoader>.Get().entries)
			{
				if (string.IsNullOrEmpty(personalityInfo.RequiredDlcId) || DlcManager.IsContentSubscribed(personalityInfo.RequiredDlcId))
				{
					base.Add(new Personality(personalityInfo.Name.ToUpper(), Strings.Get(string.Format("STRINGS.DUPLICANTS.PERSONALITIES.{0}.NAME", personalityInfo.Name.ToUpper())), personalityInfo.Gender.ToUpper(), personalityInfo.PersonalityType, personalityInfo.StressTrait, personalityInfo.JoyTrait, personalityInfo.StickerType, personalityInfo.CongenitalTrait, personalityInfo.HeadShape, personalityInfo.Mouth, personalityInfo.Neck, personalityInfo.Eyes, personalityInfo.Hair, personalityInfo.Body, personalityInfo.Belt, personalityInfo.Cuff, personalityInfo.Foot, personalityInfo.Hand, personalityInfo.Pelvis, personalityInfo.Leg, Strings.Get(string.Format("STRINGS.DUPLICANTS.PERSONALITIES.{0}.DESC", personalityInfo.Name.ToUpper())), personalityInfo.ValidStarter, personalityInfo.Grave, personalityInfo.Model)
					{
						requiredDlcId = personalityInfo.RequiredDlcId
					});
				}
			}
		}

		// Token: 0x060074BF RID: 29887 RVA: 0x002D8578 File Offset: 0x002D6778
		private void AddTrait(Personality personality, string trait_name)
		{
			Trait trait = Db.Get().traits.TryGet(trait_name);
			if (trait != null)
			{
				personality.AddTrait(trait);
			}
		}

		// Token: 0x060074C0 RID: 29888 RVA: 0x002D85A0 File Offset: 0x002D67A0
		private void SetAttribute(Personality personality, string attribute_name, int value)
		{
			Klei.AI.Attribute attribute = Db.Get().Attributes.TryGet(attribute_name);
			if (attribute == null)
			{
				Debug.LogWarning("Attribute does not exist: " + attribute_name);
				return;
			}
			personality.SetAttribute(attribute, value);
		}

		// Token: 0x060074C1 RID: 29889 RVA: 0x002D85DA File Offset: 0x002D67DA
		public List<Personality> GetStartingPersonalities()
		{
			return this.resources.FindAll((Personality x) => x.startingMinion);
		}

		// Token: 0x060074C2 RID: 29890 RVA: 0x002D8608 File Offset: 0x002D6808
		public List<Personality> GetAll(bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.resources.FindAll((Personality personality) => (!onlyStartingMinions || personality.startingMinion) && (!onlyEnabledMinions || !personality.Disabled) && (!(SaveLoader.Instance != null) || !DlcManager.IsDlcId(personality.requiredDlcId) || SaveLoader.Instance.GameInfo.dlcIds.Contains(personality.requiredDlcId)));
		}

		// Token: 0x060074C3 RID: 29891 RVA: 0x002D8640 File Offset: 0x002D6840
		public Personality GetRandom(bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.GetAll(onlyEnabledMinions, onlyStartingMinions).GetRandom<Personality>();
		}

		// Token: 0x060074C4 RID: 29892 RVA: 0x002D8650 File Offset: 0x002D6850
		public Personality GetRandom(Tag model, bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.GetAll(onlyEnabledMinions, onlyStartingMinions).FindAll((Personality personality) => personality.model == model || model == null).GetRandom<Personality>();
		}

		// Token: 0x060074C5 RID: 29893 RVA: 0x002D8688 File Offset: 0x002D6888
		public Personality GetRandom(List<Tag> models, bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.GetAll(onlyEnabledMinions, onlyStartingMinions).FindAll((Personality personality) => models.Contains(personality.model)).GetRandom<Personality>();
		}

		// Token: 0x060074C6 RID: 29894 RVA: 0x002D86C0 File Offset: 0x002D68C0
		public Personality GetPersonalityFromNameStringKey(string name_string_key)
		{
			foreach (Personality personality in Db.Get().Personalities.resources)
			{
				if (personality.nameStringKey.Equals(name_string_key, StringComparison.CurrentCultureIgnoreCase))
				{
					return personality;
				}
			}
			return null;
		}

		// Token: 0x02001F6F RID: 8047
		public class PersonalityLoader : AsyncCsvLoader<Personalities.PersonalityLoader, Personalities.PersonalityInfo>
		{
			// Token: 0x0600AF2A RID: 44842 RVA: 0x003B0E16 File Offset: 0x003AF016
			public PersonalityLoader() : base(Assets.instance.personalitiesFile)
			{
			}

			// Token: 0x0600AF2B RID: 44843 RVA: 0x003B0E28 File Offset: 0x003AF028
			public override void Run()
			{
				base.Run();
			}
		}

		// Token: 0x02001F70 RID: 8048
		public class PersonalityInfo : Resource
		{
			// Token: 0x04008E8D RID: 36493
			public int HeadShape;

			// Token: 0x04008E8E RID: 36494
			public int Mouth;

			// Token: 0x04008E8F RID: 36495
			public int Neck;

			// Token: 0x04008E90 RID: 36496
			public int Eyes;

			// Token: 0x04008E91 RID: 36497
			public int Hair;

			// Token: 0x04008E92 RID: 36498
			public int Body;

			// Token: 0x04008E93 RID: 36499
			public int Belt;

			// Token: 0x04008E94 RID: 36500
			public int Cuff;

			// Token: 0x04008E95 RID: 36501
			public int Foot;

			// Token: 0x04008E96 RID: 36502
			public int Hand;

			// Token: 0x04008E97 RID: 36503
			public int Pelvis;

			// Token: 0x04008E98 RID: 36504
			public int Leg;

			// Token: 0x04008E99 RID: 36505
			public string Gender;

			// Token: 0x04008E9A RID: 36506
			public string PersonalityType;

			// Token: 0x04008E9B RID: 36507
			public string StressTrait;

			// Token: 0x04008E9C RID: 36508
			public string JoyTrait;

			// Token: 0x04008E9D RID: 36509
			public string StickerType;

			// Token: 0x04008E9E RID: 36510
			public string CongenitalTrait;

			// Token: 0x04008E9F RID: 36511
			public string Design;

			// Token: 0x04008EA0 RID: 36512
			public bool ValidStarter;

			// Token: 0x04008EA1 RID: 36513
			public string Grave;

			// Token: 0x04008EA2 RID: 36514
			public string Model;

			// Token: 0x04008EA3 RID: 36515
			public string RequiredDlcId;
		}
	}
}
