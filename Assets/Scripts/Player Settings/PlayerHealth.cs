﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : Photon.MonoBehaviour {
	private GameObject healthBox;
	private Vector3 position;

	public int health;
	public GameObject obj;
	public Image healthUI;
	public Text healthText;
	
	// Use this for initialization
	void Start () {
		//healthUI = GetCom<VisualHealth>();
		//obj = gameObject.tag ("VisualUI");
		//healthUI = Image. ("VisualUI");
	}
	
	void FixedUpdate() {
		Debug.Log("FixedUpdate time on PlayerHealth:" + Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update () {
		HandleHealth ();
	}
	
	private void HandleHealth() {
		//healthText.text = "Health: " + health;
		//healthUI.fillAmount = health;
		
		//		if (health > 50) {
		//			healthUI.color = new Color32((byte) MapValues(health, 50, 100, 255,0), 255,0,255);
		//		} else {
		//			healthUI.color = new Color32(255,(byte)MapValues(health, 0, 50,0,255), 0, 255);
		//		}
	}
	
	[RPC] public void ChangeHealth(int amount) {
		health -= amount;
		checkDeath ();
	}
	
	public void checkDeath() {
		if (health <= 0) Dead ();
	}
	
	void OnGUI() {
		if (GetComponent<PhotonView> ().isMine && gameObject.tag == "Player") {
			CheckForSuicide ();
			CheckForDamage ();
		}
	}

	void CheckForSuicide () {
		if (GUI.Button (new Rect (Screen.width - 100, 0, 100, 40), "Suicide")) {
			Dead ();
		}
	}

	void CheckForDamage () {
		if (GUI.Button (new Rect (Screen.width - 100, 100, 100, 40), "Take Damage")) {
			ChangeHealth (10);
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		healthBox = collision.gameObject;

		if(healthBox.tag == "healthBox"){
			PhotonNetwork.Destroy(healthBox);
			position = healthBox.transform.position;
			health = 100;
			Invoke("ItemReinstantiate", 5.0f);
		}
	}
	
	void ItemReinstantiate () {		
		PhotonNetwork.Instantiate("FirstAid", position, Quaternion.identity,0);
		print ("Item has been Instantiated");
	}

	void Dead() {		
		if (GetComponent<PhotonView> ().isMine) {
			ResetTimerAndCamera ();
		}
		print ("Died at " + Time.deltaTime + "!");
		PhotonNetwork.Destroy (gameObject);
	}

	void ResetTimerAndCamera () {
		if (gameObject.tag == "Player") {
			NewNetwork nn = GameObject.FindObjectOfType<NewNetwork> ();
			nn.standbyCamera.SetActive (true);
			nn.respawnTimer = 3f;
		}
	}
	
	private float MapValues (float x, float inMin, float inMax, float outMin, float outMax) {
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}	
}