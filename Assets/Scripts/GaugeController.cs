using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GaugeController : MonoBehaviour
{
    // 体力ゲージの画像を設定
    [SerializeField] private Image healthImage;
    // 燃焼エフェクトの画像を設定
    [SerializeField] private Image burnImage;

    // アニメーションの持続時間
    public float duration = 0.5f;

    // 現在の体力率
    private float currentRate = 1f;

    private void Start()
    {
        // ゲージを初期値に設定
        SetGauge(1f);
    }

    // ゲージの値を設定するメソッド
    public void SetGauge(float value)
    {
        // DoTweenを使ってアニメーションを連結して動かす
        healthImage.DOFillAmount(value, duration)
            .OnComplete(() =>
            {
                burnImage.DOFillAmount(value, duration / 2f).SetDelay(0.5f);
            });

        // 現在の体力率を更新
        currentRate = value;
    }

    // ダメージを受けた際にゲージを減少させるメソッド
    public void TakeDamage(float rate)
    {
        SetGauge(currentRate - rate);
    }
}
