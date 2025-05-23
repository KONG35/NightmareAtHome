using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterColController : MonoBehaviour
{
    private PlayerCharacter player;

    private bool isRun;
    private Coroutine cor;
    private void Start()
    {
        isRun = true;
        player = PlayerCharacter.Instance;
    }
    private void OnEnable()
    {
        cor = StartCoroutine(TimerCor());
    }
    private void OnDisable()
    {
        StopCoroutine("TimerCor");
        cor = null;
    }
    private void OnCollisionStay(Collision col)
    {
        if (col.transform.tag == "Player")
        {
            if(isRun)
            {
                player.CurHp -= 10f;
                // ! 수정하기
                GameObject go = Instantiate(SpawnManager.Instance.bloodParticle.gameObject, player.transform.position, Quaternion.identity);
                go.gameObject.SetActive(true);
                isRun = false;
            }
        }
    }
    IEnumerator TimerCor()
    {
        yield return null;

        while (true)
        {
            if(!isRun)
                yield return new WaitForSeconds(1f);

            isRun = true;

            yield return null;
        }
    }
}
