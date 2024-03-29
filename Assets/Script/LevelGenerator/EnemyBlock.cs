
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

	public EnemyVerticalMovement VerticalMovement;

	private void Awake()
	{
		Collider= GetComponent<Collider2D>();
		Animator = GetComponent<Animator>();
		Animator.runtimeAnimatorController = null;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			collision.GetComponent<PlayerController>().DieFromEnemy();
		}
	}
}
