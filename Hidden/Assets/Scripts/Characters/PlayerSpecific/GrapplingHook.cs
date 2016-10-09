//attach this to the grapple GameObject

using UnityEngine;
using System.Collections;
using App.Game.Object;
using App.Game.Utility;

namespace App.Game.Player
{
	public class GrapplingHook : Projectile 
	{
		private CharacterData data { get { return CharacterData.charaData; } }
		private InteractionHandler handler;
		private LineRenderer line;

		public Vector2 grappleDirection = Vector2.up;

		protected override void Start()
		{
			base.Start();

			handler = gameObject.GetComponent<InteractionHandler>();

			doLine();

			handler.colFired += new colResponder(colResponse);

			Debug.Log("Start Called");
		}

		protected override void Update()
		{
			Debug.Log("Move is: " + move);

			if(move == true)
				translatePosition();
		}

		protected override void OnCollisionEnter2D()
		{

		}
	
		public override void translatePosition ()
		{
			line.SetPositions(setPositions(initPos, transform.position));
			base.translatePosition ();
		}

		private Vector3[] setPositions(Vector3 v1, Vector3 v2)
		{
			return new Vector3[2]{v1, v2};
		}

		void colResponse(object sender, Collision2D col, InteractionEventArgs args)
		{
			Debug.Log("STOP");
			base.stop();
		}

		void doLine()
		{
			line = gameObject.GetComponent<LineRenderer>();

			if(line == null)
				line = gameObject.AddComponent<LineRenderer>();

			line.SetColors(Color.black, Color.black);
		}
	}
}
