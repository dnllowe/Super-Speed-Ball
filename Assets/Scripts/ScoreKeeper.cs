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
    oTimer timer;

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
    /// Returns ScoreKeeper's timer
    /// </summary>
    /// <returns></returns>
    public oTimer GetTimer() {
        return timer;
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

    void UpdateScore() {

    }

    void Awake() {
        timer = gameObject.AddComponent<oTimer>();
    }

    void Update() {
        // Reload same scene after designated time frame
        if (isGameOver && timer.HasReachedMark()) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Play next level. If at last level, go back to beginning
        if (isGameWon && timer.HasReachedMark()) {
            if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            } else {
                SceneManager.LoadScene(0);
            }
        }
    }
}

