using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //�V�[���̐؂�ւ��ɕK�v

public class ChangeSence : MonoBehaviour
{

    public string sceneName; //�ǂݍ��ރV�[����

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�V�[����ǂݍ���
    public void Load()
    {
        GetComponent<AudioSource>().Play();
        Initiate.Fade(sceneName, Color.black, 1.0f);
    }
}
