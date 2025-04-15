using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPickItemUI : MonoBehaviour
{
    public Button btn;
    public Image SkillImage;
    public TextMeshProUGUI ExText;
    public Image Star;
    public List<Image> LvImage;


    public void InitItem(Sprite SkillImage, string Ex,int CurLv,int MaxLv,Action btnAction)
    {

        this.SkillImage.sprite = SkillImage;
        ExText.text = Ex;

        foreach (var star in LvImage)
            star.gameObject.SetActive(false);
        while (LvImage.Count<MaxLv)
        {
            LvImage.Add(Instantiate(Star, Star.transform.parent));
        }

        foreach (var star in LvImage)
        {
            star.color = Color.black;
            star.gameObject.SetActive(true);
        }
        for(int i=0;i<CurLv;i++)
        {
            LvImage[i].color = Color.white;
        }

        if (btn == null)
            btn = this.GetComponent<Button>();
        btn.onClick.AddListener(()=>btnAction.Invoke());

        StartCoroutine(StarAnim(LvImage[CurLv]));
    }

    IEnumerator StarAnim(Image Cur)
    {
        float time = 0.0f;
        while(true)
        {
            time += Time.deltaTime;
            Mathf.PingPong(time, 1f);
            Cur.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(time, 1f));
            yield return new WaitForEndOfFrame();

        }
    }

    [Button]
    public void TestButton()
    {
        InitItem(null, "테스트 인데용", 0, 5 , null);
    }
}
