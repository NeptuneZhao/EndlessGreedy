using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000561 RID: 1377
public class EntityCellVisualizer : KMonoBehaviour
{
	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06001FDB RID: 8155 RVA: 0x000B31B1 File Offset: 0x000B13B1
	public BuildingCellVisualizerResources Resources
	{
		get
		{
			return BuildingCellVisualizerResources.Instance();
		}
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06001FDC RID: 8156 RVA: 0x000B31B8 File Offset: 0x000B13B8
	protected int CenterCell
	{
		get
		{
			return Grid.PosToCell(this);
		}
	}

	// Token: 0x06001FDD RID: 8157 RVA: 0x000B31C0 File Offset: 0x000B13C0
	protected virtual void DefinePorts()
	{
	}

	// Token: 0x06001FDE RID: 8158 RVA: 0x000B31C2 File Offset: 0x000B13C2
	protected override void OnPrefabInit()
	{
		this.LoadDiseaseIcon();
		this.DefinePorts();
	}

	// Token: 0x06001FDF RID: 8159 RVA: 0x000B31D0 File Offset: 0x000B13D0
	public void ConnectedEventWithDelay(float delay, int connectionCount, int cell, string soundName)
	{
		base.StartCoroutine(this.ConnectedDelay(delay, connectionCount, cell, soundName));
	}

	// Token: 0x06001FE0 RID: 8160 RVA: 0x000B31E4 File Offset: 0x000B13E4
	private IEnumerator ConnectedDelay(float delay, int connectionCount, int cell, string soundName)
	{
		float startTime = Time.realtimeSinceStartup;
		float currentTime = startTime;
		while (currentTime < startTime + delay)
		{
			currentTime += Time.unscaledDeltaTime;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.ConnectedEvent(cell);
		string sound = GlobalAssets.GetSound(soundName, false);
		if (sound != null)
		{
			Vector3 position = base.transform.GetPosition();
			position.z = 0f;
			EventInstance instance = SoundEvent.BeginOneShot(sound, position, 1f, false);
			instance.setParameterByName("connectedCount", (float)connectionCount, false);
			SoundEvent.EndOneShot(instance);
		}
		yield break;
	}

	// Token: 0x06001FE1 RID: 8161 RVA: 0x000B3210 File Offset: 0x000B1410
	private int ComputeCell(CellOffset cellOffset)
	{
		CellOffset offset = cellOffset;
		if (this.rotatable != null)
		{
			offset = this.rotatable.GetRotatedCellOffset(cellOffset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.gameObject), offset);
	}

	// Token: 0x06001FE2 RID: 8162 RVA: 0x000B324C File Offset: 0x000B144C
	public void ConnectedEvent(int cell)
	{
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if (this.ComputeCell(portEntry.cellOffset) == cell && portEntry.visualizer != null)
			{
				SizePulse pulse = portEntry.visualizer.AddComponent<SizePulse>();
				pulse.speed = 20f;
				pulse.multiplier = 0.75f;
				pulse.updateWhenPaused = true;
				SizePulse pulse2 = pulse;
				pulse2.onComplete = (System.Action)Delegate.Combine(pulse2.onComplete, new System.Action(delegate()
				{
					UnityEngine.Object.Destroy(pulse);
				}));
			}
		}
	}

	// Token: 0x06001FE3 RID: 8163 RVA: 0x000B3328 File Offset: 0x000B1528
	public virtual void AddPort(EntityCellVisualizer.Ports type, CellOffset cell)
	{
		this.AddPort(type, cell, Color.white);
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x000B3337 File Offset: 0x000B1537
	public virtual void AddPort(EntityCellVisualizer.Ports type, CellOffset cell, Color tint)
	{
		this.AddPort(type, cell, tint, tint, 1.5f, false);
	}

	// Token: 0x06001FE5 RID: 8165 RVA: 0x000B3349 File Offset: 0x000B1549
	public virtual void AddPort(EntityCellVisualizer.Ports type, CellOffset cell, Color connectedTint, Color disconnectedTint, float scale = 1.5f, bool hideBG = false)
	{
		this.ports.Add(new EntityCellVisualizer.PortEntry(type, cell, connectedTint, disconnectedTint, scale, hideBG));
		this.addedPorts |= type;
	}

	// Token: 0x06001FE6 RID: 8166 RVA: 0x000B3374 File Offset: 0x000B1574
	protected override void OnCleanUp()
	{
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if (portEntry.visualizer != null)
			{
				UnityEngine.Object.Destroy(portEntry.visualizer);
			}
		}
		GameObject[] array = new GameObject[]
		{
			this.switchVisualizer,
			this.wireVisualizerAlpha,
			this.wireVisualizerBeta
		};
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i]);
		}
		base.OnCleanUp();
	}

	// Token: 0x06001FE7 RID: 8167 RVA: 0x000B3418 File Offset: 0x000B1618
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (this.icons == null)
		{
			this.icons = new Dictionary<GameObject, Image>();
		}
		Components.EntityCellVisualizers.Add(this);
	}

	// Token: 0x06001FE8 RID: 8168 RVA: 0x000B343E File Offset: 0x000B163E
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		Components.EntityCellVisualizers.Remove(this);
	}

	// Token: 0x06001FE9 RID: 8169 RVA: 0x000B3454 File Offset: 0x000B1654
	public void DrawIcons(HashedString mode)
	{
		EntityCellVisualizer.Ports ports = (EntityCellVisualizer.Ports)0;
		if (base.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
		{
			ports = (EntityCellVisualizer.Ports)0;
		}
		else if (mode == OverlayModes.Power.ID)
		{
			ports = (EntityCellVisualizer.Ports.PowerIn | EntityCellVisualizer.Ports.PowerOut);
		}
		else if (mode == OverlayModes.GasConduits.ID)
		{
			ports = (EntityCellVisualizer.Ports.GasIn | EntityCellVisualizer.Ports.GasOut);
		}
		else if (mode == OverlayModes.LiquidConduits.ID)
		{
			ports = (EntityCellVisualizer.Ports.LiquidIn | EntityCellVisualizer.Ports.LiquidOut);
		}
		else if (mode == OverlayModes.SolidConveyor.ID)
		{
			ports = (EntityCellVisualizer.Ports.SolidIn | EntityCellVisualizer.Ports.SolidOut);
		}
		else if (mode == OverlayModes.Radiation.ID)
		{
			ports = (EntityCellVisualizer.Ports.HighEnergyParticleIn | EntityCellVisualizer.Ports.HighEnergyParticleOut);
		}
		else if (mode == OverlayModes.Disease.ID)
		{
			ports = (EntityCellVisualizer.Ports.DiseaseIn | EntityCellVisualizer.Ports.DiseaseOut);
		}
		else if (mode == OverlayModes.Temperature.ID || mode == OverlayModes.HeatFlow.ID)
		{
			ports = (EntityCellVisualizer.Ports.HeatSource | EntityCellVisualizer.Ports.HeatSink);
		}
		bool flag = false;
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if ((portEntry.type & ports) == portEntry.type)
			{
				this.DrawUtilityIcon(portEntry);
				flag = true;
			}
			else if (portEntry.visualizer != null && portEntry.visualizer.activeInHierarchy)
			{
				portEntry.visualizer.SetActive(false);
			}
		}
		if (mode == OverlayModes.Power.ID)
		{
			if (!flag)
			{
				Switch component = base.GetComponent<Switch>();
				if (component != null)
				{
					int cell = Grid.PosToCell(base.transform.GetPosition());
					Color32 c = component.IsHandlerOn() ? this.Resources.switchColor : this.Resources.switchOffColor;
					this.DrawUtilityIcon(cell, this.Resources.switchIcon, ref this.switchVisualizer, c, 1f, false);
					return;
				}
				WireUtilityNetworkLink component2 = base.GetComponent<WireUtilityNetworkLink>();
				if (component2 != null)
				{
					int cell2;
					int cell3;
					component2.GetCells(out cell2, out cell3);
					this.DrawUtilityIcon(cell2, (Game.Instance.circuitManager.GetCircuitID(cell2) == ushort.MaxValue) ? this.Resources.electricityBridgeIcon : this.Resources.electricityConnectedIcon, ref this.wireVisualizerAlpha, this.Resources.electricityInputColor, 1f, false);
					this.DrawUtilityIcon(cell3, (Game.Instance.circuitManager.GetCircuitID(cell3) == ushort.MaxValue) ? this.Resources.electricityBridgeIcon : this.Resources.electricityConnectedIcon, ref this.wireVisualizerBeta, this.Resources.electricityInputColor, 1f, false);
					return;
				}
			}
		}
		else
		{
			foreach (GameObject gameObject in new GameObject[]
			{
				this.switchVisualizer,
				this.wireVisualizerAlpha,
				this.wireVisualizerBeta
			})
			{
				if (gameObject != null && gameObject.activeInHierarchy)
				{
					gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06001FEA RID: 8170 RVA: 0x000B3738 File Offset: 0x000B1938
	private Sprite GetSpriteForPortType(EntityCellVisualizer.Ports type, bool connected)
	{
		if (type <= EntityCellVisualizer.Ports.SolidOut)
		{
			if (type <= EntityCellVisualizer.Ports.LiquidIn)
			{
				switch (type)
				{
				case EntityCellVisualizer.Ports.PowerIn:
					if (!connected)
					{
						return this.Resources.electricityInputIcon;
					}
					return this.Resources.electricityBridgeConnectedIcon;
				case EntityCellVisualizer.Ports.PowerOut:
					if (!connected)
					{
						return this.Resources.electricityOutputIcon;
					}
					return this.Resources.electricityBridgeConnectedIcon;
				case EntityCellVisualizer.Ports.PowerIn | EntityCellVisualizer.Ports.PowerOut:
					break;
				case EntityCellVisualizer.Ports.GasIn:
					return this.Resources.gasInputIcon;
				default:
					if (type == EntityCellVisualizer.Ports.GasOut)
					{
						return this.Resources.gasOutputIcon;
					}
					if (type == EntityCellVisualizer.Ports.LiquidIn)
					{
						return this.Resources.liquidInputIcon;
					}
					break;
				}
			}
			else
			{
				if (type == EntityCellVisualizer.Ports.LiquidOut)
				{
					return this.Resources.liquidOutputIcon;
				}
				if (type == EntityCellVisualizer.Ports.SolidIn)
				{
					return this.Resources.liquidInputIcon;
				}
				if (type == EntityCellVisualizer.Ports.SolidOut)
				{
					return this.Resources.liquidOutputIcon;
				}
			}
		}
		else if (type <= EntityCellVisualizer.Ports.DiseaseIn)
		{
			if (type == EntityCellVisualizer.Ports.HighEnergyParticleIn)
			{
				return this.Resources.highEnergyParticleInputIcon;
			}
			if (type == EntityCellVisualizer.Ports.HighEnergyParticleOut)
			{
				return this.GetIconForHighEnergyOutput();
			}
			if (type == EntityCellVisualizer.Ports.DiseaseIn)
			{
				return this.diseaseSourceSprite;
			}
		}
		else
		{
			if (type == EntityCellVisualizer.Ports.DiseaseOut)
			{
				return this.diseaseSourceSprite;
			}
			if (type == EntityCellVisualizer.Ports.HeatSource)
			{
				return this.Resources.heatSourceIcon;
			}
			if (type == EntityCellVisualizer.Ports.HeatSink)
			{
				return this.Resources.heatSinkIcon;
			}
		}
		return null;
	}

	// Token: 0x06001FEB RID: 8171 RVA: 0x000B38AC File Offset: 0x000B1AAC
	protected virtual void DrawUtilityIcon(EntityCellVisualizer.PortEntry port)
	{
		int cell = this.ComputeCell(port.cellOffset);
		bool flag = true;
		bool connected = true;
		EntityCellVisualizer.Ports type = port.type;
		if (type <= EntityCellVisualizer.Ports.GasOut)
		{
			if (type - EntityCellVisualizer.Ports.PowerIn > 1)
			{
				if (type == EntityCellVisualizer.Ports.GasIn || type == EntityCellVisualizer.Ports.GasOut)
				{
					flag = (null != Grid.Objects[cell, 12]);
				}
			}
			else
			{
				bool flag2 = base.GetComponent<Building>() as BuildingPreview != null;
				BuildingEnabledButton component = base.GetComponent<BuildingEnabledButton>();
				connected = (!flag2 && Game.Instance.circuitManager.GetCircuitID(cell) != ushort.MaxValue);
				flag = (flag2 || (component != null && component.IsEnabled));
			}
		}
		else if (type <= EntityCellVisualizer.Ports.LiquidOut)
		{
			if (type == EntityCellVisualizer.Ports.LiquidIn || type == EntityCellVisualizer.Ports.LiquidOut)
			{
				flag = (null != Grid.Objects[cell, 16]);
			}
		}
		else if (type == EntityCellVisualizer.Ports.SolidIn || type == EntityCellVisualizer.Ports.SolidOut)
		{
			flag = (null != Grid.Objects[cell, 20]);
		}
		this.DrawUtilityIcon(cell, this.GetSpriteForPortType(port.type, connected), ref port.visualizer, flag ? port.connectedTint : port.disconnectedTint, port.scale, port.hideBG);
	}

	// Token: 0x06001FEC RID: 8172 RVA: 0x000B39E8 File Offset: 0x000B1BE8
	protected virtual void LoadDiseaseIcon()
	{
		DiseaseVisualization.Info info = Assets.instance.DiseaseVisualization.GetInfo(this.DiseaseCellVisName);
		if (info.name != null)
		{
			this.diseaseSourceSprite = Assets.instance.DiseaseVisualization.overlaySprite;
			this.diseaseSourceColour = GlobalAssets.Instance.colorSet.GetColorByName(info.overlayColourName);
		}
	}

	// Token: 0x06001FED RID: 8173 RVA: 0x000B3A48 File Offset: 0x000B1C48
	protected virtual Sprite GetIconForHighEnergyOutput()
	{
		IHighEnergyParticleDirection component = base.GetComponent<IHighEnergyParticleDirection>();
		Sprite result = this.Resources.highEnergyParticleOutputIcons[0];
		if (component != null)
		{
			int directionIndex = EightDirectionUtil.GetDirectionIndex(component.Direction);
			result = this.Resources.highEnergyParticleOutputIcons[directionIndex];
		}
		return result;
	}

	// Token: 0x06001FEE RID: 8174 RVA: 0x000B3A88 File Offset: 0x000B1C88
	private void DrawUtilityIcon(int cell, Sprite icon_img, ref GameObject visualizerObj, Color tint, float scaleMultiplier = 1.5f, bool hideBG = false)
	{
		Vector3 position = Grid.CellToPosCCC(cell, Grid.SceneLayer.Building);
		if (visualizerObj == null)
		{
			visualizerObj = global::Util.KInstantiate(Assets.UIPrefabs.ResourceVisualizer, GameScreenManager.Instance.worldSpaceCanvas, null);
			visualizerObj.transform.SetAsFirstSibling();
			this.icons.Add(visualizerObj, visualizerObj.transform.GetChild(0).GetComponent<Image>());
		}
		if (!visualizerObj.gameObject.activeInHierarchy)
		{
			visualizerObj.gameObject.SetActive(true);
		}
		visualizerObj.GetComponent<Image>().enabled = !hideBG;
		this.icons[visualizerObj].raycastTarget = this.enableRaycast;
		this.icons[visualizerObj].sprite = icon_img;
		visualizerObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = tint;
		visualizerObj.transform.SetPosition(position);
		if (visualizerObj.GetComponent<SizePulse>() == null)
		{
			visualizerObj.transform.localScale = Vector3.one * scaleMultiplier;
		}
	}

	// Token: 0x06001FEF RID: 8175 RVA: 0x000B3B9C File Offset: 0x000B1D9C
	public Image GetPowerOutputIcon()
	{
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if (portEntry.type == EntityCellVisualizer.Ports.PowerOut)
			{
				return (portEntry.visualizer != null) ? portEntry.visualizer.transform.GetChild(0).GetComponent<Image>() : null;
			}
		}
		return null;
	}

	// Token: 0x06001FF0 RID: 8176 RVA: 0x000B3C20 File Offset: 0x000B1E20
	public Image GetPowerInputIcon()
	{
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if (portEntry.type == EntityCellVisualizer.Ports.PowerIn)
			{
				return (portEntry.visualizer != null) ? portEntry.visualizer.transform.GetChild(0).GetComponent<Image>() : null;
			}
		}
		return null;
	}

	// Token: 0x040011F6 RID: 4598
	protected List<EntityCellVisualizer.PortEntry> ports = new List<EntityCellVisualizer.PortEntry>();

	// Token: 0x040011F7 RID: 4599
	public EntityCellVisualizer.Ports addedPorts;

	// Token: 0x040011F8 RID: 4600
	private GameObject switchVisualizer;

	// Token: 0x040011F9 RID: 4601
	private GameObject wireVisualizerAlpha;

	// Token: 0x040011FA RID: 4602
	private GameObject wireVisualizerBeta;

	// Token: 0x040011FB RID: 4603
	public const EntityCellVisualizer.Ports HEAT_PORTS = EntityCellVisualizer.Ports.HeatSource | EntityCellVisualizer.Ports.HeatSink;

	// Token: 0x040011FC RID: 4604
	public const EntityCellVisualizer.Ports POWER_PORTS = EntityCellVisualizer.Ports.PowerIn | EntityCellVisualizer.Ports.PowerOut;

	// Token: 0x040011FD RID: 4605
	public const EntityCellVisualizer.Ports GAS_PORTS = EntityCellVisualizer.Ports.GasIn | EntityCellVisualizer.Ports.GasOut;

	// Token: 0x040011FE RID: 4606
	public const EntityCellVisualizer.Ports LIQUID_PORTS = EntityCellVisualizer.Ports.LiquidIn | EntityCellVisualizer.Ports.LiquidOut;

	// Token: 0x040011FF RID: 4607
	public const EntityCellVisualizer.Ports SOLID_PORTS = EntityCellVisualizer.Ports.SolidIn | EntityCellVisualizer.Ports.SolidOut;

	// Token: 0x04001200 RID: 4608
	public const EntityCellVisualizer.Ports ENERGY_PARTICLES_PORTS = EntityCellVisualizer.Ports.HighEnergyParticleIn | EntityCellVisualizer.Ports.HighEnergyParticleOut;

	// Token: 0x04001201 RID: 4609
	public const EntityCellVisualizer.Ports DISEASE_PORTS = EntityCellVisualizer.Ports.DiseaseIn | EntityCellVisualizer.Ports.DiseaseOut;

	// Token: 0x04001202 RID: 4610
	public const EntityCellVisualizer.Ports MATTER_PORTS = EntityCellVisualizer.Ports.GasIn | EntityCellVisualizer.Ports.GasOut | EntityCellVisualizer.Ports.LiquidIn | EntityCellVisualizer.Ports.LiquidOut | EntityCellVisualizer.Ports.SolidIn | EntityCellVisualizer.Ports.SolidOut;

	// Token: 0x04001203 RID: 4611
	protected Sprite diseaseSourceSprite;

	// Token: 0x04001204 RID: 4612
	protected Color32 diseaseSourceColour;

	// Token: 0x04001205 RID: 4613
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001206 RID: 4614
	protected bool enableRaycast = true;

	// Token: 0x04001207 RID: 4615
	protected Dictionary<GameObject, Image> icons;

	// Token: 0x04001208 RID: 4616
	public string DiseaseCellVisName = DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE;

	// Token: 0x0200135D RID: 4957
	[Flags]
	public enum Ports
	{
		// Token: 0x0400664F RID: 26191
		PowerIn = 1,
		// Token: 0x04006650 RID: 26192
		PowerOut = 2,
		// Token: 0x04006651 RID: 26193
		GasIn = 4,
		// Token: 0x04006652 RID: 26194
		GasOut = 8,
		// Token: 0x04006653 RID: 26195
		LiquidIn = 16,
		// Token: 0x04006654 RID: 26196
		LiquidOut = 32,
		// Token: 0x04006655 RID: 26197
		SolidIn = 64,
		// Token: 0x04006656 RID: 26198
		SolidOut = 128,
		// Token: 0x04006657 RID: 26199
		HighEnergyParticleIn = 256,
		// Token: 0x04006658 RID: 26200
		HighEnergyParticleOut = 512,
		// Token: 0x04006659 RID: 26201
		DiseaseIn = 1024,
		// Token: 0x0400665A RID: 26202
		DiseaseOut = 2048,
		// Token: 0x0400665B RID: 26203
		HeatSource = 4096,
		// Token: 0x0400665C RID: 26204
		HeatSink = 8192
	}

	// Token: 0x0200135E RID: 4958
	protected class PortEntry
	{
		// Token: 0x060086CE RID: 34510 RVA: 0x00329BEB File Offset: 0x00327DEB
		public PortEntry(EntityCellVisualizer.Ports type, CellOffset cellOffset, Color connectedTint, Color disconnectedTint, float scale, bool hideBG)
		{
			this.type = type;
			this.cellOffset = cellOffset;
			this.visualizer = null;
			this.connectedTint = connectedTint;
			this.disconnectedTint = disconnectedTint;
			this.scale = scale;
			this.hideBG = hideBG;
		}

		// Token: 0x0400665D RID: 26205
		public EntityCellVisualizer.Ports type;

		// Token: 0x0400665E RID: 26206
		public CellOffset cellOffset;

		// Token: 0x0400665F RID: 26207
		public GameObject visualizer;

		// Token: 0x04006660 RID: 26208
		public Color connectedTint;

		// Token: 0x04006661 RID: 26209
		public Color disconnectedTint;

		// Token: 0x04006662 RID: 26210
		public float scale;

		// Token: 0x04006663 RID: 26211
		public bool hideBG;
	}
}
