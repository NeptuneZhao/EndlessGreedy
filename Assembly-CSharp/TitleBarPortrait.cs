using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DDA RID: 3546
[AddComponentMenu("KMonoBehaviour/scripts/TitleBarPortrait")]
public class TitleBarPortrait : KMonoBehaviour
{
	// Token: 0x060070AD RID: 28845 RVA: 0x002AA7DC File Offset: 0x002A89DC
	public void SetSaturation(bool saturated)
	{
		this.ImageObject.GetComponent<Image>().material = (saturated ? this.DefaultMaterial : this.DesatMaterial);
	}

	// Token: 0x060070AE RID: 28846 RVA: 0x002AA800 File Offset: 0x002A8A00
	public void SetPortrait(GameObject selectedTarget)
	{
		MinionIdentity component = selectedTarget.GetComponent<MinionIdentity>();
		if (component != null)
		{
			this.SetPortrait(component);
			return;
		}
		Building component2 = selectedTarget.GetComponent<Building>();
		if (component2 != null)
		{
			this.SetPortrait(component2.Def.GetUISprite("ui", false));
			return;
		}
		MeshRenderer componentInChildren = selectedTarget.GetComponentInChildren<MeshRenderer>();
		if (componentInChildren)
		{
			this.SetPortrait(Sprite.Create((Texture2D)componentInChildren.material.mainTexture, new Rect(0f, 0f, (float)componentInChildren.material.mainTexture.width, (float)componentInChildren.material.mainTexture.height), new Vector2(0.5f, 0.5f)));
		}
	}

	// Token: 0x060070AF RID: 28847 RVA: 0x002AA8B8 File Offset: 0x002A8AB8
	public void SetPortrait(Sprite image)
	{
		if (this.PortraitShadow)
		{
			this.PortraitShadow.SetActive(true);
		}
		if (this.FaceObject)
		{
			this.FaceObject.SetActive(false);
		}
		if (this.ImageObject)
		{
			this.ImageObject.SetActive(true);
		}
		if (this.AnimControllerObject)
		{
			this.AnimControllerObject.SetActive(false);
		}
		if (image == null)
		{
			this.ClearPortrait();
			return;
		}
		this.ImageObject.GetComponent<Image>().sprite = image;
	}

	// Token: 0x060070B0 RID: 28848 RVA: 0x002AA94C File Offset: 0x002A8B4C
	private void SetPortrait(MinionIdentity identity)
	{
		if (this.PortraitShadow)
		{
			this.PortraitShadow.SetActive(true);
		}
		if (this.FaceObject)
		{
			this.FaceObject.SetActive(false);
		}
		if (this.ImageObject)
		{
			this.ImageObject.SetActive(false);
		}
		CrewPortrait component = base.GetComponent<CrewPortrait>();
		if (component != null)
		{
			component.SetIdentityObject(identity, true);
			return;
		}
		if (this.AnimControllerObject)
		{
			this.AnimControllerObject.SetActive(true);
			CrewPortrait.SetPortraitData(identity, this.AnimControllerObject.GetComponent<KBatchedAnimController>(), true);
		}
	}

	// Token: 0x060070B1 RID: 28849 RVA: 0x002AA9E8 File Offset: 0x002A8BE8
	public void ClearPortrait()
	{
		if (this.PortraitShadow)
		{
			this.PortraitShadow.SetActive(false);
		}
		if (this.FaceObject)
		{
			this.FaceObject.SetActive(false);
		}
		if (this.ImageObject)
		{
			this.ImageObject.SetActive(false);
		}
		if (this.AnimControllerObject)
		{
			this.AnimControllerObject.SetActive(false);
		}
	}

	// Token: 0x04004D77 RID: 19831
	public GameObject FaceObject;

	// Token: 0x04004D78 RID: 19832
	public GameObject ImageObject;

	// Token: 0x04004D79 RID: 19833
	public GameObject PortraitShadow;

	// Token: 0x04004D7A RID: 19834
	public GameObject AnimControllerObject;

	// Token: 0x04004D7B RID: 19835
	public Material DefaultMaterial;

	// Token: 0x04004D7C RID: 19836
	public Material DesatMaterial;
}
