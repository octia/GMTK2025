using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal class CreditsDepartment
{
    [SerializeField]
    internal string DepartmentName;

    [SerializeField]
    internal List<Participant> Participants;
}
