using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private OVRScreenFade _screenFade;
    [SerializeField] private string _sceneName = "Cutscene";

    private bool _fading = false;

    private void Start() {
        Time.timeScale = 1;
        _screenFade = FindObjectOfType<OVRScreenFade>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadNextScene();
        }
    }

    public void LoadNextScene()
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
        SceneManager.LoadScene(_sceneName);
    }
}
