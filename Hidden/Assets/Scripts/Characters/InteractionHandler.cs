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

		public List<string> colCheckTags;
		public List<string> colCheckNames;

		public List<string> trigCheckTags;
		public List<string> trigCheckNames;

		public List<string> rayCheckTags;
		public List<string> rayCheckNames;

		void OnCollisionEnter2D(Collision2D col)
		{
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