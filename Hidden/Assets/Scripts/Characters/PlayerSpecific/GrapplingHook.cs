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
		private LineRenderer line;

		public Vector2 grappleDirection = Vector2.up;

		protected override void Start()
		{
			base.Start();
			doLine();
		}

		protected override void Update()
		{
			if(move == true)
				translatePosition();

			data.grappleLocked = locked;
		}
			
		public override void translatePosition ()
		{
			line.SetPositions(setPositions(initPos + new Vector2(0f, -0.5f), transform.position));
			base.translatePosition ();
		}

		protected override void OnCollisionEnter2D(Collision2D col)
		{
			if(col.gameObject.name == "Pipes" || col.gameObject.name == "Pipe" || col.gameObject.tag == "GrappleObject")
			{
				base.stop();
				locked = true;
			}
		}

		private Vector3[] setPositions(Vector3 v1, Vector3 v2)
		{
			return new Vector3[2]{v1, v2};
		}

		void colResponse(object sender, Collision2D col, InteractionEventArgs args)
		{
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
