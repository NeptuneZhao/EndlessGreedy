﻿using System;
using Klei;
using UnityEngine;

// Token: 0x020006FE RID: 1790
[AddComponentMenu("KMonoBehaviour/Workable/LiquidPumpingStation")]
public class LiquidPumpingStation : Workable, ISim200ms
{
	// Token: 0x06002DCE RID: 11726 RVA: 0x00101201 File Offset: 0x000FF401
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.resetProgressOnStop = true;
		this.showProgressBar = false;
	}

	// Token: 0x06002DCF RID: 11727 RVA: 0x00101218 File Offset: 0x000FF418
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.infos = new LiquidPumpingStation.LiquidInfo[LiquidPumpingStation.liquidOffsets.Length * 2];
		this.RefreshStatusItem();
		this.Sim200ms(0f);
		base.SetWorkTime(10f);
		this.RefreshDepthAvailable();
		this.RegisterListenersToCellChanges();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_arrow",
			"meter_scale"
		});
		foreach (GameObject gameObject in base.GetComponent<Storage>().items)
		{
			if (!(gameObject == null) && gameObject != null)
			{
				gameObject.DeleteObject();
			}
		}
	}

	// Token: 0x06002DD0 RID: 11728 RVA: 0x00101300 File Offset: 0x000FF500
	private void RegisterListenersToCellChanges()
	{
		int widthInCells = base.GetComponent<BuildingComplete>().Def.WidthInCells;
		CellOffset[] array = new CellOffset[widthInCells * 4];
		for (int i = 0; i < 4; i++)
		{
			int y = -(i + 1);
			for (int j = 0; j < widthInCells; j++)
			{
				array[i * widthInCells + j] = new CellOffset(j, y);
			}
		}
		Extents extents = new Extents(Grid.PosToCell(base.transform.GetPosition()), array);
		this.partitionerEntry_solids = GameScenePartitioner.Instance.Add("LiquidPumpingStation", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnLowerCellChanged));
		this.partitionerEntry_buildings = GameScenePartitioner.Instance.Add("LiquidPumpingStation", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnLowerCellChanged));
	}

	// Token: 0x06002DD1 RID: 11729 RVA: 0x001013DC File Offset: 0x000FF5DC
	private void UnregisterListenersToCellChanges()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry_solids);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry_buildings);
	}

	// Token: 0x06002DD2 RID: 11730 RVA: 0x001013FE File Offset: 0x000FF5FE
	private void OnLowerCellChanged(object o)
	{
		this.RefreshDepthAvailable();
	}

	// Token: 0x06002DD3 RID: 11731 RVA: 0x00101408 File Offset: 0x000FF608
	private void RefreshDepthAvailable()
	{
		int num = PumpingStationGuide.GetDepthAvailable(Grid.PosToCell(this), base.gameObject);
		int num2 = 4;
		if (num != this.depthAvailable)
		{
			KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
			for (int i = 1; i <= num2; i++)
			{
				component.SetSymbolVisiblity("pipe" + i.ToString(), i <= num);
			}
			PumpingStationGuide.OccupyArea(base.gameObject, num);
			this.depthAvailable = num;
		}
	}

	// Token: 0x06002DD4 RID: 11732 RVA: 0x0010147C File Offset: 0x000FF67C
	public void Sim200ms(float dt)
	{
		if (this.session != null)
		{
			return;
		}
		int num = this.infoCount;
		for (int i = 0; i < this.infoCount; i++)
		{
			this.infos[i].amount = 0f;
		}
		if (base.GetComponent<Operational>().IsOperational)
		{
			int cell = Grid.PosToCell(this);
			for (int j = 0; j < LiquidPumpingStation.liquidOffsets.Length; j++)
			{
				if (this.depthAvailable >= Math.Abs(LiquidPumpingStation.liquidOffsets[j].y))
				{
					int num2 = Grid.OffsetCell(cell, LiquidPumpingStation.liquidOffsets[j]);
					bool flag = false;
					Element element = Grid.Element[num2];
					if (element.IsLiquid)
					{
						float num3 = Grid.Mass[num2];
						for (int k = 0; k < this.infoCount; k++)
						{
							if (this.infos[k].element == element)
							{
								LiquidPumpingStation.LiquidInfo[] array = this.infos;
								int num4 = k;
								array[num4].amount = array[num4].amount + num3;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.infos[this.infoCount].amount = num3;
							this.infos[this.infoCount].element = element;
							this.infoCount++;
						}
					}
				}
			}
		}
		int l = 0;
		while (l < this.infoCount)
		{
			LiquidPumpingStation.LiquidInfo liquidInfo = this.infos[l];
			if (liquidInfo.amount <= 1f)
			{
				if (liquidInfo.source != null)
				{
					liquidInfo.source.DeleteObject();
				}
				this.infos[l] = this.infos[this.infoCount - 1];
				this.infoCount--;
			}
			else
			{
				if (liquidInfo.source == null)
				{
					liquidInfo.source = base.GetComponent<Storage>().AddLiquid(liquidInfo.element.id, liquidInfo.amount, liquidInfo.element.defaultValues.temperature, byte.MaxValue, 0, false, true).GetComponent<SubstanceChunk>();
					Pickupable component = liquidInfo.source.GetComponent<Pickupable>();
					component.KPrefabID.AddTag(GameTags.LiquidSource, false);
					component.SetOffsets(new CellOffset[]
					{
						new CellOffset(0, 1)
					});
					component.targetWorkable = this;
					Pickupable pickupable = component;
					pickupable.OnReservationsChanged = (Action<Pickupable, bool, Pickupable.Reservation>)Delegate.Combine(pickupable.OnReservationsChanged, new Action<Pickupable, bool, Pickupable.Reservation>(this.OnReservationsChanged));
				}
				liquidInfo.source.GetComponent<Pickupable>().TotalAmount = liquidInfo.amount;
				this.infos[l] = liquidInfo;
				l++;
			}
		}
		if (num != this.infoCount)
		{
			this.RefreshStatusItem();
		}
	}

	// Token: 0x06002DD5 RID: 11733 RVA: 0x0010174C File Offset: 0x000FF94C
	private void RefreshStatusItem()
	{
		if (this.infoCount > 0)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.PumpingStation, this);
			return;
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmptyPumpingStation, this);
	}

	// Token: 0x06002DD6 RID: 11734 RVA: 0x001017BC File Offset: 0x000FF9BC
	public string ResolveString(string base_string)
	{
		string text = "";
		for (int i = 0; i < this.infoCount; i++)
		{
			if (this.infos[i].source != null)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					this.infos[i].element.name,
					": ",
					GameUtil.GetFormattedMass(this.infos[i].amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
				});
			}
		}
		return base_string.Replace("{Liquids}", text);
	}

	// Token: 0x06002DD7 RID: 11735 RVA: 0x0010185F File Offset: 0x000FFA5F
	public static bool IsLiquidAccessible(Element element)
	{
		return true;
	}

	// Token: 0x06002DD8 RID: 11736 RVA: 0x00101862 File Offset: 0x000FFA62
	public override float GetPercentComplete()
	{
		if (this.session != null)
		{
			return this.session.GetPercentComplete();
		}
		return 0f;
	}

	// Token: 0x06002DD9 RID: 11737 RVA: 0x00101880 File Offset: 0x000FFA80
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		float amount = pickupableStartWorkInfo.amount;
		Element element = pickupableStartWorkInfo.originalPickupable.PrimaryElement.Element;
		this.session = new LiquidPumpingStation.WorkSession(Grid.PosToCell(this), element.id, pickupableStartWorkInfo.originalPickupable.GetComponent<SubstanceChunk>(), amount, base.gameObject);
		this.meter.SetPositionPercent(0f);
		this.meter.SetSymbolTint(new KAnimHashedString("meter_target"), element.substance.colour);
	}

	// Token: 0x06002DDA RID: 11738 RVA: 0x00101914 File Offset: 0x000FFB14
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.session != null)
		{
			Storage component = worker.GetComponent<Storage>();
			float consumedAmount = this.session.GetConsumedAmount();
			if (consumedAmount > 0f)
			{
				SubstanceChunk source = this.session.GetSource();
				SimUtil.DiseaseInfo diseaseInfo = (this.session != null) ? this.session.GetDiseaseInfo() : SimUtil.DiseaseInfo.Invalid;
				PrimaryElement component2 = source.GetComponent<PrimaryElement>();
				Pickupable component3 = LiquidSourceManager.Instance.CreateChunk(component2.Element, consumedAmount, this.session.GetTemperature(), diseaseInfo.idx, diseaseInfo.count, base.transform.GetPosition()).GetComponent<Pickupable>();
				component3.TotalAmount = consumedAmount;
				component3.Trigger(1335436905, source.GetComponent<Pickupable>());
				worker.SetWorkCompleteData(component3);
				this.Sim200ms(0f);
				if (component3 != null)
				{
					component.Store(component3.gameObject, false, false, true, false);
				}
			}
			this.session.Cleanup();
			this.session = null;
		}
		base.GetComponent<KAnimControllerBase>().Play("on", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06002DDB RID: 11739 RVA: 0x00101A38 File Offset: 0x000FFC38
	private void OnReservationsChanged(Pickupable _ignore, bool _ignore2, Pickupable.Reservation _ignore3)
	{
		bool forceUnfetchable = false;
		for (int i = 0; i < this.infoCount; i++)
		{
			if (this.infos[i].source != null && this.infos[i].source.GetComponent<Pickupable>().ReservedAmount > 0f)
			{
				forceUnfetchable = true;
				break;
			}
		}
		for (int j = 0; j < this.infoCount; j++)
		{
			if (this.infos[j].source != null)
			{
				FetchableMonitor.Instance smi = this.infos[j].source.GetSMI<FetchableMonitor.Instance>();
				if (smi != null)
				{
					smi.SetForceUnfetchable(forceUnfetchable);
				}
			}
		}
	}

	// Token: 0x06002DDC RID: 11740 RVA: 0x00101AE2 File Offset: 0x000FFCE2
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.session != null)
		{
			this.meter.SetPositionPercent(this.session.GetPercentComplete());
			if (this.session.GetLastTickAmount() <= 0f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002DDD RID: 11741 RVA: 0x00101B18 File Offset: 0x000FFD18
	protected override void OnCleanUp()
	{
		this.UnregisterListenersToCellChanges();
		base.OnCleanUp();
		if (this.session != null)
		{
			this.session.Cleanup();
			this.session = null;
		}
		for (int i = 0; i < this.infoCount; i++)
		{
			if (this.infos[i].source != null)
			{
				this.infos[i].source.DeleteObject();
			}
		}
	}

	// Token: 0x04001AB0 RID: 6832
	private static readonly CellOffset[] liquidOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(0, -1),
		new CellOffset(1, -1),
		new CellOffset(0, -2),
		new CellOffset(1, -2),
		new CellOffset(0, -3),
		new CellOffset(1, -3),
		new CellOffset(0, -4),
		new CellOffset(1, -4)
	};

	// Token: 0x04001AB1 RID: 6833
	private LiquidPumpingStation.LiquidInfo[] infos;

	// Token: 0x04001AB2 RID: 6834
	private int infoCount;

	// Token: 0x04001AB3 RID: 6835
	private int depthAvailable = -1;

	// Token: 0x04001AB4 RID: 6836
	private HandleVector<int>.Handle partitionerEntry_buildings;

	// Token: 0x04001AB5 RID: 6837
	private HandleVector<int>.Handle partitionerEntry_solids;

	// Token: 0x04001AB6 RID: 6838
	private LiquidPumpingStation.WorkSession session;

	// Token: 0x04001AB7 RID: 6839
	private MeterController meter;

	// Token: 0x0200153B RID: 5435
	private class WorkSession
	{
		// Token: 0x06008DA1 RID: 36257 RVA: 0x003406A8 File Offset: 0x0033E8A8
		public WorkSession(int cell, SimHashes element, SubstanceChunk source, float amount_to_pickup, GameObject pump)
		{
			this.cell = cell;
			this.element = element;
			this.source = source;
			this.amountToPickup = amount_to_pickup;
			this.temperature = ElementLoader.FindElementByHash(element).defaultValues.temperature;
			this.diseaseInfo = SimUtil.DiseaseInfo.Invalid;
			this.amountPerTick = 40f;
			this.pump = pump;
			this.lastTickAmount = this.amountPerTick;
			this.ConsumeMass();
		}

		// Token: 0x06008DA2 RID: 36258 RVA: 0x0034071E File Offset: 0x0033E91E
		private void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
		{
			((LiquidPumpingStation.WorkSession)data).OnSimConsume(mass_cb_info);
		}

		// Token: 0x06008DA3 RID: 36259 RVA: 0x0034072C File Offset: 0x0033E92C
		private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
		{
			if (this.consumedAmount == 0f)
			{
				this.temperature = mass_cb_info.temperature;
			}
			else
			{
				this.temperature = GameUtil.GetFinalTemperature(this.temperature, this.consumedAmount, mass_cb_info.temperature, mass_cb_info.mass);
			}
			this.consumedAmount += mass_cb_info.mass;
			this.lastTickAmount = mass_cb_info.mass;
			this.diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(this.diseaseInfo.idx, this.diseaseInfo.count, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
			if (this.consumedAmount >= this.amountToPickup)
			{
				this.amountPerTick = 0f;
				this.lastTickAmount = 0f;
			}
			this.ConsumeMass();
		}

		// Token: 0x06008DA4 RID: 36260 RVA: 0x003407F0 File Offset: 0x0033E9F0
		private void ConsumeMass()
		{
			if (this.amountPerTick > 0f)
			{
				float num = Mathf.Min(this.amountPerTick, this.amountToPickup - this.consumedAmount);
				num = Mathf.Max(num, 1f);
				HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(this.OnSimConsumeCallback), this, "LiquidPumpingStation");
				int depthAvailable = PumpingStationGuide.GetDepthAvailable(this.cell, this.pump);
				SimMessages.ConsumeMass(Grid.OffsetCell(this.cell, new CellOffset(0, -depthAvailable)), this.element, num, (byte)(depthAvailable + 1), handle.index);
			}
		}

		// Token: 0x06008DA5 RID: 36261 RVA: 0x00340890 File Offset: 0x0033EA90
		public float GetPercentComplete()
		{
			return this.consumedAmount / this.amountToPickup;
		}

		// Token: 0x06008DA6 RID: 36262 RVA: 0x0034089F File Offset: 0x0033EA9F
		public float GetLastTickAmount()
		{
			return this.lastTickAmount;
		}

		// Token: 0x06008DA7 RID: 36263 RVA: 0x003408A7 File Offset: 0x0033EAA7
		public SimUtil.DiseaseInfo GetDiseaseInfo()
		{
			return this.diseaseInfo;
		}

		// Token: 0x06008DA8 RID: 36264 RVA: 0x003408AF File Offset: 0x0033EAAF
		public SubstanceChunk GetSource()
		{
			return this.source;
		}

		// Token: 0x06008DA9 RID: 36265 RVA: 0x003408B7 File Offset: 0x0033EAB7
		public float GetConsumedAmount()
		{
			return this.consumedAmount;
		}

		// Token: 0x06008DAA RID: 36266 RVA: 0x003408BF File Offset: 0x0033EABF
		public float GetTemperature()
		{
			if (this.temperature <= 0f)
			{
				global::Debug.LogWarning("TODO(YOG): Fix bad temperature in liquid pumping station.");
				return ElementLoader.FindElementByHash(this.element).defaultValues.temperature;
			}
			return this.temperature;
		}

		// Token: 0x06008DAB RID: 36267 RVA: 0x003408F4 File Offset: 0x0033EAF4
		public void Cleanup()
		{
			this.amountPerTick = 0f;
			this.diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		}

		// Token: 0x04006C44 RID: 27716
		private int cell;

		// Token: 0x04006C45 RID: 27717
		private float amountToPickup;

		// Token: 0x04006C46 RID: 27718
		private float consumedAmount;

		// Token: 0x04006C47 RID: 27719
		private float temperature;

		// Token: 0x04006C48 RID: 27720
		private float amountPerTick;

		// Token: 0x04006C49 RID: 27721
		private SimHashes element;

		// Token: 0x04006C4A RID: 27722
		private float lastTickAmount;

		// Token: 0x04006C4B RID: 27723
		private SubstanceChunk source;

		// Token: 0x04006C4C RID: 27724
		private SimUtil.DiseaseInfo diseaseInfo;

		// Token: 0x04006C4D RID: 27725
		private GameObject pump;
	}

	// Token: 0x0200153C RID: 5436
	private struct LiquidInfo
	{
		// Token: 0x04006C4E RID: 27726
		public float amount;

		// Token: 0x04006C4F RID: 27727
		public Element element;

		// Token: 0x04006C50 RID: 27728
		public SubstanceChunk source;
	}
}
