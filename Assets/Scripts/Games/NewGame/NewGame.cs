﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// In this game, 
/// </summary>
public class NewGame : GameBase
{
	const string INSTRUCTIONS = "Press <color=cyan>Spacebar</color> as soon as you see the <color=green>GREEN</color> square.";
    /// <summary>
    /// Casses and the proper responses
    /// </summary>
	const string FINISHED = "FINISHED!";
	const string RESPONSE_GUESS = "No Guessing!";
    const string RESPONSE_INCORRECT = "Don't click red!";
    const string RESPONSE_CORRECT = "Good!";
	const string RESPONSE_TIMEOUT = "Missed it!";
	const string RESPONSE_SLOW = "Too Slow!";
	Color RESPONSE_COLOR_GOOD = Color.green;
	Color RESPONSE_COLOR_BAD = Color.red;

    /// <summary>
    /// A reference to the UI canvas so we can instantiate the feedback text.
    /// </summary>
    public GameObject uiCanvas;
	/// <summary>
	/// The object that will be displayed briefly to the player.
	/// </summary>
	public GameObject stimulus;
	/// <summary>
	/// A prefab for an animated text label that appears when a trial fails/succeeds.
	/// </summary>
	public GameObject feedbackTextPrefab;
	/// <summary>
	/// The instructions text label.
	/// </summary>
	public Text instructionsText;


	/// <summary>
	/// Called when the game session has started.
	/// </summary>
	public override GameBase StartSession(TextAsset sessionFile)
	{
		base.StartSession(sessionFile);

		//instructionsText.text = INSTRUCTIONS;
		StartCoroutine(RunTrials(SessionData));

		return this;
	}


	/// <summary>
	/// Iterates through all the trials, and calls the appropriate Start/End/Finished events.
	/// </summary>
	protected virtual IEnumerator RunTrials(SessionData data)
	{
		foreach (Trial t in data.trials)
		{
			StartTrial(t);
			yield return StartCoroutine(DisplayStimulus(t));
			EndTrial(t);
		}
		FinishedSession();
		yield break;
	}


	/// <summary>
	/// Displays the Stimulus for a specified duration.
	/// During that duration the player needs to respond as quickly as possible.
	/// </summary>
	protected virtual IEnumerator DisplayStimulus(Trial t)
	{
		GameObject stim = stimulus;
        
        stim.SetActive(false);
        /// <summary>
        /// If the trial isRandom then it will spawn in a random range between -225 - 225
        /// else it would go by value marked on the trial entry
        /// </summary>
        if (t.isRandom)
        {
            t.positionX = Random.Range(-225.0f, 225.0f);
            t.positionY = Random.Range(-225.0f, 225.0f);
            stim.transform.Translate(t.positionX, t.positionY, 0.0f); 
        }
        else
        {
            stim.transform.Translate(t.positionX, t.positionY, 0.0f);
        }
        ///<summary>
        /// If the trial isRed then it will make the stimulus red
        /// else it will be green and valid
        ///</summary>
        if (t.isRed)
        {
            stim.GetComponent<Image>().color = RESPONSE_COLOR_BAD;
        }
        else
        {
            stim.GetComponent<Image>().color = RESPONSE_COLOR_GOOD;
        }
		yield return new WaitForSeconds(t.delay);

		StartInput();
		stim.SetActive(true);
        yield return new WaitForSeconds(((NewGameTrial)t).duration);
		stim.SetActive(false);
		EndInput();

		yield break;
	}


	/// <summary>
	/// Called when the game session is finished.
	/// e.g. All session trials have been completed.
	/// </summary>
	protected override void FinishedSession()
	{
		base.FinishedSession();
		instructionsText.text = FINISHED;
	}


	/// <summary>
	/// Called when the player makes a response during a Trial.
	/// StartInput needs to be called for this to execute, or override the function.
	/// </summary>
	public override void PlayerResponded(KeyCode key, float time)
	{
		if (!listenForInput)
		{
			return;
		}
		base.PlayerResponded(key, time);
		if (key == KeyCode.Space)
		{
			EndInput();
			AddResult(CurrentTrial, time);
		}
	}


