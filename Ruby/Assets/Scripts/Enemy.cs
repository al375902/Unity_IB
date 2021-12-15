using System;
using UnityEngine;

/// <summary>
/// This class handle Enemy behaviour. It make them walk back & forth as long as they aren't fixed, and then just idle
/// without being able to interact with the player anymore once fixed.
/// </summary>
public class Enemy : MonoBehaviour
{
	// ====== ENEMY MOVEMENT ========
	public float speed;
	public float timeToChange;
	public bool horizontal;

	public GameObject smokeParticleEffect;
	public ParticleSystem fixedParticleEffect;


	
	Rigidbody2D rigidbody2d;
	float remainingTimeToChange;
	Vector2 direction = Vector2.right;
	bool repaired = false;
	
	// ===== ANIMATION ========
	Animator animator;
	
	// ================= SOUNDS =======================

	
	void Start ()
	{
		rigidbody2d = GetComponent<Rigidbody2D>();
		remainingTimeToChange = timeToChange;

		direction = horizontal ? Vector2.right : Vector2.down;

		animator = GetComponent<Animator>();

	}
	
	void Update()
	{
		if(repaired)
			return;
		
		remainingTimeToChange -= Time.deltaTime;

		if (remainingTimeToChange <= 0)
		{
			remainingTimeToChange += timeToChange;
			direction *= -1;
		}

		animator.SetFloat("ForwardX", direction.x);
		animator.SetFloat("ForwardY", direction.y);
	}

	void FixedUpdate()
	{
		rigidbody2d.MovePosition(rigidbody2d.position + direction * speed * Time.deltaTime);
	}

	void OnCollisionStay2D(Collision2D other)
	{
		if(repaired)
			return;
		
		RubyController controller = other.collider.GetComponent<RubyController>();
		
		if(controller != null)
			controller.ChangeHealth(-1);
	}

	public void Fix()
	{
		animator.SetTrigger("Fixed");
		repaired = true;
		
		smokeParticleEffect.SetActive(false);

		Instantiate(fixedParticleEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);

		rigidbody2d.simulated = false;

	}
}
