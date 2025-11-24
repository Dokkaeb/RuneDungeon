using UnityEngine;
using UnityEngine.UI;

public class KeypadButton : MonoBehaviour
{
    [SerializeField] int _buttonValue;
    [SerializeField] EnterPassword _passwordHandler;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    public void OnButtonPressed()
    {
        if(_passwordHandler != null)
        {
            _passwordHandler.AppendNum(_buttonValue, this._button);
        }
    }
}
