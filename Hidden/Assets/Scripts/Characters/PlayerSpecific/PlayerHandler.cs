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
		private GameObject hideObj;

		void Start () 
		{
			if(handler == null)
				Debug.LogError("You need to inlcude a InteractionHandler reference!");

			subscribeEvents();
		}

		void Update()
		{
			if(data.canInteract && data.interact > 0)
				interact();

			else if(data.canInteract && data.interact <= 0)
			{
				data.rend.sortingOrder = 0;
				data.rgbody.constraints = RigidbodyConstraints2D.FreezeRotation;
			}
			
		}

		private void interact()
		{
			transform.position = hideObj.transform.position;
			data.rend.sortingOrder = data.hideSortLayer;
			data.rgbody.constraints = RigidbodyConstraints2D.FreezeAll;
		}

		private void trigResponse(object sender, Collider2D trig, InteractionEventArgs e)
		{
			if(trig.gameObject.tag == "Locker" && e.type == eventType.Enter)
			{
				data.canInteract = true;
				hideObj = trig.gameObject;
				data.gameObject.layer = LayerMask.NameToLayer("IgnoreRaycast");
			}

			else if(trig.gameObject.tag == "Locker" && e.type == eventType.Exit)
			{
				data.canInteract = false;
				data.gameObject.layer = LayerMask.NameToLayer("Player");
			}
		}
			
		private void subscribeEvents()
		{
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