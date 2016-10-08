/* This is the script that controls player movement.
 * It access the character data singleton to refernce
 * player attributes (health, state, etc)
 */

using UnityEngine;
using System.Collections;

namespace App.Game.Player
{
	public class PlayerMove : MonoBehaviour {


		public static PlayerMove playerMove;

		/// <summary>
		/// Access to the Characterdata singleton
		/// </summary>
		/// <value>The data.</value>
		private CharacterData data 
		{
			get { return CharacterData.charaData; }
		}

	#region Data

		public float PowerPerJump = 25f;
		private Vector2 moveVector;
		private bool facingRight;
		private float currentSpeed;
		private float h_axis;
		private float v_axis;

		private bool isDesktop
		{
			get{ return (Application.isMobilePlatform) ? false : true; }
		}
		//jumping and falling 
		private float jumpForce
		{
			get { return CharacterData.charaData.jumpForce ; }
		}
		private float groundRadius
		{
			get { return CharacterData.charaData.gndRadius ; }
		}
		private Transform groundCheck
		{
			get { return CharacterData.charaData.gndCheck ; }
		}
		private LayerMask WhatIsGround
		{
			get { return CharacterData.charaData.gndMask ; }
		}
		private bool grounded
		{
			get { return CharacterData.charaData.gndBool ; }
		}

	#endregion

	#region Unity_Methods

		void Awake()
		{
			if(playerMove == null)
			{
				playerMove = this;
			}
			if(playerMove != this)
			{
				Destroy(gameObject);
			}
		}

		void Update()
		{
			MoveUpdate();
			Animations();
		}

	#endregion

		void MoveUpdate ()
		{
			if(h_axis > 0 && !facingRight)
				Flip();
			if(h_axis < 0 && facingRight)
				Flip();
			
			KeyboardMove();
			moveVector = new Vector2(h_axis * data.moveSpeed, data.rgbody.velocity.y);

			if(data.rgbody.velocity.y <= -60.0f)
			{
				moveVector = new Vector2(moveVector.x, -60.0f);
			}

			data.rgbody.velocity = moveVector;
		}

		/// <summary>
		/// Will use this to update animations based on user movement inputs
		/// </summary>
		void Animations()
		{
			
		}
	#region Input

		void KeyboardMove()
		{
			if(Input.GetButtonDown("Jump"))
			{
				this.SendMessage("Jump");
			}
			if(Input.GetButton("Horizontal"))
			{
				h_axis = Input.GetAxis("Horizontal");
			}
			if(Input.GetButtonUp("Horizontal"))
			{
				h_axis = 0;
			}
		}
		void Move(float value)
		{
			Debug.Log("Move Called");
			h_axis = value;
		}
		void StopMove(float value)
		{
			h_axis = value;
		}
		void Jump ()
		{
			data.anim.SetTrigger("Jump");

			if(data.gndBool == true)
			{
				data.rgbody.AddForce(new Vector2(0, jumpForce));
			}
			else
			{
				data.rgbody.velocity = new Vector2(data.rgbody.velocity.x, 0);
				data.rgbody.AddForce(new Vector2(0, data.jumpForce));
			}
		}

	#endregion

	#region Other_Methods

		void Flip()
		{
			facingRight = !facingRight;
			Vector3 theScale = data.player.transform.localScale;
			theScale.x *= -1;
			data.player.transform.localScale = theScale;
		}

		public void overrideMove()
		{
			currentSpeed = h_axis = v_axis = 0;
		}

	#endregion
	}
}
