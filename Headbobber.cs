using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class Headbobber: MonoBehaviour 
 {
 
  private float timer = 0.0f;
  float bobbingSpeed = 0.07f;
  float bobbingAmount = 0.3f;
  float midpoint = 0.7f;

  public Rigidbody player;
  
  void Update () {
        Debug.Log(timer);
        if (!player.GetComponent<Controller>().isWallRunning)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, player.GetComponent<Controller>().movementInput.x * -3);
        }
      
      float waveslice = 0.0f;
  
  Vector3 cSharpConversion = transform.localPosition; 
  
     if (player.velocity.magnitude<=0.01f || !player.GetComponent<Controller>().isGrounded) {
        timer = 0.0f;
            
     }
     else {
        waveslice = Mathf.Sin(timer);
        timer = timer + bobbingSpeed;
        if (timer > Mathf.PI * 2) {
           timer = timer - (Mathf.PI * 2);
        }
     }
     if (waveslice != 0) {
        float translateChange = waveslice * bobbingAmount;
        float totalAxes = 1;
        totalAxes = Mathf.Clamp (totalAxes, 0.0f, 1.0f);
        translateChange = totalAxes * translateChange;
        cSharpConversion.y = midpoint + translateChange;
     }
     else {
        cSharpConversion.y = midpoint;
     }
  
  transform.localPosition = cSharpConversion;
  }
  
  
 
 }
