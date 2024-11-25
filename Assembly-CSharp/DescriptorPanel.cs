using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C31 RID: 3121
[AddComponentMenu("KMonoBehaviour/scripts/DescriptorPanel")]
public class DescriptorPanel : KMonoBehaviour
{
	// Token: 0x06005FC1 RID: 24513 RVA: 0x0023915F File Offset: 0x0023735F
	public bool HasDescriptors()
	{
		return this.labels.Count > 0;
	}

	// Token: 0x06005FC2 RID: 24514 RVA: 0x00239170 File Offset: 0x00237370
	public void SetDescriptors(IList<Descriptor> descriptors)
	{
		int i;
		for (i = 0; i < descriptors.Count; i++)
		{
			GameObject gameObject;
			if (i >= this.labels.Count)
			{
				gameObject = Util.KInstantiate((this.customLabelPrefab != null) ? this.customLabelPrefab : ScreenPrefabs.Instance.DescriptionLabel, base.gameObject, null);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				this.labels.Add(gameObject);
			}
			else
			{
				gameObject = this.labels[i];
			}
			gameObject.GetComponent<LocText>().text = descriptors[i].IndentedText();
			gameObject.GetComponent<ToolTip>().toolTip = descriptors[i].tooltipText;
			gameObject.SetActive(true);
		}
		while (i < this.labels.Count)
		{
			this.labels[i].SetActive(false);
			i++;
		}
	}

	// Token: 0x0400408B RID: 16523
	[SerializeField]
	private GameObject customLabelPrefab;

	// Token: 0x0400408C RID: 16524
	private List<GameObject> labels = new List<GameObject>();
}