	/// <summary>
	/// Adds a result to the SessionData for the given trial.
	/// </summary>
	protected override void AddResult(Trial t, float time)
	{
		TrialResult r = new TrialResult(t);
		r.responseTime = time;
        // stim isRed and times out
        // Result: Correct response
        if (time == 0 && t.isRed)
        {
            DisplayFeedback(RESPONSE_CORRECT, RESPONSE_COLOR_GOOD);
            GUILog.Log("Success! Didn't click red! responseTime = {0}", time);
            sessionData.results.Add(r);
            return;
        }
        // If not red and times out
        // Result: Fail. No response
        if (time == 0 && !t.isRed)
		{
			// No response.
			DisplayFeedback(RESPONSE_TIMEOUT, RESPONSE_COLOR_BAD);
			GUILog.Log("Fail! No response!");
		}
		else
		{
            // Player Guesses
			if (IsGuessResponse(time))
			{
				// Responded before the guess limit, aka guessed.
				DisplayFeedback(RESPONSE_GUESS, RESPONSE_COLOR_BAD);
				GUILog.Log("Fail! Guess response! responseTime = {0}", time);
			}

            // Player responds in time and it is not red
			else if (IsValidResponse(time) && !t.isRed)
			{
				// Responded correctly.
				DisplayFeedback(RESPONSE_CORRECT, RESPONSE_COLOR_GOOD);
				r.success = true;
				r.accuracy = GetAccuracy(t, time);
				GUILog.Log("Success! responseTime = {0}", time);
			}

            // player responds and it is red
            else if (IsValidResponse(time) && t.isRed)
            {
                // Responded correctly but wrong color
                DisplayFeedback(RESPONSE_INCORRECT, RESPONSE_COLOR_BAD);
                r.success = false;
                r.accuracy = GetAccuracy(t, time);
                GUILog.Log("Fail! responseTime = {0}", time);
            }
            
            // No responce
            else
			{
                // Catch in case it is red and gets to here
                if (t.isRed)
                {
                    DisplayFeedback(RESPONSE_CORRECT, RESPONSE_COLOR_GOOD);
                    GUILog.Log("Success! Didn't click red! responseTime = {0}", time);
                }
                // No response and fail
                else
                {
                    DisplayFeedback(RESPONSE_SLOW, RESPONSE_COLOR_BAD);
                    GUILog.Log("Fail! Slow response! responseTime = {0}", time);
                }
			}
		}
		sessionData.results.Add(r);
	}


	/// <summary>
	/// Display visual feedback on whether the trial has been responded to correctly or incorrectly.
	/// </summary>
	private void DisplayFeedback(string text, Color color)
	{
		GameObject g = Instantiate(feedbackTextPrefab);
		g.transform.SetParent(uiCanvas.transform);
		g.transform.localPosition = feedbackTextPrefab.transform.localPosition;
		Text t = g.GetComponent<Text>();
		t.text = text;
		t.color = color;
	}


	/// <summary>
	/// Returns the players response accuracy.
	/// The perfect accuracy would be 1, most inaccuracy is 0.
	/// </summary>
	protected float GetAccuracy(Trial t, float time)
	{
		NewGameData data = sessionData.gameData as NewGameData;
		bool hasResponseTimeLimit =  data.ResponseTimeLimit > 0;

		float rTime = time - data.GuessTimeLimit;
		float totalTimeWindow = hasResponseTimeLimit ? 
			data.ResponseTimeLimit : (t as NewGameTrial).duration;

		return 1f - (rTime / (totalTimeWindow - data.GuessTimeLimit));
	}


	/// <summary>
	/// Returns True if the given response time is considered a guess.
	/// </summary>
	protected bool IsGuessResponse(float time)
	{
        NewGameData data = sessionData.gameData as NewGameData;
		return data.GuessTimeLimit > 0 && time < data.GuessTimeLimit;
	}


	/// <summary>
	/// Returns True if the given response time is considered valid.
	/// </summary>
	protected bool IsValidResponse(float time)
	{
		NewGameData data = sessionData.gameData as NewGameData;
		return data.ResponseTimeLimit <= 0 || time < data.ResponseTimeLimit;
	}
}
