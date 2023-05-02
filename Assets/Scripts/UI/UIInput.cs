using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInput : Singleton<UIInput>
{
    //������������
    [SerializeField] private PlayerInput playerInput;
    //����ϵͳ��UI����ģ�����
    private InputSystemUIInputModule UIInputModule;

    protected override void Awake()
    {
        base.Awake();
        //ȡ������ϵͳ��UI����ģ��
        UIInputModule = GetComponent<InputSystemUIInputModule>();
        //�Ƚ���UI�������
        UIInputModule.enabled = false;
    }
    //ѡ��UI����
    //Selectable�������пɱ�ѡ�е�Unity UI��Ļ���
    public void SelectUI(Selectable UIObject)
    {
        //ѡ��UI
        UIObject.Select();
        //��UI���õ���ȷ��״̬
        UIObject.OnSelect(null);
        //UI����ģ�����ã����Խ���UI����
        UIInputModule.enabled = true;
    }
    //��������UI���뺯��
    public void DisableAllUIInputs()
    {
        //���������������
        playerInput.DisableAllInputs();
        //��������ģ�����
        UIInputModule.enabled = false;
    }
}
