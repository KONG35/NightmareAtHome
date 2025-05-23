using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTableManager : Singleton<DataTableManager>
{
    public List<WeaponIconData> weaponIconList;
    public List<ProjectlieData> ProjectlieList;
    public List<MeleeEffData> MeleeEffList;
    public List<AttributeDefSO> attributeSOList;
    public List<AbilityDefSO> AbilitySOList;

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
    [System.Serializable]
    public struct MeleeEffData
    {
        public string name;
        public MeleeObject Obj;
    }

    public AttributeDefSO GetSO(eAttributeSo e)
    {
        if (attributeSOList.Count <= (int)e)
            return null;   
        return attributeSOList[(int)e];
    }
}

public enum eAttributeSo
{
    LV,
    HP,
    EXP,
    MoveSpeed,
    AttackDamage,
    AttackMultiplier,
    AttackRange,
    AttackSpeed,
    CriticalChance,
    CriticalMultiplier,
    PierceCount,
    BounceCount,
    ProjectileSpeed,
    WeaponSize,



    COUNT
}

