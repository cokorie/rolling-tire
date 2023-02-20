using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;
    public static bool isGameStarted;
    public GameObject startingText;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        Time.timeScale = 1;
        isGameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }

        if (SwipeManager.tap && !isGameStarted)
        {
            isGameStarted = true;
            Destroy(startingText);
        }
    }
}
