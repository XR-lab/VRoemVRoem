using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public GameObject gameOverUI, policarProp, policecarPropNextPosition, headLightsNextPosition;
    
    public int objectHitCounter;
    public bool playerCrashed;


    [SerializeField] private int possibleHits;

    public UnityEvent normalChaseEvent, firstHitEvent, secondHitEvent, lastHitEvent;

    private Vector3 policarPropStartLocation;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        policarPropStartLocation = policarProp.transform.position;

        if (normalChaseEvent == null) { new UnityEvent(); }
        if (firstHitEvent == null) { new UnityEvent(); }
        if (secondHitEvent == null) { new UnityEvent(); }
        if (lastHitEvent == null) { new UnityEvent(); }

        firstHitEvent.AddListener(FirstHit);
        secondHitEvent.AddListener(SecondHit);
        lastHitEvent.AddListener(LastHit);
    }

    // Update is called once per frame
    void Update()
    {
        switch (objectHitCounter) 
        {
            case 1:
                firstHitEvent.Invoke();
                break;
            case 2:
                secondHitEvent.Invoke();
                break;
            case 3:
                lastHitEvent.Invoke();
                break;
        }
    }


    void FirstHit()
    {
        policarProp.transform.position = Vector3.Lerp(policarProp.transform.position, headLightsNextPosition.transform.position, 1 * Time.deltaTime);


    }

    void SecondHit()
    {
        policarProp.transform.position = Vector3.Lerp(policarProp.transform.position, policecarPropNextPosition.transform.position, 1 * Time.deltaTime);   
    }


    
    void LastHit()
    {

        gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }



    public void RestartGame()
    {
        Debug.Log("kaas");

        SceneManager.LoadScene(0);
    }

    







}
