using UnityEngine;
using System.Collections;

public class SpriteFlipper : MonoBehaviour {
	private Animator anim;
	public bool isCurrentlyDark;

	void Start() {
		isCurrentlyDark = false;
		anim = GetComponent<Animator>();
	}

	//isDark is true if we want to make this dark; false if we want to make this light
	public void setDark(bool isDark) {
		anim.SetBool("isDark", isDark);
		isCurrentlyDark = isDark;
	}

	//Switch to the game over animation forever
	public void lose() {
		anim.SetTrigger("gameOver");
	}
}
