using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class ItemBase : MonoBehaviour , IPoolable
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
            StartCoroutine(Magnet(other));
        }
    }

    IEnumerator Magnet(Collider other)
    {
        while (true)
        {
            float distance = Vector3.Distance(other.transform.position, this.transform.position);
            if (distance < 0.5f)
            {
                Action(other.gameObject.GetComponent<PlayerCharacter>());
                SpawnManager.Instance.DespawnItem(this);
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(transform.position, other.transform.position, Time.deltaTime * distance);
            }
            yield return new WaitForEndOfFrame();
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
