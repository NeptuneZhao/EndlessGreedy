using System;
using Klei;
using UnityEngine;

// Token: 0x02000875 RID: 2165
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ElementChunk")]
public class ElementChunk : KMonoBehaviour
{
	// Token: 0x06003C5F RID: 15455 RVA: 0x0014EA4F File Offset: 0x0014CC4F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GameComps.OreSizeVisualizers.Add(base.gameObject);
		GameComps.ElementSplitters.Add(base.gameObject);
		base.Subscribe<ElementChunk>(-2064133523, ElementChunk.OnAbsorbDelegate);
	}

	// Token: 0x06003C60 RID: 15456 RVA: 0x0014EA8C File Offset: 0x0014CC8C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		base.transform.SetPosition(position);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		Element element = component.Element;
		KSelectable component2 = base.GetComponent<KSelectable>();
		Func<Element> data = () => element;
		component2.AddStatusItem(Db.Get().MiscStatusItems.ElementalCategory, data);
		component2.AddStatusItem(Db.Get().MiscStatusItems.OreMass, base.gameObject);
		component2.AddStatusItem(Db.Get().MiscStatusItems.OreTemp, base.gameObject);
	}

	// Token: 0x06003C61 RID: 15457 RVA: 0x0014EB40 File Offset: 0x0014CD40
	protected override void OnCleanUp()
	{
		GameComps.ElementSplitters.Remove(base.gameObject);
		GameComps.OreSizeVisualizers.Remove(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x06003C62 RID: 15458 RVA: 0x0014EB68 File Offset: 0x0014CD68
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable != null)
		{
			PrimaryElement primaryElement = pickupable.PrimaryElement;
			if (primaryElement != null)
			{
				float mass = primaryElement.Mass;
				if (mass > 0f)
				{
					PrimaryElement component = base.GetComponent<PrimaryElement>();
					float mass2 = component.Mass;
					float temperature = (mass2 > 0f) ? SimUtil.CalculateFinalTemperature(mass2, component.Temperature, mass, primaryElement.Temperature) : primaryElement.Temperature;
					component.SetMassTemperature(mass2 + mass, temperature);
				}
				if (CameraController.Instance != null)
				{
					string sound = GlobalAssets.GetSound("Ore_absorb", false);
					Vector3 position = pickupable.transform.GetPosition();
					position.z = 0f;
					if (sound != null && CameraController.Instance.IsAudibleSound(position, sound))
					{
						KFMOD.PlayOneShot(sound, position, 1f);
					}
				}
			}
		}
	}

	// Token: 0x040024DB RID: 9435
	private static readonly EventSystem.IntraObjectHandler<ElementChunk> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<ElementChunk>(delegate(ElementChunk component, object data)
	{
		component.OnAbsorb(data);
	});
}
