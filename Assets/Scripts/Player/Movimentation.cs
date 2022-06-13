using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Movimentation : MonoBehaviour
{
    private Vector3 targetPos;
    public float speed = 10;
    private Animator anim;
    private DetectAreaMouse detectArea;

    private Vector3 mousePos;
    private bool isRunning = false;
    private float rotationZ;
    private Vector3 difference;

    PhotonView view;

    private MenuController menuController;

    void Start()
    { 
        //targetPos = new Vector2(0, -4);
        detectArea = GameObject.Find("AreaMouse").GetComponent<DetectAreaMouse>();
        menuController = GameObject.Find("MenuController").GetComponent<MenuController>();
        view = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
    }

    void Animation() {
        isRunning = transform.position != targetPos;
        
        difference = mousePos - transform.position;
        difference.Normalize();    
        
        if(!isRunning && !menuController.IsMenuPlayerEnable()) rotationZ = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;

        if(rotationZ >= -45 && rotationZ <= 45)
            anim.SetInteger("direction", 3);
        else if(rotationZ > 45 && rotationZ <= 135)
            anim.SetInteger("direction", 1);
        else if(rotationZ < -45 && rotationZ >= -135)
            anim.SetInteger("direction", 2);
        else
            anim.SetInteger("direction", 0);
        
        anim.SetBool("isRunning", isRunning);
    }

    void OnMouseDown() {
        if(!menuController.IsMenuPlayerEnable()) 
            menuController.OpenMenuPlayer();
    }

    void Update()
    {
        if(view.IsMine) {

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButton(0) && detectArea.getIsDetected() && !menuController.IsMenuPlayerEnable())
            {
                targetPos = new Vector3(mousePos.x, mousePos.y);
                rotationZ = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
                speed = 10;
            }

            if(menuController.IsMenuPlayerEnable()) 
                targetPos = transform.position;
            
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);

            //transform.rotation = Quaternion.LookRotation(Vector3.forward, targetPos);

            Animation();
        }
    }
}