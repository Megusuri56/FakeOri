using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground_0 : MonoBehaviour
{
    [Header("Layer Setting")]
    public float[] Layer_Speed = new float[7];
    public GameObject[] Layer_Objects = new GameObject[7];

    private Transform _camera;
    private Vector2[] startPos = new Vector2[7];
    private Vector2 boundSize;
    private Vector2 size;
    private GameObject Layer_0;

    public bool Lighten = false;
    public float speed = 0.1f;
    void Awake()
    {
        _camera = Camera.main.transform;
        size = new Vector2(Layer_Objects[0].transform.localScale.x, Layer_Objects[0].transform.localScale.y);
        boundSize = new Vector2(Layer_Objects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x, Layer_Objects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.y);
        for (int i=0;i<4;i++){
            startPos[i] = Vector2.zero;//(_camera.position.x, _camera.position.y);
        }
        if (Lighten)
        {
            foreach (GameObject Layer in Layer_Objects)
            {
                foreach (SpriteRenderer sr in Layer.GetComponentsInChildren<SpriteRenderer>())
                {
                    sr.color = Color.black;
                }
            }
            Invoke("lighten", speed);
        }
    }

    void Update(){
        for (int i=0;i<4;i++){
            Vector2 temp = _camera.position * (1-Layer_Speed[i]);
            Vector2 distance = _camera.position  * Layer_Speed[i];
            Layer_Objects[i].transform.position = new Vector2 (startPos[i].x + distance.x, startPos[i].y + distance.y/3);
            if (temp.x > startPos[i].x + boundSize.x*size.x){
                startPos[i].x += boundSize.x * size.x;
            }else if(temp.x < startPos[i].x - boundSize.x*size.x){
                startPos[i].x -= boundSize.x * size.x;
            }
            if (temp.y > startPos[i].y + boundSize.y*size.y){
                startPos[i].y += boundSize.y * size.y;
            }else if(temp.y < startPos[i].y - boundSize.y*size.y){
                startPos[i].y -= boundSize.y * size.y;
            }
            
        }
    }
    public void lighten()
    {
        foreach(GameObject Layer in Layer_Objects)
        {
            foreach(SpriteRenderer sr in Layer.GetComponentsInChildren<SpriteRenderer>()){
                sr.color = sr.color + new Color(0.1f, 0.1f, 0.1f);
            }
        }
        if (!Layer_Objects[0].GetComponent<SpriteRenderer>().color.Equals(Color.white))
        {
            Invoke("lighten", speed);
        }
        
    }
    public void darken()
    {
        foreach (GameObject Layer in Layer_Objects)
        {
            foreach (SpriteRenderer sr in Layer.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = sr.color - new Color(0.1f, 0.1f, 0.1f,0f);
            }
        }
        if (Layer_Objects[0].GetComponent<SpriteRenderer>().color.r>0)
        {
            Invoke("darken", speed);
        }
    }
}
