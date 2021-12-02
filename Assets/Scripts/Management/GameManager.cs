using UnityEngine;


public class GameManager : MonoBehaviour
{

    public GameObject completeLevelUI;
    public void completeLevel()
    {
        completeLevelUI.SetActive(true);
    }

}
    
