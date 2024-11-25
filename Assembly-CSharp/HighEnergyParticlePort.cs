using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020008ED RID: 2285
public class HighEnergyParticlePort : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x060041A5 RID: 16805 RVA: 0x00174B47 File Offset: 0x00172D47
	public int GetHighEnergyParticleInputPortPosition()
	{
		return this.m_building.GetHighEnergyParticleInputCell();
	}

	// Token: 0x060041A6 RID: 16806 RVA: 0x00174B54 File Offset: 0x00172D54
	public int GetHighEnergyParticleOutputPortPosition()
	{
		return this.m_building.GetHighEnergyParticleOutputCell();
	}

	// Token: 0x060041A7 RID: 16807 RVA: 0x00174B61 File Offset: 0x00172D61
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060041A8 RID: 16808 RVA: 0x00174B69 File Offset: 0x00172D69
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.HighEnergyParticlePorts.Add(this);
	}

	// Token: 0x060041A9 RID: 16809 RVA: 0x00174B7C File Offset: 0x00172D7C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.HighEnergyParticlePorts.Remove(this);
	}

	// Token: 0x060041AA RID: 16810 RVA: 0x00174B90 File Offset: 0x00172D90
	public bool InputActive()
	{
		Operational component = base.GetComponent<Operational>();
		return this.particleInputEnabled && component != null && component.IsFunctional && (!this.requireOperational || component.IsOperational);
	}

	// Token: 0x060041AB RID: 16811 RVA: 0x00174BCF File Offset: 0x00172DCF
	public bool AllowCapture(HighEnergyParticle particle)
	{
		return this.onParticleCaptureAllowed == null || this.onParticleCaptureAllowed(particle);
	}

	// Token: 0x060041AC RID: 16812 RVA: 0x00174BE7 File Offset: 0x00172DE7
	public void Capture(HighEnergyParticle particle)
	{
		this.currentParticle = particle;
		if (this.onParticleCapture != null)
		{
			this.onParticleCapture(particle);
		}
	}

	// Token: 0x060041AD RID: 16813 RVA: 0x00174C04 File Offset: 0x00172E04
	public void Uncapture(HighEnergyParticle particle)
	{
		if (this.onParticleUncapture != null)
		{
			this.onParticleUncapture(particle);
		}
		this.currentParticle = null;
	}

	// Token: 0x060041AE RID: 16814 RVA: 0x00174C24 File Offset: 0x00172E24
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.particleInputEnabled)
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.PARTICLE_PORT_INPUT, UI.BUILDINGEFFECTS.TOOLTIPS.PARTICLE_PORT_INPUT, Descriptor.DescriptorType.Requirement, false));
		}
		if (this.particleOutputEnabled)
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.PARTICLE_PORT_OUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.PARTICLE_PORT_OUTPUT, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x04002B82 RID: 11138
	[MyCmpGet]
	private Building m_building;

	// Token: 0x04002B83 RID: 11139
	public HighEnergyParticlePort.OnParticleCapture onParticleCapture;

	// Token: 0x04002B84 RID: 11140
	public HighEnergyParticlePort.OnParticleCaptureAllowed onParticleCaptureAllowed;

	// Token: 0x04002B85 RID: 11141
	public HighEnergyParticlePort.OnParticleCapture onParticleUncapture;

	// Token: 0x04002B86 RID: 11142
	public HighEnergyParticle currentParticle;

	// Token: 0x04002B87 RID: 11143
	public bool requireOperational = true;

	// Token: 0x04002B88 RID: 11144
	public bool particleInputEnabled;

	// Token: 0x04002B89 RID: 11145
	public bool particleOutputEnabled;

	// Token: 0x04002B8A RID: 11146
	public CellOffset particleInputOffset;

	// Token: 0x04002B8B RID: 11147
	public CellOffset particleOutputOffset;

	// Token: 0x02001859 RID: 6233
	// (Invoke) Token: 0x06009800 RID: 38912
	public delegate void OnParticleCapture(HighEnergyParticle particle);

	// Token: 0x0200185A RID: 6234
	// (Invoke) Token: 0x06009804 RID: 38916
	public delegate bool OnParticleCaptureAllowed(HighEnergyParticle particle);
}
