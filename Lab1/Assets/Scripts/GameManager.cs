using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float worldSpeed;

    public int critterCounter;
    [SerializeField] private GameObject boss1;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        critterCounter = 0;
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) { 
            Pause();
        }

        if(critterCounter > 20)
        {
            critterCounter = 0;
            Instantiate(boss1, new Vector2(0, 5f), Quaternion.Euler(0,0,0));
        }
    }

    public void Pause()
    {
        if(UIController.Instance.pausePanel.activeSelf == false)
        {
            UIController.Instance.pausePanel.SetActive(true);
            Time.timeScale = 0f;
            AudioManager.Instance.PlaySound(AudioManager.Instance.pause);
        }
        else
        {
            UIController.Instance.pausePanel.SetActive(false);
            Time.timeScale = 1f;
            PlayerController.Instance.ExitBoost();
            AudioManager.Instance.PlaySound(AudioManager.Instance.unpause);
        }
    }

    public void QuitGame() { 
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver() {
        StartCoroutine(ShowGameOverScreen());
    }

    IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameOver");
    }

    public void SetWorldSpeed(float speed) { 
        worldSpeed = speed;
    }
}
