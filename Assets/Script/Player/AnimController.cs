using UnityEngine;
using System.Collections;

public class AnimController : MonoBehaviour {

	private Animator PlayerAnimator;
	private AnimatorStateInfo currentBaseState, nextBaseState;	

	int idleState = Animator.StringToHash("Base Layer.idle");	
	int specialAttackState = Animator.StringToHash("Base Layer.specialAttack");	

	public int comboNumber;

	void Awake()
	{
		PlayerAnimator = GetComponentInChildren<Animator>();;
	}

	public void SetAnimVar(string namaVar, bool nilai)
	{
		PlayerAnimator.SetBool(namaVar,nilai);
	}

	public void SetAnimVar(string namaVar, int nilai)
	{
		PlayerAnimator.SetInteger(namaVar,nilai);
	}

	public bool getAnimBoolVar(string namaVar)
	{
		return PlayerAnimator.GetBool(namaVar);
	}

	public int getAnimIntVar(string namaVar)
	{
		return PlayerAnimator.GetInteger(namaVar);
	}

	public void AttackCombo()
	{
		comboNumber++;
		SetAnimVar("isAttack", true);
		SetAnimVar("Combo",comboNumber);
	}

	public void SpecialAttack()
	{
		SetAnimVar("isAttack", true);
		SetAnimVar("specialAttack", true);
	}
	
	void FixedUpdate()
	{
		currentBaseState = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
		if(currentBaseState.nameHash == idleState)
		{
			if(!PlayerAnimator.IsInTransition(0))
			{
				SetAnimVar("isAttack",false);

			}

		}

		nextBaseState = PlayerAnimator.GetNextAnimatorStateInfo(0);
		if(nextBaseState.nameHash == idleState)
		{
				comboNumber = 0;
				SetAnimVar("Combo",comboNumber);
				SetAnimVar("specialAttack",false);
		}
		
	}
}
