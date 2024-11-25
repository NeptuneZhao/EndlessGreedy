using System;
using UnityEngine;

// Token: 0x02000C5F RID: 3167
[AddComponentMenu("KMonoBehaviour/scripts/HierarchyReferences")]
public class HierarchyReferences : KMonoBehaviour
{
	// Token: 0x06006142 RID: 24898 RVA: 0x00243F0C File Offset: 0x0024210C
	public bool HasReference(string name)
	{
		ElementReference[] array = this.references;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Name == name)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006143 RID: 24899 RVA: 0x00243F48 File Offset: 0x00242148
	public SpecifiedType GetReference<SpecifiedType>(string name) where SpecifiedType : Component
	{
		foreach (ElementReference elementReference in this.references)
		{
			if (elementReference.Name == name)
			{
				if (elementReference.behaviour is SpecifiedType)
				{
					return (SpecifiedType)((object)elementReference.behaviour);
				}
				global::Debug.LogError(string.Format("Behavior is not specified type", Array.Empty<object>()));
			}
		}
		global::Debug.LogError(string.Format("Could not find UI reference '{0}' or convert to specified type)", name));
		return default(SpecifiedType);
	}

	// Token: 0x06006144 RID: 24900 RVA: 0x00243FC8 File Offset: 0x002421C8
	public Component GetReference(string name)
	{
		foreach (ElementReference elementReference in this.references)
		{
			if (elementReference.Name == name)
			{
				return elementReference.behaviour;
			}
		}
		global::Debug.LogWarning("Couldn't find reference to object named " + name + " Make sure the name matches the field in the inspector.");
		return null;
	}

	// Token: 0x040041F2 RID: 16882
	public ElementReference[] references;
}
