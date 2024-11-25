using System;
using STRINGS;
using UnityEngine;

// Token: 0x020007E9 RID: 2025
[AddComponentMenu("KMonoBehaviour/scripts/CopyBuildingSettings")]
public class CopyBuildingSettings : KMonoBehaviour
{
	// Token: 0x060037F9 RID: 14329 RVA: 0x00131C9D File Offset: 0x0012FE9D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<CopyBuildingSettings>(493375141, CopyBuildingSettings.OnRefreshUserMenuDelegate);
	}

	// Token: 0x060037FA RID: 14330 RVA: 0x00131CB8 File Offset: 0x0012FEB8
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_mirror", UI.USERMENUACTIONS.COPY_BUILDING_SETTINGS.NAME, new System.Action(this.ActivateCopyTool), global::Action.BuildingUtility1, null, null, null, UI.USERMENUACTIONS.COPY_BUILDING_SETTINGS.TOOLTIP, true), 1f);
	}

	// Token: 0x060037FB RID: 14331 RVA: 0x00131D12 File Offset: 0x0012FF12
	private void ActivateCopyTool()
	{
		CopySettingsTool.Instance.SetSourceObject(base.gameObject);
		PlayerController.Instance.ActivateTool(CopySettingsTool.Instance);
	}

	// Token: 0x060037FC RID: 14332 RVA: 0x00131D34 File Offset: 0x0012FF34
	public static bool ApplyCopy(int targetCell, GameObject sourceGameObject)
	{
		ObjectLayer layer = ObjectLayer.Building;
		Building component = sourceGameObject.GetComponent<BuildingComplete>();
		if (component != null)
		{
			layer = component.Def.ObjectLayer;
		}
		GameObject gameObject = Grid.Objects[targetCell, (int)layer];
		if (gameObject == null)
		{
			return false;
		}
		if (gameObject == sourceGameObject)
		{
			return false;
		}
		KPrefabID component2 = sourceGameObject.GetComponent<KPrefabID>();
		if (component2 == null)
		{
			return false;
		}
		KPrefabID component3 = gameObject.GetComponent<KPrefabID>();
		if (component3 == null)
		{
			return false;
		}
		CopyBuildingSettings component4 = sourceGameObject.GetComponent<CopyBuildingSettings>();
		if (component4 == null)
		{
			return false;
		}
		CopyBuildingSettings component5 = gameObject.GetComponent<CopyBuildingSettings>();
		if (component5 == null)
		{
			return false;
		}
		if (component4.copyGroupTag != Tag.Invalid)
		{
			if (component4.copyGroupTag != component5.copyGroupTag)
			{
				return false;
			}
		}
		else if (component3.PrefabID() != component2.PrefabID())
		{
			return false;
		}
		component3.Trigger(-905833192, sourceGameObject);
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, UI.COPIED_SETTINGS, gameObject.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
		return true;
	}

	// Token: 0x040021B0 RID: 8624
	[MyCmpReq]
	private KPrefabID id;

	// Token: 0x040021B1 RID: 8625
	public Tag copyGroupTag;

	// Token: 0x040021B2 RID: 8626
	private static readonly EventSystem.IntraObjectHandler<CopyBuildingSettings> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<CopyBuildingSettings>(delegate(CopyBuildingSettings component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
