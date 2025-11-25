using UnityEngine;
using UnityEngine.UI;

public class HpBarView : MonoBehaviour
{
    private Image _hpFillImg;
    private Witch _player;

    private void Awake()
    {
        _hpFillImg = GetComponent<Image>();
    }

    public void Init(Witch player)
    {
        _player = player;
        _player.OnHpChanged += OnHpUpdate;
        OnHpUpdate(_player.CurrentHp,_player.MaxHp);
    }
    private void OnDestroy()
    {
        if( _player != null )
        {
            _player.OnHpChanged -= OnHpUpdate;
        }
    }
    private void OnHpUpdate(int currnetHp, int maxHp)
    {
        float _fillAmount = (float)currnetHp / maxHp;

        _hpFillImg.fillAmount = _fillAmount;
    }
}
