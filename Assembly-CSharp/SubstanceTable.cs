using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000B28 RID: 2856
public class SubstanceTable : ScriptableObject, ISerializationCallbackReceiver
{
	// Token: 0x0600552F RID: 21807 RVA: 0x001E7176 File Offset: 0x001E5376
	public List<Substance> GetList()
	{
		return this.list;
	}

	// Token: 0x06005530 RID: 21808 RVA: 0x001E7180 File Offset: 0x001E5380
	public Substance GetSubstance(SimHashes substance)
	{
		int count = this.list.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.list[i].elementID == substance)
			{
				return this.list[i];
			}
		}
		return null;
	}

	// Token: 0x06005531 RID: 21809 RVA: 0x001E71C7 File Offset: 0x001E53C7
	public void OnBeforeSerialize()
	{
		this.BindAnimList();
	}

	// Token: 0x06005532 RID: 21810 RVA: 0x001E71CF File Offset: 0x001E53CF
	public void OnAfterDeserialize()
	{
		this.BindAnimList();
	}

	// Token: 0x06005533 RID: 21811 RVA: 0x001E71D8 File Offset: 0x001E53D8
	private void BindAnimList()
	{
		foreach (Substance substance in this.list)
		{
			if (substance.anim != null && (substance.anims == null || substance.anims.Length == 0))
			{
				substance.anims = new KAnimFile[1];
				substance.anims[0] = substance.anim;
			}
		}
	}

	// Token: 0x06005534 RID: 21812 RVA: 0x001E7260 File Offset: 0x001E5460
	public void RemoveDuplicates()
	{
		this.list = this.list.Distinct(new SubstanceTable.SubstanceEqualityComparer()).ToList<Substance>();
	}

	// Token: 0x040037CE RID: 14286
	[SerializeField]
	private List<Substance> list;

	// Token: 0x040037CF RID: 14287
	public Material solidMaterial;

	// Token: 0x040037D0 RID: 14288
	public Material liquidMaterial;

	// Token: 0x02001B7C RID: 7036
	private class SubstanceEqualityComparer : IEqualityComparer<Substance>
	{
		// Token: 0x0600A37E RID: 41854 RVA: 0x00389E81 File Offset: 0x00388081
		public bool Equals(Substance x, Substance y)
		{
			return x.elementID.Equals(y.elementID);
		}

		// Token: 0x0600A37F RID: 41855 RVA: 0x00389E9F File Offset: 0x0038809F
		public int GetHashCode(Substance obj)
		{
			return obj.elementID.GetHashCode();
		}
	}
}
