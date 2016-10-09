using UnityEngine;
using System.Collections;

namespace App.Game.Utility
{
	public class DeathActuator : GenericActuator 
	{
		protected override void OnTriggerEnter2D(Collider2D col)
		{
			base.OnTriggerEnter2D(col);
		}

		protected override void OnTriggerExit2D(Collider2D col)
		{
			base.OnTriggerEnter2D(col);
		}

		protected virtual void updateState()
		{
			//switch states
			if(actType == actuatorType.on_off_sensor)
				state = (state > 0)? 0 : 1;

			//toggle states
			else if(actType == actuatorType.toggle)
				state = (state > 0)? 0 : 1;

			else if(actType == actuatorType.three_state)
			{
				if(state < 2)
					state += 1;
				else 
					state = 0;
			}
		}

		protected virtual void trigDispatch()
		{
			if(targets != null)
			{
				foreach(Transform target in targets)
				{
					target.SendMessage("OnDeath", new ActuatorArgs(gameObject, actType, state), SendMessageOptions.RequireReceiver);
				}
			}
		}
	}
}