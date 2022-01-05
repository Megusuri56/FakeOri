using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkingController : MonoBehaviour
{
    public GameObject SpeechBubble;
    private GameObject BubbleObj;
    public Vector3 offset = new Vector3(0, 0, 0);
    private Text textObj;
    public TextAsset textAsset;
    private bool isTalking = false;
    private bool isDestroied = false;

    private string[] textAll;
    private float waitingTime = 0f;
    public float dialogSpeed = 2.5f;
    public float lineHeight = 1f;

    int index;
    // Start is called before the first frame update
    public void startTalk(GameObject talker)
    {
        if (!isTalking)
        {
            BubbleObj = Instantiate(SpeechBubble, transform.position + offset, Quaternion.identity,talker.transform);
            textObj = BubbleObj.GetComponentInChildren<Text>();
            getText();
            isTalking = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTalking && !isDestroied)
        {
            Invoke("showDialogue", waitingTime);
        }
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
            Vector2 size = BubbleObj.GetComponent<SpriteRenderer>().size;
            if (size.y < textAll[index].Length * lineHeight)
            {
                BubbleObj.GetComponent<SpriteRenderer>().size = new Vector2(textAll[index].Length * lineHeight, size.y);
                //BubbleObj.GetComponentInChildren<RectTransform>().size
            }
            textObj.text = textAll[index];
            waitingTime = textAll[index].Length / dialogSpeed;
            index++;
        }
        else
        {
            Invoke("destroy", waitingTime);
        }
    }
    void destroy()
    {
        Destroy(BubbleObj);
        isDestroied = true;
    }
}
