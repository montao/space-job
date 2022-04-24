using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class moveCam : MonoBehaviour{
    public float speed = 25.0f; //max speed of cam
    public float sensitivity = 0.25f;
    private Vector3 lastMousePos = new Vector3(255, 255, 255);
    //smooth
    public bool smooth = true;
    public float acceleration = 0.1f;
    private float actualSpeed = 0.0f; // from 0 to 1
    private Vector3 lastDirection = new Vector3();
    // Start is called before the first frame update
    void Start() {
        
    }

    void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {

        // view position
        lastMousePos = Input.mousePosition - lastMousePos;
        lastMousePos *= sensitivity;
        lastMousePos = new Vector3(transform.eulerAngles.x - lastMousePos.y, transform.eulerAngles.y + lastMousePos.x, 0); // x-rotation + mouse.y
        transform.eulerAngles = lastMousePos;
        lastMousePos = Input.mousePosition;



        // movement of camera
        Vector3 direction = new Vector3(); // (0.0,0.0,0.0)

        if(Input.GetKey(KeyCode.W)){
            direction.z += 1.0f;
        }
        if(Input.GetKey(KeyCode.S)){
            direction.z -= 1.0f;
        }
        if(Input.GetKey(KeyCode.D)){
            direction.x += 1.0f;
        }
        if(Input.GetKey(KeyCode.A)){
            direction.x -= 1.0f;
        }

        direction.Normalize();

        if(direction != Vector3.zero){ // if movement
            if(actualSpeed < 1){
                actualSpeed += acceleration * Time.deltaTime * 40;
            }
            else actualSpeed = 1.0f;
            lastDirection = direction;
        }
        else { // no movement
            if(actualSpeed > 0){
                actualSpeed -= acceleration * Time.deltaTime * 20;
            }  
            else actualSpeed = 0.0f;
        }

        if(smooth){
            transform.Translate(lastDirection * actualSpeed * speed * Time.deltaTime);
        }
        else transform.Translate(direction * speed * Time.deltaTime);
    }
    void OnGUI() {
        GUILayout.Box("actual Speed: "/* + actualSpeed.ToString() */);
        
    }
}
