using UnityEngine;
using UnityEngine.Events;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Rain : MonoBehaviour
{
    [System.Serializable]
    public class RainSaved : UnityEvent<string> { };
    [SerializeField]
    private RainSaved saved;

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

        if (File.Exists(Application.persistentDataPath + "/Rain"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/Rain", FileMode.Open);
            RainSave save = (RainSave)bf.Deserialize(file);
            file.Close();

            Debug.Log("Nya");

            isRaining = save.isRaining;
            nextCycle = save.nextCycle;
            currentCycleTime = save.currentCycleTime;

            if (isRaining)
            {
                SetSmoothness(0.45f, 0.45f);
                rainParticles.Play();
            }
            else
            {
                SetSmoothness(0f, 0f);
                rainParticles.Stop();
            }
        }

        else
        {

            int rain = Random.Range(0, 2);
            //Set current state of rain
            if (rain == 1)
            {
                isRaining = true;
                SetSmoothness(0.45f, 0.45f);
                rainParticles.Play();
            }
            else
            {
                isRaining = false;
                SetSmoothness(0f, 0f);
                rainParticles.Stop();
            }

            nextCycle = Random.Range(100, 500);
            currentCycleTime = 0;

        }

        Vector3 pos = new Vector3(player.transform.position.x, rainParticles.transform.position.y, player.transform.position.z);
        rainParticles.transform.position = pos;

        cycleChange = false;
        setupDone = true;
    }

    private void SetSmoothness(float grass, float stone)
    {
        mat.SetFloat("Vector1_46396BE3", grass);
        mat.SetFloat("Vector1_1F126B9E", stone);
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void SceneChange()
    {
        Save();
        saved.Invoke("IslandMap");
    }

    private void Save()
    {
        //Reset material smoothness
        SetSmoothness(0f, 0f);

        BinaryFormatter bf = new BinaryFormatter();

        RainSave save = new RainSave(isRaining, nextCycle, currentCycleTime);
        FileStream file = File.Create(Application.persistentDataPath + "/Rain");
        bf.Serialize(file, save);
        file.Close();
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

                    SetSmoothness(grass, stone);

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

                    SetSmoothness(grass, stone);

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
            }
        }
    }
}
