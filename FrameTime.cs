using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class FrameTime : MonoBehaviour//计算并显示帧率相关脚本
{
    public AddObjectManager addObjectManager=null;//添加物品管理器
    [Tooltip("每隔多少秒统计一次 FPS")]
    public float windowSeconds = 0.5f;
    public TMP_Text averageFrameTmp;
    public TMP_Text maxFrameTmp;
    public TMP_Text p95Tmp;
    public TMP_Text totalCountTmp;
    public TMP_Text activeCountTmp;    
    public Toggle addObjectTick;
    float elapsed;//累计时间，用于计算平均帧率
    float maxDeltaTime;//获取限定时间内最长的帧时间,如果有一帧时间过长，即使整体帧数正常也会有卡顿感
    float p95Time;
    List<float> deltaTimeList=new List<float>(2048);
    int index;
    string averageFrameText = "AverageFPS : --";
    string maxFrameText = "MaxFrameTime : --";
    string p95Text = "P95Time : --";
    private string totalCountText = "";
    private string activeCountText = "";
    void Update()
    {
        elapsed += Time.unscaledDeltaTime; // 不受 Time.timeScale 影响
        deltaTimeList.Add(Time.unscaledDeltaTime);
        index++;

        if (elapsed >= windowSeconds)
        {
            float fps = index / elapsed;
            deltaTimeList.Sort();
            maxDeltaTime = deltaTimeList[index - 1];
            p95Time = deltaTimeList[(int)((index-1) * 0.95f)];

            averageFrameText = $"AverageFPS : {fps:F1}";
            maxFrameText = $"MaxFrameTime : {maxDeltaTime * 1000:F1} ms";
            p95Text = $"P95Time : {p95Time * 1000:F1} ms";
            totalCountText = $"ToTal Balls : {addObjectManager.totalCount}";
            activeCountText = $"Active Balls : {addObjectManager.activeCount}";
            deltaTimeList.Clear();


            // 重置窗口
            elapsed = 0f;
            index = 0;
            maxDeltaTime = 0f;
        }

        if (averageFrameTmp) averageFrameTmp.text = averageFrameText;
        if (maxFrameTmp) maxFrameTmp.text = maxFrameText;
        if (p95Tmp) p95Tmp.text = p95Text;
        if (totalCountTmp) totalCountTmp.text = totalCountText;
        if (activeCountTmp) activeCountTmp.text = activeCountText;
        addObjectTick.isOn = addObjectManager.start;
    }
}
