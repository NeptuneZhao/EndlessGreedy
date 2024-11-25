using System;
using UnityEngine;

namespace FoodRehydrator
{
	// Token: 0x02000E2A RID: 3626
	public class AccessabilityManager : KMonoBehaviour
	{
		// Token: 0x060073B4 RID: 29620 RVA: 0x002C469E File Offset: 0x002C289E
		protected override void OnSpawn()
		{
			base.OnSpawn();
			Components.FoodRehydrators.Add(base.gameObject);
			base.Subscribe(824508782, new Action<object>(this.ActiveChangedHandler));
		}

		// Token: 0x060073B5 RID: 29621 RVA: 0x002C46CE File Offset: 0x002C28CE
		protected override void OnCleanUp()
		{
			Components.FoodRehydrators.Remove(base.gameObject);
			base.OnCleanUp();
		}

		// Token: 0x060073B6 RID: 29622 RVA: 0x002C46E6 File Offset: 0x002C28E6
		public void Reserve(GameObject reserver)
		{
			this.reserver = reserver;
			global::Debug.Assert(reserver != null && reserver.GetComponent<MinionResume>() != null);
		}

		// Token: 0x060073B7 RID: 29623 RVA: 0x002C470C File Offset: 0x002C290C
		public void Unreserve()
		{
			this.activeWorkable = null;
			this.reserver = null;
		}

		// Token: 0x060073B8 RID: 29624 RVA: 0x002C471C File Offset: 0x002C291C
		public void SetActiveWorkable(Workable work)
		{
			DebugUtil.DevAssert(this.activeWorkable == null || work == null, "FoodRehydrator::AccessabilityManager activating a second workable", null);
			this.activeWorkable = work;
			this.operational.SetActive(this.activeWorkable != null, false);
		}

		// Token: 0x060073B9 RID: 29625 RVA: 0x002C476B File Offset: 0x002C296B
		public bool CanAccess(GameObject worker)
		{
			return this.operational.IsOperational && (this.reserver == null || this.reserver == worker);
		}

		// Token: 0x060073BA RID: 29626 RVA: 0x002C4798 File Offset: 0x002C2998
		protected void ActiveChangedHandler(object obj)
		{
			if (!this.operational.IsActive)
			{
				this.CancelActiveWorkable();
			}
		}

		// Token: 0x060073BB RID: 29627 RVA: 0x002C47AD File Offset: 0x002C29AD
		public void CancelActiveWorkable()
		{
			if (this.activeWorkable != null)
			{
				this.activeWorkable.StopWork(this.activeWorkable.worker, true);
			}
		}

		// Token: 0x04004FA9 RID: 20393
		[MyCmpReq]
		private Operational operational;

		// Token: 0x04004FAA RID: 20394
		private GameObject reserver;

		// Token: 0x04004FAB RID: 20395
		private Workable activeWorkable;
	}
}
