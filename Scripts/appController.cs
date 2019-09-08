using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class appController : MonoBehaviour
{
    public static string currentSelectedModel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void quit()
    {
        Application.Quit();
    }
    public void EricButton()
    {
        currentSelectedModel = "Eric";
    }
    public void ClaudiaButton()
    {
        currentSelectedModel = "Claudia";
    }
    public void changeLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
