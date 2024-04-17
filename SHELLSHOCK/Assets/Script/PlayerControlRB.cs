﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControlRB : MonoBehaviour
{
    //todo: make it so speed is doubled whenmoving in a single direction
    //this might be gay af actually but ykno might be worth considering

    //  private Rigidbody rb;
    private Camera cam;
    private float camPitch;
    private float camYaw;
    public float sensitivityH; //default 5, horizontal sensitivity
    public float sensitivityV; //default 5, vertical sensitivity
    public bool CursorLock;

    
    //Movement
    private CharacterController controller;
    public float velocityFB;
    public float velocityRL;
    private bool movingFB = false;
    private bool movingRL = false;
    public float velocitylimit; //Speed limit
    public float AccelerationMultiplier; //default 4f, increases acceleration, is equal to self * velocitylimit
    public float DeccelerationMultiplier; //default 10f, increases decceleration when not using movement keys, is equal to self * velocitylimit
    private float acceleration;
    private float decceleration;
    private Text text;

    //Jumping
    private float gravity;
    public float JumpStrength; //Self-explanatory
    public float falloffindex; //The number by which falloff is divided or multiplied by JumpStrength
    public bool dividefalloff; //When true, falloff is jumpstrength/falloffindex, when false it is multiplied instead
    private float falloff;
    private bool grounded;
    private bool jumping;
    public float AirVelocity; //Divides Decelleration when in midair, basically acts as momentum when running and jumping

    //backdraft, the shotgun recoil that makes you move
    Vector3 backdraftAmount;
    [SerializeField] float backdraftStrength;
    [SerializeField] float backdraftFalloff;

    //for shellshock score
    public float airtime;
    public float highestAirtime = 0;
   
    // Start is called before the first frame update
    void Start()
    {
        // rb = gameObject.transform.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        CursorLock = true;
        cam = GameObject.Find("Camera").transform.GetComponent<Camera>();


        controller = gameObject.transform.GetComponent<CharacterController>();
      //  text = GameObject.Find("Text").GetComponent<Text>();
        acceleration = velocitylimit * AccelerationMultiplier;
        decceleration = acceleration * DeccelerationMultiplier;


        if (dividefalloff)
        {
            falloff = JumpStrength / falloffindex;
        }
        else {
            falloff = JumpStrength * falloffindex;
        }
        
       
        gravity = 0f;
       
    }

    // Update is called once per frame
    void Update()
    {


        //Testing Purposes
        //text.text = "VelocityFB: " + velocityFB + " VelocityRL: " + velocityRL + "Gravity: " + gravity + "Grounded: " + grounded;
        //The text visual seems to display that IsGrounded is always false, but after testing it it isn't the case, use a Debug.Log to verify
        //text.text = "Gravity: " + gravity + " isGrounded: " + controller.isGrounded + " jumping: " + jumping;

        //Mouse Input for camera and player rotation
        if (CursorLock == true) {
            camYaw += Input.GetAxis("Mouse X") * sensitivityH;
            camPitch -= Input.GetAxis("Mouse Y") * sensitivityV;
            //Camera boundaries
            if (camPitch > 90f)
            {
                camPitch = 90f;
            }
            if (camPitch < -90f)
            {
                camPitch = -90f;
            }
            //Camera and Player rotation based on Input, see above
            cam.transform.eulerAngles = new Vector3(camPitch, camYaw, 0f);

            gameObject.transform.eulerAngles = new Vector3(0f, camYaw, 0f);
        }
        

        //Cursor lock/Unlock
        if (Input.GetKeyDown("tab"))
        {
            if (CursorLock)
            {
                Cursor.lockState = CursorLockMode.None;
                CursorLock = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                CursorLock = true;
            }

        }

        //Input
        if (Input.GetKey("w"))
        {
            velocityFB += acceleration * Time.deltaTime;
            movingFB = true;
        }
        if (Input.GetKeyUp("w")) {
            movingFB = false;
        }

        if (Input.GetKey("s")) {
            velocityFB -= acceleration * Time.deltaTime;
            movingFB = true;
        }
        if (Input.GetKeyUp("s"))
        {
            movingFB = false;
        }

        if (Input.GetKey("d"))
        {
            velocityRL += acceleration * Time.deltaTime;
            movingRL = true;
        }
        if (Input.GetKeyUp("d"))
        {
            movingRL = false;
        }

        if (Input.GetKey("a"))
        {
            velocityRL -= acceleration * Time.deltaTime;
            movingRL = true;
        }
        if (Input.GetKeyUp("a"))
        {
            movingRL = false;
        }
        /*
        if (Input.GetKey("space") && controller.isGrounded) {
            //jump
            JUMP(JumpStrength);
            
        }
        if (Input.GetKeyUp("space")) {
            jumping = false;
        }*/


        //Limits
        if (velocityFB > velocitylimit) {
            velocityFB = velocitylimit;
        }
        if (velocityFB < (velocitylimit * -1f)) {
            velocityFB = velocitylimit * -1f;
        }

        if (velocityRL > velocitylimit)
        {
            velocityRL = velocitylimit;
        }
        if (velocityRL < (velocitylimit * -1f))
        {
            velocityRL = velocitylimit * -1f;
        }

        //Decceleration //TO FIX MOVEMENT STUTTER FOR FUTURE PROJECTS USING THIS MOVEMENT SCRIPT, CHECK THE was STATEMENTS, PROBLEM WAS THAT YOU NEED TO COMPARE VELOCITY TO DECELLERATION, NOT TO ACCELERATION, IDK WHY I DID THAT LMAO
        if (movingFB == false && velocityFB != 0) {
            if (velocityFB > decceleration * Time.deltaTime) // was 0 + (acceleration * Time.deltaTime)
            {
                if (controller.isGrounded == false)
                {
                    velocityFB -= (decceleration / AirVelocity) * Time.deltaTime;
                }
                else {
                    velocityFB -= decceleration * Time.deltaTime;
                }
                
            }
            else if (velocityFB < (decceleration * Time.deltaTime) * -1f) //was 0 - (acceleration * Time.deltaTime)
            {
                if (controller.isGrounded == false)
                {
                    velocityFB += (decceleration / AirVelocity) * Time.deltaTime;
                }
                else {
                    velocityFB += decceleration * Time.deltaTime;
                }
                
            }
            else {
                velocityFB = 0;
            }
            
        }

        if (movingRL == false && velocityRL != 0) 
        {
            if (velocityRL > (decceleration * Time.deltaTime)) //was 0 + (acceleration * Time.deltaTime)
            {
                if (controller.isGrounded == false)
                {
                    velocityRL -= (decceleration / AirVelocity) * Time.deltaTime;
                }
                else {
                    velocityRL -= decceleration * Time.deltaTime;
                }
                
            }
            else if (velocityRL < (decceleration * Time.deltaTime) * -1f) //was 0 - (acceleration * Time.deltaTime)
            {
                if (controller.isGrounded == false)
                {
                    velocityRL += (decceleration / AirVelocity) * Time.deltaTime;
                }
                else {
                    velocityRL += decceleration * Time.deltaTime;
                }
                
            }
            else
            {
                velocityRL = 0;
            }

        }

        //backdraft Deceleration
        if (backdraftAmount != Vector3.zero) {
            //x
            if (backdraftAmount.x > 0)
            {
                if (backdraftAmount.x > backdraftFalloff * Time.deltaTime)
                {
                    backdraftAmount.x -= backdraftFalloff * Time.deltaTime;
                }
                else
                {
                    backdraftAmount.x = 0f;
                }
            }
            else if (backdraftAmount.x < 0) {
                if (backdraftAmount.x < (backdraftFalloff * -1f) * Time.deltaTime)
                {
                    backdraftAmount.x += backdraftFalloff * Time.deltaTime;
                }
                else
                {
                    backdraftAmount.x = 0f;
                }
            }
            //z
            if (backdraftAmount.z > 0)
            {
                if (backdraftAmount.z > backdraftFalloff * Time.deltaTime)
                {
                    backdraftAmount.z -= backdraftFalloff * Time.deltaTime;
                }
                else
                {
                    backdraftAmount.z = 0f;
                }
            }
            else if (backdraftAmount.z < 0)
            {
                if (backdraftAmount.z < (backdraftFalloff * -1f) * Time.deltaTime)
                {
                    backdraftAmount.z += backdraftFalloff * Time.deltaTime;
                }
                else
                {
                    backdraftAmount.z = 0f;
                }
            }
        }
        //end backdraft decelleration

        //Jumping
        if (controller.isGrounded && jumping == false)
        {
            //THAT was the issue with IsGrounded toggling on and off incessantly, basically never set gravity to 0 since IsGrounded and controller SkinWidth prevents that from happening
            gravity = 0; //comment back if it causes issues again //EDIT 2024/04/17 DID fix the falling off platform at high velocity bug, but fucks up airtime for some reason //EDIT doesnt actually fuck it up it just flickers really low values, i filtered it out in the handleText() on ammoScript
            if (airtime > highestAirtime) {
                highestAirtime = airtime;
            }
            airtime = 0;
        }
        else {
            gravity -= falloff * Time.deltaTime;
            if (gravity < falloff * -5f)
            {
                gravity = falloff * -5f;
            }
            airtime += Time.deltaTime;

            if (!controller.isGrounded) {
                jumping = false;
            }
        }


        //Move
        controller.Move((gameObject.transform.forward * velocityFB) * Time.deltaTime);
        controller.Move((gameObject.transform.right * velocityRL) * Time.deltaTime);
        //for backdraft
        // print("current backdraft move amount is " + backdraftAmount * Time.deltaTime);
         controller.Move((backdraftAmount) * Time.deltaTime);
       // controller.Move(((gameObject.transform.forward) * backdraftAmount.x) * Time.deltaTime);
        //controller.Move(((gameObject.transform.right) * backdraftAmount.z) * Time.deltaTime);
        if (controller.isGrounded == false) {
            controller.Move((gameObject.transform.up * gravity) * Time.deltaTime);
        }
       

    }

    public void JUMP(float STRENGTH) //Call this function to make the player jump, used in JumpPads
    {
        jumping = true;
        gravity = STRENGTH;
    }

    public void backdraft(Vector3 direction) {
        if (velocityFB > 0) {
            velocityFB = 0f;
        }
        //velocityRL = 0f;
        backdraftAmount = new Vector3(direction.x * backdraftStrength, 0f, direction.z * backdraftStrength);
        print("backdraft direction y is " + direction.y);
        print("backdraft amount is " + backdraftAmount);
        if (direction.y > 0) { //basically dont count it as a jump if youre not aiming downwards, otherwise airtime starts increasing on its own despite the controller being technically grounded, something about the direct velocity manipulation fucking with that
            jumping = true;
        }
        
        gravity = direction.y * backdraftStrength;
    }
}
