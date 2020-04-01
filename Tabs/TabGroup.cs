using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CustomUIComponents.Tabs
{
	public class TabGroup : MonoBehaviour
	{
		[HideInInspector] public List<TabButton> tabButtons = new List<TabButton>();

		[HideInInspector] public Sprite tabIdleSprite, tabHoverSprite, tabActiveSprite;
		[HideInInspector] public Color tabIdleColor, tabHoverColor, tabActiveColor = Color.white;

		public bool allowSelectionCirculation = true;

		[Tooltip("Starting / Current Tab")] public TabButton selectedTab;
		public PageGroup pageGroup;

		public UnityEvent<TabButton> onTabSelected;
		public UnityEvent<TabButton> onTabDeselected;

		[Header("Dynamic Text Color")] public bool dynamicTextColor = true;
		public Color idleColor, hoverColor, activeColor = Color.black;

		[Header("Transition")] public TabTransitionType transitionType;

		public enum TabTransitionType
		{
			None,
			Color,
			Sprite
		}

		private void Start()
		{
			if (selectedTab != null)
				SelectTab(selectedTab);

			ResetTabs();
		}

		private void Update()
		{
			var activeIndex = selectedTab.transform.GetSiblingIndex();

			if (Input.GetKeyDown(KeyCode.Q) && (allowSelectionCirculation || selectedTab.transform.GetSiblingIndex() > 0))
			{
				activeIndex--;
				
				if (activeIndex < 0)
					activeIndex = transform.childCount - 1;
				
				var child = transform.GetChild(activeIndex);
				SelectTab(child.GetComponent<TabButton>());
			}
			else if (Input.GetKeyDown(KeyCode.E) && (allowSelectionCirculation || selectedTab.transform.GetSiblingIndex() < transform.childCount - 1))
			{
				activeIndex++;
				
				if (activeIndex > transform.childCount - 1)
					activeIndex = 0;
				
				var child = transform.GetChild(activeIndex);
				SelectTab(child.GetComponent<TabButton>());
			}
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

			if (dynamicTextColor)
				button.text.color = hoverColor;

			switch (transitionType)
			{
				case TabTransitionType.Color:
					button.background.color = tabHoverColor;
					break;
				case TabTransitionType.Sprite:
					button.background.sprite = tabHoverSprite;
					break;
				case TabTransitionType.None:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void OnTabExit(TabButton button)
		{
			ResetTabs();
		}

		public void SelectTab(TabButton button)
		{
			if (selectedTab != null)
				selectedTab.Deselect();

			selectedTab = button;
			selectedTab.Select();
			ResetTabs();

			if (dynamicTextColor)
				button.text.color = activeColor;

			switch (transitionType)
			{
				case TabTransitionType.Color:
					button.background.color = tabActiveColor;
					break;
				case TabTransitionType.Sprite:
					button.background.sprite = tabActiveSprite;
					break;
				case TabTransitionType.None:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			var index = button.transform.GetSiblingIndex();

			for (var i = 0; i < pageGroup.pages.Length; i++)
				if (i == index)
					pageGroup.ShowPage(i);
		}

		private void ResetTabs()
		{
			if (transitionType == TabTransitionType.None)
				return;

			foreach (var button in tabButtons.Where(button => button != selectedTab))
			{
				if (dynamicTextColor)
					button.text.color = idleColor;

				switch (transitionType)
				{
					case TabTransitionType.Color:
						button.background.color = tabIdleColor;
						break;
					case TabTransitionType.Sprite:
						button.background.sprite = tabIdleSprite;
						break;
					case TabTransitionType.None:
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

			switch (myScript.transitionType)
			{
				case TabGroup.TabTransitionType.Color:
					myScript.tabIdleColor = EditorGUILayout.ColorField("Idle", myScript.tabIdleColor);
					myScript.tabHoverColor = EditorGUILayout.ColorField("Hover", myScript.tabHoverColor);
					myScript.tabActiveColor = EditorGUILayout.ColorField("Active", myScript.tabActiveColor);
					break;
				case TabGroup.TabTransitionType.Sprite:
					myScript.tabIdleSprite = (Sprite) EditorGUILayout.ObjectField("Idle", myScript.tabIdleSprite, typeof(Sprite), false);
					myScript.tabHoverSprite = (Sprite) EditorGUILayout.ObjectField("Hover", myScript.tabHoverSprite, typeof(Sprite), false);
					myScript.tabActiveSprite = (Sprite) EditorGUILayout.ObjectField("Active", myScript.tabActiveSprite, typeof(Sprite), false);
					break;
				case TabGroup.TabTransitionType.None:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}