using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingBlock : MonoBehaviour
{
    float movep = 0.0f;
    public float times=0.0f; //���b�ňړ���
    public float wait = 0.0f;//��~����
    bool isReverse = false;  //���]���邩

    Vector3 startPos; //�X�^�[�g�ʒu
    Vector3 endPos; //�ړ��ʒu

    //�v���C���[��������瓮���悤��
    //�������Ƃ���true�ɂ���
    public bool isMoveOn = false;

    //�������Ă邩�ǂ�������p
    public bool isMoving = true;

    //�ړ����������ʁix����y���j
    public float moveX = 3.0f;
    public float moveY = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�����ʒu���擾
        this.startPos = this.transform.position;
        this.endPos= new Vector2(startPos.x + this.moveX
                   , startPos.y + this.moveY);

        if (this.isMoveOn == true)
        {
            //�v���C���[���̂��Ă��瓮���ݒ�Ȃ̂�
            //�n�߂͓������Ȃ�
            this.isMoving = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(this.isMoving)
        {
            //�ړ��������v�Z
            float kyori
                = Vector2.Distance(startPos, endPos);
            //Debug.Log(kyori);
            float ds = kyori / times;//1�b�̈ړ�����
            float df = ds * Time.deltaTime;
            movep += df / kyori;
            if(isReverse)
            {
                //�t�ړ�
                transform.position
              = Vector2.Lerp(endPos, startPos, movep);
            }
            else
            {
                //���ړ�
                transform.position
              = Vector2.Lerp(startPos, endPos, movep);
            }
            if(movep>=1.0f)
            {
                movep = 0.0f;//�ړ��⊮�l�����Z�b�g
                this.isReverse=!this.isReverse;//�ړ��𔽓]
                this.isMoving = false;//�ړ���~
                if(isMoveOn==false)
                {
                    //��������ɓ����Ȃ��ݒ�Ȃ�
                    Invoke("Move", wait);//�ړ��t���O��x�����Ă��Ă�
                }
            }
        }
    }
    //�ړ��t���O��ON�ɂ���
    public void Move()
    {
        this.isMoving=true;
    }
    //�ړ��t���O��OFF�ɂ���
    public void Stop()
    {
        this.isMoving = false;
    }
    //�ړ��͈͕\��
    void OnDrawGizmosSelected()
    {
        Vector2 fromPos;
        if (startPos == Vector3.zero)
        {
            fromPos = this.transform.position;
        }
        else
        {
            fromPos = startPos;
        }
        //�ړ���
        Gizmos.DrawLine(fromPos
             , new Vector2(fromPos.x + moveX
                         , fromPos.y + moveY));
        //�X�v���C�g�̃T�C�Y
        Vector2 size = GetComponent<SpriteRenderer>().size;
        Gizmos.DrawWireCube(fromPos
                         , new Vector2(size.x, size.y));
        Vector2 toPos
        = new Vector3(fromPos.x + this.moveX
                    , fromPos.y + this.moveY);
        Gizmos.DrawWireCube(toPos
                         , new Vector2(size.x, size.y));
    }

    /// <summary>
    /// �����蔻��
    /// ���������Ƃ��ɌĂяo�����
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        //�Ԃ��������̂̃^�O���v���C���[���`�F�b�N
        if (collision.gameObject.tag == "Player")
        {
            //�ړ��u���b�N�̎q���ɂ���
            collision.transform.SetParent(this.transform);

            if(this.isMoveOn==true)
            {
                // ������瓮���ݒ肪ON�Ȃ�
                // �������n�߂�
                this.isMoving = true;
            }
        }
    }

    /// <summary>
    /// �����肨��������ɌĂяo�����
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerExit2D(Collider2D collision)
    {
        //�Ԃ��������̂̃^�O���v���C���[���`�F�b�N
        if (collision.gameObject.tag == "Player")
        {
            //�ړ��u���b�N�̎q�����͂���
            collision.transform.SetParent(null);
        }
    }

}
