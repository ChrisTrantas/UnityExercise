using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Linq;


/// <summary>
/// A trial is when a player has to respond to a situation, 
/// which becomes marked as a success or failure depending on the player's response.
/// </summary>
public class Trial
{

	#region ATTRIBUTES

	public const string ATTRIBUTE_IS_GO = "isGo";
	public const string ATTRIBUTE_DELAY = "delay";
    public const string ATTRIBUTE_POSX = "posX";
    public const string ATTRIBUTE_POSY = "posY";
    public const string ATTRIBUTE_IS_RED = "isRed";
    public const string ATTRIBUTE_IS_RANDOM = "isRandom";

	#endregion


	/// <summary>
	/// A delay before the Trial begins.
	/// </summary>
	public float delay = 0;
    public float positionX;
    public float positionY;
    public bool isRed;
    public bool isRandom;


	public Trial(SessionData data, XmlElement n)
	{
		ParseGameSpecificVars(n, data);
	}
	

	/// <summary>
	/// Parses Game specific variables for this Trial from the given XmlElement.
	/// If no parsable attributes are found, or fail, then it will generate some from the given GameData.
	/// Used when parsing a Trial that IS defined in the Session file.
	/// </summary>
	public virtual void ParseGameSpecificVars(XmlNode n, SessionData data)
	{
		XMLUtil.ParseAttribute(n, ATTRIBUTE_DELAY, ref delay);
        XMLUtil.ParseAttribute(n, ATTRIBUTE_POSX, ref positionX);
        XMLUtil.ParseAttribute(n, ATTRIBUTE_POSY, ref positionY);
        XMLUtil.ParseAttribute(n, ATTRIBUTE_IS_RED, ref isRed);
        XMLUtil.ParseAttribute(n, ATTRIBUTE_IS_RANDOM, ref isRandom);
    }

	
	/// <summary>
	/// Writes any tracked variables to the given XElement.
	/// </summary>
	public virtual void WriteOutputData(ref XElement elem)
	{
		XMLUtil.CreateAttribute(ATTRIBUTE_DELAY, delay.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_POSX, positionX.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_POSY, positionY.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_IS_RED, isRed.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_IS_RANDOM, isRandom.ToString(), ref elem);
    }
}
