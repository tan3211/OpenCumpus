using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI //UIを使うのに必要

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject mainImage; //画像を持つGameObject
    public Sprite KeitaisoKaiseki; //形態素解析画像
    public GameObject nextButton //次へボタン


    Image titelImage; //画像を保存しているImageコンポーネント
 
    // Start is called before the first frame update
    void Start()
    {
        //画像を非表示にする
        invoke("InactiveImage", 1.0f);
        //ボタン(パネル)を非表示にする
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
