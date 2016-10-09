using UnityEngine;
using System.Collections;

namespace App.Game.Player
{
	public class ColliderResize : MonoBehaviour 
	{
		private CharacterData data { get { return CharacterData.charaData; } } 

		public float adjustmentIncremenet = 0.1f;
		public Vector2 crouchResize;
		public Vector2 crawlResize;
		private Vector2 normalSize = new Vector2(1f, 1f);

		private bool hitPipe;

		public void resizeCollider(inputValues value)
		{
			if(value == inputValues.crouch)
			{
				transform.localScale = crouchResize;
			}

			else if(value == inputValues.crawl)
			{
				transform.localScale = crawlResize;
			}

			else
			{
				transform.localScale = normalSize;
			}
		}

		void OnCollision2D()
		{

		}
	}
}