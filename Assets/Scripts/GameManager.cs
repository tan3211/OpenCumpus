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
            Debug.Log("Player attacks!");
            Attack();    
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
        // 解析パネルを表示
        kaisekiPanel.SetActive(true);
        isKaisekiPanelActive = true;
    }

    // 攻撃
    void Attack()
    {
        // プレイヤーの攻撃
        enemyHealth -= playerAttack;
        Debug.Log("敵の体力: " + enemyHealth);
        if (enemyHealth <= 0)
        {
            GameClear();
            return;
        }

        // 敵の攻撃
        playerHealth -= enemyAttack;
        Debug.Log("プレイヤーの体力: " + playerHealth);
        if (playerHealth <= 0)
        {
            GameOver();
            return;
        }
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
