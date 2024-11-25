using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200064F RID: 1615
public class ArtifactSelector : KMonoBehaviour
{
	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x06002780 RID: 10112 RVA: 0x000E0E07 File Offset: 0x000DF007
	public int AnalyzedArtifactCount
	{
		get
		{
			return this.analyzedArtifactCount;
		}
	}

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x06002781 RID: 10113 RVA: 0x000E0E0F File Offset: 0x000DF00F
	public int AnalyzedSpaceArtifactCount
	{
		get
		{
			return this.analyzedSpaceArtifactCount;
		}
	}

	// Token: 0x06002782 RID: 10114 RVA: 0x000E0E17 File Offset: 0x000DF017
	public List<string> GetAnalyzedArtifactIDs()
	{
		return this.analyzedArtifatIDs;
	}

	// Token: 0x06002783 RID: 10115 RVA: 0x000E0E20 File Offset: 0x000DF020
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ArtifactSelector.Instance = this;
		this.placedArtifacts.Add(ArtifactType.Terrestrial, new List<string>());
		this.placedArtifacts.Add(ArtifactType.Space, new List<string>());
		this.placedArtifacts.Add(ArtifactType.Any, new List<string>());
	}

	// Token: 0x06002784 RID: 10116 RVA: 0x000E0E6C File Offset: 0x000DF06C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int num = 0;
		int num2 = 0;
		foreach (string artifactID in this.analyzedArtifatIDs)
		{
			ArtifactType artifactType = this.GetArtifactType(artifactID);
			if (artifactType != ArtifactType.Space)
			{
				if (artifactType == ArtifactType.Terrestrial)
				{
					num++;
				}
			}
			else
			{
				num2++;
			}
		}
		if (num > this.analyzedArtifactCount)
		{
			this.analyzedArtifactCount = num;
		}
		if (num2 > this.analyzedSpaceArtifactCount)
		{
			this.analyzedSpaceArtifactCount = num2;
		}
	}

	// Token: 0x06002785 RID: 10117 RVA: 0x000E0F00 File Offset: 0x000DF100
	public bool RecordArtifactAnalyzed(string id)
	{
		if (this.analyzedArtifatIDs.Contains(id))
		{
			return false;
		}
		this.analyzedArtifatIDs.Add(id);
		return true;
	}

	// Token: 0x06002786 RID: 10118 RVA: 0x000E0F1F File Offset: 0x000DF11F
	public void IncrementAnalyzedTerrestrialArtifacts()
	{
		this.analyzedArtifactCount++;
	}

	// Token: 0x06002787 RID: 10119 RVA: 0x000E0F2F File Offset: 0x000DF12F
	public void IncrementAnalyzedSpaceArtifacts()
	{
		this.analyzedSpaceArtifactCount++;
	}

	// Token: 0x06002788 RID: 10120 RVA: 0x000E0F40 File Offset: 0x000DF140
	public string GetUniqueArtifactID(ArtifactType artifactType = ArtifactType.Any)
	{
		List<string> list = new List<string>();
		foreach (string item in ArtifactConfig.artifactItems[artifactType])
		{
			if (!this.placedArtifacts[artifactType].Contains(item))
			{
				list.Add(item);
			}
		}
		string text = "artifact_officemug";
		if (list.Count == 0 && artifactType != ArtifactType.Any)
		{
			foreach (string item2 in ArtifactConfig.artifactItems[ArtifactType.Any])
			{
				if (!this.placedArtifacts[ArtifactType.Any].Contains(item2))
				{
					list.Add(item2);
					artifactType = ArtifactType.Any;
				}
			}
		}
		if (list.Count != 0)
		{
			text = list[UnityEngine.Random.Range(0, list.Count)];
		}
		this.placedArtifacts[artifactType].Add(text);
		return text;
	}

	// Token: 0x06002789 RID: 10121 RVA: 0x000E1054 File Offset: 0x000DF254
	public void ReserveArtifactID(string artifactID, ArtifactType artifactType = ArtifactType.Any)
	{
		if (this.placedArtifacts[artifactType].Contains(artifactID))
		{
			DebugUtil.Assert(true, string.Format("Tried to add {0} to placedArtifacts but it already exists in the list!", artifactID));
		}
		this.placedArtifacts[artifactType].Add(artifactID);
	}

	// Token: 0x0600278A RID: 10122 RVA: 0x000E108D File Offset: 0x000DF28D
	public ArtifactType GetArtifactType(string artifactID)
	{
		if (this.placedArtifacts[ArtifactType.Terrestrial].Contains(artifactID))
		{
			return ArtifactType.Terrestrial;
		}
		if (this.placedArtifacts[ArtifactType.Space].Contains(artifactID))
		{
			return ArtifactType.Space;
		}
		return ArtifactType.Any;
	}

	// Token: 0x040016C6 RID: 5830
	public static ArtifactSelector Instance;

	// Token: 0x040016C7 RID: 5831
	[Serialize]
	private Dictionary<ArtifactType, List<string>> placedArtifacts = new Dictionary<ArtifactType, List<string>>();

	// Token: 0x040016C8 RID: 5832
	[Serialize]
	private int analyzedArtifactCount;

	// Token: 0x040016C9 RID: 5833
	[Serialize]
	private int analyzedSpaceArtifactCount;

	// Token: 0x040016CA RID: 5834
	[Serialize]
	private List<string> analyzedArtifatIDs = new List<string>();

	// Token: 0x040016CB RID: 5835
	private const string DEFAULT_ARTIFACT_ID = "artifact_officemug";
}
