/* This is the script that controls player movement.
 * It access the character data singleton to refernce
 * player attributes (health, state, etc)
 */

using UnityEngine;
using System.Collections;
using App.Game.Utility;

namespace App.Game.Player
{
	public class PlayerMove : MonoBehaviour {

		/// <summary>
		/// Static reference to playerMove singleton
		/// </summary>
		public static PlayerMove playerMove;

		/// <summary>
		/// Access to the Characterdata singleton
		/// </summary>
		/// <value>The data.</value>
		private CharacterData data 
		{
			get { return CharacterData.charaData; }
		}

		private bool overrrideNormalMove
		{
			get
			{
				return overrideNM;
			}
			set
			{
				if(data.grapple)
					overrideNM = false;

				else 
					overrideNM = true;
			}
		}

		private bool overrideNM;

	#region Data

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

		void Start()
		{
			
		}

		void Update()
		{
			Debug.Log(data.rgbody.velocity.x);

			Animations();
		}

		void LateUpdate()
		{
			MoveUpdate();
		}

	#endregion

		void MoveUpdate ()
		{
			if(h_axis > 0 && facingRight)
				Flip();
			if(h_axis < 0 && !facingRight)
				Flip();
			
			KeyboardMove();
			moveVector = new Vector2(h_axis * data.moveSpeed, data.rgbody.velocity.y);

			if(data.rgbody.velocity.y <= data.termVelocity)
			{
				moveVector = new Vector2(moveVector.x, data.termVelocity);
			}

			data.rgbody.velocity = moveVector;
		}

		/// <summary>
		/// Will use this to update animations based on user movement inputs
		/// </summary>
		void Animations()
		{
			if(!overrrideNormalMove)
			{
				if(h_axis > 0)
					data.anim.SetInteger("Walking", 1);
				
				else if(h_axis < 0)
					data.anim.SetInteger("Walking", -1);

				else 
					data.anim.SetInteger("Walking", 0);
			}
			else
			{
				data.anim.SetBool("Grapple", data.grapple);
			}

			data.anim.SetBool("Crouch", data.crouch);
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
			if(Input.GetButton("Grapple"))
			{
				data.grapple = true;
			}
			if(Input.GetButtonUp("Vertical"))
			{
				data.grapple = false;
			}
			if(Input.GetButton("Crouch"))
			{
				data.crouch = true;
			}
			if(Input.GetButtonUp("Crouch"))
			{
				data.crouch = false;
			}
			if(Input.GetButton("Interact"))
			{
				data.interact = !data.interact;
			}
		}

		void Grapple()
		{

		}

		void Crouch()
		{
			stopNormalMovement();
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
			Debug.Log("Jumping");
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
			//Vector3 theScale = data.player.transform.localScale;
			//theScale.x *= -1;
			//data.player.transform.localScale = theScale;
		}

		public void stopNormalMovement()
		{
			currentSpeed = h_axis = v_axis = 0;
			data.rgbody.velocity = new Vector2(0,0);
		}

	#endregion
	}
}
