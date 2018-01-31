using UnityEngine;
using System.Collections;

public class RoundScript1 : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		StartCoroutine (Switch());
	}

	IEnumerator Switch()
	{
		yield return new WaitForSeconds(1.7f);
		Application.LoadLevel ("StalinStage");
	}
}
