using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour {

    /// <summary>
    /// Whether ball has fallen below panel or not
    /// </summary>
    bool isGameOver = false;

    /// <summary>
    /// Whether ball has fallen below panel or not
    /// </summary>
    public bool IsGameOver {
        get { return isGameOver; }
        set { isGameOver = value; }
    }

    /// <summary>
    /// Whether play has collected all gems
    /// </summary>
    bool isGameWon = false;

    /// <summary>
    /// Whether play has collected all gems
    /// </summary>
    public bool IsGameWon {
        get { return isGameWon; }
        set { isGameWon = value; }
    }

    /// <summary>
    /// Times game over and game won events
    /// </summary>
    oTimer transitionTimer;

    /// <summary>
    /// Keeps track of time left in game
    /// </summary>
    oTimer gameTimer;

    /// <summary>
    /// Milliseconds remaining on game timer
    /// </summary>
    public int timeLeft = 15000;

    /// <summary>
    /// Parent for GUI elements
    /// </summary>
    public GameObject GUI;

    /// <summary>
    /// UI for score and multiplier
    /// </summary>
    public GUIElement scoreUI;

    /// <summary>
    /// Text for scoreUI
    /// </summary>
    public UnityEngine.UI.Text scoreText;

    /// <summary>
    /// UI for time remaining
    /// </summary>
    public GUIElement timeUI;

    /// <summary>
    /// Text for timeUI
    /// </summary>
    public UnityEngine.UI.Text timeText;

    /// <summary>
    /// How many milliseconds to wait until reloading scene after game over
    /// </summary>
    int restartTime = 2000;

    /// <summary>
    /// How many milliseconds to wait until reloading scene after game over
    /// </summary>
    public int RestartTime {
        get { return restartTime; }
    }

    /// <summary>
    /// Total gems in level
    /// </summary>
    int totalGems = 0;

    /// <summary>
    /// Total gems in level
    /// </summary>
    public int TotalGems {
        get { return totalGems; }
    }

    /// <summary>
    /// How many gems the player has collected
    /// </summary>
    int gemsCollected = 0;

    /// <summary>
    /// How many gems the player has collected
    /// </summary>
    public int GemsCollected {
        get { return gemsCollected; }
    }

    /// <summary>
    /// How many gems player has collected without ball hitting panel
    /// </summary>
    int consecutiveGemsCollected = 0;

    /// <summary>
    /// How many gems player has collected without ball hitting panel
    /// </summary>
    public int ConsecutiveGemsCollected {
        get { return consecutiveGemsCollected; }
    }

    /// <summary>
    /// How many points player receives for collecting gems
    /// </summary>
    int pointsPerGem = 100;

    /// <summary>
    /// Player's total score
    /// </summary>
    int score = 0;

    /// <summary>
    /// Player's total score
    /// </summary>
    public int Score {
        get { return score; }
    }

    /// <summary>
    /// Multiply by pointsPerGem to get score increase for each gem
    /// </summary>
    float multiplier = 1.0f;

    /// <summary>
    /// Returns ScoreKeeper's transition timer
    /// </summary>
    /// <returns></returns>
    public oTimer GetTransitionTimer() {
        return transitionTimer;
    }

    /// <summary>
    /// Returns ScoreKeeper's game timer
    /// </summary>
    /// <returns></returns>
    public oTimer GetGameTimer() {
        return gameTimer;
    }
    /// <summary>
    /// Increase totalGems value by 1
    /// </summary>
    public void IncreaseTotalGemsCount() {
        totalGems++;
    }

    /// <summary>
    /// Increase consecutiveGemsCollected by 1
    /// </summary>
    public void IncreaseConsecutiveGemsCollected() {
        consecutiveGemsCollected++;
    }

    /// <summary>
    /// Reset consecutiveGemsCollected to 0
    /// </summary>
    public void ResetConsecutiveGemsCollected() {
        consecutiveGemsCollected = 0;
    }

    /// <summary>
    /// Increase multiplier by 0.5
    /// </summary>
    public void IncreaseMultiplier() {
        multiplier += 0.5f;
    }

    /// <summary>
    /// Reset multiplier to 1
    /// </summary>
    public void ResetMultiplier() {
        multiplier = 1.0f;
    }

    /// <summary>
    /// Increase score according to multiplier
    /// </summary>
    public void IncreaseScore() {
        score += (int)((pointsPerGem * multiplier));
    }

    /// <summary>
    /// Increase value of gems collected by 1
    /// </summary>
    public void IncreaseGemsCollected() {
        gemsCollected++;
    }

    public void UpdateScore() {
        scoreText.text = string.Format("Score: {0}", score);
    }

    void Start() {

        scoreText = GameObject.Find(
            "GUI/Score").GetComponent<UnityEngine.UI.Text>();

        timeText = GameObject.Find(
            "GUI/Time").GetComponent<UnityEngine.UI.Text>();

        scoreText.text = "Score: 0";
        timeText.text = "00:15:00";
        gameTimer.StartTimer();
    }

    void Awake() {
        transitionTimer = gameObject.AddComponent<oTimer>();
        gameTimer = gameObject.AddComponent<oTimer>();
    }

    void Update() {

        // Count down time while game is still going
        if(!isGameOver && !isGameWon) {
            timeLeft -= gameTimer.GetElapsedTime();
            gameTimer.RestartTimer();

            int msLeft = (timeLeft % (60000) % 1000) / 10;
            int secLeft = timeLeft % (60000) / 1000;
            int minLeft = timeLeft / 60000;

            string msString;
            string secString;
            string minString;
 
            if(msLeft > 9) {
                msString = string.Format("{0}", msLeft);
            } else {
                msString = string.Format("0{0}", msLeft);
            }

            if(secLeft > 9) {
                secString = string.Format("{0}:", secLeft);
            } else {
                secString = string.Format("0{0}:", secLeft);
            }

            if(minLeft > 9) {
                minString = string.Format("{0}:", minLeft);
            } else {
                minString = string.Format("0{0}:", minLeft);
            }

            timeText.text = minString + secString + msString;
        }

        // Reload same scene after designated time frame
        if (isGameOver && transitionTimer.HasReachedMark()) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Play next level. If at last level, go back to beginning
        if (isGameWon && transitionTimer.HasReachedMark()) {
            if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            } else {
                SceneManager.LoadScene(0);
            }
        }
    }
}

