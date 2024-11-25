using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x020004FC RID: 1276
[AddComponentMenu("KMonoBehaviour/scripts/AmbienceManager")]
public class AmbienceManager : KMonoBehaviour
{
	// Token: 0x06001C71 RID: 7281 RVA: 0x000959C8 File Offset: 0x00093BC8
	protected override void OnSpawn()
	{
		if (!RuntimeManager.IsInitialized)
		{
			base.enabled = false;
			return;
		}
		for (int i = 0; i < this.quadrants.Length; i++)
		{
			this.quadrants[i] = new AmbienceManager.Quadrant(this.quadrantDefs[i]);
		}
	}

	// Token: 0x06001C72 RID: 7282 RVA: 0x00095A0C File Offset: 0x00093C0C
	protected override void OnForcedCleanUp()
	{
		AmbienceManager.Quadrant[] array = this.quadrants;
		for (int i = 0; i < array.Length; i++)
		{
			foreach (AmbienceManager.Layer layer in array[i].GetAllLayers())
			{
				layer.Stop();
			}
		}
	}

	// Token: 0x06001C73 RID: 7283 RVA: 0x00095A74 File Offset: 0x00093C74
	private void LateUpdate()
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		Vector2I min = visibleArea.Min;
		Vector2I max = visibleArea.Max;
		Vector2I vector2I = min + (max - min) / 2;
		Vector3 a = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = vector + (a - vector) / 2f;
		Vector3 vector3 = a - vector;
		if (vector3.x > vector3.y)
		{
			vector3.y = vector3.x;
		}
		else
		{
			vector3.x = vector3.y;
		}
		a = vector2 + vector3 / 2f;
		vector = vector2 - vector3 / 2f;
		Vector3 vector4 = vector3 / 2f / 2f;
		this.quadrants[0].Update(new Vector2I(min.x, min.y), new Vector2I(vector2I.x, vector2I.y), new Vector3(vector.x + vector4.x, vector.y + vector4.y, this.emitterZPosition));
		this.quadrants[1].Update(new Vector2I(vector2I.x, min.y), new Vector2I(max.x, vector2I.y), new Vector3(vector2.x + vector4.x, vector.y + vector4.y, this.emitterZPosition));
		this.quadrants[2].Update(new Vector2I(min.x, vector2I.y), new Vector2I(vector2I.x, max.y), new Vector3(vector.x + vector4.x, vector2.y + vector4.y, this.emitterZPosition));
		this.quadrants[3].Update(new Vector2I(vector2I.x, vector2I.y), new Vector2I(max.x, max.y), new Vector3(vector2.x + vector4.x, vector2.y + vector4.y, this.emitterZPosition));
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		for (int i = 0; i < this.quadrants.Length; i++)
		{
			num += (float)this.quadrants[i].spaceLayer.tileCount;
			num2 += (float)this.quadrants[i].facilityLayer.tileCount;
			num3 += (float)this.quadrants[i].totalTileCount;
		}
		AudioMixer.instance.UpdateSpaceVisibleSnapshot(num / num3);
		AudioMixer.instance.UpdateFacilityVisibleSnapshot(num2 / num3);
	}

	// Token: 0x04001002 RID: 4098
	private float emitterZPosition;

	// Token: 0x04001003 RID: 4099
	public AmbienceManager.QuadrantDef[] quadrantDefs;

	// Token: 0x04001004 RID: 4100
	public AmbienceManager.Quadrant[] quadrants = new AmbienceManager.Quadrant[4];

	// Token: 0x020012C7 RID: 4807
	public class Tuning : TuningData<AmbienceManager.Tuning>
	{
		// Token: 0x0400644E RID: 25678
		public int backwallTileValue = 1;

		// Token: 0x0400644F RID: 25679
		public int foundationTileValue = 2;

		// Token: 0x04006450 RID: 25680
		public int buildingTileValue = 3;
	}

	// Token: 0x020012C8 RID: 4808
	public class Layer : IComparable<AmbienceManager.Layer>
	{
		// Token: 0x060084D4 RID: 34004 RVA: 0x0032457A File Offset: 0x0032277A
		public Layer(EventReference sound, EventReference one_shot_sound = default(EventReference))
		{
			this.sound = sound;
			this.oneShotSound = one_shot_sound;
		}

		// Token: 0x060084D5 RID: 34005 RVA: 0x00324590 File Offset: 0x00322790
		public void Reset()
		{
			this.tileCount = 0;
			this.averageTemperature = 0f;
			this.averageRadiation = 0f;
		}

		// Token: 0x060084D6 RID: 34006 RVA: 0x003245AF File Offset: 0x003227AF
		public void UpdatePercentage(int cell_count)
		{
			this.tilePercentage = (float)this.tileCount / (float)cell_count;
		}

		// Token: 0x060084D7 RID: 34007 RVA: 0x003245C1 File Offset: 0x003227C1
		public void UpdateAverageTemperature()
		{
			this.averageTemperature /= (float)this.tileCount;
			this.soundEvent.setParameterByName("averageTemperature", this.averageTemperature, false);
		}

		// Token: 0x060084D8 RID: 34008 RVA: 0x003245EF File Offset: 0x003227EF
		public void UpdateAverageRadiation()
		{
			this.averageRadiation = ((this.tileCount > 0) ? (this.averageRadiation / (float)this.tileCount) : 0f);
			this.soundEvent.setParameterByName("averageRadiation", this.averageRadiation, false);
		}

		// Token: 0x060084D9 RID: 34009 RVA: 0x00324630 File Offset: 0x00322830
		public void UpdateParameters(Vector3 emitter_position)
		{
			if (!this.soundEvent.isValid())
			{
				return;
			}
			Vector3 pos = new Vector3(emitter_position.x, emitter_position.y, 0f);
			this.soundEvent.set3DAttributes(pos.To3DAttributes());
			this.soundEvent.setParameterByName("tilePercentage", this.tilePercentage, false);
		}

		// Token: 0x060084DA RID: 34010 RVA: 0x0032468D File Offset: 0x0032288D
		public void SetCustomParameter(string parameterName, float value)
		{
			this.soundEvent.setParameterByName(parameterName, value, false);
		}

		// Token: 0x060084DB RID: 34011 RVA: 0x0032469E File Offset: 0x0032289E
		public int CompareTo(AmbienceManager.Layer layer)
		{
			return layer.tileCount - this.tileCount;
		}

		// Token: 0x060084DC RID: 34012 RVA: 0x003246AD File Offset: 0x003228AD
		public void SetVolume(float volume)
		{
			if (this.volume != volume)
			{
				this.volume = volume;
				if (this.soundEvent.isValid())
				{
					this.soundEvent.setVolume(volume);
				}
			}
		}

		// Token: 0x060084DD RID: 34013 RVA: 0x003246D9 File Offset: 0x003228D9
		public void Stop()
		{
			if (this.soundEvent.isValid())
			{
				this.soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				this.soundEvent.release();
			}
			this.isRunning = false;
		}

		// Token: 0x060084DE RID: 34014 RVA: 0x00324708 File Offset: 0x00322908
		public void Start(Vector3 emitter_position)
		{
			if (!this.isRunning)
			{
				if (!this.oneShotSound.IsNull)
				{
					EventInstance eventInstance = KFMOD.CreateInstance(this.oneShotSound);
					if (!eventInstance.isValid())
					{
						string str = "Could not find event: ";
						EventReference eventReference = this.oneShotSound;
						global::Debug.LogWarning(str + eventReference.ToString());
						return;
					}
					ATTRIBUTES_3D attributes = new Vector3(emitter_position.x, emitter_position.y, 0f).To3DAttributes();
					eventInstance.set3DAttributes(attributes);
					eventInstance.setVolume(this.tilePercentage * 2f);
					eventInstance.start();
					eventInstance.release();
					return;
				}
				else
				{
					this.soundEvent = KFMOD.CreateInstance(this.sound);
					if (this.soundEvent.isValid())
					{
						this.soundEvent.start();
					}
					this.isRunning = true;
				}
			}
		}

		// Token: 0x04006451 RID: 25681
		private const string TILE_PERCENTAGE_ID = "tilePercentage";

		// Token: 0x04006452 RID: 25682
		private const string AVERAGE_TEMPERATURE_ID = "averageTemperature";

		// Token: 0x04006453 RID: 25683
		private const string AVERAGE_RADIATION_ID = "averageRadiation";

		// Token: 0x04006454 RID: 25684
		public EventReference sound;

		// Token: 0x04006455 RID: 25685
		public EventReference oneShotSound;

		// Token: 0x04006456 RID: 25686
		public int tileCount;

		// Token: 0x04006457 RID: 25687
		public float tilePercentage;

		// Token: 0x04006458 RID: 25688
		public float volume;

		// Token: 0x04006459 RID: 25689
		public bool isRunning;

		// Token: 0x0400645A RID: 25690
		private EventInstance soundEvent;

		// Token: 0x0400645B RID: 25691
		public float averageTemperature;

		// Token: 0x0400645C RID: 25692
		public float averageRadiation;
	}

	// Token: 0x020012C9 RID: 4809
	[Serializable]
	public class QuadrantDef
	{
		// Token: 0x0400645D RID: 25693
		public string name;

		// Token: 0x0400645E RID: 25694
		public EventReference[] liquidSounds;

		// Token: 0x0400645F RID: 25695
		public EventReference[] gasSounds;

		// Token: 0x04006460 RID: 25696
		public EventReference[] solidSounds;

		// Token: 0x04006461 RID: 25697
		public EventReference fogSound;

		// Token: 0x04006462 RID: 25698
		public EventReference spaceSound;

		// Token: 0x04006463 RID: 25699
		public EventReference rocketInteriorSound;

		// Token: 0x04006464 RID: 25700
		public EventReference facilitySound;

		// Token: 0x04006465 RID: 25701
		public EventReference radiationSound;
	}

	// Token: 0x020012CA RID: 4810
	public class Quadrant
	{
		// Token: 0x060084E0 RID: 34016 RVA: 0x003247EC File Offset: 0x003229EC
		public Quadrant(AmbienceManager.QuadrantDef def)
		{
			this.name = def.name;
			this.fogLayer = new AmbienceManager.Layer(def.fogSound, default(EventReference));
			this.allLayers.Add(this.fogLayer);
			this.loopingLayers.Add(this.fogLayer);
			this.spaceLayer = new AmbienceManager.Layer(def.spaceSound, default(EventReference));
			this.allLayers.Add(this.spaceLayer);
			this.loopingLayers.Add(this.spaceLayer);
			this.m_isClusterSpaceEnabled = DlcManager.FeatureClusterSpaceEnabled();
			if (this.m_isClusterSpaceEnabled)
			{
				this.rocketInteriorLayer = new AmbienceManager.Layer(def.rocketInteriorSound, default(EventReference));
				this.allLayers.Add(this.rocketInteriorLayer);
			}
			this.facilityLayer = new AmbienceManager.Layer(def.facilitySound, default(EventReference));
			this.allLayers.Add(this.facilityLayer);
			this.loopingLayers.Add(this.facilityLayer);
			this.m_isRadiationEnabled = Sim.IsRadiationEnabled();
			if (this.m_isRadiationEnabled)
			{
				this.radiationLayer = new AmbienceManager.Layer(def.radiationSound, default(EventReference));
				this.allLayers.Add(this.radiationLayer);
			}
			for (int i = 0; i < 4; i++)
			{
				this.gasLayers[i] = new AmbienceManager.Layer(def.gasSounds[i], default(EventReference));
				this.liquidLayers[i] = new AmbienceManager.Layer(def.liquidSounds[i], default(EventReference));
				this.allLayers.Add(this.gasLayers[i]);
				this.allLayers.Add(this.liquidLayers[i]);
				this.loopingLayers.Add(this.gasLayers[i]);
				this.loopingLayers.Add(this.liquidLayers[i]);
			}
			for (int j = 0; j < this.solidLayers.Length; j++)
			{
				if (j >= def.solidSounds.Length)
				{
					string str = "Missing solid layer: ";
					SolidAmbienceType solidAmbienceType = (SolidAmbienceType)j;
					global::Debug.LogError(str + solidAmbienceType.ToString());
				}
				this.solidLayers[j] = new AmbienceManager.Layer(default(EventReference), def.solidSounds[j]);
				this.allLayers.Add(this.solidLayers[j]);
				this.oneShotLayers.Add(this.solidLayers[j]);
			}
			this.solidTimers = new AmbienceManager.Quadrant.SolidTimer[AmbienceManager.Quadrant.activeSolidLayerCount];
			for (int k = 0; k < AmbienceManager.Quadrant.activeSolidLayerCount; k++)
			{
				this.solidTimers[k] = new AmbienceManager.Quadrant.SolidTimer();
			}
		}

		// Token: 0x060084E1 RID: 34017 RVA: 0x00324AE4 File Offset: 0x00322CE4
		public void Update(Vector2I min, Vector2I max, Vector3 emitter_position)
		{
			this.emitterPosition = emitter_position;
			this.totalTileCount = 0;
			for (int i = 0; i < this.allLayers.Count; i++)
			{
				this.allLayers[i].Reset();
			}
			for (int j = min.y; j < max.y; j++)
			{
				if (j % 2 != 1)
				{
					for (int k = min.x; k < max.x; k++)
					{
						if (k % 2 != 0)
						{
							int num = Grid.XYToCell(k, j);
							if (Grid.IsValidCell(num))
							{
								this.totalTileCount++;
								if (Grid.IsVisible(num))
								{
									if (Grid.GravitasFacility[num])
									{
										this.facilityLayer.tileCount += 8;
									}
									else
									{
										Element element = Grid.Element[num];
										if (element != null)
										{
											if (element.IsLiquid && Grid.IsSubstantialLiquid(num, 0.35f))
											{
												AmbienceType ambience = element.substance.GetAmbience();
												if (ambience != AmbienceType.None)
												{
													this.liquidLayers[(int)ambience].tileCount++;
													this.liquidLayers[(int)ambience].averageTemperature += Grid.Temperature[num];
												}
											}
											else if (element.IsGas)
											{
												AmbienceType ambience2 = element.substance.GetAmbience();
												if (ambience2 != AmbienceType.None)
												{
													this.gasLayers[(int)ambience2].tileCount++;
													this.gasLayers[(int)ambience2].averageTemperature += Grid.Temperature[num];
												}
											}
											else if (element.IsSolid)
											{
												SolidAmbienceType solidAmbienceType = element.substance.GetSolidAmbience();
												if (Grid.Foundation[num])
												{
													solidAmbienceType = SolidAmbienceType.Tile;
													this.solidLayers[(int)solidAmbienceType].tileCount += TuningData<AmbienceManager.Tuning>.Get().foundationTileValue;
													this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().foundationTileValue;
												}
												else if (Grid.Objects[num, 2] != null)
												{
													solidAmbienceType = SolidAmbienceType.Tile;
													this.solidLayers[(int)solidAmbienceType].tileCount += TuningData<AmbienceManager.Tuning>.Get().backwallTileValue;
													this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().backwallTileValue;
												}
												else if (solidAmbienceType != SolidAmbienceType.None)
												{
													this.solidLayers[(int)solidAmbienceType].tileCount++;
												}
												else if (element.id == SimHashes.Regolith || element.id == SimHashes.MaficRock)
												{
													this.spaceLayer.tileCount++;
												}
											}
											else if (element.id == SimHashes.Vacuum && CellSelectionObject.IsExposedToSpace(num))
											{
												if (Grid.Objects[num, 1] != null)
												{
													this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().buildingTileValue;
												}
												this.spaceLayer.tileCount++;
											}
										}
									}
									if (Grid.Radiation[num] > 0f)
									{
										this.radiationLayer.averageRadiation += Grid.Radiation[num];
										this.radiationLayer.tileCount++;
									}
								}
								else
								{
									this.fogLayer.tileCount++;
								}
							}
						}
					}
				}
			}
			Vector2I vector2I = max - min;
			int cell_count = vector2I.x * vector2I.y;
			for (int l = 0; l < this.allLayers.Count; l++)
			{
				this.allLayers[l].UpdatePercentage(cell_count);
			}
			this.loopingLayers.Sort();
			this.topLayers.Clear();
			for (int m = 0; m < this.loopingLayers.Count; m++)
			{
				AmbienceManager.Layer layer = this.loopingLayers[m];
				if (m < 3 && layer.tilePercentage > 0f)
				{
					layer.Start(emitter_position);
					layer.UpdateAverageTemperature();
					layer.UpdateParameters(emitter_position);
					this.topLayers.Add(layer);
				}
				else
				{
					layer.Stop();
				}
			}
			if (this.m_isClusterSpaceEnabled)
			{
				float volume = 0f;
				if (ClusterManager.Instance != null && ClusterManager.Instance.activeWorld != null && ClusterManager.Instance.activeWorld.IsModuleInterior)
				{
					volume = 1f;
				}
				this.rocketInteriorLayer.Start(emitter_position);
				this.rocketInteriorLayer.SetCustomParameter("RocketState", (float)ClusterManager.RocketInteriorState);
				this.rocketInteriorLayer.SetVolume(volume);
			}
			if (this.m_isRadiationEnabled)
			{
				this.radiationLayer.Start(emitter_position);
				this.radiationLayer.UpdateAverageRadiation();
				this.radiationLayer.UpdateParameters(emitter_position);
			}
			this.oneShotLayers.Sort();
			for (int n = 0; n < AmbienceManager.Quadrant.activeSolidLayerCount; n++)
			{
				if (this.solidTimers[n].ShouldPlay() && this.oneShotLayers[n].tilePercentage > 0f)
				{
					this.oneShotLayers[n].Start(emitter_position);
				}
			}
		}

		// Token: 0x060084E2 RID: 34018 RVA: 0x00325020 File Offset: 0x00323220
		public List<AmbienceManager.Layer> GetAllLayers()
		{
			return this.allLayers;
		}

		// Token: 0x04006466 RID: 25702
		public string name;

		// Token: 0x04006467 RID: 25703
		public Vector3 emitterPosition;

		// Token: 0x04006468 RID: 25704
		public AmbienceManager.Layer[] gasLayers = new AmbienceManager.Layer[4];

		// Token: 0x04006469 RID: 25705
		public AmbienceManager.Layer[] liquidLayers = new AmbienceManager.Layer[4];

		// Token: 0x0400646A RID: 25706
		public AmbienceManager.Layer fogLayer;

		// Token: 0x0400646B RID: 25707
		public AmbienceManager.Layer spaceLayer;

		// Token: 0x0400646C RID: 25708
		public AmbienceManager.Layer rocketInteriorLayer;

		// Token: 0x0400646D RID: 25709
		public AmbienceManager.Layer facilityLayer;

		// Token: 0x0400646E RID: 25710
		public AmbienceManager.Layer radiationLayer;

		// Token: 0x0400646F RID: 25711
		public AmbienceManager.Layer[] solidLayers = new AmbienceManager.Layer[20];

		// Token: 0x04006470 RID: 25712
		private List<AmbienceManager.Layer> allLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04006471 RID: 25713
		private List<AmbienceManager.Layer> loopingLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04006472 RID: 25714
		private List<AmbienceManager.Layer> oneShotLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04006473 RID: 25715
		private List<AmbienceManager.Layer> topLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04006474 RID: 25716
		public static int activeSolidLayerCount = 2;

		// Token: 0x04006475 RID: 25717
		public int totalTileCount;

		// Token: 0x04006476 RID: 25718
		private bool m_isRadiationEnabled;

		// Token: 0x04006477 RID: 25719
		private bool m_isClusterSpaceEnabled;

		// Token: 0x04006478 RID: 25720
		private const string ROCKET_STATE_FOR_AMBIENCE = "RocketState";

		// Token: 0x04006479 RID: 25721
		private AmbienceManager.Quadrant.SolidTimer[] solidTimers;

		// Token: 0x0200248F RID: 9359
		public class SolidTimer
		{
			// Token: 0x0600BA42 RID: 47682 RVA: 0x003D3191 File Offset: 0x003D1391
			public SolidTimer()
			{
				this.solidTargetTime = Time.unscaledTime + UnityEngine.Random.value * AmbienceManager.Quadrant.SolidTimer.solidMinTime;
			}

			// Token: 0x0600BA43 RID: 47683 RVA: 0x003D31B0 File Offset: 0x003D13B0
			public bool ShouldPlay()
			{
				if (Time.unscaledTime > this.solidTargetTime)
				{
					this.solidTargetTime = Time.unscaledTime + AmbienceManager.Quadrant.SolidTimer.solidMinTime + UnityEngine.Random.value * (AmbienceManager.Quadrant.SolidTimer.solidMaxTime - AmbienceManager.Quadrant.SolidTimer.solidMinTime);
					return true;
				}
				return false;
			}

			// Token: 0x0400A226 RID: 41510
			public static float solidMinTime = 9f;

			// Token: 0x0400A227 RID: 41511
			public static float solidMaxTime = 15f;

			// Token: 0x0400A228 RID: 41512
			public float solidTargetTime;
		}
	}
}
