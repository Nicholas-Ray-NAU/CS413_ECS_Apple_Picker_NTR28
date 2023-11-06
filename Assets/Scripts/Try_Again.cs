using UnityEngine;
using UnityEngine.SceneManagement;

public class Try_Again : MonoBehaviour
{
    public void PlayAgain()
    {
        Menu_Button.isHard = false;
        Menu_Button.isMedium = false;
        SceneManager.LoadScene("_Scene_Start");
    }
}
