using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StarParticles : MonoBehaviour
{
    float sensitivity = 0.25f;
    private Camera cam;
    private ParticleSystem.Particle[] stars;
    public int maxStars = 100;
    public float starSize = 1.0f;

    public float speed = 0.1f;
    private float maxSpeed = 100.0f;

    public float starDistance = 10.0f;

    private float starDistanceSqr;
    private bool visable = true;
    private Vector3 moveStar;

    void Start() {
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
            visable = true;
        }
    }

    void Update() {


        for (int i = 0; i < maxStars; i++){
            if(Input.GetKey(KeyCode.W)){
                if(speed < maxSpeed){
                    speed += Time.deltaTime;
                }
                moveStar = stars[i].position - cam.transform.forward  * speed  * Time.deltaTime ;
                stars[i].position = moveStar ;   
                transform.position = moveStar;             
            }
            if(Input.GetKey(KeyCode.A)){
                moveStar = stars[i].position + cam.transform.right * speed * Time.deltaTime;
                stars[i].position = moveStar;
                transform.position = moveStar;    
            }
            if(Input.GetKey(KeyCode.D)){
                moveStar = stars[i].position - cam.transform.right * speed * Time.deltaTime;
                stars[i].position = moveStar;
                transform.position = moveStar;    
            }
            if(Input.GetKey(KeyCode.S)){
                if(speed > 0){
                    speed -= Time.deltaTime;
                }
            }  

        } 

        GetComponent<ParticleSystem>().SetParticles(stars, stars.Length);
    }
}
