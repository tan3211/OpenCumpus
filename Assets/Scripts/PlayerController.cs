using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //アニメーション対応
    //Animator animator; //アニメーター
    //public string stopAnime = "PlayerStop";
    public static string gameState = "playing"; //ゲームの状態

    // Start is called before the first frame update
    void Start()
    {
        gameState = "playing"; //ゲーム中にする
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != "playing")
        {
            return;
        }
    }

    public void GameClear()
    {
        gameState = "gameclear";
        GameStop(); //ゲーム停止
    }
    
    public void GameOver()
    {
        gameState = "gameover";
        GameStop();
    }

    void GameStop()
    {

    }
}
