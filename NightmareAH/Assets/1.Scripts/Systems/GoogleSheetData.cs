using System.Collections.Generic;
using UnityEngine;

// 공통 데이터 인터페이스
public interface IBaseSheetData
{
    void Parse(string[] values); // 각 데이터 클래스마다 구현
}
public class MeleeMonsterSheetData : IBaseSheetData
{
    public string Key { get; private set; }
    public MeleeMonster monster;

    public void Parse(string[] values)
    {
        //if (values.Length < 6) return;

        Key = values[0];
        monster = new MeleeMonster(values[0], values[1], MonsterAttackType.Melee, float.Parse(values[2]), float.Parse(values[3]), float.Parse(values[4]), float.Parse(values[5]), float.Parse(values[6]), int.Parse(values[7]));

    }
}
public class RangedMonsterSheetData : IBaseSheetData
{
    public string Key { get; private set; }
    public RangedMonster monster;

    public void Parse(string[] values)
    {
        //if (values.Length < 7) return;

        Key = values[0];
        monster = new RangedMonster(values[0], values[1], MonsterAttackType.Range, float.Parse(values[2]), float.Parse(values[3]), float.Parse(values[4]), float.Parse(values[5]),int.Parse(values[6]), float.Parse(values[7]), int.Parse(values[8]));

    }
}

public class SpawnSheetData : IBaseSheetData
{
    public string Key { get; private set; }
    public Spawn spawn;

    public void Parse(string[] values)
    {
        if (values.Length < 5) return;

        Key = values[0]; // 라운드 번호
        spawn = new Spawn(int.Parse(values[0]), values[1], float.Parse(values[2]), float.Parse(values[3]), float.Parse(values[4]), int.Parse(values[5]));

    }
}
public class WeaponData : IBaseSheetData
{
    public string key { get; private set; }
    public baseWeapon Weapon;
    /// <summary>
    /// 0 index     인덱스
    /// 1 type      무기타입
    /// 2 name      무기이름
    /// 3 damage    데미지
    /// 4 frequency 무기 사용빈도
    /// 5 minrenge  무기 발사,공격 가능 최소범위
    /// 6 maxrange  무기 발사,공격 가능 최대범위
    /// 7 speed     무기 투사체의 속도
    /// 8 pierceCount 관통 카운트
    /// 9 piercePer   관통시 데미지감소율
    /// 10 scale    무기 범위 크기
    /// </summary>
    /// <param name="values"></param>
    public void Parse(string[] values)
    {
        switch((eWeaponIndex)int.Parse(values[0]))
        {
            case eWeaponIndex.Flapper:
                Weapon = new MeleeWeapon(float.Parse(values[6]), float.Parse(values[3]), 
                    float.Parse(values[3]), float.Parse(values[10]), int.Parse(values[12]),DataTableManager.Instance.weaponIconList.Find(x => x.name == values[11]));

                break;
            case eWeaponIndex.Kitchenknife:
                Weapon = new KitchenKnifeWeapon(DataTableManager.Instance.ProjectlieList.Find(x=>x.name=="Kitchenknife").Obj,float.Parse(values[6]), float.Parse(values[5]), 
                    float.Parse(values[7]), int.Parse(values[8]), float.Parse(values[9]), float.Parse(values[3]), float.Parse(values[4]), int.Parse(values[12]), DataTableManager.Instance.weaponIconList.Find(x => x.name == values[11]));
                break;
            case eWeaponIndex.Can:
                break;
        }
    }


    enum eWeaponIndex
    {
        Flapper,
        Kitchenknife,
        Can,
    }
}
public enum LanguageType
{
    English=0,
    Korean
}
 //언어 데이터
public class UITextData : IBaseSheetData
{
    public int Key { get; private set; }
    public Dictionary<LanguageType, string> TextDict { get; private set; } = new Dictionary<LanguageType, string>();

    public void Parse(string[] values)
    {
        if (values.Length < 3) return;

        Key = int.Parse(values[0]);
        TextDict[LanguageType.English] = values[1].Trim();
        TextDict[LanguageType.Korean] = values[2].Trim();
    }

    public string GetText(LanguageType lang)
    {
        return TextDict.ContainsKey(lang) ? TextDict[lang] : TextDict[LanguageType.English];
    }
}
