using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifferenceManager : MonoBehaviour
{
    [SerializeField] List<DifferenceObject> differenceObjects = new List<DifferenceObject>();
    [SerializeField] float distance = -10f;

    public int spottedDifferences = 0;

    [SerializeField] private Button homeButton;
    private SpotDifferencesEntryPoint _entryPoint;

    void Start()
    {
        foreach (DifferenceObject diffObj in differenceObjects)
        {
            diffObj.InitializeDifference(distance);
        }
    }

    public void ResolveDifference(DifferenceObject diffObj)
    {
        // Increment spotted differences in the manager.
        spottedDifferences++;

        if (differenceObjects.Contains(diffObj)) differenceObjects.Remove(diffObj);
        else differenceObjects.Remove(diffObj.GetCounterpart());

        if (differenceObjects.Count <= 0 )
        {
            print("Game Over!");
        }
    }

    public void SetEntryPoint(SpotDifferencesEntryPoint entryPoint)
    {
        _entryPoint = entryPoint;
    }

    private void SetFinishForPackage()
    {
        StartCoroutine(FinishAfterFireworks());
    }

    private IEnumerator FinishAfterFireworks()
    {
        yield return new WaitForSeconds(5f);
        _entryPoint.InvokeGameFinished();
    }
}