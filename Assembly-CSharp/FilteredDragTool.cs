using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200090E RID: 2318
public class FilteredDragTool : DragTool
{
	// Token: 0x060042FD RID: 17149 RVA: 0x0017CDD5 File Offset: 0x0017AFD5
	public bool IsActiveLayer(string layer)
	{
		return this.currentFilterTargets[ToolParameterMenu.FILTERLAYERS.ALL] == ToolParameterMenu.ToggleState.On || (this.currentFilterTargets.ContainsKey(layer.ToUpper()) && this.currentFilterTargets[layer.ToUpper()] == ToolParameterMenu.ToggleState.On);
	}

	// Token: 0x060042FE RID: 17150 RVA: 0x0017CE14 File Offset: 0x0017B014
	public bool IsActiveLayer(ObjectLayer layer)
	{
		if (this.currentFilterTargets.ContainsKey(ToolParameterMenu.FILTERLAYERS.ALL) && this.currentFilterTargets[ToolParameterMenu.FILTERLAYERS.ALL] == ToolParameterMenu.ToggleState.On)
		{
			return true;
		}
		bool result = false;
		foreach (KeyValuePair<string, ToolParameterMenu.ToggleState> keyValuePair in this.currentFilterTargets)
		{
			if (keyValuePair.Value == ToolParameterMenu.ToggleState.On && this.GetObjectLayerFromFilterLayer(keyValuePair.Key) == layer)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x060042FF RID: 17151 RVA: 0x0017CEA8 File Offset: 0x0017B0A8
	protected virtual void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
		filters.Add(ToolParameterMenu.FILTERLAYERS.WIRES, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.BUILDINGS, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LOGIC, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.BACKWALL, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x06004300 RID: 17152 RVA: 0x0017CF15 File Offset: 0x0017B115
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ResetFilter(this.filterTargets);
	}

	// Token: 0x06004301 RID: 17153 RVA: 0x0017CF29 File Offset: 0x0017B129
	protected override void OnSpawn()
	{
		base.OnSpawn();
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
	}

	// Token: 0x06004302 RID: 17154 RVA: 0x0017CF57 File Offset: 0x0017B157
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		base.OnCleanUp();
	}

	// Token: 0x06004303 RID: 17155 RVA: 0x0017CF85 File Offset: 0x0017B185
	public void ResetFilter()
	{
		this.ResetFilter(this.filterTargets);
	}

	// Token: 0x06004304 RID: 17156 RVA: 0x0017CF93 File Offset: 0x0017B193
	protected void ResetFilter(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Clear();
		this.GetDefaultFilters(filters);
		this.currentFilterTargets = filters;
	}

	// Token: 0x06004305 RID: 17157 RVA: 0x0017CFA9 File Offset: 0x0017B1A9
	protected override void OnActivateTool()
	{
		this.active = true;
		base.OnActivateTool();
		this.OnOverlayChanged(OverlayScreen.Instance.mode);
	}

	// Token: 0x06004306 RID: 17158 RVA: 0x0017CFC8 File Offset: 0x0017B1C8
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		this.active = false;
		ToolMenu.Instance.toolParameterMenu.ClearMenu();
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x06004307 RID: 17159 RVA: 0x0017CFE8 File Offset: 0x0017B1E8
	public virtual string GetFilterLayerFromGameObject(GameObject input)
	{
		BuildingComplete component = input.GetComponent<BuildingComplete>();
		BuildingUnderConstruction component2 = input.GetComponent<BuildingUnderConstruction>();
		if (component)
		{
			return this.GetFilterLayerFromObjectLayer(component.Def.ObjectLayer);
		}
		if (component2)
		{
			return this.GetFilterLayerFromObjectLayer(component2.Def.ObjectLayer);
		}
		if (input.GetComponent<Clearable>() != null || input.GetComponent<Moppable>() != null)
		{
			return "CleanAndClear";
		}
		if (input.GetComponent<Diggable>() != null)
		{
			return "DigPlacer";
		}
		return "Default";
	}

	// Token: 0x06004308 RID: 17160 RVA: 0x0017D074 File Offset: 0x0017B274
	public string GetFilterLayerFromObjectLayer(ObjectLayer gamer_layer)
	{
		if (gamer_layer > ObjectLayer.FoundationTile)
		{
			switch (gamer_layer)
			{
			case ObjectLayer.GasConduit:
			case ObjectLayer.GasConduitConnection:
				return "GasPipes";
			case ObjectLayer.GasConduitTile:
			case ObjectLayer.ReplacementGasConduit:
			case ObjectLayer.LiquidConduitTile:
			case ObjectLayer.ReplacementLiquidConduit:
				goto IL_AC;
			case ObjectLayer.LiquidConduit:
			case ObjectLayer.LiquidConduitConnection:
				return "LiquidPipes";
			case ObjectLayer.SolidConduit:
				break;
			default:
				switch (gamer_layer)
				{
				case ObjectLayer.SolidConduitConnection:
					break;
				case ObjectLayer.LadderTile:
				case ObjectLayer.ReplacementLadder:
				case ObjectLayer.WireTile:
				case ObjectLayer.ReplacementWire:
					goto IL_AC;
				case ObjectLayer.Wire:
				case ObjectLayer.WireConnectors:
					return "Wires";
				case ObjectLayer.LogicGate:
				case ObjectLayer.LogicWire:
					return "Logic";
				default:
					if (gamer_layer == ObjectLayer.Gantry)
					{
						goto IL_7C;
					}
					goto IL_AC;
				}
				break;
			}
			return "SolidConduits";
		}
		if (gamer_layer != ObjectLayer.Building)
		{
			if (gamer_layer == ObjectLayer.Backwall)
			{
				return "BackWall";
			}
			if (gamer_layer != ObjectLayer.FoundationTile)
			{
				goto IL_AC;
			}
			return "Tiles";
		}
		IL_7C:
		return "Buildings";
		IL_AC:
		return "Default";
	}

	// Token: 0x06004309 RID: 17161 RVA: 0x0017D134 File Offset: 0x0017B334
	private ObjectLayer GetObjectLayerFromFilterLayer(string filter_layer)
	{
		string text = filter_layer.ToLower();
		uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
		if (num <= 2200975418U)
		{
			if (num <= 388608975U)
			{
				if (num != 25076977U)
				{
					if (num == 388608975U)
					{
						if (text == "solidconduits")
						{
							return ObjectLayer.SolidConduit;
						}
					}
				}
				else if (text == "wires")
				{
					return ObjectLayer.Wire;
				}
			}
			else if (num != 614364310U)
			{
				if (num == 2200975418U)
				{
					if (text == "backwall")
					{
						return ObjectLayer.Backwall;
					}
				}
			}
			else if (text == "liquidpipes")
			{
				return ObjectLayer.LiquidConduit;
			}
		}
		else if (num <= 2875565775U)
		{
			if (num != 2366751346U)
			{
				if (num == 2875565775U)
				{
					if (text == "gaspipes")
					{
						return ObjectLayer.GasConduit;
					}
				}
			}
			else if (text == "buildings")
			{
				return ObjectLayer.Building;
			}
		}
		else if (num != 3464443665U)
		{
			if (num == 4178729166U)
			{
				if (text == "tiles")
				{
					return ObjectLayer.FoundationTile;
				}
			}
		}
		else if (text == "logic")
		{
			return ObjectLayer.LogicWire;
		}
		throw new ArgumentException("Invalid filter layer: " + filter_layer);
	}

	// Token: 0x0600430A RID: 17162 RVA: 0x0017D27C File Offset: 0x0017B47C
	private void OnOverlayChanged(HashedString overlay)
	{
		if (!this.active)
		{
			return;
		}
		string text = null;
		if (overlay == OverlayModes.Power.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.WIRES;
		}
		else if (overlay == OverlayModes.LiquidConduits.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT;
		}
		else if (overlay == OverlayModes.GasConduits.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.GASCONDUIT;
		}
		else if (overlay == OverlayModes.SolidConveyor.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT;
		}
		else if (overlay == OverlayModes.Logic.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.LOGIC;
		}
		this.currentFilterTargets = this.filterTargets;
		if (text != null)
		{
			using (List<string>.Enumerator enumerator = new List<string>(this.filterTargets.Keys).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text2 = enumerator.Current;
					this.filterTargets[text2] = ToolParameterMenu.ToggleState.Disabled;
					if (text2 == text)
					{
						this.filterTargets[text2] = ToolParameterMenu.ToggleState.On;
					}
				}
				goto IL_102;
			}
		}
		if (this.overlayFilterTargets.Count == 0)
		{
			this.ResetFilter(this.overlayFilterTargets);
		}
		this.currentFilterTargets = this.overlayFilterTargets;
		IL_102:
		ToolMenu.Instance.toolParameterMenu.PopulateMenu(this.currentFilterTargets);
	}

	// Token: 0x04002C23 RID: 11299
	private Dictionary<string, ToolParameterMenu.ToggleState> filterTargets = new Dictionary<string, ToolParameterMenu.ToggleState>();

	// Token: 0x04002C24 RID: 11300
	private Dictionary<string, ToolParameterMenu.ToggleState> overlayFilterTargets = new Dictionary<string, ToolParameterMenu.ToggleState>();

	// Token: 0x04002C25 RID: 11301
	private Dictionary<string, ToolParameterMenu.ToggleState> currentFilterTargets;

	// Token: 0x04002C26 RID: 11302
	private bool active;
}
