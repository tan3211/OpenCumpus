using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject mainImage; // �摜������GameObject
    public Sprite gameOverSpr; // GAME OVER�摜
    public Sprite gameClearSpr; // GAME CLEAR�摜
    public GameObject panel; // �p�l��
    public GameObject messagePanel; // ���b�Z�[�W�p�l��
    public GameObject kaisekiPanel; // ��̓p�l��
    public Sprite KeitaisoKaiseki; // �`�ԑf��͉摜

    // �e�{�^�����i�[���邽�߂̕ϐ�
    public Button textButton1;
    public Button textButton2;
    public Button textButton3;

    // �v���C���[�ƓG�̃X�e�[�^�X
    private int playerHealth = 20;
    private int playerAttack = 3;
    private int enemyHealth = 10;
    private int enemyAttack = 5;

    private bool isKaisekiPanelActive = false; // ��̓p�l�����\������Ă��邩�ǂ����������t���O

    Image titelImage; // �摜��ۑ����Ă���Image�R���|�[�l���g
 
    // Start is called before the first frame update
    void Start()
    {
        // �摜���\���ɂ���
        Invoke("InactiveImage", 1.0f);
        // �{�^��(�p�l��)���\���ɂ���
        panel.SetActive(false);
        messagePanel.SetActive(false);
        kaisekiPanel.SetActive(false);
        // �p�l����\������
        Invoke("ActivePanel", 1.5f);

        // �{�^���̃N���b�N�C�x���g��ݒ�
        textButton1.onClick.AddListener(() => OnButtonClicked(textButton1));
        textButton2.onClick.AddListener(() => OnButtonClicked(textButton2));
        textButton3.onClick.AddListener(() => OnButtonClicked(textButton3));
    }

    // Update is called once per frame
    void Update()
    {
        if (isKaisekiPanelActive && Input.GetKeyDown(KeyCode.Space))
        {
            kaisekiPanel.SetActive(false); // �p�l�����\���ɂ���
            isKaisekiPanelActive = false; // �t���O�����Z�b�g
            Debug.Log("Player attacks!");
            Attack();    
        }
    }

    // �摜���\���ɂ���
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    // ���b�Z�[�W�p�l����\������
    void ActivePanel()
    {
        messagePanel.SetActive(true);
    }

    // �{�^�����N���b�N���ꂽ�Ƃ��̏���
    void OnButtonClicked(Button button)
    {
        // ��̓p�l����\��
        kaisekiPanel.SetActive(true);
        isKaisekiPanelActive = true;
    }

    // �U��
    void Attack()
    {
        // �v���C���[�̍U��
        enemyHealth -= playerAttack;
        Debug.Log("�G�̗̑�: " + enemyHealth);
        if (enemyHealth <= 0)
        {
            GameClear();
            return;
        }

        // �G�̍U��
        playerHealth -= enemyAttack;
        Debug.Log("�v���C���[�̗̑�: " + playerHealth);
        if (playerHealth <= 0)
        {
            GameOver();
            return;
        }
    }

    // �Q�[���I�[�o�[���̏���
    void GameOver()
    {
        messagePanel.SetActive(false);
        mainImage.SetActive(true); // �摜��\������
        panel.SetActive(true); // �{�^��(�p�l��)��\������
        mainImage.GetComponent<Image>().sprite = gameOverSpr; // �摜��ݒ肷��
        Debug.Log("�Q�[���I�[�o�[");
    }

    // �Q�[���N���A���̏���
    void GameClear()
    {
        messagePanel.SetActive(false);
        mainImage.SetActive(true); // �摜��\������
        panel.SetActive(true); // �{�^��(�p�l��)��\������
        mainImage.GetComponent<Image>().sprite = gameClearSpr; // �摜��ݒ肷��
        Debug.Log("�Q�[���N���A");
    }
}
