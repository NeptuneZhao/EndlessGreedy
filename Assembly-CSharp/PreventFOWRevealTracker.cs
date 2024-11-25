using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000A01 RID: 2561
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/PreventFOWRevealTracker")]
public class PreventFOWRevealTracker : KMonoBehaviour
{
	// Token: 0x06004A28 RID: 18984 RVA: 0x001A7CD4 File Offset: 0x001A5ED4
	[OnSerializing]
	private void OnSerialize()
	{
		this.preventFOWRevealCells.Clear();
		for (int i = 0; i < Grid.VisMasks.Length; i++)
		{
			if (Grid.PreventFogOfWarReveal[i])
			{
				this.preventFOWRevealCells.Add(i);
			}
		}
	}

	// Token: 0x06004A29 RID: 18985 RVA: 0x001A7D18 File Offset: 0x001A5F18
	[OnDeserialized]
	private void OnDeserialized()
	{
		foreach (int i in this.preventFOWRevealCells)
		{
			Grid.PreventFogOfWarReveal[i] = true;
		}
	}

	// Token: 0x040030A7 RID: 12455
	[Serialize]
	public List<int> preventFOWRevealCells;
}
