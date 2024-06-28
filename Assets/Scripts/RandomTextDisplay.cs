using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomTextDisplay : MonoBehaviour
{
    public TextMeshProUGUI buttonText; // �{�^����TextMeshPro�̃e�L�X�g�v�f

    // �����_���ȃe�L�X�g�̃��X�g
    private string[] randomTexts = {
        "����΂�",
        "�Ȃ����",
        "�撣����",
        "������߂Ȃ���",
        "�t�@�C�g�I",
    };

    // Start is called before the first frame update
    void Start()
    {
        DisplayRandomText();
    }

    // �����_���ȃe�L�X�g��\�����郁�\�b�h
    void DisplayRandomText()
    {
        // �����_���ȃC���f�b�N�X�𐶐�
        int randomIndex = Random.Range(0, randomTexts.Length);

        // �{�^���̃e�L�X�g��ݒ�
        buttonText.text = randomTexts[randomIndex];
    }
}
