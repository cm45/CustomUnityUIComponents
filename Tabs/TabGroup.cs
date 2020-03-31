using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UI
{
	public class TabGroup : MonoBehaviour
	{
		public List<TabButton> tabButtons = new List<TabButton>();

		[HideInInspector] public Sprite tabIdleSprite, tabHoverSprite, tabActiveSprite;
		[HideInInspector] public Color tabIdleColor, tabHoverColor, tabActiveColor;

		public TabAnimationType animationType;

		public TabButton selectedTab;

		public enum TabAnimationType
		{
			None,
			Color,
			Sprite
		}

		private void Start()
		{
			if (selectedTab)
				OnTabSelected(selectedTab);

			ResetTabs();
		}

		public void Subscribe(TabButton button)
		{
			tabButtons.Add(button);
		}

		public void OnTabEnter(TabButton button)
		{
			ResetTabs();

			if (selectedTab == button)
				return;

			switch (animationType)
			{
				case TabAnimationType.Color:
					button.background.color = tabHoverColor;
					break;
				case TabAnimationType.Sprite:
					button.background.sprite = tabHoverSprite;
					break;
				case TabAnimationType.None:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void OnTabExit(TabButton button)
		{
			ResetTabs();
		}

		public void OnTabSelected(TabButton button)
		{
			selectedTab = button;
			ResetTabs();

			switch (animationType)
			{
				case TabAnimationType.Color:
					button.background.color = tabActiveColor;
					break;
				case TabAnimationType.Sprite:
					button.background.sprite = tabActiveSprite;
					break;
				case TabAnimationType.None:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void ResetTabs()
		{
			if (animationType == TabAnimationType.None)
				return;

			foreach (var button in tabButtons.Where(button => button != selectedTab))
			{
				switch (animationType)
				{
					case TabAnimationType.Color:
						button.background.color = tabIdleColor;
						break;
					case TabAnimationType.Sprite:
						button.background.sprite = tabIdleSprite;
						break;
					case TabAnimationType.None:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}

	[CustomEditor(typeof(TabGroup))]
	public class MyScriptEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var myScript = (TabGroup) target;

			switch (myScript.animationType)
			{
				case TabGroup.TabAnimationType.Color:
					myScript.tabIdleColor = EditorGUILayout.ColorField("Idle", myScript.tabIdleColor);
					myScript.tabHoverColor = EditorGUILayout.ColorField("Hover", myScript.tabHoverColor);
					myScript.tabActiveColor = EditorGUILayout.ColorField("Active", myScript.tabActiveColor);
					break;
				case TabGroup.TabAnimationType.Sprite:
					myScript.tabIdleSprite = (Sprite) EditorGUILayout.ObjectField("Idle", myScript.tabIdleSprite, typeof(Sprite), false);
					myScript.tabHoverSprite = (Sprite) EditorGUILayout.ObjectField("Hover", myScript.tabHoverSprite, typeof(Sprite), false);
					myScript.tabActiveSprite = (Sprite) EditorGUILayout.ObjectField("Active", myScript.tabActiveSprite, typeof(Sprite), false);
					break;
				case TabGroup.TabAnimationType.None:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}