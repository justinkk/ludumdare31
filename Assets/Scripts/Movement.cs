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

	//Turn the current sprite light
	void lighten() {
		spriteArray[currentX,currentY].setDark(false);
	}

	//Turn the current sprite dark
	void darken() {
		spriteArray[currentX,currentY].setDark(true);
	}

	void Start() {
		//Set position to bottom-left
		currentX = 0;
		currentY = 0;

		//Initialize array of sprites
		spriteArray = new SpriteFlipper[width, height];
		for (int r = 0; r < width; r++) { //left to right
			for (int c = 0; c < height; c++) { //bottom to top
				//Find the child object with the name of the current index
				GameObject currentSprite = spriteParent.transform.Find("Sprite" + r + "," + c).gameObject;
				spriteArray[r,c] = currentSprite.GetComponent<SpriteFlipper>();
			}
		}

		//Have the first sprite visible
		darken();
	}
	
	void Update() {
		if(Input.GetKeyDown("right")) {
			//Movement only allowed in even rows, and when not on the very right
			if(currentX < width - 1 && currentY % 2 == 1) {
				lighten();
				currentX++;
				darken();
			}
		} else if(Input.GetKeyDown("left")) {
			//Movement only allowed in even rows, and when not on the very left
			if(currentX > 0 && currentY % 2 == 1) {
				lighten();
				currentX--;
				darken();
			}
		} else if(Input.GetKeyDown("up")) {
			//Movement up not allowed when at very top and when in an even row except on the edges
			if(currentY < height - 1 && (currentY % 2 == 0 || currentX == 0 || currentX == width - 1)) {
				lighten();
				currentY++;
				darken();
			}
		} else if(Input.GetKeyDown("down")) {
			//Movement down not allowed when at very bottom and when in an odd row except on the edges
			if(currentY > 0 && (currentY % 2 == 1 || currentX == 0 || currentX == width - 1)) {
				lighten();
				currentY--;
				darken();
			}
		}

		/*if(Input.GetKeyDown("space")) {
			if(currentX == 0) {
				spriteArray[0,1].setDark(true);
				currentX = 1;
			} else {
				spriteArray[0,1].setDark(false);
				currentX = 0;
			}
		}*/
	}
}
