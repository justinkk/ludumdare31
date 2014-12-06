using UnityEngine;
using System.Collections;

public class SpriteFlipper : MonoBehaviour {

	private Animator anim;

	void Start() {
		anim = GetComponent<Animator>();
	}

	//isDark is true if we want to make this dark; false if we want to make this light
	public void setDark(bool isDark) {
		anim.SetBool("isDark", isDark);
	}
}
