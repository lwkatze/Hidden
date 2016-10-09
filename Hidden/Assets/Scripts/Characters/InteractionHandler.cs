using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace App.Game.Utility
{
	public delegate void colResponder(object sender, Collision2D col, InteractionEventArgs args);
	public delegate void trigResponder(object sender, Collider2D trig, InteractionEventArgs args);
	public delegate void rayResponder(object sender, RaycastHit2D rayCast, InteractionEventArgs args);

	public class InteractionHandler : MonoBehaviour 
	{
		public event colResponder colFired;
		public event trigResponder trigFired;
		public event rayResponder rayHit;

		public bool colAnythingBut = false;
		public List<string> colCheckTags;
		public List<string> colCheckNames;

		public bool trigAnythingBut = false;
		public List<string> trigCheckTags;
		public List<string> trigCheckNames;

		public bool rayAnythingBut = false;
		public List<string> rayCheckTags;
		public List<string> rayCheckNames;

		public Transform rayEndpoint;
		public LayerMask raycastMask;
		public bool raycastAll;

		void OnCollisionEnter2D(Collision2D col)
		{
			Debug.Log("Collision detected");
			Dispatch(this, col, new InteractionEventArgs(eventType.Enter));
		}

		void OnCollisionStay2D(Collision2D col)
		{
			Dispatch(this, col, new InteractionEventArgs(eventType.Stay));
		}

		void OnCollisionExit2D(Collision2D col)
		{
			Dispatch(this, col, new InteractionEventArgs(eventType.Exit));
		}
			
		void OnTriggerEnter2D(Collider2D trig)
		{
			Dispatch(this, trig, new InteractionEventArgs(eventType.Enter));
		}

		void OnTriggerStay2D(Collider2D trig)
		{
			Dispatch(this, trig, new InteractionEventArgs(eventType.Stay)); 
		}

		void OnTriggerExit2D(Collider2D trig)
		{
			Dispatch(this, trig, new InteractionEventArgs(eventType.Exit));
		}

		void FixedUpdate()
		{
			if(rayEndpoint != null)
			{
				RaycastHit2D hit;

				Vector2 direction = (transform.position.x < rayEndpoint.position.x)? Vector2.left : Vector2.right;
				float distance = Mathf.Sqrt(Mathf.Pow(transform.position.x - rayEndpoint.position.x, 2f) + Mathf.Pow(transform.position.y - rayEndpoint.position.y, 2));

				if(raycastAll)
					hit = Physics2D.Linecast(transform.position, rayEndpoint.position);

				else
					hit = Physics2D.Raycast(transform.position, rayEndpoint.position, raycastMask);

				if(hit.collider != null)
				{
					Dispatch(this, hit, new InteractionEventArgs(eventType.Nothing));
				}
			}
		}
			
		void Dispatch(object sender, Collision2D col, InteractionEventArgs args)
		{
			bool condition = false;

			if(colCheckTags != null)
			{
				foreach(string str in colCheckTags)
				{
					if(str == col.gameObject.tag)
					{
						condition = true;
						break;
					}
				}
			}

			if(colCheckNames != null)
			{
				foreach(string str in colCheckNames)
				{
					if(str == col.gameObject.name)
					{
						condition = true;
						break;
					}
				}
			}

			if(colFired != null && condition == true)
				colFired(this, col, args);
		}

		void Dispatch(object sender, Collider2D trig, InteractionEventArgs args)
		{
			bool condition = false;

			if(trigCheckTags != null)
			{
				foreach(string str in trigCheckTags)
				{
					if(str == trig.gameObject.tag)
					{
						condition = true;
						break;
					}
				}
			}

			if(trigCheckNames != null)
			{
				foreach(string str in trigCheckNames)
				{
					if(str == trig.gameObject.name)
					{
						condition = true;
						break;
					}
				}
			}

			if(trigFired != null && condition == true)
				trigFired(this, trig, args);
		}

		void Dispatch(object sender, RaycastHit2D cast, InteractionEventArgs args)
		{
			bool condition = false;

			if(rayCheckTags != null)
			{
				foreach(string str in rayCheckTags)
				{
					if(str == cast.collider.tag)
					{
						condition = true;
						break;
					}
				}
			}

			if(rayCheckNames != null)
			{
				foreach(string str in rayCheckNames)
				{
					if(str == cast.collider.name)
					{
						condition = true;
						break;
					}
				}
			}

			if(rayHit != null && condition == true)
				rayHit(this, cast, args);
		}
	}

	public enum eventType
	{
		Enter, 
		Stay, 
		Exit,
		Nothing
	}

	public class InteractionEventArgs : System.EventArgs
	{
		public static InteractionEventArgs empty = new InteractionEventArgs();
		public eventType type { get; set; }

		public InteractionEventArgs(eventType _type = eventType.Nothing)
		{
			type = _type;
		}
	}
}