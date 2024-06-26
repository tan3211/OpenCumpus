using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject mainImage; // 画像を持つGameObject
    public Sprite gameOverSpr; // GAME OVER画像
    public Sprite gameClearSpr; // GAME CLEAR画像
    public GameObject panel; // パネル
    public GameObject messagePanel; // メッセージパネル
    public GameObject kaisekiPanel; // 解析パネル
    public Sprite KeitaisoKaiseki; // 形態素解析画像
    public GameObject playerImage; // プレイヤー画像のGameObject
    public GameObject attackEffect; // 攻撃エフェクトのGameObject
    public GameObject enemyImage; // 敵画像のGameObject

    // 各ボタンを格納するための変数
    public Button textButton1;
    public Button textButton2;
    public Button textButton3;

    // プレイヤーと敵のステータス
    private int playerHealth = 20;
    private int playerAttack = 3;
    private int enemyHealth = 10;
    private int enemyAttack = 5;

    private bool isKaisekiPanelActive = false; // 解析パネルが表示されているかどうかを示すフラグ

    private Vector3 originalPosition; // 元の位置を保持する変数
    public float moveDistance = 0.1f; // 左に移動する距離
    public float moveDuration = 0.2f; // 移動にかかる時間
    public float effectDuration = 0.5f; // エフェクトの表示時間
    public float fadeDuration = 1.5f; // フェードアウトの時間
    public float shakeIntensity = 0.1f; // 振動の強さ
    public float shakeSpeed = 7f; // 振動の速度


    Image titelImage; // 画像を保存しているImageコンポーネント
 
    // Start is called before the first frame update
    void Start()
    {
        // 画像を非表示にする
        Invoke("InactiveImage", 1.0f);
        // ボタン(パネル)を非表示にする
        panel.SetActive(false);
        messagePanel.SetActive(false);
        kaisekiPanel.SetActive(false);
        attackEffect.SetActive(false);
        // パネルを表示する
        Invoke("ActivePanel", 1.5f);

        // ボタンのクリックイベントを設定
        textButton1.onClick.AddListener(() => OnButtonClicked(textButton1));
        textButton2.onClick.AddListener(() => OnButtonClicked(textButton2));
        textButton3.onClick.AddListener(() => OnButtonClicked(textButton3));
    }

    // Update is called once per frame
    void Update()
    {
        if (isKaisekiPanelActive && Input.GetKeyDown(KeyCode.Space))
        {
            kaisekiPanel.SetActive(false); // パネルを非表示にする
            isKaisekiPanelActive = false; // フラグをリセット
            StartCoroutine(PlayerAttack()); // 攻撃
        }
    }

    // 画像を非表示にする
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    // メッセージパネルを表示する
    void ActivePanel()
    {
        messagePanel.SetActive(true);
    }

    // ボタンがクリックされたときの処理
    void OnButtonClicked(Button button)
    {
        messagePanel.SetActive(false);
        // 解析パネルを表示
        kaisekiPanel.SetActive(true);
        isKaisekiPanelActive = true;
    }

    IEnumerator PlayerAttack()
    {
        // プレイヤーの攻撃処理
        StartCoroutine(PlayerAttackAnimation());
        enemyHealth -= playerAttack;
        Debug.Log("プレイヤーの体力: " + playerHealth);
        if (enemyHealth <= 0)
        {
            StartCoroutine(FadeOutAndDestroy(enemyImage, GameClear));
        }
        else
        {
            // 1.5秒待ってから敵の攻撃
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(EnemyAttack());
        }
    }

    IEnumerator EnemyAttack()
    {
        // 敵の攻撃処理
        StartCoroutine(EnemyAttackAnimation());
        playerHealth -= enemyAttack;
        Debug.Log("敵の体力: " + enemyHealth);
        if (playerHealth <= 0)
        {
            StartCoroutine(FadeOutAndDestroy(playerImage, GameOver));
        }
        yield return new WaitForSeconds(1.0f);
        messagePanel.SetActive(true);
        yield break;
    }

    // プレイヤーの攻撃エフェクトを含むアニメーション
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

    // 敵の攻撃エフェクトを含むアニメーション
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

    // フェードアウトアニメーション
    IEnumerator FadeOutAndDestroy(GameObject characterImage, System.Action onComplete)
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
        onComplete();
    }

    // ゲームオーバー時の処理
    void GameOver()
    {
        messagePanel.SetActive(false);
        mainImage.SetActive(true); // 画像を表示する
        panel.SetActive(true); // ボタン(パネル)を表示する
        mainImage.GetComponent<Image>().sprite = gameOverSpr; // 画像を設定する
        Debug.Log("ゲームオーバー");
    }

    // ゲームクリア時の処理
    void GameClear()
    {
        messagePanel.SetActive(false);
        mainImage.SetActive(true); // 画像を表示する
        panel.SetActive(true); // ボタン(パネル)を表示する
        mainImage.GetComponent<Image>().sprite = gameClearSpr; // 画像を設定する
        Debug.Log("ゲームクリア");
    }
}
