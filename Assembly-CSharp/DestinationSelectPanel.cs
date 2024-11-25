using System;
using System.Collections.Generic;
using System.Linq;
using Klei.CustomSettings;
using ProcGen;
using UnityEngine;

// Token: 0x02000C33 RID: 3123
[AddComponentMenu("KMonoBehaviour/scripts/DestinationSelectPanel")]
public class DestinationSelectPanel : KMonoBehaviour
{
	// Token: 0x17000726 RID: 1830
	// (get) Token: 0x06005FCA RID: 24522 RVA: 0x00239513 File Offset: 0x00237713
	// (set) Token: 0x06005FCB RID: 24523 RVA: 0x0023951A File Offset: 0x0023771A
	public static int ChosenClusterCategorySetting
	{
		get
		{
			return DestinationSelectPanel.chosenClusterCategorySetting;
		}
		set
		{
			DestinationSelectPanel.chosenClusterCategorySetting = value;
		}
	}

	// Token: 0x14000026 RID: 38
	// (add) Token: 0x06005FCC RID: 24524 RVA: 0x00239524 File Offset: 0x00237724
	// (remove) Token: 0x06005FCD RID: 24525 RVA: 0x0023955C File Offset: 0x0023775C
	public event Action<ColonyDestinationAsteroidBeltData> OnAsteroidClicked;

	// Token: 0x17000727 RID: 1831
	// (get) Token: 0x06005FCE RID: 24526 RVA: 0x00239594 File Offset: 0x00237794
	private float min
	{
		get
		{
			return this.asteroidContainer.rect.x + this.offset;
		}
	}

	// Token: 0x17000728 RID: 1832
	// (get) Token: 0x06005FCF RID: 24527 RVA: 0x002395BC File Offset: 0x002377BC
	private float max
	{
		get
		{
			return this.min + this.asteroidContainer.rect.width;
		}
	}

