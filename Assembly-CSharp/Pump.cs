using System;
using UnityEngine;

// Token: 0x02000751 RID: 1873
[AddComponentMenu("KMonoBehaviour/scripts/Pump")]
public class Pump : KMonoBehaviour, ISim1000ms
{
	// Token: 0x060031FC RID: 12796 RVA: 0x00112A5F File Offset: 0x00110C5F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.consumer.EnableConsumption(false);
	}

	// Token: 0x060031FD RID: 12797 RVA: 0x00112A73 File Offset: 0x00110C73
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.elapsedTime = 0f;
		this.pumpable = this.UpdateOperational();
		this.dispenser.GetConduitManager().AddConduitUpdater(new Action<float>(this.OnConduitUpdate), ConduitFlowPriority.LastPostUpdate);
	}

	// Token: 0x060031FE RID: 12798 RVA: 0x00112AB0 File Offset: 0x00110CB0
	protected override void OnCleanUp()
	{
		this.dispenser.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.OnConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x060031FF RID: 12799 RVA: 0x00112AD4 File Offset: 0x00110CD4
	public void Sim1000ms(float dt)
	{
		this.elapsedTime += dt;
		if (this.elapsedTime >= 1f)
		{
			this.pumpable = this.UpdateOperational();
			this.elapsedTime = 0f;
		}
		if (this.operational.IsOperational && this.pumpable)
		{
			this.operational.SetActive(true, false);
			return;
		}
		this.operational.SetActive(false, false);
	}

	// Token: 0x06003200 RID: 12800 RVA: 0x00112B44 File Offset: 0x00110D44
	private bool UpdateOperational()
	{
		Element.State state = Element.State.Vacuum;
		ConduitType conduitType = this.dispenser.conduitType;
		if (conduitType != ConduitType.Gas)
		{
			if (conduitType == ConduitType.Liquid)
			{
				state = Element.State.Liquid;
			}
		}
		else
		{
			state = Element.State.Gas;
		}
		bool flag = this.IsPumpable(state, (int)this.consumer.consumptionRadius);
		StatusItem status_item = (state == Element.State.Gas) ? Db.Get().BuildingStatusItems.NoGasElementToPump : Db.Get().BuildingStatusItems.NoLiquidElementToPump;
		this.noElementStatusGuid = this.selectable.ToggleStatusItem(status_item, this.noElementStatusGuid, !flag, null);
		this.operational.SetFlag(Pump.PumpableFlag, !this.storage.IsFull() && flag);
		return flag;
	}

	// Token: 0x06003201 RID: 12801 RVA: 0x00112BE8 File Offset: 0x00110DE8
	private bool IsPumpable(Element.State expected_state, int radius)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		for (int i = 0; i < (int)this.consumer.consumptionRadius; i++)
		{
			for (int j = 0; j < (int)this.consumer.consumptionRadius; j++)
			{
				int num2 = num + j + Grid.WidthInCells * i;
				if (Grid.Element[num2].IsState(expected_state))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003202 RID: 12802 RVA: 0x00112C50 File Offset: 0x00110E50
	private void OnConduitUpdate(float dt)
	{
		this.conduitBlockedStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.ConduitBlocked, this.conduitBlockedStatusGuid, this.dispenser.blocked, null);
	}

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x06003203 RID: 12803 RVA: 0x00112C84 File Offset: 0x00110E84
	public ConduitType conduitType
	{
		get
		{
			return this.dispenser.conduitType;
		}
	}

	// Token: 0x04001D6B RID: 7531
	public static readonly Operational.Flag PumpableFlag = new Operational.Flag("vent", Operational.Flag.Type.Requirement);

	// Token: 0x04001D6C RID: 7532
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001D6D RID: 7533
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04001D6E RID: 7534
	[MyCmpGet]
	private ElementConsumer consumer;

	// Token: 0x04001D6F RID: 7535
	[MyCmpGet]
	private ConduitDispenser dispenser;

	// Token: 0x04001D70 RID: 7536
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04001D71 RID: 7537
	private const float OperationalUpdateInterval = 1f;

	// Token: 0x04001D72 RID: 7538
	private float elapsedTime;

	// Token: 0x04001D73 RID: 7539
	private bool pumpable;

	// Token: 0x04001D74 RID: 7540
	private Guid conduitBlockedStatusGuid;

	// Token: 0x04001D75 RID: 7541
	private Guid noElementStatusGuid;
}
