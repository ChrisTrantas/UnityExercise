﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;


/// <summary>
/// Contains all the data that is needed and generated by a single game session.
/// </summary>
public class SessionData
{
	// General Attributes.
	const string ATTRIBUTE_GAMETYPE = "gameType";
	const string ATTRIBUTE_SHUFFLE = "shuffle";

	/// <summary>
	/// The filename of the Session file.
	/// </summary>
	public string fileName = string.Empty;
	/// <summary>
	/// The GameType for this game, as specified in the session file.
	/// </summary>
	public GameType gameType = GameType.Unassigned;
	/// <summary>
	/// True if the session has been completed.
	/// </summary>
	public bool completed = false;
	/// <summary>
	/// True if the session output has been successfully written to an Xml document.
	/// </summary>
	public bool outputWritten = false;
	/// <summary>
	/// Indicates that the Trials have been shuffled.
	/// </summary>
	public bool shuffleTrials = false;
	/// <summary>
	/// Contains data specific to the GameType.
	/// </summary>
	public GameData gameData = null;
	/// <summary>
	/// Contains data for each trial that will be used by the game.
	/// </summary>
	public List<Trial> trials = new List<Trial>();
	/// <summary>
	/// Contains the results for all the trials completed in this session.
	/// </summary>
	public List<TrialResult> results = new List<TrialResult>();


	#region ACCESSORS

	/// <summary>
	/// Returns a string of the GameType.
	/// </summary>
	public string GameName
	{
		get
		{
			return gameType.ToString();
		}
	}
	/// <summary>
	/// Returns true if the session has started.
	/// </summary>
	public bool SessionStarted
	{
		get
		{
			return results.Count > 0;
		}
	}

	#endregion


	public void ParseElement(XmlElement elem)
	{
		XMLUtil.ParseAttribute(elem, ATTRIBUTE_GAMETYPE, ref gameType);
		XMLUtil.ParseAttribute(elem, ATTRIBUTE_SHUFFLE, ref shuffleTrials, true);
	}

	
	public void WriteOutputData(ref XElement elem)
	{
		XMLUtil.CreateAttribute(ATTRIBUTE_GAMETYPE, gameType.ToString(), ref elem);
		XMLUtil.CreateAttribute(ATTRIBUTE_SHUFFLE, shuffleTrials.ToString(), ref elem);
	}
}