using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformHelper : MonoBehaviour
{
    private static TransformHelper _instance = null;

    [Header("Movement settings")]
    [SerializeField] private float movementDuration;
    [SerializeField] private AnimationCurve movementCurve;

    private static TransformHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                throw new System.Exception("You must add TransformHelper on scene");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as TransformHelper;
        }
    }

    /// <summary>
    /// Move the transform smoothly
    /// </summary>
    public static void SmoothMove(Transform transform, Vector3 endPosition)
    {
        Instance.StartCoroutine(Instance.Move(transform, endPosition));
    }

    private IEnumerator Move(Transform transform, Vector3 endPosition)
    {
        Vector3 startPosition = transform.position;
        float timer = 0.0f;
        while (timer < movementDuration)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, movementCurve.Evaluate(timer / movementDuration));
            yield return null;
        }
        transform.position = endPosition;
    }

    /// <summary>
    /// Rotate the transform smoothly
    /// </summary>
    public static void SmoothRotate(Transform transform, Vector3 targetRotation)
    {
        Instance.StartCoroutine(Instance.Rotate(transform, targetRotation));
    }

    private IEnumerator Rotate(Transform transform, Vector3 targetRotation)
    {
        while (transform.rotation != Quaternion.Euler(targetRotation))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), 200 * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// Returns a list of points that are equally spaced from each other along the X axis
    /// </summary>
    public static List<Vector3> GetPointsForLine(Vector3 position, int length, int numberOfPoints)
    {
        List<Vector3> points = new List<Vector3>();

        float step = (float)length / numberOfPoints;
        float temp = position.x - (float)length / 2 + step / 2;

        for (int i = 0; i < numberOfPoints; i++)
        {
            points.Add(new Vector3(position.x + temp, position.y, position.z));
            temp += step;
        }

        return points;
    }
}
