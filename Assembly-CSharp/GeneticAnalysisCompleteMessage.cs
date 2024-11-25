using System;
using KSerialization;
using STRINGS;

// Token: 0x02000CC8 RID: 3272
public class GeneticAnalysisCompleteMessage : Message
{
	// Token: 0x06006533 RID: 25907 RVA: 0x0025D9CF File Offset: 0x0025BBCF
	public GeneticAnalysisCompleteMessage()
	{
	}

	// Token: 0x06006534 RID: 25908 RVA: 0x0025D9D7 File Offset: 0x0025BBD7
	public GeneticAnalysisCompleteMessage(Tag subSpeciesID)
	{
		this.subSpeciesID = subSpeciesID;
	}

	// Token: 0x06006535 RID: 25909 RVA: 0x0025D9E6 File Offset: 0x0025BBE6
	public override string GetSound()
	{
		return "";
	}

	// Token: 0x06006536 RID: 25910 RVA: 0x0025D9F0 File Offset: 0x0025BBF0
	public override string GetMessageBody()
	{
		PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = PlantSubSpeciesCatalog.Instance.FindSubSpecies(this.subSpeciesID);
		return MISC.NOTIFICATIONS.GENETICANALYSISCOMPLETE.MESSAGEBODY.Replace("{Plant}", subSpeciesInfo.speciesID.ProperName()).Replace("{Subspecies}", subSpeciesInfo.GetNameWithMutations(subSpeciesInfo.speciesID.ProperName(), true, false)).Replace("{Info}", subSpeciesInfo.GetMutationsTooltip());
	}

	// Token: 0x06006537 RID: 25911 RVA: 0x0025DA57 File Offset: 0x0025BC57
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.GENETICANALYSISCOMPLETE.NAME;
	}

	// Token: 0x06006538 RID: 25912 RVA: 0x0025DA64 File Offset: 0x0025BC64
	public override string GetTooltip()
	{
		PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = PlantSubSpeciesCatalog.Instance.FindSubSpecies(this.subSpeciesID);
		return MISC.NOTIFICATIONS.GENETICANALYSISCOMPLETE.TOOLTIP.Replace("{Plant}", subSpeciesInfo.speciesID.ProperName());
	}

	// Token: 0x06006539 RID: 25913 RVA: 0x0025DA9C File Offset: 0x0025BC9C
	public override bool IsValid()
	{
		return this.subSpeciesID.IsValid;
	}

	// Token: 0x0400447B RID: 17531
	[Serialize]
	public Tag subSpeciesID;
}
