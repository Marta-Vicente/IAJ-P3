using UnityEditor;
using UnityEngine;

public class Scream : MonoBehaviour
{
    public GameObject asset;
    public float expansionSpeed;
    public float lifetime;
    public Vector3 centerPosition = Vector3.zero;

    private float currentTime = 0.0f;

    public Scream(Vector3 center, GameObject asset)
    {
        centerPosition = center;
        this.asset = asset;
    }

    public void Awake()
    {
        expansionSpeed = 100.0f;
        lifetime = 0.25f;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        float scale = 3.0f + currentTime * expansionSpeed;

        this.asset.transform.position = centerPosition;

        this.asset.transform.localScale = new Vector3(scale, 1.0f, scale);

        
        if (currentTime >= lifetime)
        {
            Destroy(asset);
            Destroy(gameObject);
        }
    }
}