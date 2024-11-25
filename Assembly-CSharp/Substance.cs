using System;
using FMODUnity;
using Klei;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000B26 RID: 2854
[Serializable]
public class Substance
{
	// Token: 0x0600551E RID: 21790 RVA: 0x001E6CB0 File Offset: 0x001E4EB0
	public GameObject SpawnResource(Vector3 position, float mass, float temperature, byte disease_idx, int disease_count, bool prevent_merge = false, bool forceTemperature = false, bool manual_activation = false)
	{
		GameObject gameObject = null;
		PrimaryElement primaryElement = null;
		if (!prevent_merge)
		{
			int cell = Grid.PosToCell(position);
			GameObject gameObject2 = Grid.Objects[cell, 3];
			if (gameObject2 != null)
			{
				Pickupable component = gameObject2.GetComponent<Pickupable>();
				if (component != null)
				{
					Tag b = GameTagExtensions.Create(this.elementID);
					for (ObjectLayerListItem objectLayerListItem = component.objectLayerListItem; objectLayerListItem != null; objectLayerListItem = objectLayerListItem.nextItem)
					{
						KPrefabID component2 = objectLayerListItem.gameObject.GetComponent<KPrefabID>();
						if (component2.PrefabTag == b)
						{
							PrimaryElement component3 = component2.GetComponent<PrimaryElement>();
							if (component3.Mass + mass <= PrimaryElement.MAX_MASS)
							{
								gameObject = component2.gameObject;
								primaryElement = component3;
								temperature = SimUtil.CalculateFinalTemperature(primaryElement.Mass, primaryElement.Temperature, mass, temperature);
								position = gameObject.transform.GetPosition();
								break;
							}
						}
					}
				}
			}
		}
		if (gameObject == null)
		{
			gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.nameTag), Grid.SceneLayer.Ore, null, 0);
			primaryElement = gameObject.GetComponent<PrimaryElement>();
			primaryElement.Mass = mass;
		}
		else
		{
			global::Debug.Assert(primaryElement != null);
			Pickupable component4 = primaryElement.GetComponent<Pickupable>();
			if (component4 != null)
			{
				component4.TotalAmount += mass / primaryElement.MassPerUnit;
			}
			else
			{
				primaryElement.Mass += mass;
			}
		}
		primaryElement.Temperature = temperature;
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		gameObject.transform.SetPosition(position);
		if (!manual_activation)
		{
			this.ActivateSubstanceGameObject(gameObject, disease_idx, disease_count);
		}
		return gameObject;
	}

	// Token: 0x0600551F RID: 21791 RVA: 0x001E6E2C File Offset: 0x001E502C
	public void ActivateSubstanceGameObject(GameObject obj, byte disease_idx, int disease_count)
	{
		obj.SetActive(true);
		obj.GetComponent<PrimaryElement>().AddDisease(disease_idx, disease_count, "Substances.SpawnResource");
	}

	// Token: 0x06005520 RID: 21792 RVA: 0x001E6E48 File Offset: 0x001E5048
	private void SetTexture(MaterialPropertyBlock block, string texture_name)
	{
		Texture texture = this.material.GetTexture(texture_name);
		if (texture != null)
		{
			this.propertyBlock.SetTexture(texture_name, texture);
		}
	}

	// Token: 0x06005521 RID: 21793 RVA: 0x001E6E78 File Offset: 0x001E5078
	public void RefreshPropertyBlock()
	{
		if (this.propertyBlock == null)
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}
		if (this.material != null)
		{
			this.SetTexture(this.propertyBlock, "_MainTex");
			float @float = this.material.GetFloat("_WorldUVScale");
			this.propertyBlock.SetFloat("_WorldUVScale", @float);
			if (ElementLoader.FindElementByHash(this.elementID).IsSolid)
			{
				this.SetTexture(this.propertyBlock, "_MainTex2");
				this.SetTexture(this.propertyBlock, "_HeightTex2");
				this.propertyBlock.SetFloat("_Frequency", this.material.GetFloat("_Frequency"));
				this.propertyBlock.SetColor("_ShineColour", this.material.GetColor("_ShineColour"));
				this.propertyBlock.SetColor("_ColourTint", this.material.GetColor("_ColourTint"));
			}
		}
	}

	// Token: 0x06005522 RID: 21794 RVA: 0x001E6F73 File Offset: 0x001E5173
	internal AmbienceType GetAmbience()
	{
		if (this.audioConfig == null)
		{
			return AmbienceType.None;
		}
		return this.audioConfig.ambienceType;
	}

	// Token: 0x06005523 RID: 21795 RVA: 0x001E6F8A File Offset: 0x001E518A
	internal SolidAmbienceType GetSolidAmbience()
	{
		if (this.audioConfig == null)
		{
			return SolidAmbienceType.None;
		}
		return this.audioConfig.solidAmbienceType;
	}

	// Token: 0x06005524 RID: 21796 RVA: 0x001E6FA1 File Offset: 0x001E51A1
	internal string GetMiningSound()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.miningSound;
	}

	// Token: 0x06005525 RID: 21797 RVA: 0x001E6FBC File Offset: 0x001E51BC
	internal string GetMiningBreakSound()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.miningBreakSound;
	}

	// Token: 0x06005526 RID: 21798 RVA: 0x001E6FD7 File Offset: 0x001E51D7
	internal string GetOreBumpSound()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.oreBumpSound;
	}

	// Token: 0x06005527 RID: 21799 RVA: 0x001E6FF2 File Offset: 0x001E51F2
	internal string GetFloorEventAudioCategory()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.floorEventAudioCategory;
	}

	// Token: 0x06005528 RID: 21800 RVA: 0x001E700D File Offset: 0x001E520D
	internal string GetCreatureChewSound()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.creatureChewSound;
	}

	// Token: 0x040037BD RID: 14269
	public string name;

	// Token: 0x040037BE RID: 14270
	public SimHashes elementID;

	// Token: 0x040037BF RID: 14271
	internal Tag nameTag;

	// Token: 0x040037C0 RID: 14272
	public Color32 colour;

	// Token: 0x040037C1 RID: 14273
	[FormerlySerializedAs("debugColour")]
	public Color32 uiColour;

	// Token: 0x040037C2 RID: 14274
	[FormerlySerializedAs("overlayColour")]
	public Color32 conduitColour = Color.white;

	// Token: 0x040037C3 RID: 14275
	[NonSerialized]
	internal bool renderedByWorld;

	// Token: 0x040037C4 RID: 14276
	[NonSerialized]
	internal int idx;

	// Token: 0x040037C5 RID: 14277
	public Material material;

	// Token: 0x040037C6 RID: 14278
	public KAnimFile anim;

	// Token: 0x040037C7 RID: 14279
	[SerializeField]
	internal bool showInEditor = true;

	// Token: 0x040037C8 RID: 14280
	[NonSerialized]
	internal KAnimFile[] anims;

	// Token: 0x040037C9 RID: 14281
	[NonSerialized]
	internal ElementsAudio.ElementAudioConfig audioConfig;

	// Token: 0x040037CA RID: 14282
	[NonSerialized]
	internal MaterialPropertyBlock propertyBlock;

	// Token: 0x040037CB RID: 14283
	public EventReference fallingStartSound;

	// Token: 0x040037CC RID: 14284
	public EventReference fallingStopSound;
}
