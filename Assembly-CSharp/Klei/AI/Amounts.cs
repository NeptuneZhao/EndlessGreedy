using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F3D RID: 3901
	public class Amounts : Modifications<Amount, AmountInstance>
	{
		// Token: 0x06007801 RID: 30721 RVA: 0x002F7BFD File Offset: 0x002F5DFD
		public Amounts(GameObject go) : base(go, null)
		{
		}

		// Token: 0x06007802 RID: 30722 RVA: 0x002F7C07 File Offset: 0x002F5E07
		public float GetValue(string amount_id)
		{
			return base.Get(amount_id).value;
		}

		// Token: 0x06007803 RID: 30723 RVA: 0x002F7C15 File Offset: 0x002F5E15
		public void SetValue(string amount_id, float value)
		{
			base.Get(amount_id).value = value;
		}

		// Token: 0x06007804 RID: 30724 RVA: 0x002F7C24 File Offset: 0x002F5E24
		public override AmountInstance Add(AmountInstance instance)
		{
			instance.Activate();
			return base.Add(instance);
		}

		// Token: 0x06007805 RID: 30725 RVA: 0x002F7C33 File Offset: 0x002F5E33
		public override void Remove(AmountInstance instance)
		{
			instance.Deactivate();
			base.Remove(instance);
		}

		// Token: 0x06007806 RID: 30726 RVA: 0x002F7C44 File Offset: 0x002F5E44
		public void Cleanup()
		{
			for (int i = 0; i < base.Count; i++)
			{
				base[i].Deactivate();
			}
		}
	}
}
