using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private OVRScreenFade _screenFade;

    private bool _fading = false;

    private void Start() {
        Time.timeScale = 1;
        _screenFade = FindObjectOfType<OVRScreenFade>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if (_fading)
        {
            return;
        }

        _fading = true;
        _screenFade.FadeOut();
        Invoke(nameof(FadeDone), _screenFade.fadeTime);
    }

    private void FadeDone()
    {
        SceneManager.LoadScene(1);
    }
}
