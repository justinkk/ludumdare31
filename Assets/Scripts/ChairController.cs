using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChairController : MonoBehaviour {

	private float anger; //Current level of anger
	public GameObject gameControllerObject;
	public GameObject playerObject;
	private GameRunner gameController; //The game controller
	private Movement player;           //The player

	private Animator angerAnimator; //Controls the animations for the anger

	public int row;                    //The y location
	public int column;                 //The x location
	//private Text textField;            //The text field
	public int phonesBothering;        //Number of phones currently bothering you

	// Use this for initialization
	void Start () {
		anger = 0.0f;
		//GameObject gameControllerObject = transform.Find("GameController").gameObject;
		gameController = gameControllerObject.GetComponent<GameRunner>();
		//GameObject playerObject = transform.Find("Player").gameObject;
		player = playerObject.GetComponent<Movement>();

		//Get ready to display the amount of anger
		angerAnimator = transform.Find("Anger").gameObject.GetComponent<Animator>();
		//textField = transform.Find("Text").gameObject.GetComponent<Text>();

		phonesBothering = 0;
	}
	
	
	// Update is called once per frame
	void Update () {
		/* font: Digital 7 by Style-7
		 * http://www.dafont.com/es/digital-7.font
		 */

		 //Find change
		 float deltaAnger = -1.0f * gameController.coolOff*Time.deltaTime;

		//If the player is blokcing your view
		if(player.currentX == column && player.currentY >= row) {
			deltaAnger += gameController.blockingAnger*Time.deltaTime;
		}

		//If phones are bothering you
		deltaAnger += phonesBothering * gameController.phoneAnger * Time.deltaTime;

		//Apply change
		anger += deltaAnger;
		if(anger < 0) {
			anger = 0;
		}

		//Display
		angerAnimator.SetFloat("AngerPercentage", anger / gameController.maxAnger);
		//textField.text = "" + anger;

		//End the game if you are too angry
		if(anger >= gameController.maxAnger) {
			gameController.lose(column, row);
		}
	}
}
