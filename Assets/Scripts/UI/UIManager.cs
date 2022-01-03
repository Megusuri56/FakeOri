using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text HP;
    public CreatureController mc;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        showHP();
    }
    void showHP()
    {
        HP.text = "";
        for (int i = 0; i < mc.hitPoint; i++)
        {
            HP.text += "❤";
        }
    }
}
