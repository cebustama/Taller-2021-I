using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

[AddComponentMenu("Playground/Conditions/Condition Key Press")]
public class ConditionKeyPress : ConditionBase
{
	public KeyCode keyToPress = KeyCode.Space;

	[Header("Type of Event")]
	public KeyEventTypes eventType = KeyEventTypes.JustPressed;

	public float frequency = 0.5f;

	private float timeLastEventFired;

	public float keyPressCooldown = 0f;
	private float keyPressTimer = 0f;


	private void Start()
	{
		timeLastEventFired = -frequency;
	}

	private void Update()
	{
		if (keyPressTimer > 0)
        {
			keyPressTimer -= Time.deltaTime;
        }

		switch(eventType)
		{
			case KeyEventTypes.JustPressed:
				if(Input.GetKeyDown(keyToPress) && keyPressTimer <= 0f)
				{
					keyPressTimer = keyPressCooldown;
					ExecuteAllActions(null);
				}
				break;
			case KeyEventTypes.Released:
				if(Input.GetKeyUp(keyToPress) && keyPressTimer <= 0)
				{
					keyPressTimer = keyPressCooldown;
					ExecuteAllActions(null);
				}
				break;
			case KeyEventTypes.KeptPressed:
				if(Time.time >= timeLastEventFired + frequency
					&& Input.GetKey(keyToPress))
				{
					ExecuteAllActions(null);
					timeLastEventFired = Time.time;
				}
				break;
		}
	}




	public enum KeyEventTypes
	{
		JustPressed,
		Released,
		KeptPressed
	}



}