using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkingController : MonoBehaviour
{
    public GameObject SpeechBubble;
    private GameObject bubbleObj;
    public Vector3 offset = new Vector3(0, 0, 0);
    private Text textObj;
    public TextAsset textAsset;
    private bool isTalking = false;
    private bool isDestroied = false;

    private string[] textAll;
    private float waitingTime = 0f;
    public float dialogSpeed = 2.5f;

    int index;
    // Start is called before the first frame update
    public void startTalk(GameObject talker)
    {
        if (!isTalking)
        {
            bubbleObj = Instantiate(SpeechBubble, transform.position + offset, Quaternion.identity,talker.transform);
            textObj = bubbleObj.GetComponentInChildren<Text>();
            getText();
            isTalking = true;
            Invoke("showDialogue", waitingTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    void getText()
    {
        index = 0;
        //初始化文本资源里面的对话内容
        textAll = textAsset.text.Split('\n');
    }
    void showDialogue()
    {
        if (index < textAll.Length)
        {
            textObj.text = textAll[index];
            ContentSizeFitter fitter = bubbleObj.GetComponentInChildren<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            Canvas.ForceUpdateCanvases();
            fitter.horizontalFit = ContentSizeFitter.FitMode.MinSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
            Canvas.ForceUpdateCanvases();
            waitingTime = textAll[index].Length / dialogSpeed;
            index++;
            Invoke("showDialogue", waitingTime);
        }
        else
        {
            Invoke("destroy", waitingTime);
        }
    }
    void destroy()
    {
        Destroy(bubbleObj);
        isDestroied = true;
    }
}
