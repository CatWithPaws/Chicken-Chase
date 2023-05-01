using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationComponent : MonoBehaviour
{
	[SerializeField] private Animator animator;
	public void SwitchAnimationTo(PlayerState playerState)
    {
		animator.Play(Enum.GetName(typeof(PlayerState), playerState));
    }

	public void ChangeSpeed(float newSpeed)
	{
		animator.speed = newSpeed;
	}
}
