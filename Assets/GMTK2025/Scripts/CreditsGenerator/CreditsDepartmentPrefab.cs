using TMPro;
using UnityEngine;

internal class CreditsDepartmentPrefab : MonoBehaviour
{
    [SerializeField]
    private TMP_Text departmentName;

    [SerializeField]
    private Transform participantParent;

    internal void Initialize(CreditsDepartment dept, CreditParticipantPrefab participantPrefab)
    {
        departmentName.text = dept.DepartmentName.Trim();
        foreach (var participantData in dept.Participants)
        {
            var participantInst = Instantiate(participantPrefab, participantParent);
            participantInst.Initialize(participantData);
        }
    }
}
