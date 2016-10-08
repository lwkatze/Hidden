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
		private float distDeath
		{
			get{ return CharacterData.charaData.dist_death; }
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
		private bool prev_grounded;
		private float prev_yVelocity;

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

		void Animations()
		{
			data.anim.SetBool("Grounded", grounded);
			data.anim.SetBool("PrevGrounded", prev_grounded);
			data.anim.SetFloat("vSpeedPercent", prev_yVelocity);

			if(grounded && !prev_grounded)
			{
				
				data.anim.SetFloat("vSpeed", data.vOnLand.y);
			}
			else if(grounded && prev_grounded)
			{
				
			}
			else if(!grounded && prev_grounded)
			{
				
			}
			else if(!grounded && !prev_grounded)
			{
				
			}
			prev_yVelocity = data.yVelocityScaled;
			prev_grounded = grounded;
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
				data.rgbody.AddForce(new Vector2(0, data.subtractPower(PowerPerJump)*jumpForce));
			}
		}

		[SerializeField] private float t_distance = 6.0f;	//total distance of teleport
		[SerializeField] private int t_Cost = 20;			//max power required for each full teleport
		[SerializeField] private float t_Power = 120f;			//this is the current amount of power in the battery (value seen is starting value)
		[SerializeField] private int t_MaxPower = 120;		//maximum "charge" of teleport battery, recharges over time
		[SerializeField] private int t_MinPower = 1;		//minimum charge required to teleport
		[SerializeField] private float t_RechargeRate = 10f;	//rate at which t_Power recharges

		public float t_scaleCost = 1;	//scales the cost variable, varies from 0 to 1
		public float t_Percent;		//used by HUD to see percent of total charge

		void Teleport()
		{
			Vector3 t_Vector;
			//teleport
			if (t_Power >= t_MinPower) //can still teleport if t_MaxPower < t_Power... 
			{
				t_Vector = new Vector3(t_distance * t_scaleCost * Input.GetAxis("TeleportHorizontal"), 0f, 0f); //horizontal
				data.player.transform.position += t_Vector;
				//change values
				if(t_Power >= t_Cost)
				{
					t_scaleCost = t_Cost/t_Cost;
					t_Power = t_Power - t_Cost;
				}
				else if(t_Power < t_Cost && t_Power > 0)
				{
					t_scaleCost = t_Power/t_Cost;
					t_Power = t_Power - t_scaleCost*t_Cost;
				}
			}
			if(Input.GetButtonDown ("TeleportVertical") && t_Power >= t_MinPower) //...just won't go as far
			{
				t_Vector = new Vector3(0f, t_distance * t_scaleCost * Input.GetAxis("TeleportVertical"), 0f); //vertical 
				data.player.transform.position += t_Vector;
				//change values
				if(t_Power >= t_Cost)
				{
					t_scaleCost = t_Cost/t_Cost;
					t_Power = t_Power - t_Cost;
				}
				else if(t_Power < t_Cost && t_Power > 0)
				{
					t_scaleCost = t_Power/t_Cost;
					t_Power = t_Power - t_scaleCost*t_Cost;
				}
			}
			if(t_Power >= t_Cost)
			{
				t_scaleCost = t_Cost/t_Cost;
			}
			else if(t_Power < t_Cost && t_Power > 0)
			{
				t_scaleCost = t_Power/t_Cost;
			}
			if(t_Power < 0)
			{
				t_Power = 0;		//Power can't drop below zero, can it? At least not for purposes of this game
			}
			//recharge t_Power slowly according to t_RechargeRate.  Change the variable's value accordingly.
			if(t_Power < t_MaxPower)
			{
				t_Power += t_RechargeRate*Time.deltaTime;//this will adapt the recharge time to stay consistent even with varying frame rates
			}
			//calculate percent
			t_Percent = t_Power / t_MaxPower;
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
