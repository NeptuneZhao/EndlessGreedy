using System;
using Rendering;
using UnityEngine;

// Token: 0x02000B75 RID: 2933
public class BlockTileDecorInfo : ScriptableObject
{
	// Token: 0x06005820 RID: 22560 RVA: 0x001FD828 File Offset: 0x001FBA28
	public void PostProcess()
	{
		if (this.decor != null && this.atlas != null && this.atlas.items != null)
		{
			for (int i = 0; i < this.decor.Length; i++)
			{
				if (this.decor[i].variants != null && this.decor[i].variants.Length != 0)
				{
					for (int j = 0; j < this.decor[i].variants.Length; j++)
					{
						bool flag = false;
						foreach (TextureAtlas.Item item in this.atlas.items)
						{
							string text = item.name;
							int num = text.IndexOf("/");
							if (num != -1)
							{
								text = text.Substring(num + 1);
							}
							if (this.decor[i].variants[j].name == text)
							{
								this.decor[i].variants[j].atlasItem = item;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							DebugUtil.LogErrorArgs(new object[]
							{
								base.name,
								"/",
								this.decor[i].name,
								"could not find ",
								this.decor[i].variants[j].name,
								"in",
								this.atlas.name
							});
						}
					}
				}
			}
		}
	}

	// Token: 0x0400399B RID: 14747
	public TextureAtlas atlas;

	// Token: 0x0400399C RID: 14748
	public TextureAtlas atlasSpec;

	// Token: 0x0400399D RID: 14749
	public int sortOrder;

	// Token: 0x0400399E RID: 14750
	public BlockTileDecorInfo.Decor[] decor;

	// Token: 0x02001BDA RID: 7130
	[Serializable]
	public struct ImageInfo
	{
		// Token: 0x040080E2 RID: 32994
		public string name;

		// Token: 0x040080E3 RID: 32995
		public Vector3 offset;

		// Token: 0x040080E4 RID: 32996
		[NonSerialized]
		public TextureAtlas.Item atlasItem;
	}

	// Token: 0x02001BDB RID: 7131
	[Serializable]
	public struct Decor
	{
		// Token: 0x040080E5 RID: 32997
		public string name;

		// Token: 0x040080E6 RID: 32998
		[EnumFlags]
		public BlockTileRenderer.Bits requiredConnections;

		// Token: 0x040080E7 RID: 32999
		[EnumFlags]
		public BlockTileRenderer.Bits forbiddenConnections;

		// Token: 0x040080E8 RID: 33000
		public float probabilityCutoff;

		// Token: 0x040080E9 RID: 33001
		public BlockTileDecorInfo.ImageInfo[] variants;

		// Token: 0x040080EA RID: 33002
		public int sortOrder;
	}
}
