using UnityEngine;

public class EndTrigger : MonoBehaviour {
    public GameObject gameOverUI;
    private GameManager gameManager;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter ()
    {
        // gameManager.completeLevel();

        gameOverUI.SetActive(true);


    }

}
 