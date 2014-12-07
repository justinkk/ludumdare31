using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	//Size of the possible array in which the player can move
	public int width;
	public int height;

	//These variables track the player's current position
	public int currentX;
	public int currentY;

	public GameObject gameControllerObject;     //The game controller object
	private GameRunner gameController;          //The script of the game controller
	public GameObject spriteParent;             //The collection of sprites that can be light or dark
	private SpriteFlipper[,] spriteArray;       //The array of flippers for the sprites
	public GameObject phoneControllerObject;    //The phone controller object
	private PhoneController phoneController;    //The phone controller

	//Turn the current sprite light
	public void lighten() {
		spriteArray[currentX,currentY].setDark(false);
	}

	//Turn the current sprite dark
	public void darken() {
		spriteArray[currentX,currentY].setDark(true);
	}

	//Moves to a location, clearing any cell phones in that location
	public void moveTo(int newX, int newY) {
		//Change the appearance
		lighten();
		currentX = newX;
		currentY = newY;
		darken();

		//Clear any cell phones in the new location
		//gameController.cellPhones[currentX,currentY] = false;
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

		//Set up references to scripts
		gameController = gameControllerObject.GetComponent<GameRunner>();
		phoneController = phoneControllerObject.GetComponent<PhoneController>();

		//Have the first sprite visible
		//darken();
	}
	
	void Update() {
		if(Input.GetKeyDown("right")) {
			//Movement only allowed in even rows, and when not on the very right
			if(currentX < width - 1 && currentY % 2 == 1) 
				moveTo(currentX + 1, currentY);
			/*{
				lighten();
				currentX++;
				darken();
			}*/
		} else if(Input.GetKeyDown("left")) {
			//Movement only allowed in even rows, and when not on the very left
			if(currentX > 0 && currentY % 2 == 1) 
				moveTo(currentX-1, currentY);
			/*{
				lighten();
				currentX--;
				darken();
			}*/
		} else if(Input.GetKeyDown("up")) {
			//Movement up not allowed when at very top and when in an even row except on the edges
			if(currentY < height - 1 && (currentY % 2 == 0 || currentX == 0 || currentX == width - 1))
				moveTo(currentX, currentY + 1);
			/*{
				lighten();
				currentY++;
				darken();
			}*/
		} else if(Input.GetKeyDown("down")) {
			//Movement down not allowed when at very bottom and when in an odd row except on the edges
			if(currentY > 0 && (currentY % 2 == 1 || currentX == 0 || currentX == width - 1)) {
				moveTo(currentX, currentY - 1);

				//Quiet a phone
				if (currentX != 0 && currentX != width - 1)
					phoneController.QuietPhone();

			}
			/*{
				lighten();
				currentY--;
				darken();
			}*/
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
