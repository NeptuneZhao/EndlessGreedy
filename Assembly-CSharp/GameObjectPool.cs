using System;
using UnityEngine;

// Token: 0x0200040E RID: 1038
public class GameObjectPool : ObjectPool<GameObject>
{
	// Token: 0x060015E5 RID: 5605 RVA: 0x00077783 File Offset: 0x00075983
	public GameObjectPool(Func<GameObject> instantiator, int initial_count = 0) : base(instantiator, initial_count)
	{
	}

	// Token: 0x060015E6 RID: 5606 RVA: 0x0007778D File Offset: 0x0007598D
	public override GameObject GetInstance()
	{
		return base.GetInstance();
	}

	// Token: 0x060015E7 RID: 5607 RVA: 0x00077798 File Offset: 0x00075998
	public void Destroy()
	{
		for (int i = this.unused.Count - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.unused.Pop());
		}
	}
}
