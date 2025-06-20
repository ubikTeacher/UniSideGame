using UnityEngine;
using UnityEngine.UI;

public class VirtualPad : MonoBehaviour
{
    public float MaxLength = 70;//�^�u�������ő勗��
    public bool is4DPad = false; //4�����p�b�h���ǂ���
    GameObject player; //�v���C���[�I�u�W�F�N�g
    Vector2 defPos;//�^�u�̏����ʒu
    Vector2 downPos;//�^�b�`�ʒu

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�v���C���[�L�����N�^�[���擾
        player = GameObject.FindGameObjectWithTag("Player");

        //�^�u�̏������W
        defPos = GetComponent<RectTransform>().localPosition;

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Down�C�x���g
    public void PadDown()
    {
        //�^�b�`�ʒu���擾
        downPos = Input.mousePosition;
    }

    //�h���b�O�C�x���g
    public void PadDrag()
    {
        //�^�b�`�ʒu���擾
        Vector2 mousePos = Input.mousePosition;
        //�^�u�̈ړ��������v�Z
        Vector2 newTabPos = mousePos - downPos;

        if (is4DPad == false)
        {
            newTabPos.y = 0;//���X�N���[���̏ꍇ��y��0
        }

        //�ړ��x�N�g�����v�Z
        Vector2 axis = newTabPos.normalized;

        //�Q�_�̋��������߂�
        float len = Vector2.Distance(defPos, newTabPos);

        if (len > MaxLength)
        {
            //���E�𒴂����ꍇ�͍ő勗���ɐݒ�
            newTabPos.x = axis.x * MaxLength;
            newTabPos.y = axis.y * MaxLength;
        }

        //�^�u�̈ʒu���X�V
        GetComponent<RectTransform>().localPosition = newTabPos;

        //�v���C���[�̈ړ�����
        player.GetComponent<PlayerController>().SetAxis(axis.x, axis.y);
    }

    public void PadUp()
    {
        //�^�u�̈ʒu�������ʒu�ɖ߂�
        GetComponent<RectTransform>().localPosition = defPos;
        //�v���C���[�̈ړ�����
        player.GetComponent<PlayerController>().SetAxis(0, 0);
    }
}
