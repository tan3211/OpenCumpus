using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // �v���C���[�p�ƓG�p��GaugeController�ւ̎Q��
    public GaugeController healthGaugeP;
    public GaugeController healthGaugeE;

    //public GameObject panel; // �p�l��
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
    private int maxPlayerHealth = 30;
    private int currentPlayerHealth = 30;
    private int playerAttack = 3;
    private int maxEnemyHealth = 52;
    private int currentEnemyHealth = 52;
    private int enemyAttack = 5;

    public Text attackText; // �U���͂�\�����邽�߂�UI�e�L�X�g
    public ResultManager resultManager; // ResultManager �ւ̎Q��
    private int currentAttack; // ���[�V�������̌��݂̍U����
    private int originalAttack; // ���̍U���͂�ۑ�

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

    public Image winFade;  // �����x50%�̊D�F��Image (UI)
    public RectTransform winPanel;  // WinPanel��RectTransform
    public Image winChara;
    public Image winImage;
    public Text winMessage;
    public Button winEndButton;

    public Image loseFade;  // �����x50%�̊D�F��Image (UI)
    public RectTransform losePanel;  // losePanel��RectTransform
    public Image loseChara;
    public Image loseImage;
    public Text loseMessage;
    public Button loseEndButton;

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
        // �{�^��(�p�l��)���\���ɂ���
        messagePanel.SetActive(false);
        kaisekiPanel.SetActive(false);
        attackEffect.SetActive(false);

        winPanel.gameObject.SetActive(false);
        winFade.gameObject.SetActive(false);
        winChara.gameObject.SetActive(false);
        winImage.gameObject.SetActive(false);
        winMessage.gameObject.SetActive(false);
        winEndButton.gameObject.SetActive(false);

        losePanel.gameObject.SetActive(false);
        loseFade.gameObject.SetActive(false);
        loseChara.gameObject.SetActive(false);
        loseImage.gameObject.SetActive(false);
        loseMessage.gameObject.SetActive(false);
        loseEndButton.gameObject.SetActive(false);

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

        // �����U���͂Ńe�L�X�g��ݒ�
        attackText.gameObject.SetActive(false);
        currentAttack = playerAttack;
        originalAttack = playerAttack;
        attackText.text = currentAttack.ToString();

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
        yield return new WaitForSeconds(0.5f);
        damageText.gameObject.SetActive(false);
    }

    private IEnumerator AttackIncrease(int buf)
    {
        attackText.gameObject.SetActive(true);
        float duration = 1.0f;
        float time = 0;
        int initialAttack = playerAttack;
        int finalAttack = playerAttack + buf;

        while (time < duration)
        {
            float t = time / duration;
            int attackIncrease = Mathf.RoundToInt(Mathf.Lerp(0, buf, t));

            // attack �� buf �̒l���X�V
            currentAttack = initialAttack + attackIncrease;
            int remainingBuf = buf - attackIncrease;

            // �e�L�X�g���X�V(remainingBuf��0�̂Ƃ��́u+0�v��\�����Ȃ�)
            if (remainingBuf > 0)
            {
                attackText.text = $"{currentAttack} + {remainingBuf}";
            }
            else
            {
                attackText.text = $"{currentAttack}";
            }

            // ���Ԃ�i�߂�
            time += Time.deltaTime;
            yield return null;
        }

        // ���[�V������̍ŏI�I�Ȓl��ݒ�
        playerAttack = finalAttack;
        attackText.text = $"{playerAttack}";
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator PlayerAttack()
    {
        // �v���C���[�̍U������
        int buf = resultManager.GetKeitaisoNumber();
        yield return StartCoroutine(AttackIncrease(buf));  // �U���͏㏸
        attackText.gameObject.SetActive(false); // �e�L�X�g���\��
        yield return StartCoroutine(PlayerAttackAnimation()); // �U���A�j���[�V�����̎��s
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
        playerAttack = originalAttack; // �U���I����Ɍ��̍U���͂ɖ߂�
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
        else
        {
            yield return new WaitForSeconds(1.0f);
            ActivePanel();
            yield break;
        }
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
        // ��ʑS�̂��D�F�Ƀt�F�[�h����
        loseFade.gameObject.SetActive(true);
        losePanel.gameObject.SetActive(true);
        loseFade.color = new Color(0, 0, 0, 0);  // �ŏ��͓���
        loseFade.DOFade(0.5f, 1f).OnComplete(() => {

            // loseChara��������E�փt�F�[�h�C�����Ȃ���X���C�h
            loseChara.gameObject.SetActive(true);
            loseChara.color = new Color(1, 1, 1, 0);  // �ŏ��͓���
            Vector2 loseCharaInitialPos = loseChara.rectTransform.anchoredPosition;  // ���݂̈ʒu���擾
            loseChara.rectTransform.anchoredPosition = new Vector2(loseCharaInitialPos.x - 50, loseCharaInitialPos.y);  // �X���C�h�J�n�ʒu
            loseChara.DOFade(1f, 1f);
            loseChara.rectTransform.DOAnchorPos(loseCharaInitialPos, 1f);  // ���̈ʒu�ɃX���C�h

            // loseImage��loseMessage���������փt�F�[�h�C�����Ȃ���X���C�h
            loseImage.gameObject.SetActive(true);
            loseMessage.gameObject.SetActive(true);

            loseImage.color = new Color(1, 1, 1, 0);  // �ŏ��͓���
            loseMessage.color = new Color(1, 1, 1, 0);  // �ŏ��͓���

            Vector2 loseImageInitialPos = loseImage.rectTransform.anchoredPosition;  // ���݂̈ʒu���擾
            Vector2 loseMessageInitialPos = loseMessage.rectTransform.anchoredPosition;    // ���݂̈ʒu���擾

            loseImage.rectTransform.anchoredPosition = new Vector2(loseImageInitialPos.x, loseImageInitialPos.y - 50);  // �X���C�h�J�n�ʒu
            loseMessage.rectTransform.anchoredPosition = new Vector2(loseMessageInitialPos.x, loseMessageInitialPos.y - 50);    // �X���C�h�J�n�ʒu

            loseImage.DOFade(1f, 1f);
            loseMessage.DOFade(1f, 1f);

            loseImage.rectTransform.DOAnchorPos(loseImageInitialPos, 1f);  // ���̈ʒu�ɃX���C�h
            loseMessage.rectTransform.DOAnchorPos(loseMessageInitialPos, 1f);    // ���̈ʒu�ɃX���C�h
        });

        // ���ׂẴt�F�[�h�C�����I��������loseEndButton���\�������
        DOVirtual.DelayedCall(2f, () => {
            loseEndButton.gameObject.SetActive(true);
        });
        Debug.Log("�Q�[���I�[�o�[");
    }

    // �Q�[���N���A���̏���
    public void GameClear()
    {
        messagePanel.SetActive(false);
        // ��ʑS�̂��D�F�Ƀt�F�[�h����
        winFade.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(true);
        winFade.color = new Color(0, 0, 0, 0);  // �ŏ��͓���
        winFade.DOFade(0.5f, 1f).OnComplete(() => {

            // WinChara��������E�փt�F�[�h�C�����Ȃ���X���C�h
            winChara.gameObject.SetActive(true);
            winChara.color = new Color(1, 1, 1, 0);  // �ŏ��͓���
            Vector2 winCharaInitialPos = winChara.rectTransform.anchoredPosition;  // ���݂̈ʒu���擾
            winChara.rectTransform.anchoredPosition = new Vector2(winCharaInitialPos.x - 50, winCharaInitialPos.y);  // �X���C�h�J�n�ʒu
            winChara.DOFade(1f, 1f);
            winChara.rectTransform.DOAnchorPos(winCharaInitialPos, 1f);  // ���̈ʒu�ɃX���C�h

            // WinImage��Message���������փt�F�[�h�C�����Ȃ���X���C�h
            winImage.gameObject.SetActive(true);
            winMessage.gameObject.SetActive(true);

            winImage.color = new Color(1, 1, 1, 0);  // �ŏ��͓���
            winMessage.color = new Color(1, 1, 1, 0);  // �ŏ��͓���

            Vector2 winImageInitialPos = winImage.rectTransform.anchoredPosition;  // ���݂̈ʒu���擾
            Vector2 messageInitialPos = winMessage.rectTransform.anchoredPosition;    // ���݂̈ʒu���擾

            winImage.rectTransform.anchoredPosition = new Vector2(winImageInitialPos.x, winImageInitialPos.y - 50);  // �X���C�h�J�n�ʒu
            winMessage.rectTransform.anchoredPosition = new Vector2(messageInitialPos.x, messageInitialPos.y - 50);    // �X���C�h�J�n�ʒu

            winImage.DOFade(1f, 1f);
            winMessage.DOFade(1f, 1f);

            winImage.rectTransform.DOAnchorPos(winImageInitialPos, 1f);  // ���̈ʒu�ɃX���C�h
            winMessage.rectTransform.DOAnchorPos(messageInitialPos, 1f);    // ���̈ʒu�ɃX���C�h
        });

        // ���ׂẴt�F�[�h�C�����I��������EndButton���\�������
        DOVirtual.DelayedCall(2f, () => {
            winEndButton.gameObject.SetActive(true);
        });
        Debug.Log("�Q�[���N���A");
    }

    public string GetText()
    {
        return kaisekiText;
    }
}
