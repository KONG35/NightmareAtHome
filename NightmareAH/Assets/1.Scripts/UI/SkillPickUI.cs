using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class SkillPickUI : MonoBehaviour
{
    public SkillPickItemUI[] Items;
    public void SetItems()
    {
        //var weaponlist = GoogleSheetLoader.Instance.GetDataList<WeaponData>();
        var weaponlist = DataTableManager.Instance.AbilitySOList;
        if (weaponlist.Count == 0)
            return;
        for (int i=0;i<Items.Length;i++)
        {
            int index = Random.Range(0,weaponlist.Count);
            if (weaponlist[index] == null)
            {
                i--;
                continue;
            }
            Items[i].InitItem(weaponlist[index].Icon, "", PlayerCharacter.Instance.GetWeaponLv(weaponlist[index]), weaponlist[index].LevelState.Count,
                delegate {
                    //PlayerCharacter.Instance.AddWeapon(weaponlist[index].Weapon); 
                    PlayerCharacter.Instance.AddWeapon(weaponlist[index]);
                    this.gameObject.SetActive(false); PlayerCharacter.Instance.LvUp();
                });
        }
    }
}
