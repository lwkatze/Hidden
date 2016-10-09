//this handles how the player responds to interactions from outside entities

using UnityEngine;
using System.Collections;
using App.Game.Utility;

public class PlayerHandler : MonoBehaviour 
{
	//instance of InteractionHandler to receive events
	public InteractionHandler handler;

	void Start () 
	{
		subscribeEvents();
	}

	private void colResponse(object sender, Collision2D col, InteractionEventArgs e)
	{
		Debug.Log("col detected in playerHandler");
	}

	private void trigResponse(object sender, Collider2D trig, InteractionEventArgs e)
	{
		Debug.Log("trig detected in playerHandler");
	}
		
	private void subscribeEvents()
	{
		//handler.colFired += new colResponder(colResponse);
		handler.trigFired += new trigResponder(trigResponse);
	}

	private void OnActuator(ActuatorArgs args)
	{
		if(args.sender.tag == "Respawn")
			Debug.Log("Respawn");
	}
}
