using System;
using System.Collections.Generic;
using UnityEngine;

namespace KMod
{
	// Token: 0x02000EE6 RID: 3814
	public class ModErrorsScreen : KScreen
	{
		// Token: 0x060076F0 RID: 30448 RVA: 0x002EC340 File Offset: 0x002EA540
		public static bool ShowErrors(List<Event> events)
		{
			if (Global.Instance.modManager.events.Count == 0)
			{
				return false;
			}
			GameObject parent = GameObject.Find("Canvas");
			ModErrorsScreen modErrorsScreen = Util.KInstantiateUI<ModErrorsScreen>(Global.Instance.modErrorsPrefab, parent, false);
			modErrorsScreen.Initialize(events);
			modErrorsScreen.gameObject.SetActive(true);
			return true;
		}

		// Token: 0x060076F1 RID: 30449 RVA: 0x002EC394 File Offset: 0x002EA594
		private void Initialize(List<Event> events)
		{
			foreach (Event @event in events)
			{
				HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.entryPrefab, this.entryParent.gameObject, true);
				LocText reference = hierarchyReferences.GetReference<LocText>("Title");
				LocText reference2 = hierarchyReferences.GetReference<LocText>("Description");
				KButton reference3 = hierarchyReferences.GetReference<KButton>("Details");
				string text;
				string toolTip;
				Event.GetUIStrings(@event.event_type, out text, out toolTip);
				reference.text = text;
				reference.GetComponent<ToolTip>().toolTip = toolTip;
				reference2.text = @event.mod.title;
				ToolTip component = reference2.GetComponent<ToolTip>();
				if (component != null)
				{
					ToolTip toolTip2 = component;
					Label mod = @event.mod;
					toolTip2.toolTip = mod.ToString();
				}
				reference3.isInteractable = false;
				Mod mod2 = Global.Instance.modManager.FindMod(@event.mod);
				if (mod2 != null)
				{
					if (component != null && !string.IsNullOrEmpty(mod2.description))
					{
						component.toolTip = mod2.description;
					}
					if (mod2.on_managed != null)
					{
						reference3.onClick += mod2.on_managed;
						reference3.isInteractable = true;
					}
				}
			}
		}

		// Token: 0x060076F2 RID: 30450 RVA: 0x002EC4F4 File Offset: 0x002EA6F4
		protected override void OnActivate()
		{
			base.OnActivate();
			this.closeButtonTitle.onClick += this.Deactivate;
			this.closeButton.onClick += this.Deactivate;
		}

		// Token: 0x040056B0 RID: 22192
		[SerializeField]
		private KButton closeButtonTitle;

		// Token: 0x040056B1 RID: 22193
		[SerializeField]
		private KButton closeButton;

		// Token: 0x040056B2 RID: 22194
		[SerializeField]
		private GameObject entryPrefab;

		// Token: 0x040056B3 RID: 22195
		[SerializeField]
		private Transform entryParent;
	}
}
