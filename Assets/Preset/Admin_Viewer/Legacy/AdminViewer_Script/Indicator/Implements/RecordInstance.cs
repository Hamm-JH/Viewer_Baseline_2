using Issue;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordInstance
{
    public DateTime dateTime;
    public string issueKinds;
    public List<Definition._Issue.Issue> DCrackList;
    public List<Definition._Issue.Issue> DEfflorescenceList;
    public List<Definition._Issue.Issue> DSpallingList;
    public List<Definition._Issue.Issue> DBreakageList;
    public List<Definition._Issue.Issue> DScour_ErosionList;

    public List<Definition._Issue.Issue> RCrackList;
    public List<Definition._Issue.Issue> REfflorescenceList;
    public List<Definition._Issue.Issue> RSpallingList;
    public List<Definition._Issue.Issue> RBreakageList;
    public List<Definition._Issue.Issue> RScour_ErosionList;
}