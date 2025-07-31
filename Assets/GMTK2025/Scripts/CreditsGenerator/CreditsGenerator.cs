using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;

internal class CreditsGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform creditsParent;

    [SerializeField]
    private CreditsDepartmentPrefab departmentPrefab;

    [SerializeField]
    private CreditParticipantPrefab participantPrefab;

    [SerializeField]
    private List<CreditsDepartment> departments;

    [Button]
    private void Generate()
    {
        while (creditsParent.childCount > 0)
        {
            var child = creditsParent.GetChild(creditsParent.childCount - 1);
            if (Application.isPlaying)
            {
                Destroy(child.gameObject);
            }
            else
            {
                DestroyImmediate(child.gameObject);
            }
        }
        foreach (var dept in departments)
        {
            Debug.Log("Spawning dept " + dept.DepartmentName);
            var deptInstance = Instantiate(departmentPrefab, creditsParent);
            deptInstance.Initialize(dept, participantPrefab);
        }
    }
}
