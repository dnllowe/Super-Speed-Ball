using UnityEngine;
using System.Collections;

/// <summary>
/// Keeps track of time in milliseconds
/// </summary>
public class oTimer : MonoBehaviour {
    
    /// <summary>
    /// Whether timer has started
    /// </summary>
    bool isTicking = false;

    /// <summary>
    /// Whether timer has been stopped after starting
    /// </summary>
    bool isPaused = false;

    /// <summary>
    /// Whether a mark has been set
    /// </summary>
    bool isMarkSet = false;

    /// <summary>
    /// Whether to unpause time when app enters foreground
    /// </summary>
    bool willUnpauseOnResume = true;

    /// <summary>
    /// Start time since game began
    /// </summary>
    int startTime = 0;

    /// <summary>
    /// Time when timer was stopped
    /// </summary>
    int stopTime = 0;

    /// <summary>
    /// stopTime - startTime
    /// </summary>
    int elapsedTime = 0;

    /// <summary>
    /// How much time was added to stop time to artificially increase time elapsed
    /// </summary>
    int increasedTime = 0;

    /// <summary>
    /// How much time was subtracted from stop time to artificially decrease time elapsed
    /// </summary>
    int decreasedTime = 0;

    /// <summary>
    /// Time marked for reference in future
    /// </summary>
    int mark = -1;

    /// <summary>
    /// Starts timer
    /// </summary>
    public void StartTimer() {
        //Only start timer if not running
        if (!isTicking) {
            //Turn the timer on
            isTicking = true;
            int time = (int)(Time.unscaledTime * 1000);
            //Start time should equal global time passed
            startTime = time;
        }
    }

    /// <summary>
    /// Stops timer
    /// </summary>
    public void StopTimer() {
        
        //Only stop the timer if running
        if (isTicking) {
            isTicking = false;
            int time = (int)(Time.unscaledTime * 1000);
            //Stop time should equal global time passed plus any time counted on timer
            stopTime = time + increasedTime - decreasedTime;
            elapsedTime += (stopTime - startTime);
            stopTime = 0;
            startTime = 0;
        }
    }

    /// <summary>
    /// Pause timer and specify if it should be unpaused when resuming app
    /// </summary>
    /// <param name="unpauseOnResumeInput">Whether timer should unpause when app resumes</param>
    public void PauseTimer(bool unpauseOnResumeInput = false) {
        if (isTicking) {
            StopTimer();
            isPaused = true;
            willUnpauseOnResume = unpauseOnResumeInput;
        }
    }

    /// <summary>
    /// Whether timer is set to unpause on resume
    /// </summary>
    /// <returns></returns>
    public bool WillUnpauseOnResume() {
        return willUnpauseOnResume;
    }

    /// <summary>
    /// Resume timer if it had been started
    /// </summary>
    public void UnpauseTimer() {
        if (isPaused) {
            StartTimer();
        }

        isPaused = false;
        willUnpauseOnResume = true;
    }

    /// <summary>
    /// Returns true if timer has started
    /// </summary>
    /// <returns>isTicking</returns>
    public bool IsRunning() {
        return isTicking;
    }

    /// <summary>
    /// Returns whether timer is paused
    /// </summary>
    /// <returns>isPaused</returns>
    public bool IsPaused() {
        return isPaused;
    }

    /// <summary>
    /// Returns time clocked while running or while stopped/paused
    /// </summary>
    /// <returns>elapsedTime</returns>
    public int GetElapsedTime() {

        int time = (int)(Time.unscaledTime * 1000);
        int elapse = time - startTime + increasedTime - decreasedTime;

        if (isTicking) {
            return elapse + elapsedTime;
        } else {
            return elapsedTime;
        }
    }

    /// <summary>
    /// Returns time elapsed in seconds
    /// </summary>
    /// <returns>elapsedTime / 1000</returns>
    public int GetElapsedSeconds() {

        int time = (int)(Time.unscaledTime * 1000);
        int elapse = time - startTime + increasedTime - decreasedTime;

        if (isTicking) {
            return elapse + elapsedTime / 1000;
        } else {
            return elapsedTime / 1000;
        }
    }

    /// <summary>
    /// Returns clock value when timer started
    /// </summary>
    /// <returns>startTime</returns>
    public int GetStartTime() {
        return startTime;
    }

    //Returns clock value when timer stopped
    public int GetStopTime() {
        return stopTime + increasedTime - decreasedTime;
    }

    /// <summary>
    /// Resets startTime, stopTime, and elapsedTime to 0, stops clock
    /// </summary>
    public void ResetTimer() {
        startTime = (int)(Time.unscaledTime * 1000);
        stopTime = startTime;
        elapsedTime = 0;
        increasedTime = 0;
        decreasedTime = 0;
        isTicking = false;
        isPaused = false;
        isMarkSet = false;
        willUnpauseOnResume = true;
        mark = 0;
    }

    /// <summary>
    /// Resets startTime, stopTime, and elapsedTime to 0, clock continues running
    /// </summary>
    public void RestartTimer() {
        startTime = (int)(Time.unscaledTime * 1000);
        stopTime = startTime;
        elapsedTime = 0;
        increasedTime = 0;
        decreasedTime = 0;
        isTicking = true;
        isPaused = false;
        isMarkSet = false;
        mark = 0;
        willUnpauseOnResume = true;
    }

    /// <summary>
    /// Add ms to timeElapsed
    /// </summary>
    /// <param name="ms"></param>
    public void IncreaseTimeElapsed(int ms) {
        increasedTime += ms;
    }

    /// <summary>
    /// Set a future point in time to reference
    /// </summary>
    /// <param name="value">How many ms ahead of current time to reference</param>
    public void SetMark(int value) {

        //If no value set (default value is -1), set mark for current time
        if (value <= 0) {
            mark = GetElapsedTime();
        } else {
            mark = GetElapsedTime() + value;
        }
        isMarkSet = true;
    }

    /// <summary>
    /// Get the current mark. (-1 is default / unset value)
    /// </summary>
    /// <returns>mark</returns>
    public float GetMark() {
        return mark;
    }

    /// <summary>
    /// Returns whether timer has reached mark. If mark isn't set, throws exception
    /// </summary>
    /// <returns></returns>
    public bool HasReachedMark() {
        Debug.Assert(isMarkSet, "Timer mark not set.");
        if (GetElapsedTime() >= mark) {
            return true;
        } else {
            return false;
        }
    }

    /// <summary>
    /// Increase mark time by a number of ms
    /// </summary>
    /// <param name="moreTime">How many ms to add to mark</param>
    public void AddTimeToMark(int moreTime) {
        if (isMarkSet)
            mark += moreTime;
    }

    /// <summary>
    /// Whether mark has been set
    /// </summary>
    /// <returns>isMarkSet</returns>
    public bool IsMarkSet() {
        return isMarkSet;
    }

    /// <summary>
    /// Resets mark to defaul values (-1, isMarkSet = false)
    /// </summary>
    public void ResetMark() {
        mark = -1;
        isMarkSet = false;
    }

    /// <summary>
    /// Subtract ms from timeElapsed
    /// </summary>
    /// <param name="ms"></param>
    public void DecreaseTimeElapsed(int ms) {
        decreasedTime += ms;
    }
}
