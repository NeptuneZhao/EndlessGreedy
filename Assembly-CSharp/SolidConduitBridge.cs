using System;
using UnityEngine;

// Token: 0x02000769 RID: 1897
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitBridge")]
public class SolidConduitBridge : ConduitBridgeBase
{
	// Token: 0x1700035F RID: 863
	// (get) Token: 0x0600330F RID: 13071 RVA: 0x001186B3 File Offset: 0x001168B3
	public bool IsDispensing
	{
		get
		{
			return this.dispensing;
		}
	}

	// Token: 0x06003310 RID: 13072 RVA: 0x001186BC File Offset: 0x001168BC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		SolidConduit.GetFlowManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
	}

	// Token: 0x06003311 RID: 13073 RVA: 0x00118705 File Offset: 0x00116905
	protected override void OnCleanUp()
	{
		SolidConduit.GetFlowManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x06003312 RID: 13074 RVA: 0x00118724 File Offset: 0x00116924
	private void ConduitUpdate(float dt)
	{
		this.dispensing = false;
		float num = 0f;
		if (this.operational && !this.operational.IsOperational)
		{
			base.SendEmptyOnMassTransfer();
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
		{
			base.SendEmptyOnMassTransfer();
			return;
		}
		if (flowManager.IsConduitFull(this.inputCell) && flowManager.IsConduitEmpty(this.outputCell))
		{
			Pickupable pickupable = flowManager.GetPickupable(flowManager.GetContents(this.inputCell).pickupableHandle);
			if (pickupable == null)
			{
				flowManager.RemovePickupable(this.inputCell);
				base.SendEmptyOnMassTransfer();
				return;
			}
			float num2 = pickupable.PrimaryElement.Mass;
			if (this.desiredMassTransfer != null)
			{
				num2 = this.desiredMassTransfer(dt, pickupable.PrimaryElement.Element.id, pickupable.PrimaryElement.Mass, pickupable.PrimaryElement.Temperature, pickupable.PrimaryElement.DiseaseIdx, pickupable.PrimaryElement.DiseaseCount, pickupable);
			}
			if (num2 == 0f)
			{
				base.SendEmptyOnMassTransfer();
				return;
			}
			if (num2 < pickupable.PrimaryElement.Mass)
			{
				Pickupable pickupable2 = pickupable.Take(num2);
				flowManager.AddPickupable(this.outputCell, pickupable2);
				this.dispensing = true;
				num = pickupable2.PrimaryElement.Mass;
				if (this.OnMassTransfer != null)
				{
					this.OnMassTransfer(pickupable2.PrimaryElement.ElementID, num, pickupable2.PrimaryElement.Temperature, pickupable2.PrimaryElement.DiseaseIdx, pickupable2.PrimaryElement.DiseaseCount, pickupable2);
				}
			}
			else
			{
				Pickupable pickupable3 = flowManager.RemovePickupable(this.inputCell);
				if (pickupable3)
				{
					flowManager.AddPickupable(this.outputCell, pickupable3);
					this.dispensing = true;
					num = pickupable3.PrimaryElement.Mass;
					if (this.OnMassTransfer != null)
					{
						this.OnMassTransfer(pickupable3.PrimaryElement.ElementID, num, pickupable3.PrimaryElement.Temperature, pickupable3.PrimaryElement.DiseaseIdx, pickupable3.PrimaryElement.DiseaseCount, pickupable3);
					}
				}
			}
		}
		if (num == 0f)
		{
			base.SendEmptyOnMassTransfer();
		}
	}

	// Token: 0x04001E24 RID: 7716
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001E25 RID: 7717
	private int inputCell;

	// Token: 0x04001E26 RID: 7718
	private int outputCell;

	// Token: 0x04001E27 RID: 7719
	private bool dispensing;
}
