/*
 * This is the enemy AI scipt. It controls enemy movement, reaction to player, etc. 
 * 
 * -The speed is the walking speed of the emeny. 
 * -The patrolDistance is the distance to the right of the enemy's initial position that the enemy will walk before turning around.
 * -The startDist is a percent value that gives the enemy's starting position. A value of 0 puts the enemy at its inital position, 
 *  a value of 1 sets the enemy at the end of its patrol distance. A value of 0.5 gives halfway between start and end point of patrol.
 * -timeToReact gives the number in milliseconds it takes for the enemy to respond to detecting the player.
 * -startRight determines whether the enemy walks left or right initially.
 * 
 */

using UnityEngine;
using System.Collections;
using App.Game.Utility;
using App.Game.Player;

namespace App.Game.Enemy
{
	public class EnemyAI : MonoBehaviour 
	{
		//instance of Enemydata
		private EnemyData data;

		//instance of InteractionHandler to receive events
		private InteractionHandler handler;

		public Transform rayEndPoint;
		public float speed;		//speed at which guard walks
		public float runSpeed;  //speed at which guard runs
		public float patrolDistance;	//distance of each patrol cycle
		public float startDist;	//the start distance of the enemy as a percent between 
		public float timeToReact;	//time between when the guard sees player and reacts to player
		public float timeToGiveUp = 3f;
		public float escapeDistance;
		public float confuseDistance = 3f;
		public bool startRight = true;
		public bool rayCastAll = true;
		public LayerMask rayMask;

		private bool detectPlayer;
		private bool prevDetectPlayer;
		private bool pursue = false;
		private bool lockPursue = false;

		private Vector3 initPos; //the initial position on run
		private Transform player { get { return CharacterData.charaData.transform; } }//reference to the player

		private float h_axis = 1;

		private bool facingRight; //is the enemy currently facing right?

		void Awake()
		{
			data = gameObject.GetComponent<EnemyData>();

			if(data == null)
				Debug.LogError("You forgot to add an enemy data instance to this enemy.");
		}

		void Start () 
		{
			handler = this.GetComponent<InteractionHandler>();

			if(!handler)
				handler = gameObject.AddComponent<InteractionHandler>();

			subscribeEvents();
			initilizer();
		}

		void Update()
		{
			RayCast();
			AI();
		}

		#region Movement

		void AI()
		{
			if(!detectPlayer)
				patrol();

			else if(detectPlayer && !prevDetectPlayer)
				startPersuit();

			if(pursue)
				pursuit();

			prevDetectPlayer = detectPlayer;
		}

		void patrol()
		{
			data.rgbody.velocity = new Vector2(speed*h_axis, data.rgbody.velocity.y);

			if(transform.position.x <= initPos.x && facingRight)
				Flip();

			if(transform.position.x >= initPos.x + patrolDistance && !facingRight)
				Flip();
		}

		void startPersuit()
		{
			StartCoroutine(timePersuit(timeToReact));
		}

		void stopPersuit()
		{

		}

		void pursuit()
		{
			int direction = 1;

			if(Mathf.Abs(player.transform.position.x - transform.position.x) > confuseDistance)
				direction = (player.transform.position.x > transform.position.x)? 1 : -1;

			else
			{
				int temp = Random.Range(-1, 1);

				if(temp == 0)
					temp = 1;
				
				direction = temp/Mathf.Abs(temp);
				lockPursue = true;
				StartCoroutine(lockPursuit(timeToReact, direction));
			}

			if(!lockPursue)
				transform.Translate(runSpeed*direction*Time.deltaTime, 0, 0, Space.World);

			if(Mathf.Abs(player.transform.position.x - transform.position.x) > escapeDistance)
			{
				StartCoroutine(timeGiveUp(timeToGiveUp));
			}
		}

		IEnumerator timePersuit(float seconds)
		{
			yield return new WaitForSeconds(seconds);

			pursue = true;
		}

		IEnumerator timeGiveUp(float seconds)
		{
			yield return new WaitForSeconds(seconds);

			detectPlayer = false;
			pursue = false;
		}

		IEnumerator lockPursuit(float seconds, int direction)
		{
			transform.Translate(runSpeed*direction*Time.deltaTime, 0, 0, Space.World);

			yield return new WaitForSeconds(seconds);

			lockPursue = false;
		}

		#endregion

		#region Utility/Setup Functions

		void RayCast()
		{
			if(rayEndPoint != null)
			{
				RaycastHit2D hit;

				Vector2 direction = (transform.position.x < rayEndPoint.position.x)? Vector2.left : Vector2.right;
				float distance = Mathf.Sqrt(Mathf.Pow(transform.position.x - rayEndPoint.position.x, 2f) + Mathf.Pow(transform.position.y - rayEndPoint.position.y, 2));

				if(rayCastAll)
					hit = Physics2D.Linecast(transform.position, rayEndPoint.position);

				else
					hit = Physics2D.Raycast(transform.position, rayEndPoint.position, rayMask);

				if(hit.collider != null)
				{
					if(hit.transform.tag == "Player")
						detectPlayer = true;
				}
			}
		}

		void Flip()
		{
			facingRight = !facingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;

			h_axis *= -1;

			data.walk = (h_axis > 0)? 1 : ((h_axis < 0)? -1 : 0);
		}

		private void colResponse(object sender, Collision2D col, InteractionEventArgs e)
		{
			if(col.transform.tag == "SceneObject")
				Flip();		
		}

		private void trigResponse(object sender, Collider2D trig, InteractionEventArgs e)
		{
			
		}

		private void rayResponse(object sender, RaycastHit2D cast, InteractionEventArgs e)
		{
			if(cast.transform.tag == "Player")
			{
				detectPlayer = true;
			}
		}

		private void subscribeEvents()
		{
			handler.colFired += new colResponder(colResponse);
			handler.rayHit += new rayResponder(rayResponse);
			//handler.trigFired += new trigResponder(trigResponse);
		}
			
		private void initilizer()
		{
			initPos = transform.position;

			transform.position = new Vector2(initPos.x + (patrolDistance * startDist), transform.position.y);

			if(!startRight)
				Flip();
		}

		private IEnumerator waitSeconds(float sec) 
		{
			yield return new WaitForSeconds(sec);
		}

		#endregion
	}
}