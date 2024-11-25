using System;
using UnityEngine;

// Token: 0x02000933 RID: 2355
public readonly struct JoyResponseOutfitTarget
{
	// Token: 0x06004468 RID: 17512 RVA: 0x001857B5 File Offset: 0x001839B5
	public JoyResponseOutfitTarget(JoyResponseOutfitTarget.Implementation impl)
	{
		this.impl = impl;
	}

	// Token: 0x06004469 RID: 17513 RVA: 0x001857BE File Offset: 0x001839BE
	public Option<string> ReadFacadeId()
	{
		return this.impl.ReadFacadeId();
	}

	// Token: 0x0600446A RID: 17514 RVA: 0x001857CB File Offset: 0x001839CB
	public void WriteFacadeId(Option<string> facadeId)
	{
		this.impl.WriteFacadeId(facadeId);
	}

	// Token: 0x0600446B RID: 17515 RVA: 0x001857D9 File Offset: 0x001839D9
	public string GetMinionName()
	{
		return this.impl.GetMinionName();
	}

	// Token: 0x0600446C RID: 17516 RVA: 0x001857E6 File Offset: 0x001839E6
	public Personality GetPersonality()
	{
		return this.impl.GetPersonality();
	}

	// Token: 0x0600446D RID: 17517 RVA: 0x001857F3 File Offset: 0x001839F3
	public static JoyResponseOutfitTarget FromMinion(GameObject minionInstance)
	{
		return new JoyResponseOutfitTarget(new JoyResponseOutfitTarget.MinionInstanceTarget(minionInstance));
	}

	// Token: 0x0600446E RID: 17518 RVA: 0x00185805 File Offset: 0x00183A05
	public static JoyResponseOutfitTarget FromPersonality(Personality personality)
	{
		return new JoyResponseOutfitTarget(new JoyResponseOutfitTarget.PersonalityTarget(personality));
	}

	// Token: 0x04002CC4 RID: 11460
	private readonly JoyResponseOutfitTarget.Implementation impl;

	// Token: 0x0200188F RID: 6287
	public interface Implementation
	{
		// Token: 0x060098E0 RID: 39136
		Option<string> ReadFacadeId();

		// Token: 0x060098E1 RID: 39137
		void WriteFacadeId(Option<string> permitId);

		// Token: 0x060098E2 RID: 39138
		string GetMinionName();

		// Token: 0x060098E3 RID: 39139
		Personality GetPersonality();
	}

	// Token: 0x02001890 RID: 6288
	public readonly struct MinionInstanceTarget : JoyResponseOutfitTarget.Implementation
	{
		// Token: 0x060098E4 RID: 39140 RVA: 0x00369184 File Offset: 0x00367384
		public MinionInstanceTarget(GameObject minionInstance)
		{
			this.minionInstance = minionInstance;
			this.wearableAccessorizer = minionInstance.GetComponent<WearableAccessorizer>();
		}

		// Token: 0x060098E5 RID: 39141 RVA: 0x00369199 File Offset: 0x00367399
		public string GetMinionName()
		{
			return this.minionInstance.GetProperName();
		}

		// Token: 0x060098E6 RID: 39142 RVA: 0x003691A6 File Offset: 0x003673A6
		public Personality GetPersonality()
		{
			return Db.Get().Personalities.Get(this.minionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		}

		// Token: 0x060098E7 RID: 39143 RVA: 0x003691C7 File Offset: 0x003673C7
		public Option<string> ReadFacadeId()
		{
			return this.wearableAccessorizer.GetJoyResponseId();
		}

		// Token: 0x060098E8 RID: 39144 RVA: 0x003691D4 File Offset: 0x003673D4
		public void WriteFacadeId(Option<string> permitId)
		{
			this.wearableAccessorizer.SetJoyResponseId(permitId);
		}

		// Token: 0x04007689 RID: 30345
		public readonly GameObject minionInstance;

		// Token: 0x0400768A RID: 30346
		public readonly WearableAccessorizer wearableAccessorizer;
	}

	// Token: 0x02001891 RID: 6289
	public readonly struct PersonalityTarget : JoyResponseOutfitTarget.Implementation
	{
		// Token: 0x060098E9 RID: 39145 RVA: 0x003691E2 File Offset: 0x003673E2
		public PersonalityTarget(Personality personality)
		{
			this.personality = personality;
		}

		// Token: 0x060098EA RID: 39146 RVA: 0x003691EB File Offset: 0x003673EB
		public string GetMinionName()
		{
			return this.personality.Name;
		}

		// Token: 0x060098EB RID: 39147 RVA: 0x003691F8 File Offset: 0x003673F8
		public Personality GetPersonality()
		{
			return this.personality;
		}

		// Token: 0x060098EC RID: 39148 RVA: 0x00369200 File Offset: 0x00367400
		public Option<string> ReadFacadeId()
		{
			return this.personality.GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.JoyResponse);
		}

		// Token: 0x060098ED RID: 39149 RVA: 0x00369213 File Offset: 0x00367413
		public void WriteFacadeId(Option<string> facadeId)
		{
			this.personality.SetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.JoyResponse, facadeId);
		}

		// Token: 0x0400768B RID: 30347
		public readonly Personality personality;
	}
}
