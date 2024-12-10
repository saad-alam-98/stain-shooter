using System.Linq;
using UnityEngine;
using System.Collections;

public class ClothsSpawner : MonoBehaviour
{
    public ClothComponent[] Cloths;
    public float StartDelay = 0.5f;
    public float SpawnInterval = 1.5f;

    public void StartGame()
    {
        foreach (var c in Cloths)
        {
            c.gameObject.SetActive(false);
        }
        StartCoroutine(SpawnCloth());
    }

    public IEnumerator SpawnCloth()
    {
        yield return new WaitForSeconds(StartDelay);

        while (true)
        {
            yield return new WaitForSeconds(SpawnInterval);

            var inActiveCloths = Cloths.Where(c => !c.gameObject.activeSelf).ToArray();
            var cloth = inActiveCloths[Random.Range(0, inActiveCloths.Length)];

            if (cloth != null)
            {
                cloth.Setup();
            }
        }
    }

    public void EndGame()
    {
        StopCoroutine(SpawnCloth());
    }
}
