using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public int health;
	public float speed;

	public Collider WeaponCollider;
	public Animator enemyAnimator;

	private AnimatorStateInfo currentBaseState, nextBaseState;	

	public int hittedCombo;

	public bool waitCombo;
	public float comboTime, maxComboWait=1;

	int hitState = Animator.StringToHash("Base Layer.hit");	

	void Awake()
	{
		enemyAnimator = GetComponent<Animator>();
	}

	void OnTriggerEnter(Collider colliderObject)
	{
		if(colliderObject.gameObject.tag == "Weapon")
		{
			gameObject.transform.LookAt(colliderObject.transform.root);
			waitCombo=true;

			Debug.Log("Kena");
			hittedCombo++;
			enemyAnimator.Play(hitState);

			if(hittedCombo>=3)
			{
				enemyAnimator.Play("thrown");
				hittedCombo=0;
				rigidbody.AddForce(transform.forward*-1000);
			}

		}

	}

	void Update()
	{
		if(waitCombo)
		{
			comboTime+= Time.deltaTime;
		}
	}

	void LateUpdate()
	{
		if(comboTime>=maxComboWait)
		{
			waitCombo=false;
			comboTime=0;
			hittedCombo=0;
		}
	}

	public void ColliderOnOff()
	{
		WeaponCollider.enabled = !WeaponCollider.enabled;
	}


}
