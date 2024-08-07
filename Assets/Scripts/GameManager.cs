using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // �v���C���[�p�ƓG�p��GaugeController�ւ̎Q��
    public GaugeController healthGaugeP;
    public GaugeController healthGaugeE;

    public GameObject mainImage; // �摜������GameObject
    public Sprite gameOverSpr; // GAME OVER�摜
    public Sprite gameClearSpr; // GAME CLEAR�摜
    public GameObject panel; // �p�l��
    public GameObject messagePanel; // ���b�Z�[�W�p�l��
    public GameObject kaisekiPanel; // ��̓p�l��
    public GameObject playerImage; // �v���C���[�摜��GameObject
    public GameObject attackEffect; // �U���G�t�F�N�g��GameObject
    public GameObject enemyImage; // �G�摜��GameObject
    public GameObject enemyCut; // �G�̃J�b�g�C���摜
    public GameObject playerCut; // �v���C���[�̃J�b�g�C���摜
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
    private int maxPlayerHealth = 20;
    private int currentPlayerHealth = 20;
    private int playerAttack = 3;
    private int maxEnemyHealth = 10;
    private int currentEnemyHealth = 10;
    private int enemyAttack = 5;

    public Text playerHealthText;
    public Text enemyHealthText;
    public Text playerDamageText; // �v���C���[�̃_���[�W�e�L�X�g
    public Text enemyDamageText; // �G�̃_���[�W�e�L�X�g

    private bool isKaisekiPanelActive = false; // ��̓p�l�����\������Ă��邩�ǂ����������t���O
    private bool isWordsAdded = false; // �P�ꂪ�ǉ����ꂽ���ǂ����������t���O
    private bool canPressSpace = false;

    private Vector3 originalPosition; // ���̈ʒu��ێ�����ϐ�
    public float moveDistance = 0.1f; // ���Ɉړ����鋗��
    public float moveDuration = 0.2f; // �ړ��ɂ����鎞��
    public float effectDuration = 0.5f; // �G�t�F�N�g�̕\������
    public float fadeDuration = 1.5f; // �t�F�[�h�A�E�g�̎���
    public float shakeIntensity = 0.1f; // �U���̋���
    public float shakeSpeed = 7f; // �U���̑��x
    public Image kaisekiResult;
    public Text keitaiso;

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
        mainImage.SetActive(false);
        // �{�^��(�p�l��)���\���ɂ���
        panel.SetActive(false);
        messagePanel.SetActive(false);
        kaisekiPanel.SetActive(false);
        attackEffect.SetActive(false);

        // �J�b�g�C���A�j���[�V�������J�n
        StartCoroutine(PlayCutInAnimation());

        // ���b�Z�[�W�p�l����\������
        Invoke("ActivePanel", 6.0f);
        // �S�Ẵ{�^�������X�g�ɒǉ�
        allButtons = new List<Button> { textButton1, textButton2, textButton3 };

        // �����̃{�^���e�L�X�g��ݒ�
        UpdateAllButtonTexts();

        // �{�^���̃N���b�N�C�x���g��ݒ�
        textButton1.onClick.AddListener(() => OnButtonClicked(textButton1));
        textButton2.onClick.AddListener(() => OnButtonClicked(textButton2));
        textButton3.onClick.AddListener(() => OnButtonClicked(textButton3));

        // �U���_���[�W�e�L�X�g�̏�����
        playerDamageText.gameObject.SetActive(false);
        enemyDamageText.gameObject.SetActive(false);


        StartCoroutine(EnableSpaceAfterDelay());
    }

    IEnumerator EnableSpaceAfterDelay()
    {
        while (true)
        {
            yield return new WaitUntil(() => isKaisekiPanelActive);
            yield return new WaitForSeconds(3.0f);
            canPressSpace = true;

            yield return new WaitUntil(() => !isKaisekiPanelActive);
            canPressSpace = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isKaisekiPanelActive && canPressSpace && Input.GetKeyDown(KeyCode.Space))
        {
            kaisekiPanel.SetActive(false); // �p�l�����\���ɂ���
            isKaisekiPanelActive = false; // �t���O�����Z�b�g
            keitaiso.text = "";
            kaisekiResult.sprite = null;
            StartCoroutine(PlayerAttack()); // �U��
            canPressSpace = false;
        }
    }

    IEnumerator PlayCutInAnimation()
    {
        // �ŏ���1.5�b�ҋ@
        yield return new WaitForSeconds(1.5f);

        // ��ʊO����X���C�h�C��
        Vector3 enemyStartPos = new Vector3(-Screen.width, 0, 0);
        Vector3 playerStartPos = new Vector3(Screen.width, 0, 0);
        Vector3 targetPos = Vector3.zero;

        float duration = 0.7f; // �X���C�h�C���ɂ����鎞��

        // �G�J�b�g�C���̃X���C�h�C��
        StartCoroutine(SlideImage(enemyCut, enemyStartPos, targetPos, duration));

        // �v���C���[�J�b�g�C���̃X���C�h�C��
        StartCoroutine(SlideImage(playerCut, playerStartPos, targetPos, duration));

        // �X���C�h�C�����I������̂�҂�
        yield return new WaitForSeconds(duration);

        // 3�b�ԑҋ@
        yield return new WaitForSeconds(3.0f);

        // ��ʊO�փX���C�h�A�E�g
        StartCoroutine(SlideImage(enemyCut, targetPos, enemyStartPos, duration));
        StartCoroutine(SlideImage(playerCut, targetPos, playerStartPos, duration));

        // �X���C�h�A�E�g���I������̂�҂�
        yield return new WaitForSeconds(duration);
    }

    IEnumerator SlideImage(GameObject image, Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            image.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.transform.localPosition = endPos;
    }

    IEnumerator ShowButtonsInSequence()
    {
        // �ŏ��Ƀ{�^�����\���ɂ���
        textButton1.gameObject.SetActive(false);
        textButton2.gameObject.SetActive(false);
        textButton3.gameObject.SetActive(false);

        // 0.2�b�Ԋu�Ń{�^���������\��
        yield return new WaitForSeconds(0.4f);
        textButton1.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.4f);
        textButton2.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.4f);
        textButton3.gameObject.SetActive(true);
    }

    void ActivePanel()
    {
        messagePanel.SetActive(true);
        StartCoroutine(ShowButtonsInSequence()); // �{�^���̏����\�����J�n
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

    // �v���C���[�̗̑̓e�L�X�g���X�V
    void UpdatePlayerHealthText()
    {
        playerHealthText.text = $"{currentPlayerHealth}/{maxPlayerHealth}";
    }

    // �G�̗̑̓e�L�X�g���X�V
    void UpdateEnemyHealthText()
    {
        enemyHealthText.text = $"{currentEnemyHealth}/{maxEnemyHealth}";
    }

    // �_���[�W�e�L�X�g��\��
    void ShowDamage(Text damageText, int damage)
    {
        damageText.text = $"-{damage}";
        damageText.gameObject.SetActive(true);
        StartCoroutine(HideDamage(damageText));
    }

    // �_���[�W�e�L�X�g����莞�Ԍ�ɔ�\���ɂ���
    IEnumerator HideDamage(Text damageText)
    {
        yield return new WaitForSeconds(1.0f);
        damageText.gameObject.SetActive(false);
    }


    IEnumerator PlayerAttack()
    {
        // �v���C���[�̍U������
        StartCoroutine(PlayerAttackAnimation());
        currentEnemyHealth -= playerAttack;
        if (currentEnemyHealth < 0)
        {
            currentEnemyHealth = 0;
        }
        // �_���[�W�����v�Z
        float damageRate = (float)playerAttack / maxEnemyHealth;
        // �̗̓Q�[�W���X�V
        healthGaugeE.TakeDamage(damageRate);
        UpdateEnemyHealthText();
        ShowDamage(enemyDamageText, playerAttack);
        Debug.Log("�G�̗̑�: " + currentEnemyHealth);
        if (currentEnemyHealth <= 0)
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
        currentPlayerHealth -= enemyAttack;
        if (currentPlayerHealth < 0)
        {
            currentPlayerHealth = 0;
        }
        float damageRate = (float)enemyAttack / maxPlayerHealth;
        healthGaugeP.TakeDamage(damageRate);
        UpdatePlayerHealthText();
        ShowDamage(playerDamageText, enemyAttack);
        Debug.Log("�v���C���[�̗̑�: " + currentPlayerHealth);
        if (currentPlayerHealth <= 0)
        {
            StartCoroutine(Destroy(playerImage, GameOver));
        }
        yield return new WaitForSeconds(1.0f);
        ActivePanel();
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
