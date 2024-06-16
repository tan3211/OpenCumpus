using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI���g���̂ɕK�v

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject mainImage; //�摜������GameObject
    public Sprite gameOverSpr; //GAME OVER�摜
    public Sprite gameClearSpr; //GAME CLEAR�摜
    public GameObject panel; //�p�l��
    public GameObject messagePanel;
    public Sprite KeitaisoKaiseki; //�`�ԑf��͉摜


    Image titelImage; //�摜��ۑ����Ă���Image�R���|�[�l���g
 
    // Start is called before the first frame update
    void Start()
    {
        //�摜���\���ɂ���
        Invoke("InactiveImage", 1.0f);
        //�{�^��(�p�l��)���\���ɂ���
        panel.SetActive(false);
        messagePanel.SetActive(false);
        //�p�l����\������
        Invoke("ActivePanel", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == "gameclear")
        {
            //�Q�[���N���A
            mainImage.SetActive(true); //�摜��\������
            panel.SetActive(true); //�{�^��(�p�l��)��\������
            mainImage.GetComponent<Image>().sprite = gameClearSpr; //�摜��ݒ肷��
            PlayerController.gameState = "gameend";
        }
        else if (PlayerController.gameState == "gameover")
        {
            //�Q�[���I�[�o�[
            mainImage.SetActive(true); //�摜��\������
            panel.SetActive(true); //�{�^��(�p�l��)��\������
            mainImage.GetComponent<Image>().sprite = gameOverSpr; //�摜��ݒ肷��
            PlayerController.gameState = "gameend";
        }
        else if (PlayerController.gameState == "playing")
        {
            //�Q�[����
        }
    }

    //�摜���\���ɂ���
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    //�摜��\������
    void ActivePanel()
    {
        messagePanel.SetActive(true);
    }
}
