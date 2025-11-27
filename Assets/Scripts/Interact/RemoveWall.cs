using UnityEngine;

public class RemoveWall : MonoBehaviour
{
    [SerializeField] private GameObject _wall;

    private void OnEnable()
    {
        _wall.SetActive(false);
    }
}
