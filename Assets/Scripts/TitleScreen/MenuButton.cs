using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	//[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;

    // Update is called once per frame
    void Update()
    {
		if(menuButtonController.index == thisIndex)
		{
			animator.SetTrigger("Selected");
			if(Input.GetButtonDown("Submit")){
				animator.SetTrigger("Pressed");
			}
		}else{
			animator.SetTrigger("Unselected");
		}
    }
}
