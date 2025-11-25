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
