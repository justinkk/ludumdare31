using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameRunner : MonoBehaviour {

	public float blockingAnger; //How much anger per second is added by vision being blocked
	public float phoneAnger;    //How much anger per second is added by being near a cell phone
	public float coolOff;       //How much anger per second is removed
	public float maxAnger;      //Hom much anger you need to end the game

	public string scoreMessage; //The message before the current score

	private float initialTime;  //Time at the start of the game
	private float score;        //Current score
	public GameObject scoreText;//Displays text of score
	private Text scoreDisplay;  //Text displaying current score

	public GameObject player;   //The player
	private Movement playerMovement; //The player's movement controller
	public bool gameIsRunning;  //True iff game is currently running
	//public bool[,] cellPhones;

	public GameObject chairs;   //The chairs


	//Decides when the next phone will occur
	/*	private void timeNextPhone() {
	*		nextPhoneTime = Time.time + Random.Range(minPhoneTime, maxPhoneTime);
	*	}
	*/

	void Start() {
		//Set up display
		scoreDisplay = scoreText.GetComponent<Text>();
		playerMovement = player.GetComponent<Movement>();
		initialTime = Time.time;

		/*	//Set up phones
		*	cellPhones = new bool[playerMovement.width, playerMovement.height]; //By default, all are false
		*	timeNextPhone(); //Give a little bit before the first phone is made
		*/

		//Start game
		gameIsRunning = true;
	}

	void Update() {
		if (gameIsRunning) {
			score = Time.time - initialTime;
			scoreDisplay.text = scoreMessage + (int)score;

			//Make a new phone if it's time for that
			/*	if (Time.time > nextPhoneTime)
			*		generateRandomPhone();
			*/
		}
	}

	/**Creates a cell phone in the given location and waits for the next one
	 * Must pass in a valid location for a phone
	 */
	/*
	*	private void createPhone(int phoneX, int phoneY) {
	*		cellPhones[phoneX, phoneY] = true;
	*		timeNextPhone();
	*	} 
	*/

	/*
	//Randomly chooses a location for a new cell phone and generates it
	private void generateRandomPhone() {
		int phoneX = 0, phoneY = 0;

		//Generate the Y
		float randomChance = Random.value;
		if (randomChance < 0.2f)
			phoneY = 0;
		else if (randomChance < 0.5f)
			phoneY = 2;
		else
			phoneY = 4;

		//Generate the X
		randomChance = Random.value;
		if (randomChance < 0.1f)
			phoneX = 1;
		else if (randomChance < 0.4f)
			phoneX = 2;
		else if (randomChance < 0.6f)
			phoneX = 3;
		else if (randomChance < 0.9f)
			phoneX = 4;

		//Make the phone
		createPhone(phoneX, phoneY);
	}
	*/

	//Runs the graphical effects that end the game
	private void endSequence(int chairX, int chairY) {
		playerMovement.lose();

		//Turn off the chairs
		for (int c = 1; c < 6; c++) { //Left to right
			for (int r = 0; r <= 4; r += 2) { //Bottom to top
				//Skip the chair that lost you the game
				if (c != chairX || r != chairY) {
					GameObject currentChair = chairs.transform.Find("Chair" + c + "," + r).gameObject;

					//Turn off the chair
					currentChair.GetComponent<Animator>().SetTrigger("gameOver");
					//Turn off the anger display
					currentChair.transform.Find("Anger").gameObject.GetComponent<Animator>().SetTrigger("gameOver");
					//Turn off the phone display
					currentChair.transform.Find("Phone").gameObject.GetComponent<SpriteFlipper>().lose();	
				}		
			}
		}
	}

	//Called when someone gets angry enough to end the game
	public void lose(int chairX, int chairY) {
		//Ends the game
		gameIsRunning = false;

		//Final graphical effects
		endSequence(chairX, chairY);
	}
}
