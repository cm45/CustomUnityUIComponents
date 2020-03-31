using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
	public class GetName : MonoBehaviour
	{
		public Transform parent; // TODO: Why invisisble in inspector

		private void OnValidate()
		{
			parent = transform.parent;

			if (parent == null)
				return;

			GetComponent<TMPro.TextMeshProUGUI>().text = parent.name;
		}

		public void Refresh() => OnValidate();
	}

	[CustomEditor(typeof(GetName))]
	public class GetNameEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var myTarget = (GetName) target;

			if (GUILayout.Button("Refresh"))
				myTarget.Refresh();
		}
	}
}