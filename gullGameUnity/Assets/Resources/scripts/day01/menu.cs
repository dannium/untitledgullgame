using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class menu : MonoBehaviour
{
    public GameObject fade;
    bool starting;

    bool onMenu = true;
    bool onPlay;

    public RectTransform menuScreen;
    public RectTransform playScreen;
    public RectTransform creditsScreen;

    Vector3 offScreen;
    Vector3 onScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetInt("mode", 0);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        offScreen = new Vector3(Screen.width / 2 - 6000, Screen.height / 2, 0);
        onScreen = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        if (starting)
        {
            fade.GetComponent<Image>().enabled = true;
            fade.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Lerp(fade.GetComponent<Image>().color.a, 1, 5*Time.deltaTime));
            if (fade.GetComponent<Image>().color.a > 0.999f)
            {
                if (PlayerPrefs.GetInt("mode") == 1)
                {
                    SceneManager.LoadScene("noSugMain");
                } else
                {
                    SceneManager.LoadScene("mainDay2");
                }
            }
        }


        menuScreen.position = Vector3.Lerp(menuScreen.position, (onMenu) ? onScreen : offScreen, Time.deltaTime*5);
        playScreen.position = Vector3.Lerp(playScreen.position, (!onMenu & onPlay) ? onScreen : offScreen, Time.deltaTime*5);
        creditsScreen.position = Vector3.Lerp(creditsScreen.position, (!onMenu & !onPlay) ? onScreen : offScreen, Time.deltaTime*5);
    }

    public void StartGame(int mode)
    {
        starting = true;
        PlayerPrefs.SetInt("mode", mode);
    }


    public void SwitchScreen(int play)
    {
        //doesn't let me use bools for some reason
        onMenu = (play == 1 || play == 3);
        onPlay = (play == 2 || play == 3);
    }


}