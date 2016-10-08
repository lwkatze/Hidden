using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace App.Game.Utility
{
	public delegate void colResponder(object sender, Collision2D col, System.EventArgs e);
	public delegate void trigResponder(object sender, Collider2D trig, System.EventArgs e);

	public class InteractionHandler : MonoBehaviour 
	{
		public event colResponder colFired;
		public event trigResponder trigFired;

		public List<string> checkTags;
		public List<string> checkNames;

		void OnCollisionEnter2D(Collision2D col)
		{
			Dispatch(this, col, System.EventArgs.Empty);
		}

		void OnCollisionStay2D(Collision2D col)
		{
			Dispatch(this, col, System.EventArgs.Empty);
		}

		void OnCollisionExit2D(Collision2D col)
		{
			Dispatch(this, col, System.EventArgs.Empty);
		}
			
		void OnTriggerEnter2D(Collider2D trig)
		{
			Dispatch(this, trig, System.EventArgs.Empty);
		}

		void OnTriggerStay2D(Collider2D trig)
		{
			Dispatch(this, trig, System.EventArgs.Empty); 
		}

		void OnTriggerExit2D(Collider2D trig)
		{
			Dispatch(this, trig, System.EventArgs.Empty);
		}

		void Dispatch(object sender, Collision2D col, System.EventArgs e)
		{
			bool condition = false;

			if(checkTags != null)
			{
				foreach(string str in checkTags)
				{
					if(str == col.gameObject.tag)
					{
						condition = true;
						break;
					}
				}
			}

			if(checkNames != null)
			{
				foreach(string str in checkNames)
				{
					if(str == col.gameObject.name)
					{
						condition = true;
						break;
					}
				}
			}

			if(colFired != null && condition == true)
				colFired(this, col, System.EventArgs.Empty);
		}

		void Dispatch(object sender, Collider2D trig, System.EventArgs e)
		{
			bool condition = false;

			if(checkTags != null)
			{
				foreach(string str in checkTags)
				{
					if(str == trig.gameObject.tag)
					{
						condition = true;
						break;
					}
				}
			}

			if(checkNames != null)
			{
				foreach(string str in checkNames)
				{
					if(str == trig.gameObject.name)
					{
						condition = true;
						break;
					}
				}
			}

			if(trigFired != null && condition == true)
				trigFired(this, trig, System.EventArgs.Empty);
		}
	}
}