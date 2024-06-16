using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UIを使うのに必要

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject mainImage; //画像を持つGameObject
    public Sprite gameOverSpr; //GAME OVER画像
    public Sprite gameClearSpr; //GAME CLEAR画像
    public GameObject panel; //パネル
    public GameObject messagePanel;
    public Sprite KeitaisoKaiseki; //形態素解析画像


    Image titelImage; //画像を保存しているImageコンポーネント
 
    // Start is called before the first frame update
    void Start()
    {
        //画像を非表示にする
        Invoke("InactiveImage", 1.0f);
        //ボタン(パネル)を非表示にする
        panel.SetActive(false);
        messagePanel.SetActive(false);
        //パネルを表示する
        Invoke("ActivePanel", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == "gameclear")
        {
            //ゲームクリア
            mainImage.SetActive(true); //画像を表示する
            panel.SetActive(true); //ボタン(パネル)を表示する
            mainImage.GetComponent<Image>().sprite = gameClearSpr; //画像を設定する
            PlayerController.gameState = "gameend";
        }
        else if (PlayerController.gameState == "gameover")
        {
            //ゲームオーバー
            mainImage.SetActive(true); //画像を表示する
            panel.SetActive(true); //ボタン(パネル)を表示する
            mainImage.GetComponent<Image>().sprite = gameOverSpr; //画像を設定する
            PlayerController.gameState = "gameend";
        }
        else if (PlayerController.gameState == "playing")
        {
            //ゲーム中
        }
    }

    //画像を非表示にする
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    //画像を表示する
    void ActivePanel()
    {
        messagePanel.SetActive(true);
    }
}
