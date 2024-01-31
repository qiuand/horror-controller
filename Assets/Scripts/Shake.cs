using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    Vector3 originalPos;
    public bool startShake = false;
    float shakeyStrengthMulti = 1.0f;
    float duration = 0.25f;
    public AnimationCurve curve;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (startShake)
        {
            startShake = false;
            StartCoroutine(ShakeCam(1f));
        }
    }
        public IEnumerator ShakeCam(float shakeStrength)
        {
/*            Vector3 startPos = transform.position;*/
            float timeElapsed = duration;
            while (timeElapsed > 0)
            {
            timeElapsed -= Time.deltaTime;
                float strength = curve.Evaluate(timeElapsed / duration)*shakeStrength;
                transform.position = originalPos + Random.insideUnitSphere*strength;
                yield return null;
            }
            transform.position = originalPos;
        timeElapsed = duration;
        }
    }

