using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class CarouselController : MonoBehaviour
{
    public GameObject carouselPanel; // カルーセルのPanel
    public GameObject[] banners; // バナー画像を格納する配列
    public Button triggerButton; // カルーセルを表示するトリガーボタン
    public Button leftButton;    // 左ボタン
    public Button rightButton;   // 右ボタン
    public Button closeButton;   // 閉じるボタン
    public Text currentPageText; // 現在のページ番号を表示するテキスト

    public AudioClip sound1;
    public AudioClip sound2;
    public AudioClip sound3;
    AudioSource audioSource;

    private int currentIndex = 0; // 現在表示しているバナーのインデックス

    // Start is called before the first frame update
    void Start()
    {

        carouselPanel.SetActive(false); // 起動時にカルーセルPanelを非表示に設定
        UpdateBanners(); // 初期バナーの表示を更新
        leftButton.onClick.AddListener(() => { ShowPreviousBanner(); HighlightButton(leftButton); }); // 左ボタンにクリックイベントを追加
        rightButton.onClick.AddListener(() => { ShowNextBanner(); HighlightButton(rightButton); }); // 右ボタンにクリックイベントを追加
        closeButton.onClick.AddListener(CloseCarousel); // 閉じるボタンにクリックイベントを追加
        triggerButton.onClick.AddListener(ShowCarouselPanel); // トリガーボタンがクリックされたときにカルーセルPanelを表示するイベントを追加
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) // 左矢印キーが押された場合
        {
            ShowPreviousBanner(); // 前のバナーを表示
            HighlightButton(leftButton);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) // 右矢印キーが押された場合
        {
            ShowNextBanner(); // 次のバナーを表示
            HighlightButton(rightButton);
        }
    }

    // カルーセルPanelを表示するメソッド
    void ShowCarouselPanel()
    {
        if (!carouselPanel.activeSelf) // 既に表示されていない場合にのみ表示する
        {
            audioSource.PlayOneShot(sound1, 5.0f);
            carouselPanel.transform.localScale = Vector3.zero; // Panelのスケールをゼロに設定
            carouselPanel.SetActive(true); // Panelをアクティブに設定
            carouselPanel.transform.DOScale(Vector3.one, 0.5f); // DOTweenを使ってPanelを拡大するアニメーション
        }
    }

    // 前のバナーを表示するメソッド
    void ShowPreviousBanner()
    {
        audioSource.PlayOneShot(sound2, 1.5f);
        currentIndex = (currentIndex - 1 + banners.Length) % banners.Length; // インデックスを減少させ、配列の範囲内に収める
        UpdateBanners(); // バナーの表示を更新
    }

    // 次のバナーを表示するメソッド
    void ShowNextBanner()
    {
        audioSource.PlayOneShot(sound2, 1.5f);
        currentIndex = (currentIndex + 1) % banners.Length; // インデックスを増加させ、配列の範囲内に収める
        UpdateBanners(); // バナーの表示を更新
    }

    // 現在のインデックスに基づいてバナーの表示を更新するメソッド
    void UpdateBanners()
    {
        for (int i = 0; i < banners.Length; i++)
        {
            banners[i].SetActive(i == currentIndex); // 現在のインデックスのバナーのみを表示
        }
        currentPageText.text = $"{currentIndex + 1}/{banners.Length}"; // 現在のページ番号を更新
    }

    // カルーセルPanelを非表示にするメソッド
    void CloseCarousel()
    {
        audioSource.PlayOneShot(sound3, 0.15f);
        carouselPanel.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            carouselPanel.SetActive(false); // アニメーション終了後にPanelを非表示に設定
        });
    }

    // ボタンを一瞬ハイライトするメソッド
    void HighlightButton(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        Color originalColor = buttonImage.color;
        Color lightYellow = Color.yellow * 0.6f + Color.white * 0.4f;  // 薄い黄色を定義
        buttonImage.color = lightYellow;
        DOVirtual.DelayedCall(0.2f, () => buttonImage.color = originalColor);
    }
}