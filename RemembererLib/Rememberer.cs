using System;
using UnityEngine;
using KSP.UI.Screens; // has EditorPartList

namespace Rememberer
{

    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class RememEditor : MonoBehaviour
    {
        private bool sortAsc = true;
        private int sortIndex = 1;
        private ConfigNode nodeFile;

        // capture part list sort method change
        private void SortCB(int index, bool asc)
        {
            sortIndex = index;
            sortAsc = asc;
        }

        public void Start()
        {
            Debug.Log("RememEditor - Start");

            // Imports initial sort settings from config file into a default "root" Config Node
            nodeFile = ConfigNode.Load(KSPUtil.ApplicationRootPath + "GameData/Rememberer/RememEditor.cfg");
            sortAsc = Convert.ToBoolean(nodeFile.GetValue("partListSortAsc"));  // true: ascending, false: descending
            sortIndex = Convert.ToInt32(nodeFile.GetValue("partListSortIndex"));  // 0: mame, 1: mass, 2: cost, 3: size

            // set initial sort method
            EditorPartList.Instance.partListSorter.ClickButton(sortIndex);
            if (!sortAsc)
            {
                EditorPartList.Instance.partListSorter.ClickButton(sortIndex);
            }

            //Track the user's sort changes
            EditorPartList.Instance.partListSorter.AddOnSortCallback(SortCB);
        }

        public void OnDisable()
        {
            Debug.Log("RememEditor - Disable"); 
            nodeFile.SetValue("partListSortAsc", sortAsc.ToString());
            nodeFile.SetValue("partListSortIndex", sortIndex.ToString());
            nodeFile.Save(KSPUtil.ApplicationRootPath + "GameData/Rememberer/RememEditor.cfg");
            //EditorPartList is already disabled when OnDisable is called so remove callback gives NRE
            //EditorPartList.Instance.partListSorter.RemoveOnSortCallback(SortCB);
        }
    }

}
