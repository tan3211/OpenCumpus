using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class CarouselController : MonoBehaviour
{
    public GameObject carouselPanel; // �J���[�Z����Panel
    public GameObject[] banners; // �o�i�[�摜���i�[����z��
    public Button triggerButton; // �J���[�Z����\������g���K�[�{�^��
    public Button leftButton;    // ���{�^��
    public Button rightButton;   // �E�{�^��
    public Button closeButton;   // ����{�^��
    public Text currentPageText; // ���݂̃y�[�W�ԍ���\������e�L�X�g

    public AudioClip sound1;
    public AudioClip sound2;
    public AudioClip sound3;
    AudioSource audioSource;

    private int currentIndex = 0; // ���ݕ\�����Ă���o�i�[�̃C���f�b�N�X

    // Start is called before the first frame update
    void Start()
    {

        carouselPanel.SetActive(false); // �N�����ɃJ���[�Z��Panel���\���ɐݒ�
        UpdateBanners(); // �����o�i�[�̕\�����X�V
        leftButton.onClick.AddListener(() => { ShowPreviousBanner(); HighlightButton(leftButton); }); // ���{�^���ɃN���b�N�C�x���g��ǉ�
        rightButton.onClick.AddListener(() => { ShowNextBanner(); HighlightButton(rightButton); }); // �E�{�^���ɃN���b�N�C�x���g��ǉ�
        closeButton.onClick.AddListener(CloseCarousel); // ����{�^���ɃN���b�N�C�x���g��ǉ�
        triggerButton.onClick.AddListener(ShowCarouselPanel); // �g���K�[�{�^�����N���b�N���ꂽ�Ƃ��ɃJ���[�Z��Panel��\������C�x���g��ǉ�
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) // �����L�[�������ꂽ�ꍇ
        {
            ShowPreviousBanner(); // �O�̃o�i�[��\��
            HighlightButton(leftButton);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) // �E���L�[�������ꂽ�ꍇ
        {
            ShowNextBanner(); // ���̃o�i�[��\��
            HighlightButton(rightButton);
        }
    }

    // �J���[�Z��Panel��\�����郁�\�b�h
    void ShowCarouselPanel()
    {
        if (!carouselPanel.activeSelf) // ���ɕ\������Ă��Ȃ��ꍇ�ɂ̂ݕ\������
        {
            audioSource.PlayOneShot(sound1, 5.0f);
            carouselPanel.transform.localScale = Vector3.zero; // Panel�̃X�P�[�����[���ɐݒ�
            carouselPanel.SetActive(true); // Panel���A�N�e�B�u�ɐݒ�
            carouselPanel.transform.DOScale(Vector3.one, 0.5f); // DOTween���g����Panel���g�傷��A�j���[�V����
        }
    }

    // �O�̃o�i�[��\�����郁�\�b�h
    void ShowPreviousBanner()
    {
        audioSource.PlayOneShot(sound2, 1.5f);
        currentIndex = (currentIndex - 1 + banners.Length) % banners.Length; // �C���f�b�N�X�����������A�z��͈͓̔��Ɏ��߂�
        UpdateBanners(); // �o�i�[�̕\�����X�V
    }

    // ���̃o�i�[��\�����郁�\�b�h
    void ShowNextBanner()
    {
        audioSource.PlayOneShot(sound2, 1.5f);
        currentIndex = (currentIndex + 1) % banners.Length; // �C���f�b�N�X�𑝉������A�z��͈͓̔��Ɏ��߂�
        UpdateBanners(); // �o�i�[�̕\�����X�V
    }

    // ���݂̃C���f�b�N�X�Ɋ�Â��ăo�i�[�̕\�����X�V���郁�\�b�h
    void UpdateBanners()
    {
        for (int i = 0; i < banners.Length; i++)
        {
            banners[i].SetActive(i == currentIndex); // ���݂̃C���f�b�N�X�̃o�i�[�݂̂�\��
        }
        currentPageText.text = $"{currentIndex + 1}/{banners.Length}"; // ���݂̃y�[�W�ԍ����X�V
    }

    // �J���[�Z��Panel���\���ɂ��郁�\�b�h
    void CloseCarousel()
    {
        audioSource.PlayOneShot(sound3, 0.15f);
        carouselPanel.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            carouselPanel.SetActive(false); // �A�j���[�V�����I�����Panel���\���ɐݒ�
        });
    }

    // �{�^������u�n�C���C�g���郁�\�b�h
    void HighlightButton(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        Color originalColor = buttonImage.color;
        Color lightYellow = Color.yellow * 0.6f + Color.white * 0.4f;  // �������F���`
        buttonImage.color = lightYellow;
        DOVirtual.DelayedCall(0.2f, () => buttonImage.color = originalColor);
    }
}