	// Token: 0x06005FD0 RID: 24528 RVA: 0x002395E4 File Offset: 0x002377E4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.dragTarget.onBeginDrag += this.BeginDrag;
		this.dragTarget.onDrag += this.Drag;
		this.dragTarget.onEndDrag += this.EndDrag;
		MultiToggle multiToggle = this.leftArrowButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.ClickLeft));
		MultiToggle multiToggle2 = this.rightArrowButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(this.ClickRight));
	}

	// Token: 0x06005FD1 RID: 24529 RVA: 0x0023968A File Offset: 0x0023788A
	private void BeginDrag()
	{
		this.dragStartPos = KInputManager.GetMousePos();
		this.dragLastPos = this.dragStartPos;
		this.isDragging = true;
		KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll_Start", false));
	}

	// Token: 0x06005FD2 RID: 24530 RVA: 0x002396C0 File Offset: 0x002378C0
	private void Drag()
	{
		Vector2 vector = KInputManager.GetMousePos();
		float num = vector.x - this.dragLastPos.x;
		this.dragLastPos = vector;
		this.offset += num;
		int num2 = this.selectedIndex;
		this.selectedIndex = Mathf.RoundToInt(-this.offset / this.asteroidXSeparation);
		this.selectedIndex = Mathf.Clamp(this.selectedIndex, 0, this.clusterStartWorlds.Count - 1);
		if (num2 != this.selectedIndex)
		{
			this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[this.selectedIndex]]);
			KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll", false));
		}
	}

	// Token: 0x06005FD3 RID: 24531 RVA: 0x0023977D File Offset: 0x0023797D
	private void EndDrag()
	{
		this.Drag();
		this.isDragging = false;
		KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll_Stop", false));
	}

	// Token: 0x06005FD4 RID: 24532 RVA: 0x0023979C File Offset: 0x0023799C
	private void ClickLeft()
	{
		this.selectedIndex = Mathf.Clamp(this.selectedIndex - 1, 0, this.clusterKeys.Count - 1);
		this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[this.selectedIndex]]);
	}

	// Token: 0x06005FD5 RID: 24533 RVA: 0x002397F4 File Offset: 0x002379F4
	private void ClickRight()
	{
		this.selectedIndex = Mathf.Clamp(this.selectedIndex + 1, 0, this.clusterKeys.Count - 1);
		this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[this.selectedIndex]]);
	}

	// Token: 0x06005FD6 RID: 24534 RVA: 0x00239849 File Offset: 0x00237A49
	public void Init()
	{
		this.clusterKeys = new List<string>();
		this.clusterStartWorlds = new Dictionary<string, string>();
		this.UpdateDisplayedClusters();
	}

	// Token: 0x06005FD7 RID: 24535 RVA: 0x00239867 File Offset: 0x00237A67
	public void Uninit()
	{
	}

	// Token: 0x06005FD8 RID: 24536 RVA: 0x0023986C File Offset: 0x00237A6C
	private void Update()
	{
		if (!this.isDragging)
		{
			float num = this.offset + (float)this.selectedIndex * this.asteroidXSeparation;
			float num2 = 0f;
			if (num != 0f)
			{
				num2 = -num;
			}
			num2 = Mathf.Clamp(num2, -this.asteroidXSeparation * 2f, this.asteroidXSeparation * 2f);
			if (num2 != 0f)
			{
				float num3 = this.centeringSpeed * Time.unscaledDeltaTime;
				float num4 = num2 * this.centeringSpeed * Time.unscaledDeltaTime;
				if (num4 > 0f && num4 < num3)
				{
					num4 = Mathf.Min(num3, num2);
				}
				else if (num4 < 0f && num4 > -num3)
				{
					num4 = Mathf.Max(-num3, num2);
				}
				this.offset += num4;
			}
		}
		float x = this.asteroidContainer.rect.min.x;
		float x2 = this.asteroidContainer.rect.max.x;
		this.offset = Mathf.Clamp(this.offset, (float)(-(float)(this.clusterStartWorlds.Count - 1)) * this.asteroidXSeparation + x, x2);
		this.RePlaceAsteroids();
		for (int i = 0; i < this.moonContainer.transform.childCount; i++)
		{
			this.moonContainer.transform.GetChild(i).GetChild(0).SetLocalPosition(new Vector3(0f, 1.5f + 3f * Mathf.Sin(((float)i + Time.realtimeSinceStartup) * 1.25f), 0f));
		}
	}

	// Token: 0x06005FD9 RID: 24537 RVA: 0x00239A08 File Offset: 0x00237C08
	public void UpdateDisplayedClusters()
	{
		this.clusterKeys.Clear();
		this.clusterStartWorlds.Clear();
		this.asteroidData.Clear();
		foreach (KeyValuePair<string, ClusterLayout> keyValuePair in SettingsCache.clusterLayouts.clusterCache)
		{
			if ((!DlcManager.FeatureClusterSpaceEnabled() || !(keyValuePair.Key == "clusters/SandstoneDefault")) && keyValuePair.Value.clusterCategory == (ClusterLayout.ClusterCategory)DestinationSelectPanel.ChosenClusterCategorySetting)
			{
				this.clusterKeys.Add(keyValuePair.Key);
				ColonyDestinationAsteroidBeltData value = new ColonyDestinationAsteroidBeltData(keyValuePair.Value.GetStartWorld(), 0, keyValuePair.Key);
				this.asteroidData[keyValuePair.Key] = value;
				this.clusterStartWorlds.Add(keyValuePair.Key, keyValuePair.Value.GetStartWorld());
			}
		}
		this.clusterKeys.Sort((string a, string b) => SettingsCache.clusterLayouts.clusterCache[a].menuOrder.CompareTo(SettingsCache.clusterLayouts.clusterCache[b].menuOrder));
	}

	// Token: 0x06005FDA RID: 24538 RVA: 0x00239B34 File Offset: 0x00237D34
	[ContextMenu("RePlaceAsteroids")]
	public void RePlaceAsteroids()
	{
		this.BeginAsteroidDrawing();
		for (int i = 0; i < this.clusterKeys.Count; i++)
		{
			float x = this.offset + (float)i * this.asteroidXSeparation;
			string text = this.clusterKeys[i];
			float iconScale = this.asteroidData[text].GetStartWorld.iconScale;
			this.GetAsteroid(text, (i == this.selectedIndex) ? (this.asteroidFocusScale * iconScale) : iconScale).transform.SetLocalPosition(new Vector3(x, (i == this.selectedIndex) ? (5f + 10f * Mathf.Sin(Time.realtimeSinceStartup * 1f)) : 0f, 0f));
		}
		this.EndAsteroidDrawing();
	}

	// Token: 0x06005FDB RID: 24539 RVA: 0x00239BFB File Offset: 0x00237DFB
	private void BeginAsteroidDrawing()
	{
		this.numAsteroids = 0;
	}

	// Token: 0x06005FDC RID: 24540 RVA: 0x00239C04 File Offset: 0x00237E04
	private void ShowMoons(ColonyDestinationAsteroidBeltData asteroid)
	{
		if (asteroid.worlds.Count > 0)
		{
			while (this.moonContainer.transform.childCount < asteroid.worlds.Count)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.moonPrefab, this.moonContainer.transform);
			}
			for (int i = 0; i < asteroid.worlds.Count; i++)
			{
				KBatchedAnimController componentInChildren = this.moonContainer.transform.GetChild(i).GetComponentInChildren<KBatchedAnimController>();
				int index = (-1 + i + asteroid.worlds.Count / 2) % asteroid.worlds.Count;
				ProcGen.World world = asteroid.worlds[index];
				KAnimFile anim = Assets.GetAnim(world.asteroidIcon.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : world.asteroidIcon);
				if (anim != null)
				{
					componentInChildren.SetVisiblity(true);
					componentInChildren.SwapAnims(new KAnimFile[]
					{
						anim
					});
					componentInChildren.initialMode = KAnim.PlayMode.Loop;
					componentInChildren.initialAnim = "idle_loop";
					componentInChildren.gameObject.SetActive(true);
					if (componentInChildren.HasAnimation(componentInChildren.initialAnim))
					{
						componentInChildren.Play(componentInChildren.initialAnim, KAnim.PlayMode.Loop, 1f, 0f);
					}
					componentInChildren.transform.parent.gameObject.SetActive(true);
				}
			}
			for (int j = asteroid.worlds.Count; j < this.moonContainer.transform.childCount; j++)
			{
				KBatchedAnimController componentInChildren2 = this.moonContainer.transform.GetChild(j).GetComponentInChildren<KBatchedAnimController>();
				if (componentInChildren2 != null)
				{
					componentInChildren2.SetVisiblity(false);
				}
				this.moonContainer.transform.GetChild(j).gameObject.SetActive(false);
			}
			return;
		}
		KBatchedAnimController[] componentsInChildren = this.moonContainer.GetComponentsInChildren<KBatchedAnimController>();
		for (int k = 0; k < componentsInChildren.Length; k++)
		{
			componentsInChildren[k].SetVisiblity(false);
		}
	}

	// Token: 0x06005FDD RID: 24541 RVA: 0x00239E00 File Offset: 0x00238000
	private DestinationAsteroid2 GetAsteroid(string name, float scale)
	{
		DestinationAsteroid2 destinationAsteroid;
		if (this.numAsteroids < this.asteroids.Count)
		{
			destinationAsteroid = this.asteroids[this.numAsteroids];
		}
		else
		{
			destinationAsteroid = global::Util.KInstantiateUI<DestinationAsteroid2>(this.asteroidPrefab, this.asteroidContainer.gameObject, false);
			destinationAsteroid.OnClicked += this.OnAsteroidClicked;
			this.asteroids.Add(destinationAsteroid);
		}
		destinationAsteroid.SetAsteroid(this.asteroidData[name]);
		this.asteroidData[name].TargetScale = scale;
		this.asteroidData[name].Scale += (this.asteroidData[name].TargetScale - this.asteroidData[name].Scale) * this.focusScaleSpeed * Time.unscaledDeltaTime;
		destinationAsteroid.transform.localScale = Vector3.one * this.asteroidData[name].Scale;
		this.numAsteroids++;
		return destinationAsteroid;
	}

	// Token: 0x06005FDE RID: 24542 RVA: 0x00239F08 File Offset: 0x00238108
	private void EndAsteroidDrawing()
	{
		for (int i = 0; i < this.asteroids.Count; i++)
		{
			this.asteroids[i].gameObject.SetActive(i < this.numAsteroids);
		}
	}

	// Token: 0x06005FDF RID: 24543 RVA: 0x00239F4A File Offset: 0x0023814A
	public ColonyDestinationAsteroidBeltData SelectCluster(string name, int seed)
	{
		this.selectedIndex = this.clusterKeys.IndexOf(name);
		this.asteroidData[name].ReInitialize(seed);
		return this.asteroidData[name];
	}

	// Token: 0x06005FE0 RID: 24544 RVA: 0x00239F7C File Offset: 0x0023817C
	public string GetDefaultAsteroid()
	{
		foreach (string text in this.clusterKeys)
		{
			if (this.asteroidData[text].Layout.menuOrder == 0)
			{
				return text;
			}
		}
		return this.clusterKeys.First<string>();
	}

	// Token: 0x06005FE1 RID: 24545 RVA: 0x00239FF4 File Offset: 0x002381F4
	public ColonyDestinationAsteroidBeltData SelectDefaultAsteroid(int seed)
	{
		this.selectedIndex = 0;
		string key = this.asteroidData.Keys.First<string>();
		this.asteroidData[key].ReInitialize(seed);
		return this.asteroidData[key];
	}

	// Token: 0x06005FE2 RID: 24546 RVA: 0x0023A038 File Offset: 0x00238238
	public void ScrollLeft()
	{
		int index = Mathf.Max(this.selectedIndex - 1, 0);
		this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[index]]);
	}

	// Token: 0x06005FE3 RID: 24547 RVA: 0x0023A078 File Offset: 0x00238278
	public void ScrollRight()
	{
		int index = Mathf.Min(this.selectedIndex + 1, this.clusterStartWorlds.Count - 1);
		this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[index]]);
	}

	// Token: 0x06005FE4 RID: 24548 RVA: 0x0023A0C4 File Offset: 0x002382C4
	private void DebugCurrentSetting()
	{
		ColonyDestinationAsteroidBeltData colonyDestinationAsteroidBeltData = this.asteroidData[this.clusterKeys[this.selectedIndex]];
		string text = "{world}: {seed} [{traits}] {{settings}}";
		string startWorldName = colonyDestinationAsteroidBeltData.startWorldName;
		string newValue = colonyDestinationAsteroidBeltData.seed.ToString();
		text = text.Replace("{world}", startWorldName);
		text = text.Replace("{seed}", newValue);
		List<AsteroidDescriptor> traitDescriptors = colonyDestinationAsteroidBeltData.GetTraitDescriptors();
		string[] array = new string[traitDescriptors.Count];
		for (int i = 0; i < traitDescriptors.Count; i++)
		{
			array[i] = traitDescriptors[i].text;
		}
		string newValue2 = string.Join(", ", array);
		text = text.Replace("{traits}", newValue2);
		CustomGameSettings.CustomGameMode customGameMode = CustomGameSettings.Instance.customGameMode;
		if (customGameMode != CustomGameSettings.CustomGameMode.Survival)
		{
			if (customGameMode != CustomGameSettings.CustomGameMode.Nosweat)
			{
				if (customGameMode == CustomGameSettings.CustomGameMode.Custom)
				{
					List<string> list = new List<string>();
					foreach (KeyValuePair<string, SettingConfig> keyValuePair in CustomGameSettings.Instance.QualitySettings)
					{
						if (keyValuePair.Value.coordinate_range >= 0L)
						{
							SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(keyValuePair.Key);
							if (currentQualitySetting.id != keyValuePair.Value.GetDefaultLevelId())
							{
								list.Add(string.Format("{0}={1}", keyValuePair.Value.label, currentQualitySetting.label));
							}
						}
					}
					text = text.Replace("{settings}", string.Join(", ", list.ToArray()));
				}
			}
			else
			{
				text = text.Replace("{settings}", "Nosweat");
			}
		}
		else
		{
			text = text.Replace("{settings}", "Survival");
		}
		global::Debug.Log(text);
	}

	// Token: 0x04004093 RID: 16531
	[SerializeField]
	private GameObject asteroidPrefab;

	// Token: 0x04004094 RID: 16532
	[SerializeField]
	private KButtonDrag dragTarget;

	// Token: 0x04004095 RID: 16533
	[SerializeField]
	private MultiToggle leftArrowButton;

	// Token: 0x04004096 RID: 16534
	[SerializeField]
	private MultiToggle rightArrowButton;

	// Token: 0x04004097 RID: 16535
	[SerializeField]
	private RectTransform asteroidContainer;

	// Token: 0x04004098 RID: 16536
	[SerializeField]
	private float asteroidFocusScale = 2f;

	// Token: 0x04004099 RID: 16537
	[SerializeField]
	private float asteroidXSeparation = 240f;

	// Token: 0x0400409A RID: 16538
	[SerializeField]
	private float focusScaleSpeed = 0.5f;

	// Token: 0x0400409B RID: 16539
	[SerializeField]
	private float centeringSpeed = 0.5f;

	// Token: 0x0400409C RID: 16540
	[SerializeField]
	private GameObject moonContainer;

	// Token: 0x0400409D RID: 16541
	[SerializeField]
	private GameObject moonPrefab;

	// Token: 0x0400409E RID: 16542
	private static int chosenClusterCategorySetting;

	// Token: 0x040040A0 RID: 16544
	private float offset;

	// Token: 0x040040A1 RID: 16545
	private int selectedIndex = -1;

	// Token: 0x040040A2 RID: 16546
	private List<DestinationAsteroid2> asteroids = new List<DestinationAsteroid2>();

	// Token: 0x040040A3 RID: 16547
	private int numAsteroids;

	// Token: 0x040040A4 RID: 16548
	private List<string> clusterKeys;

	// Token: 0x040040A5 RID: 16549
	private Dictionary<string, string> clusterStartWorlds;

	// Token: 0x040040A6 RID: 16550
	private Dictionary<string, ColonyDestinationAsteroidBeltData> asteroidData = new Dictionary<string, ColonyDestinationAsteroidBeltData>();

	// Token: 0x040040A7 RID: 16551
	private Vector2 dragStartPos;

	// Token: 0x040040A8 RID: 16552
	private Vector2 dragLastPos;

	// Token: 0x040040A9 RID: 16553
	private bool isDragging;

	// Token: 0x040040AA RID: 16554
	private const string debugFmt = "{world}: {seed} [{traits}] {{settings}}";
}
