using UnityEngine;

public class Warp : MonoBehaviour
{
    [SerializeField] Transform _warpPoint;
      
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Transform _playerPosition = other.GetComponent<Transform>();

            if (_playerPosition != null && _warpPoint != null)
            {
                _playerPosition.position = _warpPoint.position;

            }

        }
    }
    

}
