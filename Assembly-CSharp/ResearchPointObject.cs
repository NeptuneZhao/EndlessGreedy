using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A4F RID: 2639
[AddComponentMenu("KMonoBehaviour/scripts/ResearchPointObject")]
public class ResearchPointObject : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06004C92 RID: 19602 RVA: 0x001B5C40 File Offset: 0x001B3E40
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Research.Instance.AddResearchPoints(this.TypeID, 1f);
		ResearchType researchType = Research.Instance.GetResearchType(this.TypeID);
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Research, researchType.name, base.transform, 1.5f, false);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06004C93 RID: 19603 RVA: 0x001B5CAC File Offset: 0x001B3EAC
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		ResearchType researchType = Research.Instance.GetResearchType(this.TypeID);
		list.Add(new Descriptor(string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.RESEARCHPOINT, researchType.name), string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.RESEARCHPOINT, researchType.description), Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x040032EC RID: 13036
	public string TypeID = "";
}
