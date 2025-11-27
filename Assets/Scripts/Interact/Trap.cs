using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]private int _damage = 1;
    private float _attackDelay = 1f;
    private float _nextAttack;

    private void OnTriggerStay2D(Collider2D other)
    {
        if(Time.time < _nextAttack)
        {
            return;
        }
        Witch player = other.GetComponent<Witch>();

        if(player != null)
        {
            player.TakeDamager(_damage,transform.position);
            _nextAttack = Time.time + _attackDelay;
        }
    }
}
