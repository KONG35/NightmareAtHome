using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour ,IPoolable
{
    public eItemType Type;
    void IPoolable.OnDespawn()
    {
        gameObject.SetActive(false);
    }

    void IPoolable.OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public abstract void Action(PlayerCharacter other);
    public abstract void SetValue(float v);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Action(other.gameObject.GetComponent<PlayerCharacter>());
        }
    }


    public enum eItemType
    {
        Exp,
        Hp,
        Gold,
        Count
    }
}
