using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class myUIManager : MonoBehaviour
{
    public GameObject UI;
    public Text HP;
    public GameObject skillChoosing;

    private bool isChoosingSkill = false;

    public CreatureController mc;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChoosingSkill)
        {
            showHP();
        }
        if (isChoosingSkill && !skillChoosing.activeSelf)
        {
            isChoosingSkill = false;
            //skillChoosing.SetActive(false);
            mc.canMove = true;
        }
    }
    void showHP()
    {
        HP.text = "";
        for (int i = 0; i < mc.hitPoint; i++)
        {
            HP.text += "❤";
        }
    }
    public void chooseSkill()
    {
        isChoosingSkill = true;
        mc.canMove = false;
        skillChoosing.SetActive(true);
    }
}
