using System;
using UnityEngine;

// Token: 0x020004BF RID: 1215
[Serializable]
public class DefComponent<T> where T : Component
{
	// Token: 0x06001A31 RID: 6705 RVA: 0x0008AEDC File Offset: 0x000890DC
	public DefComponent(T cmp)
	{
		this.cmp = cmp;
	}

	// Token: 0x06001A32 RID: 6706 RVA: 0x0008AEEC File Offset: 0x000890EC
	public T Get(StateMachine.Instance smi)
	{
		T[] components = this.cmp.GetComponents<T>();
		int num = 0;
		while (num < components.Length && !(components[num] == this.cmp))
		{
			num++;
		}
		return smi.gameObject.GetComponents<T>()[num];
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x0008AF47 File Offset: 0x00089147
	public static implicit operator DefComponent<T>(T cmp)
	{
		return new DefComponent<T>(cmp);
	}

	// Token: 0x04000EE7 RID: 3815
	[SerializeField]
	private T cmp;
}
