using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public Sprite result1_1;
    public Sprite result1_2;
    public Sprite result2_1;
    public Sprite result2_2;
    public Sprite result3_1;
    public Sprite result3_2;
    public Sprite result4_1;
    public Sprite result4_2;
    public Sprite result5_1;
    public Sprite result5_2;
    public Sprite result6_1;
    public Sprite result6_2;
    public Sprite result7_1;
    public Sprite result7_2;
    public Sprite result8_1;
    public Sprite result8_2;
    public Sprite result9_1;
    public Sprite result9_2;
    public Sprite result10_1;
    public Sprite result10_2;
    public Sprite result11_1;
    public Sprite result11_2;
    public Sprite result12_1;
    public Sprite result12_2;
    public Sprite result13_1;
    public Sprite result13_2;
    public Sprite result14_1;
    public Sprite result14_2;
    public Sprite result15_1;
    public Sprite result15_2;
    public Sprite result16_1;
    public Sprite result16_2;
    public Sprite result17_1;
    public Sprite result17_2;
    public Sprite result18_1;
    public Sprite result18_2;
    public Image kaisekiResult;
    public Text keitaiso;
    public string kaisekiText;
    public string previousKaisekiText;

    // Start is called before the first frame update
    void Start()
    {
        kaisekiResult.sprite = null;
    }

    // Update is called once per frame
    void Update()
    {
        kaisekiText = GameManager.Instance.GetText();
        Debug.Log("kaisekiText: " + kaisekiText);

        if (kaisekiText != previousKaisekiText)
        {
            if (kaisekiText == "おーえんしてるよ！")
            {
                kaisekiResult.sprite = result1_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result1_2, 1.0f, 7));
            }
            else if (kaisekiText == "君ならきっとやれる！")
            {
                kaisekiResult.sprite = result2_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result2_2, 1.0f, 5));
            }
            else if (kaisekiText == "絶対ぜったい諦めるな！")
            {
                kaisekiResult.sprite = result3_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result3_2, 1.0f, 5));
            }
            else if (kaisekiText == "君の力を信じてるよっ")
            {
                kaisekiResult.sprite = result4_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result4_2, 1.0f, 8));
            }
            else if (kaisekiText == "いっけえええぇぇぇっ！")
            {
                kaisekiResult.sprite = result5_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result5_2, 1.0f, 6));
            }
            else if (kaisekiText == "負けないでーーー！")
            {
                kaisekiResult.sprite = result6_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result6_2, 1.0f, 6));
            }
            else if (kaisekiText == "負けるな勇者さん！")
            {
                kaisekiResult.sprite = result7_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result7_2, 1.0f, 5));
            }
            else if (kaisekiText == "恐らくなんとかなるさ")
            {
                kaisekiResult.sprite = result8_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result8_2, 1.0f, 6));
            }
            else if (kaisekiText == "私たちが見守ってるよ！")
            {
                kaisekiResult.sprite = result9_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result9_2, 1.0f, 7));
            }
            else if (kaisekiText == "ささっとやっつけちゃって！")
            {
                kaisekiResult.sprite = result10_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result10_2, 1.0f, 6));
            }
            else if (kaisekiText == "めっちゃ応援してま～す")
            {
                kaisekiResult.sprite = result11_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result11_2, 1.0f, 5));
            }
            else if (kaisekiText == "ユーキャンドゥーイットです！")
            {
                kaisekiResult.sprite = result12_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result12_2, 1.0f, 3));
            }
            else if (kaisekiText == "雨垂れ石を穿つ。だよ")
            {
                kaisekiResult.sprite = result13_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result13_2, 1.0f, 7));

            }
            else if (kaisekiText == "(ドキドキドキドキ…)")
            {
                kaisekiResult.sprite = result14_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result14_2, 1.0f, 5));
            }
            else if (kaisekiText == "なかなかやるじゃあないか")
            {
                kaisekiResult.sprite = result15_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result15_2, 1.0f, 5));
            }
            else if (kaisekiText == "すっげぇ。。。")
            {
                kaisekiResult.sprite = result16_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result16_2, 1.0f, 6));
            }
            else if (kaisekiText == "う～んいい勝負だねー")
            {
                kaisekiResult.sprite = result17_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result17_2, 1.0f, 5));
            }
            else if (kaisekiText == "どっちが勝つかなあ？")
            {
                kaisekiResult.sprite = result18_1;
                StartCoroutine(ChangeSpriteAndPlaySound(result18_2, 1.0f, 6));
            }
            previousKaisekiText = kaisekiText;
        }
        Debug.Log("kaisekiResult: " + kaisekiResult.sprite);
    }

    private IEnumerator ChangeSpriteAndPlaySound(Sprite newSprite, float delay, int keitaisoNumber)
    {
        yield return new WaitForSeconds(delay);
        kaisekiResult.sprite = newSprite;
        GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(1.0f);
        keitaiso.text = $"形態素数：{keitaisoNumber}";
        GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(1.0f);
        keitaiso.text = $"形態素数：{keitaisoNumber}　　⇒　　攻撃力Xup";
        GetComponent<AudioSource>().Play();
    }

}
