using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	//Size of the possible array in which the player can move
	public int width;
	public int height;

	//These variables track the player's current position
	private int currentX;
	private int currentY;

	
	public GameObject spriteParent;       //The collection of sprites that can be light or dark
	private SpriteFlipper[,] spriteArray; //The array of flippers for the sprites


	void Start() {
		//Set position to bottom-left
		currentX = 0;
		currentY = 0;

		//Initialize array of sprites
		//Find the child object with the name of the current index
		//GameObject currentSprite = spriteParent.transform.Find("Sprite0,0").gameObject;
		//
		GameObject currentSprite = GameObject.Find("Sprite0,0").gameObject;
		if (currentSprite == null) {
			print ("no sprite found");
		} else {
			//Find the SpriteFlipper script of that child object
			spriteArray[0,0] = (SpriteFlipper) currentSprite.GetComponent(typeof(SpriteFlipper));
		}
	}
	
	void Update() {
		if(Input.GetKeyDown("space")) {
			if(currentX == 0) {
				spriteArray[0,0].setDark(true);
				currentX = 1;
			} else {
				spriteArray[0,0].setDark(false);
				currentX = 0;
			}
		}
	}
}
