using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GimmicBlock : MonoBehaviour
{
    /// <summary>
    /// �����������m����
    /// </summary>
    public float length = 0.0f;

    /// <summary>
    /// ������ɍ폜���邩�ǂ����t���O�iTrue:������ɍ폜)
    /// </summary>
    public bool isDelete = false;

    /// <summary>DEAD�I�u�W�F�N�g�i�[�p</summary>
    public GameObject deadObj;

    /// <summary>���������ǂ���(���߂�false)</summary>
    bool isFell = false;

    /// <summary>�t�F�[�h�A�E�g�̎���</summary>
    float fadeoutTime = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�X�^�[�g���ɗ������Ȃ��悤�ɁA
        //�d�͕��i�̃^�C�v��ύX���Ă���
        Rigidbody2D rbody = this.GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;
        
        //�����O�ɃM�~�b�N�̉����ɂӂ�Ă�
        //���ȂȂ��悤�ɂ��Ă���
        deadObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[�̃Q�[���I�u�W�F�N�g���擾
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            //�v���C���[�ƃM�~�b�N�u���b�N�̋������擾
            float kyori=Vector2.Distance(this.transform.position
                                    , player.transform.position);
            
            //Debug.Log(kyori);
            if(length>=kyori)
            {
                //�v���C���[���˒������ɓ������̂�
                //Rigidbody��bodyType���ēxDynamic�ɖ߂��I
                Rigidbody2D rbody = this.GetComponent<Rigidbody2D>();
                rbody.bodyType = RigidbodyType2D.Dynamic;

                //���݂Ԃ��ꂽ�玀�ʂ悤�ɐݒ�
                deadObj.SetActive(true);
            }
        }

        //�����������ǂ���
        if(this.isFell)
        {
            //���������ꍇ�́A�����ɂ��Ă���
            this.fadeoutTime -= Time.deltaTime;

            //�X�v���C�g�ɐݒ肳��Ă���
            //�F���擾���A�����̐ݒ�l��ύX����
            Color clr = this.GetComponent<SpriteRenderer>().color;
            
            //�����l�����X�Ɍ��炷
            clr.a = this.fadeoutTime;
            
            //�����l��0��菬�����Ȃ�����M�~�b�N������
            if(this.fadeoutTime<0.0f)
            {
                Destroy(gameObject);
            }
        }

    }

    /// <summary>
    /// ���������Ƃ��ɌĂяo�����
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("���������ȁH");
        if(this.isDelete)
        {
            //�����t���O���I���ɂ���
            this.isFell = true;
        }
    }

    //�͈͕\��
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position
                                , this.length);
    }

}
