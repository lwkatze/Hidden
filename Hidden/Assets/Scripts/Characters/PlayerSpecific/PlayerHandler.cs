//this handles how the player responds to interactions from outside entities

using UnityEngine;
using System.Collections;
using App.Game.Utility;

public class PlayerHandler : MonoBehaviour {

	//instance of InteractionHandler to receive events
	private InteractionHandler handler;

	void Start () 
	{
		handler = this.GetComponent<InteractionHandler>();

		if(!handler)
			handler = gameObject.AddComponent<InteractionHandler>();

		subscribeEvents();
	}

	private void colResponse(object sender, Collision2D col, InteractionEventArgs e)
	{
		Debug.Log("col detected in playerHandler");
	}

	private void subscribeEvents()
	{
		Debug.Log("heya");
		handler.colFired += new colResponder(colResponse);
	}
}
