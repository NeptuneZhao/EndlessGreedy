using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DDB RID: 3547
[AddComponentMenu("KMonoBehaviour/scripts/ToolParameterMenu")]
public class ToolParameterMenu : KMonoBehaviour
{
	// Token: 0x14000031 RID: 49
	// (add) Token: 0x060070B3 RID: 28851 RVA: 0x002AAA64 File Offset: 0x002A8C64
	// (remove) Token: 0x060070B4 RID: 28852 RVA: 0x002AAA9C File Offset: 0x002A8C9C
	public event System.Action onParametersChanged;

	// Token: 0x060070B5 RID: 28853 RVA: 0x002AAAD1 File Offset: 0x002A8CD1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ClearMenu();
	}

	// Token: 0x060070B6 RID: 28854 RVA: 0x002AAAE0 File Offset: 0x002A8CE0
	public void PopulateMenu(Dictionary<string, ToolParameterMenu.ToggleState> parameters)
	{
		this.ClearMenu();
		this.currentParameters = parameters;
		foreach (KeyValuePair<string, ToolParameterMenu.ToggleState> keyValuePair in parameters)
		{
			GameObject gameObject = Util.KInstantiateUI(this.widgetPrefab, this.widgetContainer, true);
			gameObject.GetComponentInChildren<LocText>().text = Strings.Get("STRINGS.UI.TOOLS.FILTERLAYERS." + keyValuePair.Key + ".NAME");
			ToolTip componentInChildren = gameObject.GetComponentInChildren<ToolTip>();
			if (componentInChildren != null)
			{
				componentInChildren.SetSimpleTooltip(Strings.Get("STRINGS.UI.TOOLS.FILTERLAYERS." + keyValuePair.Key + ".TOOLTIP"));
			}
			this.widgets.Add(keyValuePair.Key, gameObject);
			MultiToggle toggle = gameObject.GetComponentInChildren<MultiToggle>();
			ToolParameterMenu.ToggleState value = keyValuePair.Value;
			if (value == ToolParameterMenu.ToggleState.Disabled)
			{
				toggle.ChangeState(2);
			}
			else if (value == ToolParameterMenu.ToggleState.On)
			{
				toggle.ChangeState(1);
				this.lastEnabledFilter = keyValuePair.Key;
			}
			else
			{
				toggle.ChangeState(0);
			}
			MultiToggle toggle2 = toggle;
			toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
			{
				foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.widgets)
				{
					if (keyValuePair2.Value == toggle.transform.parent.gameObject)
					{
						if (this.currentParameters[keyValuePair2.Key] == ToolParameterMenu.ToggleState.Disabled)
						{
							break;
						}
						this.ChangeToSetting(keyValuePair2.Key);
						this.OnChange();
						break;
					}
				}
			}));
		}
		this.content.SetActive(true);
	}

	// Token: 0x060070B7 RID: 28855 RVA: 0x002AAC64 File Offset: 0x002A8E64
	public void ClearMenu()
	{
		this.content.SetActive(false);
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.widgets)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.widgets.Clear();
	}

	// Token: 0x060070B8 RID: 28856 RVA: 0x002AACD4 File Offset: 0x002A8ED4
	private void ChangeToSetting(string key)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.widgets)
		{
			if (this.currentParameters[keyValuePair.Key] != ToolParameterMenu.ToggleState.Disabled)
			{
				this.currentParameters[keyValuePair.Key] = ToolParameterMenu.ToggleState.Off;
			}
		}
		this.currentParameters[key] = ToolParameterMenu.ToggleState.On;
	}

	// Token: 0x060070B9 RID: 28857 RVA: 0x002AAD58 File Offset: 0x002A8F58
	private void OnChange()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.widgets)
		{
			switch (this.currentParameters[keyValuePair.Key])
			{
			case ToolParameterMenu.ToggleState.On:
				keyValuePair.Value.GetComponentInChildren<MultiToggle>().ChangeState(1);
				this.lastEnabledFilter = keyValuePair.Key;
				break;
			case ToolParameterMenu.ToggleState.Off:
				keyValuePair.Value.GetComponentInChildren<MultiToggle>().ChangeState(0);
				break;
			case ToolParameterMenu.ToggleState.Disabled:
				keyValuePair.Value.GetComponentInChildren<MultiToggle>().ChangeState(2);
				break;
			}
		}
		if (this.onParametersChanged != null)
		{
			this.onParametersChanged();
		}
	}

	// Token: 0x060070BA RID: 28858 RVA: 0x002AAE28 File Offset: 0x002A9028
	public string GetLastEnabledFilter()
	{
		return this.lastEnabledFilter;
	}

	// Token: 0x04004D7D RID: 19837
	public GameObject content;

	// Token: 0x04004D7E RID: 19838
	public GameObject widgetContainer;

	// Token: 0x04004D7F RID: 19839
	public GameObject widgetPrefab;

	// Token: 0x04004D81 RID: 19841
	private Dictionary<string, GameObject> widgets = new Dictionary<string, GameObject>();

	// Token: 0x04004D82 RID: 19842
	private Dictionary<string, ToolParameterMenu.ToggleState> currentParameters;

	// Token: 0x04004D83 RID: 19843
	private string lastEnabledFilter;

	// Token: 0x02001EE7 RID: 7911
	public class FILTERLAYERS
	{
		// Token: 0x04008BD8 RID: 35800
		public static string BUILDINGS = "BUILDINGS";

		// Token: 0x04008BD9 RID: 35801
		public static string TILES = "TILES";

		// Token: 0x04008BDA RID: 35802
		public static string WIRES = "WIRES";

		// Token: 0x04008BDB RID: 35803
		public static string LIQUIDCONDUIT = "LIQUIDPIPES";

		// Token: 0x04008BDC RID: 35804
		public static string GASCONDUIT = "GASPIPES";

		// Token: 0x04008BDD RID: 35805
		public static string SOLIDCONDUIT = "SOLIDCONDUITS";

		// Token: 0x04008BDE RID: 35806
		public static string CLEANANDCLEAR = "CLEANANDCLEAR";

		// Token: 0x04008BDF RID: 35807
		public static string DIGPLACER = "DIGPLACER";

		// Token: 0x04008BE0 RID: 35808
		public static string LOGIC = "LOGIC";

		// Token: 0x04008BE1 RID: 35809
		public static string BACKWALL = "BACKWALL";

		// Token: 0x04008BE2 RID: 35810
		public static string CONSTRUCTION = "CONSTRUCTION";

		// Token: 0x04008BE3 RID: 35811
		public static string DIG = "DIG";

		// Token: 0x04008BE4 RID: 35812
		public static string CLEAN = "CLEAN";

		// Token: 0x04008BE5 RID: 35813
		public static string OPERATE = "OPERATE";

		// Token: 0x04008BE6 RID: 35814
		public static string METAL = "METAL";

		// Token: 0x04008BE7 RID: 35815
		public static string BUILDABLE = "BUILDABLE";

		// Token: 0x04008BE8 RID: 35816
		public static string FILTER = "FILTER";

		// Token: 0x04008BE9 RID: 35817
		public static string LIQUIFIABLE = "LIQUIFIABLE";

		// Token: 0x04008BEA RID: 35818
		public static string LIQUID = "LIQUID";

		// Token: 0x04008BEB RID: 35819
		public static string CONSUMABLEORE = "CONSUMABLEORE";

		// Token: 0x04008BEC RID: 35820
		public static string ORGANICS = "ORGANICS";

		// Token: 0x04008BED RID: 35821
		public static string FARMABLE = "FARMABLE";

		// Token: 0x04008BEE RID: 35822
		public static string GAS = "GAS";

		// Token: 0x04008BEF RID: 35823
		public static string MISC = "MISC";

		// Token: 0x04008BF0 RID: 35824
		public static string HEATFLOW = "HEATFLOW";

		// Token: 0x04008BF1 RID: 35825
		public static string ABSOLUTETEMPERATURE = "ABSOLUTETEMPERATURE";

		// Token: 0x04008BF2 RID: 35826
		public static string RELATIVETEMPERATURE = "RELATIVETEMPERATURE";

		// Token: 0x04008BF3 RID: 35827
		public static string ADAPTIVETEMPERATURE = "ADAPTIVETEMPERATURE";

		// Token: 0x04008BF4 RID: 35828
		public static string STATECHANGE = "STATECHANGE";

		// Token: 0x04008BF5 RID: 35829
		public static string ALL = "ALL";
	}

	// Token: 0x02001EE8 RID: 7912
	public enum ToggleState
	{
		// Token: 0x04008BF7 RID: 35831
		On,
		// Token: 0x04008BF8 RID: 35832
		Off,
		// Token: 0x04008BF9 RID: 35833
		Disabled
	}
}
