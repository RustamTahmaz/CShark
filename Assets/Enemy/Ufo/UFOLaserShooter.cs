using UnityEngine;
using System.Collections;

public class UFOLaserShooter : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform  laserSpawn;
    public float      fireInterval = 1.5f;

    private Coroutine fireRoutine;

    private void OnEnable()
    {
        // begin firing once the component is turned on
        fireRoutine = StartCoroutine(FireContinuously());
    }

    private void OnDisable()
    {
        if (fireRoutine != null)
            StopCoroutine(fireRoutine);
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            Instantiate(laserPrefab, laserSpawn.position, laserSpawn.rotation);
            AudioManager.Instance.PlaySfx(AudioManager.Instance.laserSfx);
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
