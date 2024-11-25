using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009CD RID: 2509
[AddComponentMenu("KMonoBehaviour/scripts/OffscreenIndicator")]
public class OffscreenIndicator : KMonoBehaviour
{
	// Token: 0x060048F0 RID: 18672 RVA: 0x001A0D3B File Offset: 0x0019EF3B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		OffscreenIndicator.Instance = this;
	}

	// Token: 0x060048F1 RID: 18673 RVA: 0x001A0D49 File Offset: 0x0019EF49
	protected override void OnForcedCleanUp()
	{
		OffscreenIndicator.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060048F2 RID: 18674 RVA: 0x001A0D58 File Offset: 0x0019EF58
	private void Update()
	{
		foreach (KeyValuePair<GameObject, GameObject> keyValuePair in this.targets)
		{
			this.UpdateArrow(keyValuePair.Value, keyValuePair.Key);
		}
	}

	// Token: 0x060048F3 RID: 18675 RVA: 0x001A0DB8 File Offset: 0x0019EFB8
	public void ActivateIndicator(GameObject target)
	{
		if (!this.targets.ContainsKey(target))
		{
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(target, "ui", false);
			if (uisprite != null)
			{
				this.ActivateIndicator(target, uisprite);
			}
		}
	}

	// Token: 0x060048F4 RID: 18676 RVA: 0x001A0DEC File Offset: 0x0019EFEC
	public void ActivateIndicator(GameObject target, GameObject iconSource)
	{
		if (!this.targets.ContainsKey(target))
		{
			MinionIdentity component = iconSource.GetComponent<MinionIdentity>();
			if (component != null)
			{
				GameObject gameObject = Util.KInstantiateUI(this.IndicatorPrefab, this.IndicatorContainer, true);
				gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("icon").gameObject.SetActive(false);
				CrewPortrait reference = gameObject.GetComponent<HierarchyReferences>().GetReference<CrewPortrait>("Portrait");
				reference.gameObject.SetActive(true);
				reference.SetIdentityObject(component, true);
				this.targets.Add(target, gameObject);
			}
		}
	}

	// Token: 0x060048F5 RID: 18677 RVA: 0x001A0E78 File Offset: 0x0019F078
	public void ActivateIndicator(GameObject target, global::Tuple<Sprite, Color> icon)
	{
		if (!this.targets.ContainsKey(target))
		{
			GameObject gameObject = Util.KInstantiateUI(this.IndicatorPrefab, this.IndicatorContainer, true);
			Image reference = gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("icon");
			if (icon != null)
			{
				reference.sprite = icon.first;
				reference.color = icon.second;
				this.targets.Add(target, gameObject);
			}
		}
	}

	// Token: 0x060048F6 RID: 18678 RVA: 0x001A0EDF File Offset: 0x0019F0DF
	public void DeactivateIndicator(GameObject target)
	{
		if (this.targets.ContainsKey(target))
		{
			UnityEngine.Object.Destroy(this.targets[target]);
			this.targets.Remove(target);
		}
	}

	// Token: 0x060048F7 RID: 18679 RVA: 0x001A0F10 File Offset: 0x0019F110
	private void UpdateArrow(GameObject arrow, GameObject target)
	{
		if (target == null)
		{
			UnityEngine.Object.Destroy(arrow);
			this.targets.Remove(target);
			return;
		}
		Vector3 vector = Camera.main.WorldToViewportPoint(target.transform.position);
		if ((double)vector.x > 0.3 && (double)vector.x < 0.7 && (double)vector.y > 0.3 && (double)vector.y < 0.7)
		{
			arrow.GetComponent<HierarchyReferences>().GetReference<CrewPortrait>("Portrait").SetIdentityObject(null, true);
			arrow.SetActive(false);
			return;
		}
		arrow.SetActive(true);
		arrow.rectTransform().SetLocalPosition(Vector3.zero);
		Vector3 b = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
		b.z = target.transform.position.z;
		Vector3 normalized = (target.transform.position - b).normalized;
		arrow.transform.up = normalized;
		this.UpdateTargetIconPosition(target, arrow);
	}

	// Token: 0x060048F8 RID: 18680 RVA: 0x001A1034 File Offset: 0x0019F234
	private void UpdateTargetIconPosition(GameObject goTarget, GameObject indicator)
	{
		Vector3 vector = goTarget.transform.position;
		vector = Camera.main.WorldToViewportPoint(vector);
		if (vector.z < 0f)
		{
			vector.x = 1f - vector.x;
			vector.y = 1f - vector.y;
			vector.z = 0f;
			vector = this.Vector3Maxamize(vector);
		}
		vector = Camera.main.ViewportToScreenPoint(vector);
		vector.x = Mathf.Clamp(vector.x, this.edgeInset, (float)Screen.width - this.edgeInset);
		vector.y = Mathf.Clamp(vector.y, this.edgeInset, (float)Screen.height - this.edgeInset);
		indicator.transform.position = vector;
		indicator.GetComponent<HierarchyReferences>().GetReference<Image>("icon").rectTransform.up = Vector3.up;
		indicator.GetComponent<HierarchyReferences>().GetReference<CrewPortrait>("Portrait").transform.up = Vector3.up;
	}

	// Token: 0x060048F9 RID: 18681 RVA: 0x001A1140 File Offset: 0x0019F340
	public Vector3 Vector3Maxamize(Vector3 vector)
	{
		float num = 0f;
		num = ((vector.x > num) ? vector.x : num);
		num = ((vector.y > num) ? vector.y : num);
		num = ((vector.z > num) ? vector.z : num);
		return vector / num;
	}

	// Token: 0x04002FB6 RID: 12214
	public GameObject IndicatorPrefab;

	// Token: 0x04002FB7 RID: 12215
	public GameObject IndicatorContainer;

	// Token: 0x04002FB8 RID: 12216
	private Dictionary<GameObject, GameObject> targets = new Dictionary<GameObject, GameObject>();

	// Token: 0x04002FB9 RID: 12217
	public static OffscreenIndicator Instance;

	// Token: 0x04002FBA RID: 12218
	[SerializeField]
	private float edgeInset = 25f;
}
