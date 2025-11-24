using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class EnterPassword : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject _passwordPanel;
    [SerializeField] string _password = "1234";
    private int _passwordLength;
    private string _inputString;
    private List<Button> _pressedButtons = new List<Button>();

    private void Awake()
    {
        _passwordLength = _password.Length;
    }
    private void Start()
    {
        _passwordPanel.SetActive(false);
    }
    public void Interact(Witch interactor)
    {
        if(_passwordPanel != null)
        {
            _passwordPanel.SetActive(true);
            ResetAttempt();
        }
    }
    public void ClosePanel()
    {
        _passwordPanel.SetActive(false);
        ResetAttempt();
    }
    //버튼입력값 추가 메서드
    public void AppendNum(int num,Button button)
    {
        if(_passwordPanel == null || !_passwordPanel.activeSelf)
        {
            return;
        }
        //이미 눌린 버튼 못누르게
        if (_pressedButtons.Contains(button))
        {
            return;
        }
        //누른 버튼 입력값 받아오기
        _inputString += num.ToString();

        //누른 버튼 리스트에 추가시키고 눌린상태로 만들기
        if(button != null)
        {
            _pressedButtons.Add(button);
            button.interactable = false;
        }
        // 패스워드 길이랑 입력값 길이랑 같으면 맞는지 체크
        if(_inputString.Length == _passwordLength)
        {
            CheckPassword();
        }
    }
    // 입력받은 값 초기화 메서드
    private void ResetAttempt()
    {
        _inputString = "";

        //눌려져있던 버튼들 다시 누를수있게하기
        foreach(var btn in _pressedButtons)
        {
            if(btn != null)
            {
                btn.interactable = true;
            }
        }
        //리스트 비우기
        _pressedButtons.Clear();
    }
    //맞는지 아닌지 확인용 메서드
    public void CheckPassword()
    {
        if(_inputString == _password)
        {
            ClosePanel();
            //문열기
        }
        else
        {
            IncorrectPassword();
        }

    }
    private void IncorrectPassword()
    {
        ResetAttempt();
    }
}
