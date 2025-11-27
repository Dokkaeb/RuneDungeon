using UnityEngine;

public class ChaseTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _enemy.SetActive(true);
        }
    }
}
