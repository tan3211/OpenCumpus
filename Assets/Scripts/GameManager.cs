using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject mainImage; // �摜������GameObject
    public Sprite gameOverSpr; // GAME OVER�摜
    public Sprite gameClearSpr; // GAME CLEAR�摜
    public GameObject panel; // �p�l��
    public GameObject messagePanel; // ���b�Z�[�W�p�l��
    public GameObject kaisekiPanel; // ��̓p�l��
    public GameObject playerImage; // �v���C���[�摜��GameObject
    public GameObject attackEffect; // �U���G�t�F�N�g��GameObject
    public GameObject enemyImage; // �G�摜��GameObject
    public string kaisekiText;

    // �e�{�^�����i�[���邽�߂̕ϐ�
    public Button textButton1;
    public Button textButton2;
    public Button textButton3;

    private List<Button> allButtons;
    private List<string> buttonTexts = new List<string>
    {
        "���[���񂵂Ă��I", "�N�Ȃ炫���Ƃ���I", "��΂����������߂�ȁI", "�N�̗͂�M���Ă���", "���������������������I",
        "�����Ȃ��Ł[�[�[�I", "������ȗE�҂���I", "���炭�Ȃ�Ƃ��Ȃ邳", "��������������Ă��I", "�������Ƃ����������āI",
        "�߂����቞�����Ă܁`��", "���[�L�����h�D�[�C�b�g�ł��I", "�J����΂���B����", "(�h�L�h�L�h�L�h�L�c)"
    };
    private List<string> usedTexts = new List<string>();

    // �v���C���[�ƓG�̃X�e�[�^�X
    private int playerHealth = 20;
    private int playerAttack = 3;
    private int enemyHealth = 10;
    private int enemyAttack = 5;

    private bool isKaisekiPanelActive = false; // ��̓p�l�����\������Ă��邩�ǂ����������t���O
    private bool isWordsAdded = false; // �P�ꂪ�ǉ����ꂽ���ǂ����������t���O

    private Vector3 originalPosition; // ���̈ʒu��ێ�����ϐ�
    public float moveDistance = 0.1f; // ���Ɉړ����鋗��
    public float moveDuration = 0.2f; // �ړ��ɂ����鎞��
    public float effectDuration = 0.5f; // �G�t�F�N�g�̕\������
    public float fadeDuration = 1.5f; // �t�F�[�h�A�E�g�̎���
    public float shakeIntensity = 0.1f; // �U���̋���
    public float shakeSpeed = 7f; // �U���̑��x


    Image titelImage; // �摜��ۑ����Ă���Image�R���|�[�l���g

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // �摜���\���ɂ���
        Invoke("InactiveImage", 1.0f);
        // �{�^��(�p�l��)���\���ɂ���
        panel.SetActive(false);
        messagePanel.SetActive(false);
        kaisekiPanel.SetActive(false);
        attackEffect.SetActive(false);
        // ���b�Z�[�W�p�l����\������
        Invoke("ActivePanel", 1.5f);
        // �S�Ẵ{�^�������X�g�ɒǉ�
        allButtons = new List<Button> { textButton1, textButton2, textButton3 };

        // �����̃{�^���e�L�X�g��ݒ�
        UpdateAllButtonTexts();

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
            StartCoroutine(PlayerAttack()); // �U��
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

    void UpdateAllButtonTexts()
    {
        foreach (Button button in allButtons)
        {
            SetRandomButtonText(button);
        }
    }


    void SetRandomButtonText(Button button)
    {
        if (buttonTexts.Count == 0)
        {
            // ���ׂẴe�L�X�g���g�p�ς݂̏ꍇ�A���X�g�����Z�b�g
            buttonTexts.AddRange(usedTexts);
            usedTexts.Clear();
        }

        // �����_���Ƀe�L�X�g��I��
        int index = Random.Range(0, buttonTexts.Count);
        string selectedText = buttonTexts[index];

        // �{�^���̃e�L�X�g��ݒ�
        button.GetComponentInChildren<Text>().text = selectedText;

        // �g�p�ς݃��X�g�Ɉړ�
        buttonTexts.RemoveAt(index);
        usedTexts.Add(selectedText);
    }

    // �{�^�����N���b�N���ꂽ�Ƃ��̏���
    void OnButtonClicked(Button button)
    {
        messagePanel.SetActive(false);
        // ��̓p�l����\��
        kaisekiPanel.SetActive(true);
        isKaisekiPanelActive = true;

        // �N���b�N���ꂽ�{�^���̃e�L�X�g���擾���AkaisekiPanel�̃e�L�X�g�ɐݒ�
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            kaisekiText = buttonText.text;
        }

        if (!isWordsAdded)
        {
            buttonTexts.AddRange(new List<string> { "�Ȃ��Ȃ���邶�Ⴀ�Ȃ���", "���������B�B�B", "���`�񂢂��������ˁ[", "�ǂ����������Ȃ��H" });
            // �t���O���X�V���Ēǉ��������x�Ǝ��s���Ȃ��悤�ɂ���
            isWordsAdded = true;
        }

        // �S�Ẵ{�^���̃e�L�X�g��V���������_���ȃe�L�X�g�ɍX�V
        UpdateAllButtonTexts();
    }

    IEnumerator PlayerAttack()
    {
        // �v���C���[�̍U������
        StartCoroutine(PlayerAttackAnimation());
        enemyHealth -= playerAttack;
        Debug.Log("�v���C���[�̗̑�: " + playerHealth);
        if (enemyHealth <= 0)
        {
            StartCoroutine(Destroy(enemyImage, GameClear));
        }
        else
        {
            // 1.5�b�҂��Ă���G�̍U��
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(EnemyAttack());
        }
    }

    IEnumerator EnemyAttack()
    {
        // �G�̍U������
        StartCoroutine(EnemyAttackAnimation());
        playerHealth -= enemyAttack;
        Debug.Log("�G�̗̑�: " + enemyHealth);
        if (playerHealth <= 0)
        {
            StartCoroutine(Destroy(playerImage, GameOver));
        }
        yield return new WaitForSeconds(1.0f);
        messagePanel.SetActive(true);
        yield break;
    }

    // �v���C���[�̍U���A�j���[�V����
    IEnumerator PlayerAttackAnimation()
    {
        // �v���C���[�̌��̈ʒu��ۑ�
        originalPosition = playerImage.transform.position;

        // �v���C���[�����Ɉړ�
        Vector3 targetPosition = originalPosition - new Vector3(moveDistance, 0, 0);
        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            playerImage.transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �U���G�t�F�N�g��\��
        attackEffect.transform.position = enemyImage.transform.position;
        attackEffect.SetActive(true);

        // �G�t�F�N�g�̕\�����Ԃ�҂�
        yield return new WaitForSeconds(effectDuration);

        // �U���G�t�F�N�g���\���ɂ���
        attackEffect.SetActive(false);

        // �v���C���[�����̈ʒu�ɖ߂�
        elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            playerImage.transform.position = Vector3.Lerp(targetPosition, originalPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerImage.transform.position = originalPosition;
    }

    // �G�̍U���A�j���[�V����
    IEnumerator EnemyAttackAnimation()
    {
        Vector3 originalEnemyPosition = enemyImage.transform.position;
        Vector3 targetEnemyPosition = originalEnemyPosition + new Vector3(moveDistance, 0, 0);
        float elapsedTime = 0;

        // �G���E�Ɉړ�
        while (elapsedTime < moveDuration)
        {
            enemyImage.transform.position = Vector3.Lerp(originalEnemyPosition, targetEnemyPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �G�t�F�N�g��\��
        attackEffect.transform.position = playerImage.transform.position;
        attackEffect.SetActive(true);
        yield return new WaitForSeconds(effectDuration);

        // �G�t�F�N�g���\��
        attackEffect.SetActive(false);

        // �G�����̈ʒu�ɖ߂�
        elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            enemyImage.transform.position = Vector3.Lerp(targetEnemyPosition, originalEnemyPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemyImage.transform.position = originalEnemyPosition;
    }

    // ���j���̃A�j���[�V����
    IEnumerator Destroy(GameObject characterImage, System.Action onComplete)
    {
        float elapsedTime = 0;
        Vector3 originalPosition = characterImage.transform.position;
        Image imageComponent = characterImage.GetComponent<Image>();

        while (elapsedTime < fadeDuration)
        {
            // �U��
            characterImage.transform.position = originalPosition + (Vector3)Random.insideUnitCircle * shakeIntensity * Mathf.Sin(elapsedTime * shakeSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        characterImage.SetActive(false);
        onComplete();
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

    public string GetText()
    {
        return kaisekiText;
    }
}
