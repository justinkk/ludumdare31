using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhoneController : MonoBehaviour {
	/** 
	 * Returns a pair of indices for a phone given the player's indices
	 * at that location. Don't give it bad coordinates or things will be unhappy
	 */
	public static int[] playerIndexToPhoneIndex(int playerX, int playerY) {
		return new int[] {playerX - 1, playerY / 2};
	}

	/** 
	 * Returns a pair of indices for the player given the phone at that location's
	 * indices. Don't give it bad coordinates or things will be unhappy
	 */
	public static int[] phoneIndexToPlayerIndex(int phoneX, int phoneY) {
		return new int[] {phoneX + 1, phoneY * 2};
	}

	private SpriteFlipper[,] phones;

	public float maxPhoneTime; //The maximum time in seconds between new phones
	public float minPhoneTime; //The minimum time in seconds between new phones
	private float nextPhoneTime; //The time needed before the next phone is generated

	/********Testing
	*	private float nextTime; //Testing only
	*	private int currX;
	*	private int currY;
	*/

	void Start () {
		//initialize
		phones = new SpriteFlipper[5,3];

		for (int x = 0; x < 5; x++) { //left to right
			for (int y = 0; y < 3; y++) {  //bottom to top
				//Find the right SpriteFlipper
				int[] chairIndices = phoneIndexToPlayerIndex(x,y);
				Transform chair = transform.Find("Chair" + chairIndices[0] + "," + chairIndices[1]);
				//print(chair);

				//phones[x,y] = null;
				phones[x,y] = chair.Find("Phone").gameObject.GetComponent<SpriteFlipper>();
			}
		}

		//Leave some time before the first phone appears
		nextPhoneTime = 1.5f;

		/*
		*	//Populate the dictionary
		*	phones = new Dictionary<Tuple<int,int>, bool>();
		*	for (r = 0; r <= 4; r+=2) 	//Rows 0, 2, and 4
		*		for (c = 1; c <= 4; c++)   //Evenry column from 1 to 5
		*			phones[Tuple.Create(r,c)] = false;
		*/

		/*
		 *	nextTime = Time.time + .5f;
		 *	currX = 0;
		 *	currY = 0;
		 */
	}

	// Update is called once per frame
	void Update () {
		if (Time.time > nextPhoneTime)
			generateRandomPhone();

		/* TESTING
		if (Time.time > nextTime) {
			phones[currX,currY].setDark(true);

			nextTime = Time.time + .5f;

			if (currX == 4) {
				currX = 0;
				currY++;
			} else {
				currX++;
			}
		}*/
	}

	//Times when the next phone will appear
	private void timeNextPhone() {
		nextPhoneTime = Time.time + Random.Range(minPhoneTime, maxPhoneTime);
	}

	/**Creates a cell phone in the given location and waits for the next one
	 * Must pass in a valid location for a phone
	 */
	private void createPhone(int phoneX, int phoneY) {
		phones[phoneX, phoneY].setDark(true);
		timeNextPhone();
	}

	//Randomly chooses a location for a new cell phone and generates it
	private void generateRandomPhone() {
		int phoneX = 0, phoneY = 0;

		//Generate the Y
		float randomChance = Random.value;
		if (randomChance < 0.2f)
			phoneY = 0;
		else if (randomChance < 0.5f)
			phoneY = 1;
		else
			phoneY = 2;

		//Generate the X
		randomChance = Random.value;
		if (randomChance < 0.1f)
			phoneX = 0;
		else if (randomChance < 0.4f)
			phoneX = 1;
		else if (randomChance < 0.6f)
			phoneX = 2;
		else if (randomChance < 0.9f)
			phoneX = 3;
		else
			phoneX = 4;

		//Make the phone
		createPhone(phoneX, phoneY);
	}
}
