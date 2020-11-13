// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class footSteps : MonoBehaviour {
	
	
	public GameObject stepObject;
	
	//Audios
	public AudioClip[] untagged;
	public AudioClip[] dirt;
	public AudioClip[] concrete;
	public AudioClip[] wood;
	public AudioClip[] metal;
	public AudioClip[] water;
	
	public float crouchStepLength = 0.65f;
	public float walkStepLength = 0.45f;
	public float runStepLength = 0.25f;
	
	public float minWalkSpeed = 6.0f;
	public float maxWalkSpeed = 10.0f;
	
	public float crouchVolume = 0.3f;
	public float walkVolume = 0.4f;
	public float runVolume = 0.85f;
	
	bool  step = true;
	private string curMaterial;
	
	private GameObject weapons;
	
	
	void  Start (){
		
	}
	
	void  Update (){
		
	}
	
	
	void  OnControllerColliderHit ( ControllerColliderHit hit  ){
		curMaterial = hit.collider.transform.tag;
		CharacterController controller = GetComponent<CharacterController>();
		
		if(controller.isGrounded && step)
		{
			if(controller.velocity.magnitude > 0 && controller.velocity.magnitude < minWalkSpeed)
			{
				if(curMaterial == "Untagged")
				{
					StartCoroutine(Step(untagged, crouchStepLength, crouchVolume));
				}
				if(curMaterial == "Dirt")
				{
					StartCoroutine(Step(dirt, crouchStepLength, crouchVolume));
				}
				if(curMaterial == "Concrete")
				{
					StartCoroutine(Step(concrete, crouchStepLength, crouchVolume));
				}
				if(curMaterial == "Wood")
				{
					StartCoroutine(Step(wood, crouchStepLength, crouchVolume));
				}
				if(curMaterial == "Metal")
				{
					StartCoroutine(Step(metal, crouchStepLength, crouchVolume));
				}
				if(curMaterial == "Water")
				{
					StartCoroutine(Step(water, crouchStepLength, crouchVolume));
				}
			}
			if(controller.velocity.magnitude > minWalkSpeed && controller.velocity.magnitude < maxWalkSpeed)
			{
				if(curMaterial == "Untagged")
				{
					StartCoroutine(Step(untagged, walkStepLength, walkVolume));
				}
				if(curMaterial == "Dirt")
				{
					StartCoroutine(Step(dirt, walkStepLength, walkVolume));
				}
				if(curMaterial == "Concrete")
				{
					StartCoroutine(Step(concrete, walkStepLength, walkVolume));
				}
				if(curMaterial == "Wood")
				{
					StartCoroutine(Step(wood, walkStepLength, walkVolume));
				}
				if(curMaterial == "Metal")
				{
					StartCoroutine(Step(metal, walkStepLength, walkVolume));
				}
				if(curMaterial == "Water")
				{
					StartCoroutine(Step(water, walkStepLength, walkVolume));
				}
			}
			if(controller.velocity.magnitude > maxWalkSpeed)
			{
				if(curMaterial == "Untagged")
				{
					StartCoroutine(Step(untagged, runStepLength, runVolume));
				}
				if(curMaterial == "Dirt")
				{
					StartCoroutine(Step(dirt, runStepLength, runVolume));
				}
				if(curMaterial == "Concrete")
				{
					StartCoroutine(Step(concrete, runStepLength, runVolume));
				}
				if(curMaterial == "Wood")
				{
					StartCoroutine(Step(wood, runStepLength, runVolume));
				}
				if(curMaterial == "Metal")
				{
					StartCoroutine(Step(metal, runStepLength, runVolume));
				}
				if(curMaterial == "Water")
				{
					StartCoroutine(Step(water, runStepLength, runVolume));
				}
			}
		}
	}
	
	
	IEnumerator Step ( AudioClip[] name ,   float spacing ,   float vol  ){
		step = false;
		stepObject.GetComponent<AudioSource>().clip = name[Random.Range(0, name.Length)];
		stepObject.GetComponent<AudioSource>().volume = vol;
		stepObject.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(spacing);
		step = true;
	}
	
	
}