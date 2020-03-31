using System;
using UnityEngine;

namespace CustomUIComponents.Tabs
{
    public class PageGroup : MonoBehaviour
    {
        public GameObject[] pages;

        public void ShowPage(int index)
        {
            for (var i = 0; i < pages.Length; i++)
                pages[i].SetActive(i == index);
        }
    }
}
