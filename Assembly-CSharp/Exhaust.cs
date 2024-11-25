using System;
using UnityEngine;

// Token: 0x020008A7 RID: 2215
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Exhaust")]
public class Exhaust : KMonoBehaviour, ISim200ms
{
	// Token: 0x06003DE9 RID: 15849 RVA: 0x0015625E File Offset: 0x0015445E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Exhaust>(-592767678, Exhaust.OnConduitStateChangedDelegate);
		base.Subscribe<Exhaust>(-111137758, Exhaust.OnConduitStateChangedDelegate);
		base.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.NoWire;
		this.simRenderLoadBalance = true;
	}

	// Token: 0x06003DEA RID: 15850 RVA: 0x0015629B File Offset: 0x0015449B
	protected override void OnSpawn()
	{
		this.OnConduitStateChanged(null);
	}

	// Token: 0x06003DEB RID: 15851 RVA: 0x001562A4 File Offset: 0x001544A4
	private void OnConduitStateChanged(object data)
	{
		this.operational.SetActive(this.operational.IsOperational && !this.vent.IsBlocked, false);
	}

	// Token: 0x06003DEC RID: 15852 RVA: 0x001562D0 File Offset: 0x001544D0
	private void CalculateDiseaseTransfer(PrimaryElement item1, PrimaryElement item2, float transfer_rate, out int disease_to_item1, out int disease_to_item2)
	{
		disease_to_item1 = (int)((float)item2.DiseaseCount * transfer_rate);
		disease_to_item2 = (int)((float)item1.DiseaseCount * transfer_rate);
	}

	// Token: 0x06003DED RID: 15853 RVA: 0x001562EC File Offset: 0x001544EC
	public void Sim200ms(float dt)
	{
		this.operational.SetFlag(Exhaust.canExhaust, !this.vent.IsBlocked);
		if (!this.operational.IsOperational)
		{
			if (this.isAnimating)
			{
				this.isAnimating = false;
				this.recentlyExhausted = false;
				base.Trigger(-793429877, null);
			}
			return;
		}
		this.UpdateEmission();
		this.elapsedSwitchTime -= dt;
		if (this.elapsedSwitchTime <= 0f)
		{
			this.elapsedSwitchTime = 1f;
			if (this.recentlyExhausted != this.isAnimating)
			{
				this.isAnimating = this.recentlyExhausted;
				base.Trigger(-793429877, null);
			}
			this.recentlyExhausted = false;
		}
	}

	// Token: 0x06003DEE RID: 15854 RVA: 0x001563A0 File Offset: 0x001545A0
	public bool IsAnimating()
	{
		return this.isAnimating;
	}

	// Token: 0x06003DEF RID: 15855 RVA: 0x001563A8 File Offset: 0x001545A8
	private void UpdateEmission()
	{
		if (this.consumer.ConsumptionRate == 0f)
		{
			return;
		}
		if (this.storage.items.Count == 0)
		{
			return;
		}
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (Grid.Solid[num])
		{
			return;
		}
		ConduitType typeOfConduit = this.consumer.TypeOfConduit;
		if (typeOfConduit != ConduitType.Gas)
		{
			if (typeOfConduit == ConduitType.Liquid)
			{
				this.EmitLiquid(num);
				return;
			}
		}
		else
		{
			this.EmitGas(num);
		}
	}

	// Token: 0x06003DF0 RID: 15856 RVA: 0x00156420 File Offset: 0x00154620
	private bool EmitCommon(int cell, PrimaryElement primary_element, Exhaust.EmitDelegate emit)
	{
		if (primary_element.Mass <= 0f)
		{
			return false;
		}
		int num;
		int num2;
		this.CalculateDiseaseTransfer(this.exhaustPE, primary_element, 0.05f, out num, out num2);
		primary_element.ModifyDiseaseCount(-num, "Exhaust transfer");
		primary_element.AddDisease(this.exhaustPE.DiseaseIdx, num2, "Exhaust transfer");
		this.exhaustPE.ModifyDiseaseCount(-num2, "Exhaust transfer");
		this.exhaustPE.AddDisease(primary_element.DiseaseIdx, num, "Exhaust transfer");
		emit(cell, primary_element);
		if (this.vent != null)
		{
			this.vent.UpdateVentedMass(primary_element.ElementID, primary_element.Mass);
		}
		primary_element.KeepZeroMassObject = true;
		primary_element.Mass = 0f;
		primary_element.ModifyDiseaseCount(int.MinValue, "Exhaust.SimUpdate");
		if (this.lastElementEmmited != primary_element.ElementID)
		{
			this.lastElementEmmited = primary_element.ElementID;
			if (primary_element.Element != null && primary_element.Element.substance != null)
			{
				base.Trigger(-793429877, primary_element.Element.substance.colour);
			}
		}
		this.recentlyExhausted = true;
		return true;
	}

	// Token: 0x06003DF1 RID: 15857 RVA: 0x00156548 File Offset: 0x00154748
	private void EmitLiquid(int cell)
	{
		int num = Grid.CellBelow(cell);
		Exhaust.EmitDelegate emit = (Grid.IsValidCell(num) && !Grid.Solid[num]) ? Exhaust.emit_particle : Exhaust.emit_element;
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (component.Element.IsLiquid && this.EmitCommon(cell, component, emit))
			{
				break;
			}
		}
	}

	// Token: 0x06003DF2 RID: 15858 RVA: 0x001565E4 File Offset: 0x001547E4
	private void EmitGas(int cell)
	{
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (component.Element.IsGas && this.EmitCommon(cell, component, Exhaust.emit_element))
			{
				break;
			}
		}
	}

	// Token: 0x040025FD RID: 9725
	[MyCmpGet]
	private Vent vent;

	// Token: 0x040025FE RID: 9726
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040025FF RID: 9727
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002600 RID: 9728
	[MyCmpGet]
	private ConduitConsumer consumer;

	// Token: 0x04002601 RID: 9729
	[MyCmpGet]
	private PrimaryElement exhaustPE;

	// Token: 0x04002602 RID: 9730
	private static readonly Operational.Flag canExhaust = new Operational.Flag("canExhaust", Operational.Flag.Type.Requirement);

	// Token: 0x04002603 RID: 9731
	private bool isAnimating;

	// Token: 0x04002604 RID: 9732
	private bool recentlyExhausted;

	// Token: 0x04002605 RID: 9733
	private const float MinSwitchTime = 1f;

	// Token: 0x04002606 RID: 9734
	private float elapsedSwitchTime;

	// Token: 0x04002607 RID: 9735
	private SimHashes lastElementEmmited;

	// Token: 0x04002608 RID: 9736
	private static readonly EventSystem.IntraObjectHandler<Exhaust> OnConduitStateChangedDelegate = new EventSystem.IntraObjectHandler<Exhaust>(delegate(Exhaust component, object data)
	{
		component.OnConduitStateChanged(data);
	});

	// Token: 0x04002609 RID: 9737
	private static Exhaust.EmitDelegate emit_element = delegate(int cell, PrimaryElement primary_element)
	{
		SimMessages.AddRemoveSubstance(cell, primary_element.ElementID, CellEventLogger.Instance.ExhaustSimUpdate, primary_element.Mass, primary_element.Temperature, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, -1);
	};

	// Token: 0x0400260A RID: 9738
	private static Exhaust.EmitDelegate emit_particle = delegate(int cell, PrimaryElement primary_element)
	{
		FallingWater.instance.AddParticle(cell, primary_element.Element.idx, primary_element.Mass, primary_element.Temperature, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, false, true, false);
	};

	// Token: 0x0200179A RID: 6042
	// (Invoke) Token: 0x06009633 RID: 38451
	private delegate void EmitDelegate(int cell, PrimaryElement primary_element);
}
