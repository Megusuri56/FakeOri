using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class TalkingController : MonoBehaviour
{
    private CreatureController mainChara;
    private GameObject talker;
    public GameObject SpeechBubble;
    public GameObject ChoiceBoxs;
    private GameObject bubbleObj;
    public Vector3 offset = Vector3.zero;
    private Text textObj;
    private Text innerTextObj;
    [SerializeField] private TextAsset[] textAssets;
    [SerializeField] private UnityEvent[] beforeEvents;
    [SerializeField] private UnityEvent[] afterEvents;
    private Hashtable textNameToIndex;
    private bool isTalking = false;
    private bool isDestroied = false;
    private bool isPaused = false;

    private TextAsset textAsset;
    private string[] textAll;
    private string[] choices;
    private int charCount;
    private float waitingTime = 1f;
    public float dialogSpeed = 6f;

    int index;
    // Start is called before the first frame update
    public void startTalk(GameObject someone)
    {
        if (!isTalking)
        {
            talker = someone;
            mainChara = GameObject.FindWithTag("Player").GetComponent<CreatureController>();
            mainChara.setCanMove(false);
            bubbleObj = Instantiate(SpeechBubble, transform.position + offset, Quaternion.identity,talker.transform);
            textObj = bubbleObj.GetComponentInChildren<Text>();
            innerTextObj = textObj.transform.GetChild(0).gameObject.GetComponent<Text>();
            textNameToIndex = new Hashtable();
            for(int i=0;i<textAssets.Length;i++)
            {
                textNameToIndex.Add(textAssets[i].name, i);
            }
            if (getText(0))
            {
                if (beforeEvents[0] != null)
                {
                    beforeEvents[0].Invoke();
                }
                isTalking = true;
                waitingTime = 1 / dialogSpeed;
                showDialogue();
            }
            else
            {
                destroy();
            }
            
        }
    }

    // Update is called once per frame
    void OnGUI()
    {
        Event e = Event.current;
        if (isPaused && e.isKey && Regex.IsMatch(e.character.ToString(), @"[1-9]"))
        {
            string assetName = textAsset.name + e.character.ToString();
            if (getText(assetName))
            {
                Destroy(bubbleObj);
                bubbleObj = Instantiate(SpeechBubble, transform.position + offset, Quaternion.identity, talker.transform);
                textObj = bubbleObj.GetComponentInChildren<Text>();
                innerTextObj = textObj.transform.GetChild(0).gameObject.GetComponent<Text>();
                showDialogue();
            }
            else
            {
                destroy();
            }
        }
    }
    void Update()
    {

    }
    bool getText(int assetIndex)
    {
        index = 0;
        //初始化文本资源里面的对话内容
        if (assetIndex >= textAssets.Length)
        {
            return false;
        }
        textAsset = textAssets[assetIndex];
        textAll = textAsset.text.Split('\n');
        if (textAll.Length==0 || textAll[0]=="")
        {
            return false;
        }
        isPaused = false;
        return true;
    }
    bool getText(string assetName)
    {
        index = 0;
        if (textNameToIndex.ContainsKey(assetName))
        {
            isPaused = false;
            textAsset = textAssets[(int)textNameToIndex[assetName]];
            textAll = textAsset.text.Split('\n');
            if ((int)textNameToIndex[assetName] < beforeEvents.Length && beforeEvents[(int)textNameToIndex[assetName]] != null)
            {
                beforeEvents[(int)textNameToIndex[assetName]].Invoke();
            }
            if (textAll.Length == 0 || textAll[0] == "")
            {
                return false;
            }
            return true;
        }
        return false;
    }
    void showDialogue()
    {
        if (index < textAll.Length)
        {
            if(textAll[index][0] == '$')
            {
                char[] sptor = { '$' };
                choices = textAll[index].Remove(0,1).Split(sptor,9);
                Transform bubbleTrans = bubbleObj.transform.GetChild(0);
                for (int i=1;i<=choices.Length;i++)
                {
                    GameObject choiceBox = Instantiate(ChoiceBoxs, bubbleTrans.position, Quaternion.identity, bubbleTrans);
                    choiceBox.GetComponentInChildren<Text>().text = "按下" + i + "键:" + choices[i - 1];
                    isPaused = true;
                }
            }
            else
            {
                charCount = 0;
                textObj.text = "";
                innerTextObj.text = textAll[index];
                ContentSizeFitter fitter = bubbleObj.GetComponentInChildren<ContentSizeFitter>();
                fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
                Canvas.ForceUpdateCanvases();
                fitter.horizontalFit = ContentSizeFitter.FitMode.MinSize;
                fitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
                Canvas.ForceUpdateCanvases();
                showChar();
            }
        }
        else
        {
            destroy();
        }
    }
    void showChar()
    {
        if (charCount < textAll[index].Length)
        {
            textObj.text += textAll[index][charCount];
            if(textAll[index][charCount]!=' ')
            {
                Invoke("showChar", waitingTime);
            }
            else
            {
                Invoke("showChar", 0);
            }
            charCount++;
        }
        else
        {
            index++;
            showDialogue();
        }
    }
    void destroy()
    {
        //CancelInvoke();
        Destroy(bubbleObj);
        isDestroied = true;
        mainChara.setCanMove(true);
        if ((int)textNameToIndex[textAsset.name] < afterEvents.Length && afterEvents[(int)textNameToIndex[textAsset.name]] != null)
        {
            afterEvents[(int)textNameToIndex[textAsset.name]].Invoke();
        }
    }
}
