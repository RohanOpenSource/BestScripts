using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{
    public float speed = 5;
    private float cms;
    private float cs;

    public bool isGrounded;
    public float sensitivity = 10;
    public Vector2 movementInput;
    private Vector2 localEulerAnglesInput;
    public LayerMask ground;
    public float JumpForce=10f;
    public Transform playerCam;
    private Rigidbody rb;
    public bool isWallRunning=false;
    private float camRot;
    private float camY;
    private void Start() {
        rb=GetComponent<Rigidbody>();
        camY=playerCam.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update() {
        float mousey = Input.GetAxisRaw("Mouse Y");
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        localEulerAnglesInput = new Vector2(Input.GetAxisRaw("Mouse X"), mousey);

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        
        if (isWallRunning)
        {
            
            playerCam.GetComponent<Camera>().fieldOfView = 96;
            if (Physics.Raycast(transform.position, transform.right, 1f, ground))
            {
                rb.velocity += transform.right * 0.1f;
                
                if (playerCam.localEulerAngles.z < 15f || playerCam.localEulerAngles.z>345f )
                {
                    
                    playerCam.localEulerAngles += new Vector3(0, 0, 100f*Time.deltaTime);
                }
                
            }
            if (Physics.Raycast(transform.position, -transform.right, 1f, ground))
            {
                if (playerCam.localEulerAngles.z >345f)
                {
                    playerCam.localEulerAngles += new Vector3(0, 0, -100f*Time.deltaTime);
                }
                playerCam.localEulerAngles += new Vector3(0, 0, -10f*Time.deltaTime);
                rb.velocity += transform.right * -0.1f;
            }
        }
        else
        {
            playerCam.GetComponent<Camera>().fieldOfView = 90;
        }
        if(isWallRunning && rb.velocity.magnitude<=30){
            rb.velocity+=transform.forward*100*Time.deltaTime+transform.up*-0.1f;
        }
        cs = sensitivity;
        cms=speed;
        //playerCam.localPosition=new Vector3(playerCam.localPosition.x,camY,playerCam.localPosition.z);
        if(isWallRunning && isGrounded){
            isWallRunning=false;
        }
        camRot -= localEulerAnglesInput.y * Time.deltaTime * cs;
        camRot=Mathf.Clamp(camRot, -70, 70);
        isGrounded=Physics.Raycast(transform.position,-transform.up,2,ground);
        rb.velocity+=transform.forward*movementInput.y*cms*Time.deltaTime+transform.right*movementInput.x*cms*Time.deltaTime;
        transform.localEulerAngles+=new Vector3(0,localEulerAnglesInput.x,0)*Time.deltaTime*cs;
        playerCam.localEulerAngles=new Vector3(camRot,playerCam.localEulerAngles.y,playerCam.localEulerAngles.z);
    }



    private void Jump(){
        if(isGrounded){
            rb.velocity=new Vector3(rb.velocity.x,JumpForce,rb.velocity.z);
        }
        if(isWallRunning){
            if(Physics.Raycast(transform.position,transform.right,1f,ground)){
                rb.velocity=new Vector3(0,JumpForce/2,0)+transform.right*-40; 
            }
            if(Physics.Raycast(transform.position,-transform.right,1f,ground)){
                rb.velocity=new Vector3(0,JumpForce/2,0)+transform.right*40; 
            }
            if(Physics.Raycast(transform.position,transform.forward,1f,ground)){
                rb.velocity=new Vector3(0,JumpForce/2,0)+transform.forward*-40; 
            }
        }
    }

    private bool canWallRun=true;
    private void OnCollisionEnter(Collision other) {
        if(Mathf.Abs(Vector3.Dot(other.GetContact(0).normal,Vector3.up))<0.1f && canWallRun){
            rb.velocity=new Vector3(0,20,0)+  transform.forward * 50;
            isWallRunning=true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        isWallRunning=false;
        StartCoroutine(WallRunCooldown());
    }
    private IEnumerator WallRunCooldown()
    {
        canWallRun = false;
        yield return new WaitForSeconds(0.3f);
        canWallRun = true;
    }
    


}
