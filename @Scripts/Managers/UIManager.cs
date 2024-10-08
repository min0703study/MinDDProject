
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : BaseManager<UIManager>
{
	private Stack<MonoBehaviour> popupStack = new Stack<MonoBehaviour>();
	private UI_CoreLayerBase coreLayer;

	int order = 10;

	public GameObject Root
	{
		get
		{
			GameObject root = GameObject.Find("UI_Root");
			if (root == null)
				root = new GameObject { name = "UI_Root" };
			return root;
		}
	}


	protected override void initAwake()
	{
		base.initAwake();
	}

	public T ShowPopupUI<T>(string name = null) where T : UI_PopupBase
	{
		if (popupStack.Count > 0 && popupStack.Peek() is T)
		{
			Debug.Log("이미 열려있는 팝업 입니다.");
			return null;
		}

		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = ResourceManager.Instance.Instantiate($"{name}");
		T popup = Util.GetOrAddComponent<T>(go);
		popupStack.Push(popup);

		go.transform.SetParent(Root.transform, false);

		return popup;
	}

	public T ShowCoreLayerUI<T>(string name = null) where T : UI_CoreLayerBase
	{
		Util.DestroyChilds(Root);

		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = ResourceManager.Instance.Instantiate($"{name}");
		T coreLayer = Util.GetOrAddComponent<T>(go);
		this.coreLayer = coreLayer;

		go.transform.SetParent(Root.transform, false);

		return coreLayer;
	}

	public void ClosePopupUI(UI_PopupBase popup)
	{
		if (popupStack.Count <= 0 || popupStack.Peek() != popup)
		{
			Debug.Log("Close Popup Failed!");
			return;
		}


		ResourceManager.Instance.Destroy(popup.gameObject);

		order--;
	}

	public void SetCanvas(GameObject go, bool sort = true, int sortOrder = 0)
	{
		Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
		if (canvas == null)
		{
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.overrideSorting = true;
		}

		CanvasScaler cs = Util.GetOrAddComponent<CanvasScaler>(go);
		if (cs != null)
		{
			cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			cs.referenceResolution = new Vector2(1080, 1920);
		}

		Util.GetOrAddComponent<GraphicRaycaster>(go);

		if (sort)
		{
			canvas.sortingOrder = order;
			order++;
		}
		else
		{
			canvas.sortingOrder = sortOrder;
		}

		//        if (isToast)
		//        {
		//            _toastOrder++;
		//            canvas.sortingOrder = _toastOrder;
		//        }

	}

	// 	public T Open<T>() where T : UI_Base
	// 	{
	// 		string key = typeof(T).Name + ".prefab";
	// 		//T ui = Manager.Resource.Instatiate(key, polling: true).GetOrAddComponent<T>();
	// 		T ui = null;
	// 		//_uiStack.Push(ui);

	// 		return ui;
	// 	}

	// 	public void Close()
	// 	{
	// 		//if (_uiStack.Count == 0)
	// 			return;

	// 		//UI_Base ui = _uiStack.Pop();
	// 		//SubsystemManager.Resourece.Destroy(ui.gameObject);
	// 	}

	public void CloseCoreUI(UI_CoreLayerBase coreLayer)
	{
		coreLayer.gameObject.SetActive(false);
		ResourceManager.Instance.Destroy(coreLayer.gameObject);
	}

	// 	public T OpenSceneUI<T>(string name = null) where T : UI_SceneBase
	// 	{
	// 		// 개발용으로 넣어논 맵 제거
	// 		Util.DestroyChilds(Root);

	// 		if (string.IsNullOrEmpty(name))
	// 			name = typeof(T).Name;

	// 		GameObject go = ResourceManager.Instance.Instantiate($"{name}");
	// 		T sceneUIComp = go.GetComponent<T>();
	// 		CurSceneUI = sceneUIComp;

	// 		go.transform.SetParent(Root.transform);

	// 		return sceneUIComp;
	// 	}

	// 	public T OpenJoystickUI<T>(string name = null) where T : UI_SceneBase
	// 	{		
	// 		if (string.IsNullOrEmpty(name))
	// 			name = typeof(T).Name;

	// 		GameObject go = ResourceManager.Instance.Instantiate($"{name}");
	// 		T sceneUIComp = go.GetComponent<T>();

	// 		go.transform.SetParent(Root.transform);

	// 		return sceneUIComp;
	// 	}

	public T MakeSubItem<T>(Transform parent = null, string name = null, bool pooling = true)
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = ResourceManager.Instance.Instantiate($"{name}", parent, pooling);
		go.transform.SetParent(parent, false);

		T subUIComp = go.GetComponent<T>();
		return subUIComp;
	}


	// 	public T MakeSceneUISubItem<T>(string name = null, bool pooling = true) where T : UI_Base
	// 	{
	// 		if (string.IsNullOrEmpty(name))
	// 			name = typeof(T).Name;

	// 		GameObject go = ResourceManager.Instance.Instantiate($"{name}", null , pooling);
	// 		go.transform.SetParent(CurSceneUI.transform, false);

	// 		T subUIComp = go.GetComponent<T>();
	// 		return subUIComp;
	// 	}


	// 	public T MakeWorldSpaceUI<T>(string name = null, bool pooling = false) where T : UI_Base
	// 	{
	// 		if (string.IsNullOrEmpty(name))
	// 			name = typeof(T).Name;

	// 		GameObject go = ResourceManager.Instance.Instantiate($"{name}", WorldSpaceRoot.transform, pooling);

	// 		T subUIComp = go.GetComponent<T>();
	// 		return subUIComp;
	// 	}

}
