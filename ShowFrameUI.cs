using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;

public class ShowFrameuI : MonoBehaviour//按tab键显示/隐藏帧率窗口
{
    
    public bool hide = true;
    public RectTransform infPanel;
    public RectTransform choosePanel;
    public float moveSpeed = 0.1f;
    Coroutine slider;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (slider != null) return;//还在切换时不会被打断
            if (hide) hide = false;
            else hide = true;

            slider = StartCoroutine(Show());
        }

    }
    IEnumerator Show()
    {
        if (hide)
            while (infPanel.anchoredPosition.x < 590f&&choosePanel.anchoredPosition.x < 1780f)
            {
                infPanel.anchoredPosition = Vector2.Lerp(infPanel.anchoredPosition, new Vector2(600, 0), moveSpeed);
                choosePanel.anchoredPosition = Vector2.Lerp(choosePanel.anchoredPosition, new Vector2(1800, 0), moveSpeed);
                yield return null;
            }
            
        else
        {
            while (infPanel.anchoredPosition.x > 10f&&choosePanel.anchoredPosition.x > -10f)
            {
                infPanel.anchoredPosition = Vector2.Lerp(infPanel.anchoredPosition, new Vector2(0, 0), moveSpeed);
                choosePanel.anchoredPosition = Vector2.Lerp(choosePanel.anchoredPosition, new Vector2(-20, 0), moveSpeed);
                yield return null;
            }
        }
        slider = null;
    }
}
