using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhoneController : MonoBehaviour {

	

	private Dictionary<Tuple<int,int>, bool> phones;

	// Use this for initialization
	void Start () {
		//Populate the dictionary
		phones = new Dictionary<Tuple<int,int>, bool>();
		for (r = 0; r <= 4; r+=2) 	//Rows 0, 2, and 4
			for (c = 1; c <= 4; c++)   //Evenry column from 1 to 5
				phones[Tuple.Create(r,c)] = false;

		print phones;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/**Creates a cell phone in the given location and waits for the next one
	 * Must pass in a valid location for a phone
	 */
	private void createPhone(int phoneX, int phoneY) {
		cellPhones[phoneX, phoneY] = true;
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
}
