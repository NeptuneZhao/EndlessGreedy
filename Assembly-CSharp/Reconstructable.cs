using System;
using KSerialization;
using UnityEngine;

// Token: 0x020005A9 RID: 1449
public class Reconstructable : KMonoBehaviour
{
	// Token: 0x17000185 RID: 389
	// (get) Token: 0x06002273 RID: 8819 RVA: 0x000BFCC6 File Offset: 0x000BDEC6
	public bool AllowReconstruct
	{
		get
		{
			return this.deconstructable.allowDeconstruction && (this.building.Def.ShowInBuildMenu || SelectModuleSideScreen.moduleButtonSortOrder.Contains(this.building.Def.PrefabID));
		}
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x06002274 RID: 8820 RVA: 0x000BFD05 File Offset: 0x000BDF05
	public Tag PrimarySelectedElementTag
	{
		get
		{
			return this.selectedElementsTags[0];
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x06002275 RID: 8821 RVA: 0x000BFD13 File Offset: 0x000BDF13
	public bool ReconstructRequested
	{
		get
		{
			return this.reconstructRequested;
		}
	}

	// Token: 0x06002276 RID: 8822 RVA: 0x000BFD1B File Offset: 0x000BDF1B
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06002277 RID: 8823 RVA: 0x000BFD24 File Offset: 0x000BDF24
	public void RequestReconstruct(Tag newElement)
	{
		if (!this.deconstructable.allowDeconstruction)
		{
			return;
		}
		this.reconstructRequested = !this.reconstructRequested;
		if (this.reconstructRequested)
		{
			this.deconstructable.QueueDeconstruction(false);
			this.selectedElementsTags = new Tag[]
			{
				newElement
			};
		}
		else
		{
			this.deconstructable.CancelDeconstruction();
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06002278 RID: 8824 RVA: 0x000BFD98 File Offset: 0x000BDF98
	public void CancelReconstructOrder()
	{
		this.reconstructRequested = false;
		this.deconstructable.CancelDeconstruction();
		base.Trigger(954267658, null);
	}

	// Token: 0x06002279 RID: 8825 RVA: 0x000BFDB8 File Offset: 0x000BDFB8
	public void TryCommenceReconstruct()
	{
		if (!this.deconstructable.allowDeconstruction)
		{
			return;
		}
		if (!this.reconstructRequested)
		{
			return;
		}
		string facadeID = this.building.GetComponent<BuildingFacade>().CurrentFacade;
		Vector3 position = this.building.transform.position;
		Orientation orientation = this.building.Orientation;
		GameScheduler.Instance.ScheduleNextFrame("Reconstruct", delegate(object data)
		{
			this.building.Def.TryPlace(null, position, orientation, this.selectedElementsTags, facadeID, false, 0);
		}, null, null);
	}

	// Token: 0x04001371 RID: 4977
	[MyCmpReq]
	private Deconstructable deconstructable;

	// Token: 0x04001372 RID: 4978
	[MyCmpReq]
	private Building building;

	// Token: 0x04001373 RID: 4979
	[Serialize]
	private Tag[] selectedElementsTags;

	// Token: 0x04001374 RID: 4980
	[Serialize]
	private bool reconstructRequested;
}
