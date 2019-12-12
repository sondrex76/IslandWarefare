using UnityEngine;

public class Rain : MonoBehaviour
{
    private Material mat;

    [SerializeField]
    private ParticleSystem rainParticles;
    [SerializeField]
    private Camera player;

    private bool setupDone = false;
    private bool isRaining;
    private bool cycleChange;
    private float nextCycle;
    private float currentCycleTime;

    // Start is called before the first frame update
    public void Setup()
    {
        mat = FindObjectOfType<Terrain>().materialTemplate;
        if (!rainParticles)
            rainParticles = FindObjectOfType<ParticleSystem>();
        if (!player)
            player = FindObjectOfType<Camera>();

        int rain = Random.Range(0, 2);
        //Set current state of rain
        if (rain == 1)
        {
            isRaining = true;
            mat.SetFloat("Vector1_46396BE3", 0.45f);
            mat.SetFloat("Vector1_1F126B9E", 0.45f);
            rainParticles.Play();
        }
        else
        {
            mat.SetFloat("Vector1_46396BE3", 0f);
            mat.SetFloat("Vector1_1F126B9E", 0f);
            rainParticles.Stop();
            isRaining = false;
        }

        Debug.Log(rain);

        Vector3 pos = new Vector3(player.transform.position.x, rainParticles.transform.position.y, player.transform.position.z);
        rainParticles.transform.position = pos;

        cycleChange = false;
        nextCycle = Random.Range(100, 500);
        currentCycleTime = 0;
        setupDone = true;
    }

    private void FixedUpdate()
    {
        //Check if the material is gotten
        if (setupDone)
        {
            Vector3 pos = new Vector3(player.transform.position.x, rainParticles.transform.position.y, player.transform.position.z);
            rainParticles.transform.position = pos;

            currentCycleTime += Time.fixedDeltaTime;

            if (cycleChange)
            {
                if (!isRaining)
                {
                    float grass = mat.GetFloat("Vector1_46396BE3");
                    float stone = mat.GetFloat("Vector1_1F126B9E");

                    grass -= Time.fixedDeltaTime * 0.45f / 5;
                    stone -= Time.fixedDeltaTime * 0.45f / 5;

                    mat.SetFloat("Vector1_46396BE3", grass);
                    mat.SetFloat("Vector1_1F126B9E", stone);

                    rainParticles.Stop();

                    //Check if the smoothness has changed
                    if (grass <= 0f && stone <= 0f) cycleChange = false;
                }

                else
                {
                    float grass = mat.GetFloat("Vector1_46396BE3");
                    float stone = mat.GetFloat("Vector1_1F126B9E");

                    grass += Time.fixedDeltaTime * 0.45f;
                    stone += Time.fixedDeltaTime * 0.45f;

                    mat.SetFloat("Vector1_46396BE3", grass);
                    mat.SetFloat("Vector1_1F126B9E", stone);

                    rainParticles.Play();

                    //Check if the smoothness has changed
                    if (grass >= 0.45f && stone >= 0.45f) cycleChange = false;
                }
            }

            if (currentCycleTime >= nextCycle)
            {
                Debug.Log(isRaining);
                cycleChange = true;
                isRaining = !isRaining;
                nextCycle = Random.Range(100, 500);
                currentCycleTime = 0;
                Debug.Log("Nya");
            }
        }
    }
}
