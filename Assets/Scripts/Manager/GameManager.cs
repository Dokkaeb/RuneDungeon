using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    private Vector3 _respawnPoint;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Witch player = FindAnyObjectByType<Witch>();
        if (player != null && UIManager.Instance != null)
        {
            UIManager.Instance.InitUI(player);
        }

    }
    public void SetRespawnPoint(Vector3 newPoint)
    {
        _respawnPoint = newPoint;
    }
    public void RespawnPlayer(Witch player)
    {
        player.transform.position = _respawnPoint;
        player.Respawn();
        player.SetState(new IdleState(player));
    }
}
