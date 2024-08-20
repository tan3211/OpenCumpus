using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // プレイヤー用と敵用のGaugeControllerへの参照
    public GaugeController healthGaugeP;
    public GaugeController healthGaugeE;

    //public GameObject panel; // パネル
    public GameObject messagePanel; // メッセージパネル
    public GameObject kaisekiPanel; // 解析パネル
    public GameObject playerImage; // プレイヤー画像のGameObject
    public GameObject attackEffect; // 攻撃エフェクトのGameObject
    public GameObject enemyImage; // 敵画像のGameObject
    public GameObject enemyCut; // 敵のカットイン画像
    public GameObject playerCut; // プレイヤーのカットイン画像
    public string kaisekiText;

    // 各ボタンを格納するための変数
    public Button textButton1;
    public Button textButton2;
    public Button textButton3;

    private List<Button> allButtons;
    private List<string> buttonTexts = new List<string>
    {
        "おーえんしてるよ！", "君ならきっとやれる！", "絶対ぜったい諦めるな！", "君の力を信じてるよっ", "いっけえええぇぇぇっ！",
        "負けないでーーー！", "負けるな勇者さん！", "恐らくなんとかなるさ", "私たちが見守ってるよ！", "ささっとやっつけちゃって！",
        "めっちゃ応援してま～す", "ユーキャンドゥーイットです！", "雨垂れ石を穿つ。だよ", "(ドキドキドキドキ…)"
    };
    private List<string> usedTexts = new List<string>();

    // プレイヤーと敵のステータス
    private int maxPlayerHealth = 30;
    private int currentPlayerHealth = 30;
    private int playerAttack = 3;
    private int maxEnemyHealth = 52;
    private int currentEnemyHealth = 52;
    private int enemyAttack = 5;

    public Text attackText; // 攻撃力を表示するためのUIテキスト
    public ResultManager resultManager; // ResultManager への参照
    private int currentAttack; // モーション中の現在の攻撃力
    private int originalAttack; // 元の攻撃力を保存

    public Text playerHealthText;
    public Text enemyHealthText;
    public Text playerDamageText; // プレイヤーのダメージテキスト
    public Text enemyDamageText; // 敵のダメージテキスト

    private bool isKaisekiPanelActive = false; // 解析パネルが表示されているかどうかを示すフラグ
    private bool isWordsAdded = false; // 単語が追加されたかどうかを示すフラグ
    private bool canPressSpace = false;

    private Vector3 originalPosition; // 元の位置を保持する変数
    public float moveDistance = 0.1f; // 左に移動する距離
    public float moveDuration = 0.2f; // 移動にかかる時間
    public float effectDuration = 0.5f; // エフェクトの表示時間
    public float fadeDuration = 1.5f; // フェードアウトの時間
    public float shakeIntensity = 0.1f; // 振動の強さ
    public float shakeSpeed = 7f; // 振動の速度
    public Image kaisekiResult;
    public Text keitaiso;

    public Image winFade;  // 透明度50%の灰色のImage (UI)
    public RectTransform winPanel;  // WinPanelのRectTransform
    public Image winChara;
    public Image winImage;
    public Text winMessage;
    public Button winEndButton;

    public Image loseFade;  // 透明度50%の灰色のImage (UI)
    public RectTransform losePanel;  // losePanelのRectTransform
    public Image loseChara;
    public Image loseImage;
    public Text loseMessage;
    public Button loseEndButton;

    Image titelImage; // 画像を保存しているImageコンポーネント

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
        // ボタン(パネル)を非表示にする
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

        // カットインアニメーションを開始
        StartCoroutine(PlayCutInAnimation());

        // メッセージパネルを表示する
        Invoke("ActivePanel", 6.0f);
        // 全てのボタンをリストに追加
        allButtons = new List<Button> { textButton1, textButton2, textButton3 };

        // 初期のボタンテキストを設定
        UpdateAllButtonTexts();

        // ボタンのクリックイベントを設定
        textButton1.onClick.AddListener(() => OnButtonClicked(textButton1));
        textButton2.onClick.AddListener(() => OnButtonClicked(textButton2));
        textButton3.onClick.AddListener(() => OnButtonClicked(textButton3));

        // 攻撃ダメージテキストの初期化
        playerDamageText.gameObject.SetActive(false);
        enemyDamageText.gameObject.SetActive(false);

        // 初期攻撃力でテキストを設定
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
            kaisekiPanel.SetActive(false); // パネルを非表示にする
            isKaisekiPanelActive = false; // フラグをリセット
            keitaiso.text = "";
            kaisekiResult.sprite = null;
            StartCoroutine(PlayerAttack()); // 攻撃
            canPressSpace = false;
        }
    }

    IEnumerator PlayCutInAnimation()
    {
        // 最初の1.5秒待機
        yield return new WaitForSeconds(1.5f);

        // 画面外からスライドイン
        Vector3 enemyStartPos = new Vector3(-Screen.width, 0, 0);
        Vector3 playerStartPos = new Vector3(Screen.width, 0, 0);
        Vector3 targetPos = Vector3.zero;

        float duration = 0.7f; // スライドインにかかる時間

        // 敵カットインのスライドイン
        StartCoroutine(SlideImage(enemyCut, enemyStartPos, targetPos, duration));

        // プレイヤーカットインのスライドイン
        StartCoroutine(SlideImage(playerCut, playerStartPos, targetPos, duration));

        // スライドインが終了するのを待つ
        yield return new WaitForSeconds(duration);

        // 3秒間待機
        yield return new WaitForSeconds(3.0f);

        // 画面外へスライドアウト
        StartCoroutine(SlideImage(enemyCut, targetPos, enemyStartPos, duration));
        StartCoroutine(SlideImage(playerCut, targetPos, playerStartPos, duration));

        // スライドアウトが終了するのを待つ
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
        // 最初にボタンを非表示にする
        textButton1.gameObject.SetActive(false);
        textButton2.gameObject.SetActive(false);
        textButton3.gameObject.SetActive(false);

        // 0.2秒間隔でボタンを順次表示
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
        StartCoroutine(ShowButtonsInSequence()); // ボタンの順次表示を開始
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
            // すべてのテキストが使用済みの場合、リストをリセット
            buttonTexts.AddRange(usedTexts);
            usedTexts.Clear();
        }

        // ランダムにテキストを選択
        int index = Random.Range(0, buttonTexts.Count);
        string selectedText = buttonTexts[index];

        // ボタンのテキストを設定
        button.GetComponentInChildren<Text>().text = selectedText;

        // 使用済みリストに移動
        buttonTexts.RemoveAt(index);
        usedTexts.Add(selectedText);
    }

    // ボタンがクリックされたときの処理
    void OnButtonClicked(Button button)
    {
        messagePanel.SetActive(false);
        // 解析パネルを表示
        kaisekiPanel.SetActive(true);
        isKaisekiPanelActive = true;

        // クリックされたボタンのテキストを取得し、kaisekiPanelのテキストに設定
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            kaisekiText = buttonText.text;
        }

        if (!isWordsAdded)
        {
            buttonTexts.AddRange(new List<string> { "なかなかやるじゃあないか", "すっげぇ。。。", "う～んいい勝負だねー", "どっちが勝つかなあ？" });
            // フラグを更新して追加処理を二度と実行しないようにする
            isWordsAdded = true;
        }

        // 全てのボタンのテキストを新しいランダムなテキストに更新
        UpdateAllButtonTexts();
    }

    // プレイヤーの体力テキストを更新
    void UpdatePlayerHealthText()
    {
        playerHealthText.text = $"{currentPlayerHealth}/{maxPlayerHealth}";
    }

    // 敵の体力テキストを更新
    void UpdateEnemyHealthText()
    {
        enemyHealthText.text = $"{currentEnemyHealth}/{maxEnemyHealth}";
    }

    // ダメージテキストを表示
    void ShowDamage(Text damageText, int damage)
    {
        damageText.text = $"-{damage}";
        damageText.gameObject.SetActive(true);
        StartCoroutine(HideDamage(damageText));
    }

    // ダメージテキストを一定時間後に非表示にする
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

            // attack と buf の値を更新
            currentAttack = initialAttack + attackIncrease;
            int remainingBuf = buf - attackIncrease;

            // テキストを更新(remainingBufが0のときは「+0」を表示しない)
            if (remainingBuf > 0)
            {
                attackText.text = $"{currentAttack} + {remainingBuf}";
            }
            else
            {
                attackText.text = $"{currentAttack}";
            }

            // 時間を進める
            time += Time.deltaTime;
            yield return null;
        }

        // モーション後の最終的な値を設定
        playerAttack = finalAttack;
        attackText.text = $"{playerAttack}";
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator PlayerAttack()
    {
        // プレイヤーの攻撃処理
        int buf = resultManager.GetKeitaisoNumber();
        yield return StartCoroutine(AttackIncrease(buf));  // 攻撃力上昇
        attackText.gameObject.SetActive(false); // テキストを非表示
        yield return StartCoroutine(PlayerAttackAnimation()); // 攻撃アニメーションの実行
        currentEnemyHealth -= playerAttack;
        if (currentEnemyHealth < 0)
        {
            currentEnemyHealth = 0;
        }
        // ダメージ率を計算
        float damageRate = (float)playerAttack / maxEnemyHealth;
        // 体力ゲージを更新
        healthGaugeE.TakeDamage(damageRate);
        UpdateEnemyHealthText();
        ShowDamage(enemyDamageText, playerAttack);
        Debug.Log("敵の体力: " + currentEnemyHealth);
        if (currentEnemyHealth <= 0)
        {
            StartCoroutine(Destroy(enemyImage, GameClear));
        }
        else
        {
            // 1.5秒待ってから敵の攻撃
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(EnemyAttack());
        }
        playerAttack = originalAttack; // 攻撃終了後に元の攻撃力に戻す
    }

    IEnumerator EnemyAttack()
    {
        // 敵の攻撃処理
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
        Debug.Log("プレイヤーの体力: " + currentPlayerHealth);
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

    // プレイヤーの攻撃アニメーション
    IEnumerator PlayerAttackAnimation()
    {
        // プレイヤーの元の位置を保存
        originalPosition = playerImage.transform.position;

        // プレイヤーを左に移動
        Vector3 targetPosition = originalPosition - new Vector3(moveDistance, 0, 0);
        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            playerImage.transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 攻撃エフェクトを表示
        attackEffect.transform.position = enemyImage.transform.position;
        attackEffect.SetActive(true);

        // エフェクトの表示時間を待つ
        yield return new WaitForSeconds(effectDuration);

        // 攻撃エフェクトを非表示にする
        attackEffect.SetActive(false);

        // プレイヤーを元の位置に戻す
        elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            playerImage.transform.position = Vector3.Lerp(targetPosition, originalPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerImage.transform.position = originalPosition;
    }

    // 敵の攻撃アニメーション
    IEnumerator EnemyAttackAnimation()
    {
        Vector3 originalEnemyPosition = enemyImage.transform.position;
        Vector3 targetEnemyPosition = originalEnemyPosition + new Vector3(moveDistance, 0, 0);
        float elapsedTime = 0;

        // 敵を右に移動
        while (elapsedTime < moveDuration)
        {
            enemyImage.transform.position = Vector3.Lerp(originalEnemyPosition, targetEnemyPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // エフェクトを表示
        attackEffect.transform.position = playerImage.transform.position;
        attackEffect.SetActive(true);
        yield return new WaitForSeconds(effectDuration);

        // エフェクトを非表示
        attackEffect.SetActive(false);

        // 敵を元の位置に戻す
        elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            enemyImage.transform.position = Vector3.Lerp(targetEnemyPosition, originalEnemyPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemyImage.transform.position = originalEnemyPosition;
    }

    // 撃破時のアニメーション
    IEnumerator Destroy(GameObject characterImage, System.Action onComplete)
    {
        float elapsedTime = 0;
        Vector3 originalPosition = characterImage.transform.position;
        Image imageComponent = characterImage.GetComponent<Image>();

        while (elapsedTime < fadeDuration)
        {
            // 振動
            characterImage.transform.position = originalPosition + (Vector3)Random.insideUnitCircle * shakeIntensity * Mathf.Sin(elapsedTime * shakeSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        characterImage.SetActive(false);
        onComplete();
    }

    // ゲームオーバー時の処理
    void GameOver()
    {
        messagePanel.SetActive(false);
        // 画面全体を灰色にフェードする
        loseFade.gameObject.SetActive(true);
        losePanel.gameObject.SetActive(true);
        loseFade.color = new Color(0, 0, 0, 0);  // 最初は透明
        loseFade.DOFade(0.5f, 1f).OnComplete(() => {

            // loseCharaを左から右へフェードインしながらスライド
            loseChara.gameObject.SetActive(true);
            loseChara.color = new Color(1, 1, 1, 0);  // 最初は透明
            Vector2 loseCharaInitialPos = loseChara.rectTransform.anchoredPosition;  // 現在の位置を取得
            loseChara.rectTransform.anchoredPosition = new Vector2(loseCharaInitialPos.x - 50, loseCharaInitialPos.y);  // スライド開始位置
            loseChara.DOFade(1f, 1f);
            loseChara.rectTransform.DOAnchorPos(loseCharaInitialPos, 1f);  // 元の位置にスライド

            // loseImageとloseMessageを下から上へフェードインしながらスライド
            loseImage.gameObject.SetActive(true);
            loseMessage.gameObject.SetActive(true);

            loseImage.color = new Color(1, 1, 1, 0);  // 最初は透明
            loseMessage.color = new Color(1, 1, 1, 0);  // 最初は透明

            Vector2 loseImageInitialPos = loseImage.rectTransform.anchoredPosition;  // 現在の位置を取得
            Vector2 loseMessageInitialPos = loseMessage.rectTransform.anchoredPosition;    // 現在の位置を取得

            loseImage.rectTransform.anchoredPosition = new Vector2(loseImageInitialPos.x, loseImageInitialPos.y - 50);  // スライド開始位置
            loseMessage.rectTransform.anchoredPosition = new Vector2(loseMessageInitialPos.x, loseMessageInitialPos.y - 50);    // スライド開始位置

            loseImage.DOFade(1f, 1f);
            loseMessage.DOFade(1f, 1f);

            loseImage.rectTransform.DOAnchorPos(loseImageInitialPos, 1f);  // 元の位置にスライド
            loseMessage.rectTransform.DOAnchorPos(loseMessageInitialPos, 1f);    // 元の位置にスライド
        });

        // すべてのフェードインが終わった後にloseEndButtonが表示される
        DOVirtual.DelayedCall(2f, () => {
            loseEndButton.gameObject.SetActive(true);
        });
        Debug.Log("ゲームオーバー");
    }

    // ゲームクリア時の処理
    public void GameClear()
    {
        messagePanel.SetActive(false);
        // 画面全体を灰色にフェードする
        winFade.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(true);
        winFade.color = new Color(0, 0, 0, 0);  // 最初は透明
        winFade.DOFade(0.5f, 1f).OnComplete(() => {

            // WinCharaを左から右へフェードインしながらスライド
            winChara.gameObject.SetActive(true);
            winChara.color = new Color(1, 1, 1, 0);  // 最初は透明
            Vector2 winCharaInitialPos = winChara.rectTransform.anchoredPosition;  // 現在の位置を取得
            winChara.rectTransform.anchoredPosition = new Vector2(winCharaInitialPos.x - 50, winCharaInitialPos.y);  // スライド開始位置
            winChara.DOFade(1f, 1f);
            winChara.rectTransform.DOAnchorPos(winCharaInitialPos, 1f);  // 元の位置にスライド

            // WinImageとMessageを下から上へフェードインしながらスライド
            winImage.gameObject.SetActive(true);
            winMessage.gameObject.SetActive(true);

            winImage.color = new Color(1, 1, 1, 0);  // 最初は透明
            winMessage.color = new Color(1, 1, 1, 0);  // 最初は透明

            Vector2 winImageInitialPos = winImage.rectTransform.anchoredPosition;  // 現在の位置を取得
            Vector2 messageInitialPos = winMessage.rectTransform.anchoredPosition;    // 現在の位置を取得

            winImage.rectTransform.anchoredPosition = new Vector2(winImageInitialPos.x, winImageInitialPos.y - 50);  // スライド開始位置
            winMessage.rectTransform.anchoredPosition = new Vector2(messageInitialPos.x, messageInitialPos.y - 50);    // スライド開始位置

            winImage.DOFade(1f, 1f);
            winMessage.DOFade(1f, 1f);

            winImage.rectTransform.DOAnchorPos(winImageInitialPos, 1f);  // 元の位置にスライド
            winMessage.rectTransform.DOAnchorPos(messageInitialPos, 1f);    // 元の位置にスライド
        });

        // すべてのフェードインが終わった後にEndButtonが表示される
        DOVirtual.DelayedCall(2f, () => {
            winEndButton.gameObject.SetActive(true);
        });
        Debug.Log("ゲームクリア");
    }

    public string GetText()
    {
        return kaisekiText;
    }
}
