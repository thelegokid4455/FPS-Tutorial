// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class move : MonoBehaviour
{
    //HEALTH
    public float maxHealth;
    private float curHealth;

    public Transform kickGO;
    public float kickUp = 0.5f;
    public float kickSideways = 0.5f;

    public GameObject deadPlayer;

    //Walking
    private float curSpeed;
    public float walkSpeed;
    public float runSpeed;

    public float jumpSpeed = 8;
    public  float jumpRunSpeed = 10;
    public  float gravity = 25;


    private Vector4 moveDirection = Vector4.zero;
    private bool grounded = false;

    //ANIMS
    public GameObject camObj;
    public GameObject walkObj;
    public GameObject runObj;

    private Vector3 curPos;
    public Vector3 normPos;
    public Vector3 walkPos;
    public Vector3 runPos;
    public float aSpeed;

    public GameObject walkAnim;

    private GameObject gunMG;

    public GameObject gunRunRot;
    private GameObject curRunRot;

    private bool vaulting;
    private Collider obstical;
    private float timer;
    public Vector3 vaultPos = new Vector3(0.0f, 1f, 2f);

    public AudioClip vaultAudio;


    void Start()
    {
        gunMG = GameObject.FindGameObjectWithTag("WeaponManager");
        curHealth = maxHealth;
        Time.timeScale = 1;
    }

    void Update()
    {
        if(curHealth <= 0)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        CharacterController controller = GetComponent<CharacterController>();
        CollisionFlags flags = controller.Move(moveDirection * Time.deltaTime);


        walkObj.transform.localPosition = Vector3.MoveTowards(walkObj.transform.localPosition, curPos, aSpeed);

        if (curRunRot)
        {
            var rotation = Quaternion.LookRotation(curRunRot.transform.localPosition - runObj.transform.localPosition);
            runObj.transform.localRotation = Quaternion.Slerp(runObj.transform.localRotation, rotation, Time.deltaTime * 5);
        }
        else
        {
            var rotation = Quaternion.LookRotation(new Vector3(0,0,0));
            runObj.transform.localRotation = Quaternion.Slerp(runObj.transform.localRotation, rotation, Time.deltaTime * 5);
        }
        

        if (grounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection.normalized * curSpeed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                curSpeed = runSpeed;

                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = jumpRunSpeed;
                }

            }

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                curSpeed = walkSpeed;
            }


            //ANIMATIONS

            ////////WALK
            if (GetComponent<CharacterController>().velocity.magnitude > 0 && GetComponent<CharacterController>().velocity.magnitude <= (runSpeed + 1) && !Input.GetKey(KeyCode.LeftShift))
            {
                if (gunMG.GetComponent<gunManager>().curWeapon.GetComponent<gun>().isAiming == false)
                    walkAnim.GetComponent<Animation>().CrossFade("Walk");
                else
                    walkAnim.GetComponent<Animation>().CrossFade("Aim");

                camObj.GetComponent<Animation>().CrossFade("Walk");
                curPos = walkPos;

                curRunRot = null;
                //runObj.GetComponent<Animation>().Stop();
                //runObj.GetComponent<Animation>().Play("Idle");
            }

            //////RUNNING
            else if (GetComponent<CharacterController>().velocity.magnitude >= (runSpeed - 1) && Input.GetKey(KeyCode.LeftShift))
            {
                if (gunMG.GetComponent<gunManager>().curWeapon.GetComponent<gun>().isAiming == false)
                {
                    walkAnim.GetComponent<Animation>().CrossFade("Walk");
                    curRunRot = gunRunRot;
                }
                else
                { 
                    walkAnim.GetComponent<Animation>().CrossFade("Aim");
                    curRunRot = null;
                }

                camObj.GetComponent<Animation>().CrossFade("Walk");
                curPos = runPos;


                //runObj.GetComponent<Animation>().Stop();
                //runObj.GetComponent<Animation>().Play(curRunAnim);
            }

            //////IDLE
            else if (GetComponent<CharacterController>().velocity.magnitude <= (walkSpeed + 1) && !Input.GetKey(KeyCode.LeftShift))
            {
                if (gunMG.GetComponent<gunManager>().curWeapon.GetComponent<gun>().isAiming == false)
                    walkAnim.GetComponent<Animation>().CrossFade("Idle");
                else
                    walkAnim.GetComponent<Animation>().CrossFade("Aim");

                camObj.GetComponent<Animation>().CrossFade("Idle");
                curPos = normPos;

                curRunRot = null;
                //runObj.GetComponent<Animation>().Stop();
                //runObj.GetComponent<Animation>().Play("Idle");
            }
            else
            {
                if (gunMG.GetComponent<gunManager>().curWeapon.GetComponent<gun>().isAiming == false)
                    walkAnim.GetComponent<Animation>().CrossFade("Idle");
                else
                    walkAnim.GetComponent<Animation>().CrossFade("Aim");

                camObj.GetComponent<Animation>().CrossFade("Idle");
                curPos = normPos;

                curRunRot = null;
            }

        }

        else if (vaulting)
        {
            //transform.localPosition += new Vector3(0, 0, 1f);
            //transform.Translate(Vector3.forward * 35 * Time.deltaTime);

            moveDirection = new Vector3(0, 0, 0);
            Vector3 localPos = obstical.transform.InverseTransformPoint(transform.position);
            Vector3 offsetPos = obstical.transform.position + obstical.transform.TransformDirection(vaultPos);
            Vector3 localOffsetPos = obstical.transform.InverseTransformPoint(offsetPos);
            localOffsetPos.x = localPos.x;
            offsetPos = obstical.transform.TransformPoint(localOffsetPos);

            transform.position = Vector3.Slerp(transform.position, offsetPos, 0.1f);
            transform.rotation = Quaternion.Slerp(transform.rotation, obstical.transform.rotation, 0.1f);

            camObj.GetComponent<Animation>().Play("CamVault");
            //animObj.GetComponent<Animation>().Play("Vault");
            StartCoroutine(VaultTime());
        }
        else
        {

        }

        if (!vaulting)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        grounded = (flags & CollisionFlags.CollidedBelow) != 0;

    }

    public void ApplyDamage(int dmg)
    {
        curHealth -= dmg;

        kickGO.localRotation = Quaternion.Euler(kickGO.localRotation.eulerAngles - new Vector3(Random.Range(-(kickUp), (kickUp)), Random.Range(-(kickSideways), (kickSideways)), 0));
    }

    public void Die()
    {
        //audio.PlayOneShot(deadSound);
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        Instantiate(deadPlayer, transform.position, transform.rotation);

        //Time.timeScale = 0.5f;

        Destroy(gameObject);
        //mainCamera.GetComponent<VignetteAndChromaticAberration>().enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Vault" && !vaulting)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                obstical = other;
                vaulting = true;
                GetComponent<AudioSource>().PlayOneShot(vaultAudio);
            }
        }
    }

    IEnumerator VaultTime()
    {
        yield return new WaitForSeconds(0.3f);
        vaulting = false;
        obstical = null;
        yield return new WaitForSeconds(0.1f);
    }



    void OnGUI()
    {

    }











}