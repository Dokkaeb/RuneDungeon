using UnityEngine;

public class NextStage : MonoBehaviour, IInteractable
{

    public void Interact(Witch interactor)
    {
        SceneLoader.Instance.LoadSceneByIndex(2);
    }

}
