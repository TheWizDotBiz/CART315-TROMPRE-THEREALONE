using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotgunSway : MonoBehaviour
{
    [SerializeField] private GameObject initPos; //empty object at the same local position as the anhcor, they should have a common parent, this doesnt move.
    [SerializeField] private GameObject Anchor; //anchor should be an empty gameobject holding the animated object, MOVEMENT IS APPLIED TO THIS OBJECT, MEANING IT WONT INTERFERE WITH THE ANIMATOR
    private float Hoffset = 0.05f;
    private float Voffset = 0.1f; //should be twice what Hoffset is, since there is two different sources for Hoffset: MouseX and VelocityRL from PlayerControl
    private float Zoffset = 0.1f;
    [SerializeField] private PlayerControlRB PC;
    private float falloff = 0.8f;
    private float SwaySpeed = 1f;
    private float Hindex;
    private float Vindex;
    private float Zindex;
    private float PHindex;

    private float mouseX;
    private float mouseY;
    private float playerVelocity; //retrieved from PlayerControl.cs
    private float HplayerVelocty; //same

    public bool lockAnchor;

    [SerializeField] private AmmoScript ammoscript;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
        playerVelocity = PC.velocityFB;
        HplayerVelocty = PC.velocityRL;

        if (PC.CursorLock)
        {
            updateIndexes();
        }

        moveAnchor();

        handleFallof();

        //print("Hindex: " + Hindex + " Vindex: " + Vindex + "Zindex: " + Zindex);
        //ANIM TESTING
        /*
        if (Input.GetMouseButtonDown(0)) {
            GetComponent<Animator>().SetTrigger("Shoot");
        }
        if (Input.GetKeyDown("q")) {
            GetComponent<Animator>().SetTrigger("Pump");
        }*/
    }

    void moveAnchor()
    {
        if (lockAnchor)
        {
            if (Anchor.transform.localPosition != initPos.transform.localPosition)
            {
                Anchor.transform.localPosition = initPos.transform.localPosition;
            }

        }
        else
        {
            Anchor.transform.localPosition = new Vector3(initPos.transform.localPosition.x + Hindex + PHindex, initPos.transform.localPosition.y + Vindex, initPos.transform.localPosition.z + Zindex);
            //print("lockAnchor false");
        }
    }

    void updateIndexes()
    {
        Hindex += (mouseX * Time.deltaTime) * SwaySpeed;
        if (Hindex > Hoffset)
        {
            Hindex = Hoffset;
        }
        else if (Hindex < Hoffset * -1f)
        {
            Hindex = Hoffset * -1f;
        }

        Vindex += (mouseY * Time.deltaTime) * SwaySpeed;
        if (Vindex > Voffset)
        {
            Vindex = Voffset;
        }
        else if (Vindex < Voffset * -1f)
        {
            Vindex = Voffset * -1f;
        }

        Zindex += (playerVelocity * Time.deltaTime) * SwaySpeed;
        if (Zindex > Zoffset)
        {
            Zindex = Zoffset;
        }
        else if (Zindex < Zoffset * -1f)
        {
            Zindex = Zoffset * -1f;
        }

        PHindex += (HplayerVelocty * Time.deltaTime) * SwaySpeed;
        if (PHindex > Hoffset)
        {
            PHindex = Hoffset;
        }
        else if (PHindex < Hoffset * -1f)
        {
            PHindex = Hoffset * -1f;
        }
    }

    void handleFallof()
    {
        //Horizontal
        if (mouseX == 0)
        {
            if (Hindex > 0)
            {
                Hindex -= falloff * Time.deltaTime;
                if (Hindex <= 0)
                {
                    Hindex = 0;
                }
            }
            else if (Hindex < 0)
            {
                Hindex += falloff * Time.deltaTime;
                if (Hindex >= 0)
                {
                    Hindex = 0;
                }
            }

        }

        //Vertical
        if (mouseY == 0)
        {
            if (Vindex > 0)
            {
                Vindex -= falloff * Time.deltaTime;
                if (Vindex <= 0)
                {
                    Vindex = 0;
                }
            }
            else if (Vindex < 0)
            {
                Vindex += falloff * Time.deltaTime;
                if (Vindex >= 0)
                {
                    Vindex = 0;
                }
            }
        }

        //Depth (READ PLAYER VELOCITY FROM PC)
        if (playerVelocity == 0)
        {
            if (Zindex > 0)
            {
                Zindex -= falloff * Time.deltaTime;
                if (Zindex <= 0)
                {
                    Zindex = 0;
                }
            }
            else if (Zindex < 0)
            {
                Zindex += falloff * Time.deltaTime;
                if (Zindex >= 0)
                {
                    Zindex = 0;
                }
            }
        }

        //Horizontal player-based (i.e, the one where the player moves side to side.)
        if (HplayerVelocty == 0)
        {
            if (PHindex > 0)
            {
                PHindex -= falloff * Time.deltaTime;
                if (PHindex <= 0)
                {
                    PHindex = 0;
                }
            }
            else if (PHindex < 0)
            {
                PHindex += falloff * Time.deltaTime;
                if (PHindex >= 0)
                {
                    PHindex = 0;
                }
            }
        }
    }

    //interactivity
    void pumpRunCycleChamber() {
        ammoscript.CycleChamber();
    }

    void shootRunShootShotgun() {
        ammoscript.shootShotgun();
    }
}
