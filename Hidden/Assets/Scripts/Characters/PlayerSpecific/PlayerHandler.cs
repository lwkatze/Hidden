//this handles how the player responds to interactions from outside entities

using UnityEngine;
using System.Collections;
using App.Game.Utility;

namespace App.Game.Player
{
	public class PlayerHandler : MonoBehaviour 
	{
		private CharacterData data { get { return CharacterData.charaData; } }
		private LevelChanger lvlChange { get { return LevelChanger.lvlChanger; } }

		//instance of InteractionHandler to receive events
		public InteractionHandler handler;

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

			if(transform.position.y < data.fallDeathDist)
				Death(deathType.Fall);
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
				data.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			}

			else if(trig.gameObject.tag == "Locker" && e.type == eventType.Exit)
			{
				data.canInteract = false;
				data.gameObject.layer = LayerMask.NameToLayer("Player");
			}

			if(trig.gameObject.tag == "Enemy") 
				Death(deathType.Enemy);

			if(trig.gameObject.tag == "Death")
				Death(deathType.Fall);
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

		private void Death(deathType type)
		{
			Debug.Log("Death");

			lvlChange.SendMessage("SceneLoad", sceneLoadType.reload, SendMessageOptions.RequireReceiver);
		}


	}

	public enum deathType {Enemy, Fall} 
}