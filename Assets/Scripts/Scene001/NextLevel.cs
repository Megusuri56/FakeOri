using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    private bool flag = true;

    public CG_Scene001_1 mainCharaInCG;
    public ParallaxBackground_0 bg;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && flag)
        {
            flag = false;
            mainCharaInCG.changeSpeed();
            bg.darken();
            Invoke("loadScene", 1f);
        }
    }
    void loadScene()
    {
        SceneManager.LoadSceneAsync("002");
    }
}
