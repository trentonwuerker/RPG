using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAffordance : MonoBehaviour {

	[SerializeField] Texture2D walkCursor = null;
	[SerializeField] Texture2D targetCursor = null;
	[SerializeField] Texture2D unknownCursor = null;
	[SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

	CameraRaycaster cameraRaycaster;

	// Use this for initialization
	void Start () {
		cameraRaycaster = GetComponent<CameraRaycaster> ();
		cameraRaycaster.notifyLayerChangeObservers += OnLayerChanged;
	}

	void OnLayerChanged (int newLayer) {
		switch (newLayer) {
			case 8:
				Cursor.SetCursor (walkCursor, cursorHotspot, CursorMode.ForceSoftware);
				break;
			case 9:
				Cursor.SetCursor (targetCursor, cursorHotspot, CursorMode.ForceSoftware);
				break;
			default:
				Cursor.SetCursor (unknownCursor, cursorHotspot, CursorMode.ForceSoftware);
				return;
		}
		 
	}
}
