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

public class EnemyAI : MonoBehaviour 
{
	//instance of Enemydata
	private EnemyData data;

	//instance of InteractionHandler to receive events
	private InteractionHandler handler;

	public float speed;		//speed at which guard walks
	public float patrolDistance;	//distance of each patrol cycle
	public float startDist;	//the start distance of the enemy as a percent between 
	public float timeToReact;	//time between when the guard sees player and reacts to player

	public bool startRight = true;

	private Vector3 initPos; //the initial position on run

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
		patrol();
	}

	#region Movement

	void patrol()
	{
		data.rgbody.velocity = new Vector2(speed*h_axis, data.rgbody.velocity.y);

		if(transform.position.x <= initPos.x && facingRight)
			Flip();

		if(transform.position.x >= initPos.x + patrolDistance && !facingRight)
			Flip();
	}

	#endregion

	#region Utility/Setup Functions

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

		h_axis *= -1;
	}

	private void colResponse(object sender, Collision2D col, System.EventArgs e)
	{
		Debug.Log("Col event detected in enemy " + gameObject.name);
	}

	private void trigResponse(object sender, Collider2D trig, System.EventArgs e)
	{
		Debug.Log("Trig event detected in enemy " + gameObject.name);
	}

	private void subscribeEvents()
	{
		handler.colFired += new colResponder(colResponse);
		handler.trigFired += new trigResponder(trigResponse);
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
