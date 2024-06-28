using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomTextDisplay : MonoBehaviour
{
    public TextMeshProUGUI buttonText; // ボタンのTextMeshProのテキスト要素

    // ランダムなテキストのリスト
    private string[] randomTexts = {
        "がんばれ",
        "なけるな",
        "頑張って",
        "あきらめないで",
        "ファイト！",
    };

    // Start is called before the first frame update
    void Start()
    {
        DisplayRandomText();
    }

    // ランダムなテキストを表示するメソッド
    void DisplayRandomText()
    {
        // ランダムなインデックスを生成
        int randomIndex = Random.Range(0, randomTexts.Length);

        // ボタンのテキストを設定
        buttonText.text = randomTexts[randomIndex];
    }
}
