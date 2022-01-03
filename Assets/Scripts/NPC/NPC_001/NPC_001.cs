using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_001 : MonoBehaviour
{
    public CreatureController npc;
    public GameObject hintE;
    private GameObject HintEObj;

    bool isTalking = false;
    bool pushE = false;

    public TalkingController talkingController;
    // Start is called before the first frame update
    void Start()
    {
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (!isTalking && other.gameObject.CompareTag("Player"))
        {
            beforTalk();
        }
        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            Destroy(HintEObj);
    }
    // Update is called once per frame
    void Update()
    {
        pushE = false;
        if (!isTalking && Input.GetKey("e"))
        {
            pushE = true;
        }
    }
    void beforTalk()
    {
        if (GameObject.Find("hintE(Clone)") == null)
        {
            HintEObj = Instantiate(hintE,GameObject.Find("Hint").transform);
            HintEObj.GetComponent<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }
        if (pushE)
        {
            isTalking = true;
            Destroy(HintEObj);
            talkingController.startTalk(gameObject);
        }
    }
}
