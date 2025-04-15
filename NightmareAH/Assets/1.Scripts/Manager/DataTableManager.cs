using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTableManager : Singleton<DataTableManager>
{
    public List<WeaponIconData> weaponIconList;
    public List<ProjectlieData> ProjectlieList;


    [System.Serializable]
    public struct WeaponIconData
    {
        public string name;
        public Sprite Icon;
    }

    [System.Serializable]
    public struct ProjectlieData
    {
        public string name;
        public ProjectlieObject Obj;
    }
}

