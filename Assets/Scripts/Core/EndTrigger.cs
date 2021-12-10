using UnityEngine;
using XRLab.VRoem.Core;

public class EndTrigger : MonoBehaviour {
    public GameObject FinishUI;
    private GameManager gameManager;
    private Transform _cam;
    [SerializeField] private Transform _map;
    [SerializeField] private Transform _platform;
    [SerializeField] private Transform _ui;
    [SerializeField] private Transform _fog;
    [SerializeField] private Rigidbody _playerRb;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        _cam = GameObject.FindGameObjectWithTag(Tags.OVR).transform;
    }

    void OnTriggerEnter(Collider col)
    {
        // gameManager.completeLevel();

        FinishUI.SetActive(true);
        _cam.SetParent(_map);
        _platform.SetParent(_map);
        _ui.SetParent(_map);
        _fog.SetParent(_map);
        _playerRb.velocity = Vector3.zero;
        Invoke(nameof(FreezeGame), 3);
    }

    private void FreezeGame()
    {
        Time.timeScale = 0;
    }

}
 