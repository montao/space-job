using UnityEngine;

public class infiniteStars : MonoBehaviour {

    private Transform tx;
    public int maxStars = 100;

    public float starsize = 1.0f;

    public float starDistance = 10.0f;

    private ParticleSystem.Particle[] stars;
    private float starDistanceSqr;
    public float clippingDist = 1.0f;
    private float clippingDistSqr;
    // Start is called before the first frame update
    void Start(){
       tx = transform;  
       starDistanceSqr = starDistance * starDistance;
       clippingDistSqr = clippingDist * clippingDist;
    }

    private void CreateStars() {
        stars = new ParticleSystem.Particle[maxStars];
        for(int i = 0; i < maxStars; i++){
            stars[i].position = Random.insideUnitSphere * starDistance + tx.position;
#pragma warning disable CS0618
            stars[i].color = new Color(1,1,1,1);
            stars[i].size = starsize;
#pragma warning restore CS0618
        }
    }

    // Update is called once per frame
    void Update(){
        if(stars == null){
            CreateStars();
        }

        for(int i = 0; i < maxStars; i++){
            if((stars[i].position - tx.position).sqrMagnitude > starDistanceSqr){
                stars[i].position = Random.insideUnitSphere.normalized * starDistance + tx.position;
            }
            if((stars[i].position - tx.position).sqrMagnitude <= clippingDistSqr){
                float visability = (stars[i].position - tx.position).sqrMagnitude / clippingDistSqr; //clipping distance 100%
#pragma warning disable CS0618
                stars[i].color = new Color(1,1,1,visability);
                stars[i].size = starsize * visability;
#pragma warning restore CS0618
            }
        }

        GetComponent<ParticleSystem>().SetParticles(stars, stars.Length);
    }
}
