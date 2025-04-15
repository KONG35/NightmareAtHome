using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class SkillPickUI : MonoBehaviour
{
    public SkillPickItemUI[] Items;

    public void SetItems()
    {
        var weaponlist = GoogleSheetLoader.Instance.GetDataList<WeaponData>();
        for(int i=0;i<Items.Length;i++)
        {
            int index = Random.Range(0,weaponlist.Count);
            if (weaponlist[index].Weapon == null)
            {
                i--;
                continue;
            }
            Items[i].InitItem(weaponlist[index].Weapon.Icon.Icon, "", weaponlist[index].Weapon.lv, weaponlist[index].Weapon.MaxLv,
                delegate { PlayerCharacter.Instance.AddWeapon(weaponlist[index].Weapon); this.gameObject.SetActive(false); });
        }
    }
}
