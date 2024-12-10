using System.Linq;
using UnityEngine;
using System.Collections;

public class CloudsSpawner : MonoBehaviour
{
    public Clouds[] Cloths;
    public float SpawnInterval = 1.5f;

    private void Start()
    {
        StartCoroutine(SpawnCloth());
    }

    public IEnumerator SpawnCloth()
    {
        while (true)
        {
            yield return new WaitForSeconds(SpawnInterval);

            if (Random.value < 0.1f)
                continue;

            var inActiveCloths = Cloths.Where(c => !c.gameObject.activeSelf).ToArray();
            Clouds cloud = null;
            if (inActiveCloths.Length > 0)
            {
                cloud = inActiveCloths[Random.Range(0, inActiveCloths.Length)];
            }

            if (cloud != null)
            {
                cloud.Setup();
            }
        }
    }
}
