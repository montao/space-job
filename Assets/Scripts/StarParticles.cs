using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StarParticles : MonoBehaviour
{
    //float sensitivity = 0.25f;
    public GameObject skybox;
    private Collider sky_col;
    public GameObject checkStars;
    private Collider check_col;
    private Camera cam;
    private ParticleSystem.Particle[] stars;
    public int maxStars = 100;
    public float starSize = 1.0f;

    private Vector3 lastDirection = new Vector3();
    public bool smooth = true;
    public float acceleration = 0.1f;
    private float actualSpeed = 0.0f; // from 0 to 1
    public float speed = 25.0f;

    public float starDistance = 10.0f;

    private float starDistanceSqr;
    private Vector3 moveStar;
    private ParticleSystem starsystem;

    void Start() {
        starsystem = GetComponent<ParticleSystem>();
        sky_col = skybox.GetComponent<Collider>();
        check_col = checkStars.GetComponent<Collider>();
        cam = Camera.main;
        starDistanceSqr = starDistance * starDistance;
        createStars();
    }

    private void createStars(){
        stars = new ParticleSystem.Particle[maxStars];
        
        for(int i = 0; i < maxStars; i++){
            //current particle position around camera (sphere) * distace (inside sphere) * transofrm ( keeps particle around camera)
            stars[i].position = Random.insideUnitSphere * starDistance + cam.transform.position;
            stars[i].color = new Color(1,1,1,1);
            stars[i].size = starSize;
        }
    }

    bool InsideSky(Collider sky, Vector3 star_pos){
        Vector3 closest = sky.ClosestPoint(star_pos);
        return closest == star_pos;
    }

    void Update() {
        for (int i = 0; i < maxStars; i++){
            if(Input.GetKey(KeyCode.W)){
/*                 if(speed < maxSpeed){
                    speed += Time.deltaTime;
                } */
                moveStar = stars[i].position - cam.transform.forward - new Vector3(1.0f,1.0f,1.0f)/*  * speed  * Time.deltaTime  */;

                /* stars[i].position = moveStar ;   
                transform.position = moveStar;   */           
            }
            if(Input.GetKey(KeyCode.A)){
                moveStar = stars[i].position + cam.transform.right + new Vector3(1.0f,1.0f,1.0f)/* * speed * Time.deltaTime */;
                /* stars[i].position = moveStar;
                transform.position = moveStar;  */   
            }
            if(Input.GetKey(KeyCode.D)){
                moveStar = stars[i].position - cam.transform.right - new Vector3(1.0f,1.0f,1.0f)/* * speed * Time.deltaTime */;
                /* stars[i].position = moveStar;
                transform.position = moveStar; */    
            }
            if(Input.GetKey(KeyCode.S)){
                if(speed > 0){
                    speed -= Time.deltaTime;
                }
            }  

            moveStar.Normalize();

            if(moveStar != Vector3.zero){ //if there is movement
                if(actualSpeed <1){
                    actualSpeed += acceleration * Time.deltaTime * 40;
                }
                else actualSpeed = 1.0f;
                lastDirection = moveStar;
            }
            else{ //no movement
                if(actualSpeed > 0){
                    actualSpeed -= acceleration * Time.deltaTime * 20;
                }
                else actualSpeed = 0;
            }
/*             if(smooth){
                stars[i].position = (lastDirection * actualSpeed * speed * Time.deltaTime);
                transform.position = (lastDirection * actualSpeed * speed * Time.deltaTime);
            }
            else{ */
                stars[i].position = (moveStar * speed * Time.deltaTime);
                transform.position = (moveStar * speed * Time.deltaTime);
            //}
            

            /* if(Vector3.Distance(stars[i].position, cam.transform.position)>500){
                Vector3 newPos = 1 * stars[i].position * Time.deltaTime;
                stars[i].position = newPos;
            } */
            
            /* if(InsideSky(check_col,stars[i].position) && InsideSky(sky_col,stars[i].position)){
                stars[i].position = stars[i].position;
                Debug.Log("both");
                
            }  
            else{S
                stars[i].position = Vector3.MoveTowards(stars[i].position, cam.transform.position, 100.0f*Time.deltaTime); 
                Debug.Log("outside skybox");
            }
 */
        } 
        

        //Debug.Log(Vector3.Distance(starsystem.transform.position, cam.transform.position));
        
        GetComponent<ParticleSystem>().SetParticles(stars, stars.Length);
    }
    void OnGUI() {
        GUILayout.Box("actual Speed: " + actualSpeed.ToString() );
        
    }

}
