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
