using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    public TextMeshProUGUI killCountText;
    public Transform ItemPanel;
    public ResultItemUI ItemPrefab;
    public List<ResultItemUI> ItemList;
    public List<Sprite> Icon;
    public void Awake()
    {
        ItemList = new List<ResultItemUI>();
    }
    public void SetKillCount(string count)
    {
        killCountText.text = count;
    }

    public void AddItem(eResultItemIcon IconEnum,string count)
    {
        AddItem(Icon[(int)IconEnum], count);
    }
    public void AddItem(Sprite Icon,string count)
    {
        var item = Instantiate(ItemPrefab, ItemPanel);
        item.SetItem(Icon, count);
        ItemList.Add(item);
    }
    public void RemoveItemList()
    {
        foreach (var item in ItemList)
        {
            Destroy(item);
        }
        ItemList.Clear();
    }
}
public enum eResultItemIcon
{
    COIN,
    EXP,

}