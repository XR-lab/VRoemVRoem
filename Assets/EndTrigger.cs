using UnityEngine;

public class EndTrigger : MonoBehaviour {
    public GameObject FinishUI;
    private GameManager gameManager;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter ()
    {
        // gameManager.completeLevel();

        FinishUI.SetActive(true);
        Time.timeScale = 0;

    }

}
 