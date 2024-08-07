using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GaugeController : MonoBehaviour
{
    // �̗̓Q�[�W�̉摜��ݒ�
    [SerializeField] private Image healthImage;
    // �R�ăG�t�F�N�g�̉摜��ݒ�
    [SerializeField] private Image burnImage;

    // �A�j���[�V�����̎�������
    public float duration = 0.5f;

    // ���݂̗̑͗�
    private float currentRate = 1f;

    private void Start()
    {
        // �Q�[�W�������l�ɐݒ�
        SetGauge(1f);
    }

    // �Q�[�W�̒l��ݒ肷�郁�\�b�h
    public void SetGauge(float value)
    {
        // DoTween���g���ăA�j���[�V������A�����ē�����
        healthImage.DOFillAmount(value, duration)
            .OnComplete(() =>
            {
                burnImage.DOFillAmount(value, duration / 2f).SetDelay(0.5f);
            });

        // ���݂̗̑͗����X�V
        currentRate = value;
    }

    // �_���[�W���󂯂��ۂɃQ�[�W�����������郁�\�b�h
    public void TakeDamage(float rate)
    {
        SetGauge(currentRate - rate);
    }
}
