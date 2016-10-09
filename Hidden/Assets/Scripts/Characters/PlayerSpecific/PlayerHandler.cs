//this handles how the player responds to interactions from outside entities

using UnityEngine;
using System.Collections;
using App.Game.Utility;

namespace App.Game.Player
{
	public class PlayerHandler : MonoBehaviour 
	{
		//instance of InteractionHandler to receive events
		public InteractionHandler handler;

		private CharacterData data { get { return CharacterData.charaData; } }

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
			{
				data.spawnPoints.Add(args.sender.transform.position);
			}
		}

		private void OnDeath(ActuatorArgs args)
		{
			//go to last spawn
			transform.position = data.spawnPoints[data.spawnPoints.Count - 1];
		}


	}
}