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

	//public float maxPhoneTime; //The maximum time in seconds between new phones
	//public float minPhoneTime; //The minimum time in seconds between new phones
	public float phoneDelay;     //Time between when phones spawn
	private float nextPhoneTime; //The time needed before the next phone is generated

	private float skipChance;    //Chance of skipping a phone
	private float doubleChance;   //Chance of generating two phones next time
	private bool skipped;        //True iff the last phone was skipped

	public float skipAcceleration;   //Increased chance of skipping every time a phone is not skipped
	public float minSkipChance;      //Minimum probability of skipping
	public float doubleAcceleration; //Increased chance of doubling every time a phone is not doubled
	public float maxDoubleChance;    //Maximum probability of doubling

	public GameObject playerObject;  //The player
	private Movement player;
	public GameObject gameControllerObject;
	private GameRunner gameController;

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

		//Set up references to scripts
		player = playerObject.GetComponent<Movement>();
		gameController = gameControllerObject.GetComponent<GameRunner>();

		//Set up the skippings
		skipped = false;
		skipChance = 1.0f;
		doubleChance = 0.0f;

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
		if (gameController.gameIsRunning)
			if (Time.time > nextPhoneTime) {
				//Handle skipping
				if (skipped) {
					skipped = false;
					if (skipChance > minSkipChance)
						skipChance -= skipAcceleration;
					//Always make a new phone if the last one was skipped
					generateRandomPhone();
				} else {
					if (Random.value < skipChance)
						skipped = true;
					else {
						generateRandomPhone();
						//Handle possibility for a double phone
						if (Random.value < doubleChance)
							generateRandomPhone();
						else if (doubleChance < maxDoubleChance)
							doubleChance += doubleAcceleration;
					}
				}


				//Wait to next phone
				timeNextPhone();
			}



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
		nextPhoneTime = Time.time + phoneDelay;
	}

	private ChairController[] chairsNear(int phoneX, int phoneY) {
		int chairX = phoneIndexToPlayerIndex(phoneX, phoneY)[0];
		int chairY = phoneIndexToPlayerIndex(phoneX, phoneY)[1];

		//Make the array of chair controllers
		//First, find how many spaces we will need in the array
		int arraySize = 3;
		                               //Get rid of one if we're:
		if (phoneY == 0) arraySize--;  //in the back row
		if (phoneX == 0) arraySize--;  //in the left column
		if (phoneX == 4) arraySize--;  //in the right column

		ChairController[] chairs = new ChairController[arraySize];

		if (phoneY == 0) { // In the back row
			if (phoneX == 0) { 			//In the left column
				chairs[0] = transform.Find("Chair" + (chairX + 1) + "," + chairY).GetComponent<ChairController>();
			} else if (phoneX == 4) {	//In the right column
				chairs[0] = transform.Find("Chair" + (chairX - 1) + "," + chairY).GetComponent<ChairController>();
			} else {					//In the middle
				chairs[0] = transform.Find("Chair" + (chairX + 1) + "," + chairY).GetComponent<ChairController>();
				chairs[1] = transform.Find("Chair" + (chairX - 1) + "," + chairY).GetComponent<ChairController>();
			}
		} else { //Not in the back row
			chairs[0] = transform.Find("Chair" + chairX + "," + (chairY - 2)).GetComponent<ChairController>();
			if (phoneX == 0) { 			//In the left column
				chairs[1] = transform.Find("Chair" + (chairX + 1) + "," + chairY).GetComponent<ChairController>();
			} else if (phoneX == 4) {	//In the right column
				chairs[1] = transform.Find("Chair" + (chairX - 1) + "," + chairY).GetComponent<ChairController>();
			} else {					//In the middle
				chairs[1] = transform.Find("Chair" + (chairX + 1) + "," + chairY).GetComponent<ChairController>();
				chairs[2] = transform.Find("Chair" + (chairX - 1) + "," + chairY).GetComponent<ChairController>();
			}
		}

		return chairs;
	}

	/**Creates a cell phone in the given location and waits for the next one
	 * Must pass in a valid location for a phone
	 * This also bothers everyone nearby
	 */
	private void createPhone(int phoneX, int phoneY) {
		int[] playerCoords = playerIndexToPhoneIndex(player.currentX,player.currentY); 

		//If the phone is already dark, skip this round
		if (!phones[phoneX,phoneY].isCurrentlyDark				   //Don't make two phones in one spot
			&& !(playerCoords[0] == phoneX && playerCoords[1] == phoneY)) {	//Don't make a phone where
			//Turn on the phone                                             //you currently are
			//print ("setting dark at" + phoneX + "," + phoneY);
			phones[phoneX, phoneY].setDark(true);
			//Bother everybody
			ChairController[] nearbyChairs = chairsNear(phoneX,phoneY);
			for (int i = 0; i < nearbyChairs.Length; i++) {
				nearbyChairs[i].phonesBothering++;
			}

			//Make a sound
			//Sound: "Beep 25" by Sound Jay
			//http://www.soundjay.com/beep-sounds-3.html
			transform.Find("AudioSource").gameObject.GetComponent<AudioSource>().Play();
		}
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

	//Quiet the phone where the player currently is, if the phone is on
	//Don't call this unless the player is located at a phone
	public void QuietPhone() {
		int[] phoneCoordinates = playerIndexToPhoneIndex(player.currentX, player.currentY);
		if(phones[phoneCoordinates[0],phoneCoordinates[1]].isCurrentlyDark) {
			phones[phoneCoordinates[0],phoneCoordinates[1]].setDark(false);

			ChairController[] nearbyChairs = chairsNear(phoneCoordinates[0],phoneCoordinates[1]);
			for (int i = 0; i < nearbyChairs.Length; i++) {
				nearbyChairs[i].phonesBothering--;
			}
		}
	}
}
