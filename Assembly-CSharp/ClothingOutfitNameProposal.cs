using System;
using System.Runtime.CompilerServices;

// Token: 0x020007B5 RID: 1973
public readonly struct ClothingOutfitNameProposal
{
	// Token: 0x06003626 RID: 13862 RVA: 0x00127209 File Offset: 0x00125409
	private ClothingOutfitNameProposal(string candidateName, ClothingOutfitNameProposal.Result result)
	{
		this.candidateName = candidateName;
		this.result = result;
	}

	// Token: 0x06003627 RID: 13863 RVA: 0x0012721C File Offset: 0x0012541C
	public static ClothingOutfitNameProposal ForNewOutfit(string candidateName)
	{
		ClothingOutfitNameProposal.<>c__DisplayClass3_0 CS$<>8__locals1;
		CS$<>8__locals1.candidateName = candidateName;
		if (string.IsNullOrEmpty(CS$<>8__locals1.candidateName))
		{
			return ClothingOutfitNameProposal.<ForNewOutfit>g__Make|3_0(ClothingOutfitNameProposal.Result.Error_NoInputName, ref CS$<>8__locals1);
		}
		if (ClothingOutfitTarget.DoesTemplateExist(CS$<>8__locals1.candidateName))
		{
			return ClothingOutfitNameProposal.<ForNewOutfit>g__Make|3_0(ClothingOutfitNameProposal.Result.Error_NameAlreadyExists, ref CS$<>8__locals1);
		}
		return ClothingOutfitNameProposal.<ForNewOutfit>g__Make|3_0(ClothingOutfitNameProposal.Result.NewOutfit, ref CS$<>8__locals1);
	}

	// Token: 0x06003628 RID: 13864 RVA: 0x00127268 File Offset: 0x00125468
	public static ClothingOutfitNameProposal FromExistingOutfit(string candidateName, ClothingOutfitTarget existingOutfit, bool isSameNameAllowed)
	{
		ClothingOutfitNameProposal.<>c__DisplayClass4_0 CS$<>8__locals1;
		CS$<>8__locals1.candidateName = candidateName;
		if (string.IsNullOrEmpty(CS$<>8__locals1.candidateName))
		{
			return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.Error_NoInputName, ref CS$<>8__locals1);
		}
		if (!ClothingOutfitTarget.DoesTemplateExist(CS$<>8__locals1.candidateName))
		{
			return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.NewOutfit, ref CS$<>8__locals1);
		}
		if (!isSameNameAllowed || !(CS$<>8__locals1.candidateName == existingOutfit.ReadName()))
		{
			return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.Error_NameAlreadyExists, ref CS$<>8__locals1);
		}
		if (existingOutfit.CanWriteName)
		{
			return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.SameOutfit, ref CS$<>8__locals1);
		}
		return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.Error_SameOutfitReadonly, ref CS$<>8__locals1);
	}

	// Token: 0x06003629 RID: 13865 RVA: 0x001272E3 File Offset: 0x001254E3
	[CompilerGenerated]
	internal static ClothingOutfitNameProposal <ForNewOutfit>g__Make|3_0(ClothingOutfitNameProposal.Result result, ref ClothingOutfitNameProposal.<>c__DisplayClass3_0 A_1)
	{
		return new ClothingOutfitNameProposal(A_1.candidateName, result);
	}

	// Token: 0x0600362A RID: 13866 RVA: 0x001272F1 File Offset: 0x001254F1
	[CompilerGenerated]
	internal static ClothingOutfitNameProposal <FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result result, ref ClothingOutfitNameProposal.<>c__DisplayClass4_0 A_1)
	{
		return new ClothingOutfitNameProposal(A_1.candidateName, result);
	}

	// Token: 0x04002031 RID: 8241
	public readonly string candidateName;

	// Token: 0x04002032 RID: 8242
	public readonly ClothingOutfitNameProposal.Result result;

	// Token: 0x0200166D RID: 5741
	public enum Result
	{
		// Token: 0x04006FA7 RID: 28583
		None,
		// Token: 0x04006FA8 RID: 28584
		NewOutfit,
		// Token: 0x04006FA9 RID: 28585
		SameOutfit,
		// Token: 0x04006FAA RID: 28586
		Error_NoInputName,
		// Token: 0x04006FAB RID: 28587
		Error_NameAlreadyExists,
		// Token: 0x04006FAC RID: 28588
		Error_SameOutfitReadonly
	}
}
