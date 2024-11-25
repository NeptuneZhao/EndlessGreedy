using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x020009B2 RID: 2482
public class Personality : Resource
{
	// Token: 0x17000511 RID: 1297
	// (get) Token: 0x06004816 RID: 18454 RVA: 0x0019D1D7 File Offset: 0x0019B3D7
	public string description
	{
		get
		{
			return this.GetDescription();
		}
	}

	// Token: 0x06004817 RID: 18455 RVA: 0x0019D1E0 File Offset: 0x0019B3E0
	[Obsolete("Modders: Use constructor with isStartingMinion parameter")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, string description) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, description, true, "", GameTags.Minions.Models.Standard)
	{
	}

	// Token: 0x06004818 RID: 18456 RVA: 0x0019D220 File Offset: 0x0019B420
	[Obsolete("Modders: Added additional body part customization to duplicant personalities")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, string description, bool isStartingMinion) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, description, true, "", GameTags.Minions.Models.Standard)
	{
	}

	// Token: 0x06004819 RID: 18457 RVA: 0x0019D260 File Offset: 0x0019B460
	[Obsolete("Modders: Added a custom gravestone image to duplicant personalities")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, int belt, int cuff, int foot, int hand, int pelvis, int leg, string description, bool isStartingMinion) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, description, isStartingMinion, "", GameTags.Minions.Models.Standard)
	{
	}

	// Token: 0x0600481A RID: 18458 RVA: 0x0019D2A0 File Offset: 0x0019B4A0
	[Obsolete("Modders: Added 'model' to duplicant personalities")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, int belt, int cuff, int foot, int hand, int pelvis, int leg, string description, bool isStartingMinion, string graveStone) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, description, isStartingMinion, "", GameTags.Minions.Models.Standard)
	{
	}

	// Token: 0x0600481B RID: 18459 RVA: 0x0019D2E0 File Offset: 0x0019B4E0
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, int belt, int cuff, int foot, int hand, int pelvis, int leg, string description, bool isStartingMinion, string graveStone, Tag model) : base(name_string_key, name)
	{
		this.nameStringKey = name_string_key;
		this.genderStringKey = Gender;
		this.personalityType = PersonalityType;
		this.stresstrait = StressTrait;
		this.joyTrait = JoyTrait;
		this.stickerType = StickerType;
		this.congenitaltrait = CongenitalTrait;
		this.unformattedDescription = description;
		this.headShape = headShape;
		this.mouth = mouth;
		this.neck = neck;
		this.eyes = eyes;
		this.hair = hair;
		this.body = body;
		this.belt = belt;
		this.cuff = cuff;
		this.foot = foot;
		this.hand = hand;
		this.pelvis = pelvis;
		this.leg = leg;
		this.startingMinion = isStartingMinion;
		this.graveStone = graveStone;
		this.model = model;
	}

	// Token: 0x0600481C RID: 18460 RVA: 0x0019D3C1 File Offset: 0x0019B5C1
	public string GetDescription()
	{
		this.unformattedDescription = this.unformattedDescription.Replace("{0}", this.Name);
		return this.unformattedDescription;
	}

	// Token: 0x0600481D RID: 18461 RVA: 0x0019D3E8 File Offset: 0x0019B5E8
	public void SetAttribute(Klei.AI.Attribute attribute, int value)
	{
		Personality.StartingAttribute item = new Personality.StartingAttribute(attribute, value);
		this.attributes.Add(item);
	}

	// Token: 0x0600481E RID: 18462 RVA: 0x0019D409 File Offset: 0x0019B609
	public void AddTrait(Trait trait)
	{
		this.traits.Add(trait);
	}

	// Token: 0x0600481F RID: 18463 RVA: 0x0019D417 File Offset: 0x0019B617
	public void SetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType outfitType, Option<string> outfit)
	{
		CustomClothingOutfits.Instance.Internal_SetDuplicantPersonalityOutfit(outfitType, this.Id, outfit);
	}

	// Token: 0x06004820 RID: 18464 RVA: 0x0019D42C File Offset: 0x0019B62C
	public string GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType outfitType)
	{
		string result;
		if (CustomClothingOutfits.Instance.Internal_TryGetDuplicantPersonalityOutfit(outfitType, this.Id, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004821 RID: 18465 RVA: 0x0019D454 File Offset: 0x0019B654
	public Sprite GetMiniIcon()
	{
		if (string.IsNullOrWhiteSpace(this.nameStringKey))
		{
			return Assets.GetSprite("unknown");
		}
		string str;
		if (this.nameStringKey == "MIMA")
		{
			str = "Mi-Ma";
		}
		else
		{
			str = this.nameStringKey[0].ToString() + this.nameStringKey.Substring(1).ToLower();
		}
		return Assets.GetSprite("dreamIcon_" + str);
	}

	// Token: 0x04002F36 RID: 12086
	public List<Personality.StartingAttribute> attributes = new List<Personality.StartingAttribute>();

	// Token: 0x04002F37 RID: 12087
	public List<Trait> traits = new List<Trait>();

	// Token: 0x04002F38 RID: 12088
	public int headShape;

	// Token: 0x04002F39 RID: 12089
	public int mouth;

	// Token: 0x04002F3A RID: 12090
	public int neck;

	// Token: 0x04002F3B RID: 12091
	public int eyes;

	// Token: 0x04002F3C RID: 12092
	public int hair;

	// Token: 0x04002F3D RID: 12093
	public int body;

	// Token: 0x04002F3E RID: 12094
	public int belt;

	// Token: 0x04002F3F RID: 12095
	public int cuff;

	// Token: 0x04002F40 RID: 12096
	public int foot;

	// Token: 0x04002F41 RID: 12097
	public int hand;

	// Token: 0x04002F42 RID: 12098
	public int pelvis;

	// Token: 0x04002F43 RID: 12099
	public int leg;

	// Token: 0x04002F44 RID: 12100
	public string nameStringKey;

	// Token: 0x04002F45 RID: 12101
	public string genderStringKey;

	// Token: 0x04002F46 RID: 12102
	public string personalityType;

	// Token: 0x04002F47 RID: 12103
	public Tag model;

	// Token: 0x04002F48 RID: 12104
	public string stresstrait;

	// Token: 0x04002F49 RID: 12105
	public string joyTrait;

	// Token: 0x04002F4A RID: 12106
	public string stickerType;

	// Token: 0x04002F4B RID: 12107
	public string congenitaltrait;

	// Token: 0x04002F4C RID: 12108
	public string unformattedDescription;

	// Token: 0x04002F4D RID: 12109
	public string graveStone;

	// Token: 0x04002F4E RID: 12110
	public bool startingMinion;

	// Token: 0x04002F4F RID: 12111
	public string requiredDlcId;

	// Token: 0x020019B1 RID: 6577
	public class StartingAttribute
	{
		// Token: 0x06009DD2 RID: 40402 RVA: 0x003760C0 File Offset: 0x003742C0
		public StartingAttribute(Klei.AI.Attribute attribute, int value)
		{
			this.attribute = attribute;
			this.value = value;
		}

		// Token: 0x04007A67 RID: 31335
		public Klei.AI.Attribute attribute;

		// Token: 0x04007A68 RID: 31336
		public int value;
	}
}
