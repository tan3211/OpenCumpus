using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //�A�j���[�V�����Ή�
    //Animator animator; //�A�j���[�^�[
    //public string stopAnime = "PlayerStop";
    public static string gameState = "playing"; //�Q�[���̏��

    // Start is called before the first frame update
    void Start()
    {
        gameState = "playing"; //�Q�[�����ɂ���
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
        GameStop(); //�Q�[����~
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
