using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI //UI���g���̂ɕK�v

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject mainImage; //�摜������GameObject
    public Sprite KeitaisoKaiseki; //�`�ԑf��͉摜
    public GameObject nextButton //���փ{�^��


    Image titelImage; //�摜��ۑ����Ă���Image�R���|�[�l���g
 
    // Start is called before the first frame update
    void Start()
    {
        //�摜���\���ɂ���
        invoke("InactiveImage", 1.0f);
        //�{�^��(�p�l��)���\���ɂ���
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
