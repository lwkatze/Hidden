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
		}

		protected override void Update()
		{
			if(move == true)
				translatePosition();
		}

		protected override void OnCollision2D(Collision2D col)
		{

		}

		public override void translatePosition ()
		{
			line.SetPositions(setPositions(initPos, transform.position));
			base.translatePosition ();
			Debug.Log("Angle: " + angle);
		}

		void OnCollisionEnter2D(Collision2D col)
		{
			if(col.gameObject.tag == "Untagged" || col.gameObject.tag == "Pipe" || col.gameObject.tag == "GrappleObject")
				base.stop();
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
