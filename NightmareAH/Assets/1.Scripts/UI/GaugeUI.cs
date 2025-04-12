using UnityEngine;
using UnityEngine.UI;

public class GaugeUI : MonoBehaviour
{
    public Image Front;

    public void SetBar(float Cur, float Max)
    {
        if (Cur == 0)
        {
            Front.fillAmount = 0;
            return;
        }
        if (Max == 0)
        {
            Front.fillAmount = 1;
            return;
        }
        Front.fillAmount = Cur / Max;
    }
}
