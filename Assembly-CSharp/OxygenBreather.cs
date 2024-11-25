using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020009DF RID: 2527
[RequireComponent(typeof(Health))]
[AddComponentMenu("KMonoBehaviour/scripts/OxygenBreather")]
public class OxygenBreather : KMonoBehaviour, ISim200ms
{
	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x06004949 RID: 18761 RVA: 0x001A3DCC File Offset: 0x001A1FCC
	public float ConsumptionRate
	{
		get
		{
			if (this.airConsumptionRate != null)
			{
				return this.airConsumptionRate.GetTotalValue();
			}
			return 0f;
		}
	}

	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x0600494A RID: 18762 RVA: 0x001A3DE7 File Offset: 0x001A1FE7
	public float CO2EmitRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.co2Accumulator);
		}
	}

	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x0600494B RID: 18763 RVA: 0x001A3DFE File Offset: 0x001A1FFE
	public HandleVector<int>.Handle O2Accumulator
	{
		get
		{
			return this.o2Accumulator;
		}
	}

	// Token: 0x0600494C RID: 18764 RVA: 0x001A3E06 File Offset: 0x001A2006
	protected override void OnPrefabInit()
	{
		GameUtil.SubscribeToTags<OxygenBreather>(this, OxygenBreather.OnDeadTagAddedDelegate, true);
	}

	// Token: 0x0600494D RID: 18765 RVA: 0x001A3E14 File Offset: 0x001A2014
	public bool IsLowOxygenAtMouthCell()
	{
		return this.GetOxygenPressure(this.mouthCell) < this.lowOxygenThreshold;
	}

	// Token: 0x0600494E RID: 18766 RVA: 0x001A3E2C File Offset: 0x001A202C
	protected override void OnSpawn()
	{
		this.airConsumptionRate = Db.Get().Attributes.AirConsumptionRate.Lookup(this);
		this.o2Accumulator = Game.Instance.accumulators.Add("O2", this);
		this.co2Accumulator = Game.Instance.accumulators.Add("CO2", this);
		KSelectable component = base.GetComponent<KSelectable>();
		component.AddStatusItem(Db.Get().DuplicantStatusItems.BreathingO2, this);
		component.AddStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, this);
		this.temperature = Db.Get().Amounts.Temperature.Lookup(this);
		NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
	}

	// Token: 0x0600494F RID: 18767 RVA: 0x001A3EEA File Offset: 0x001A20EA
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.o2Accumulator);
		Game.Instance.accumulators.Remove(this.co2Accumulator);
		this.SetGasProvider(null);
		base.OnCleanUp();
	}

	// Token: 0x06004950 RID: 18768 RVA: 0x001A3F25 File Offset: 0x001A2125
	public void Consume(Sim.MassConsumedCallback mass_consumed)
	{
		if (this.onSimConsume != null)
		{
			this.onSimConsume(mass_consumed);
		}
	}

	// Token: 0x06004951 RID: 18769 RVA: 0x001A3F3C File Offset: 0x001A213C
	public void Sim200ms(float dt)
	{
		if (!base.gameObject.HasTag(GameTags.Dead))
		{
			float num = this.airConsumptionRate.GetTotalValue() * dt;
			bool flag = this.gasProvider.ConsumeGas(this, num);
			if (flag)
			{
				if (this.gasProvider.ShouldEmitCO2())
				{
					float num2 = num * this.O2toCO2conversion;
					Game.Instance.accumulators.Accumulate(this.co2Accumulator, num2);
					this.accumulatedCO2 += num2;
					if (this.accumulatedCO2 >= this.minCO2ToEmit)
					{
						this.accumulatedCO2 -= this.minCO2ToEmit;
						Vector3 position = base.transform.GetPosition();
						Vector3 vector = position;
						vector.x += (this.facing.GetFacing() ? (-this.mouthOffset.x) : this.mouthOffset.x);
						vector.y += this.mouthOffset.y;
						vector.z -= 0.5f;
						if (Mathf.FloorToInt(vector.x) != Mathf.FloorToInt(position.x))
						{
							vector.x = Mathf.Floor(position.x) + (this.facing.GetFacing() ? 0.01f : 0.99f);
						}
						CO2Manager.instance.SpawnBreath(vector, this.minCO2ToEmit, this.temperature.value, this.facing.GetFacing());
					}
				}
				else if (this.gasProvider.ShouldStoreCO2())
				{
					Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
					if (equippable != null)
					{
						float num3 = num * this.O2toCO2conversion;
						Game.Instance.accumulators.Accumulate(this.co2Accumulator, num3);
						this.accumulatedCO2 += num3;
						if (this.accumulatedCO2 >= this.minCO2ToEmit)
						{
							this.accumulatedCO2 -= this.minCO2ToEmit;
							equippable.GetComponent<Storage>().AddGasChunk(SimHashes.CarbonDioxide, this.minCO2ToEmit, this.temperature.value, byte.MaxValue, 0, false, true);
						}
					}
				}
			}
			if (flag != this.hasAir)
			{
				this.hasAirTimer.Start();
				if (this.hasAirTimer.TryStop(2f))
				{
					this.hasAir = flag;
					base.Trigger(-933153513, this.hasAir);
					return;
				}
			}
			else
			{
				this.hasAirTimer.Stop();
			}
		}
	}

	// Token: 0x06004952 RID: 18770 RVA: 0x001A41AB File Offset: 0x001A23AB
	private void OnDeath(object data)
	{
		base.enabled = false;
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().DuplicantStatusItems.BreathingO2, false);
		component.RemoveStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, false);
	}

	// Token: 0x06004953 RID: 18771 RVA: 0x001A41E8 File Offset: 0x001A23E8
	private int GetMouthCellAtCell(int cell, CellOffset[] offsets)
	{
		float num = 0f;
		int result = cell;
		foreach (CellOffset offset in offsets)
		{
			int num2 = Grid.OffsetCell(cell, offset);
			float oxygenPressure = this.GetOxygenPressure(num2);
			if (oxygenPressure > num && oxygenPressure > this.noOxygenThreshold)
			{
				num = oxygenPressure;
				result = num2;
			}
		}
		return result;
	}

	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x06004954 RID: 18772 RVA: 0x001A4240 File Offset: 0x001A2440
	public int mouthCell
	{
		get
		{
			int cell = Grid.PosToCell(this);
			return this.GetMouthCellAtCell(cell, this.breathableCells);
		}
	}

	// Token: 0x06004955 RID: 18773 RVA: 0x001A4261 File Offset: 0x001A2461
	public bool IsBreathableElementAtCell(int cell, CellOffset[] offsets = null)
	{
		return this.GetBreathableElementAtCell(cell, offsets) != SimHashes.Vacuum;
	}

	// Token: 0x06004956 RID: 18774 RVA: 0x001A4278 File Offset: 0x001A2478
	public SimHashes GetBreathableElementAtCell(int cell, CellOffset[] offsets = null)
	{
		if (offsets == null)
		{
			offsets = this.breathableCells;
		}
		int mouthCellAtCell = this.GetMouthCellAtCell(cell, offsets);
		if (!Grid.IsValidCell(mouthCellAtCell))
		{
			return SimHashes.Vacuum;
		}
		Element element = Grid.Element[mouthCellAtCell];
		if (!element.IsGas || !element.HasTag(GameTags.Breathable) || Grid.Mass[mouthCellAtCell] <= this.noOxygenThreshold)
		{
			return SimHashes.Vacuum;
		}
		return element.id;
	}

	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x06004957 RID: 18775 RVA: 0x001A42E8 File Offset: 0x001A24E8
	public bool IsUnderLiquid
	{
		get
		{
			return Grid.Element[this.mouthCell].IsLiquid;
		}
	}

	// Token: 0x17000526 RID: 1318
	// (get) Token: 0x06004958 RID: 18776 RVA: 0x001A42FB File Offset: 0x001A24FB
	public bool IsSuffocating
	{
		get
		{
			return !this.hasAir;
		}
	}

	// Token: 0x17000527 RID: 1319
	// (get) Token: 0x06004959 RID: 18777 RVA: 0x001A4306 File Offset: 0x001A2506
	public SimHashes GetBreathableElement
	{
		get
		{
			return this.GetBreathableElementAtCell(Grid.PosToCell(this), null);
		}
	}

	// Token: 0x17000528 RID: 1320
	// (get) Token: 0x0600495A RID: 18778 RVA: 0x001A4315 File Offset: 0x001A2515
	public bool IsBreathableElement
	{
		get
		{
			return this.IsBreathableElementAtCell(Grid.PosToCell(this), null);
		}
	}

	// Token: 0x0600495B RID: 18779 RVA: 0x001A4324 File Offset: 0x001A2524
	private float GetOxygenPressure(int cell)
	{
		if (Grid.IsValidCell(cell) && Grid.Element[cell].HasTag(GameTags.Breathable))
		{
			return Grid.Mass[cell];
		}
		return 0f;
	}

	// Token: 0x0600495C RID: 18780 RVA: 0x001A4352 File Offset: 0x001A2552
	public OxygenBreather.IGasProvider GetGasProvider()
	{
		return this.gasProvider;
	}

	// Token: 0x0600495D RID: 18781 RVA: 0x001A435A File Offset: 0x001A255A
	public void SetGasProvider(OxygenBreather.IGasProvider gas_provider)
	{
		if (this.gasProvider != null)
		{
			this.gasProvider.OnClearOxygenBreather(this);
		}
		this.gasProvider = gas_provider;
		if (this.gasProvider != null)
		{
			this.gasProvider.OnSetOxygenBreather(this);
		}
	}

	// Token: 0x04002FED RID: 12269
	public static CellOffset[] DEFAULT_BREATHABLE_OFFSETS = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1),
		new CellOffset(1, 1),
		new CellOffset(-1, 1),
		new CellOffset(1, 0),
		new CellOffset(-1, 0)
	};

	// Token: 0x04002FEE RID: 12270
	public float O2toCO2conversion = 0.5f;

	// Token: 0x04002FEF RID: 12271
	public float lowOxygenThreshold;

	// Token: 0x04002FF0 RID: 12272
	public float noOxygenThreshold;

	// Token: 0x04002FF1 RID: 12273
	public Vector2 mouthOffset;

	// Token: 0x04002FF2 RID: 12274
	[Serialize]
	public float accumulatedCO2;

	// Token: 0x04002FF3 RID: 12275
	[SerializeField]
	public float minCO2ToEmit = 0.3f;

	// Token: 0x04002FF4 RID: 12276
	private bool hasAir = true;

	// Token: 0x04002FF5 RID: 12277
	private Timer hasAirTimer = new Timer();

	// Token: 0x04002FF6 RID: 12278
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04002FF7 RID: 12279
	[MyCmpGet]
	private Facing facing;

	// Token: 0x04002FF8 RID: 12280
	private HandleVector<int>.Handle o2Accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04002FF9 RID: 12281
	private HandleVector<int>.Handle co2Accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04002FFA RID: 12282
	private AmountInstance temperature;

	// Token: 0x04002FFB RID: 12283
	private AttributeInstance airConsumptionRate;

	// Token: 0x04002FFC RID: 12284
	public CellOffset[] breathableCells;

	// Token: 0x04002FFD RID: 12285
	public Action<Sim.MassConsumedCallback> onSimConsume;

	// Token: 0x04002FFE RID: 12286
	private OxygenBreather.IGasProvider gasProvider;

	// Token: 0x04002FFF RID: 12287
	private static readonly EventSystem.IntraObjectHandler<OxygenBreather> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<OxygenBreather>(GameTags.Dead, delegate(OxygenBreather component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x020019DF RID: 6623
	public interface IGasProvider
	{
		// Token: 0x06009E44 RID: 40516
		void OnSetOxygenBreather(OxygenBreather oxygen_breather);

		// Token: 0x06009E45 RID: 40517
		void OnClearOxygenBreather(OxygenBreather oxygen_breather);

		// Token: 0x06009E46 RID: 40518
		bool ConsumeGas(OxygenBreather oxygen_breather, float amount);

		// Token: 0x06009E47 RID: 40519
		bool ShouldEmitCO2();

		// Token: 0x06009E48 RID: 40520
		bool ShouldStoreCO2();

		// Token: 0x06009E49 RID: 40521
		bool IsLowOxygen();
	}
}
