
using UnityEngine;

public enum EnemyType
{
	Chicken, Ghost, FlyBug
}
public class EnemyBlock : BlockBase
{
	public EnemyType Type;
	public Collider2D Collider;
	public Animator Animator;

	private void Awake()
	{
		Collider= GetComponent<Collider2D>();
		Animator = GetComponent<Animator>();
		Animator.runtimeAnimatorController = null;
	}
}
