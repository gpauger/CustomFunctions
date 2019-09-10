//To decide whether any AEs have been logged, look for at least one
//DataPoint matching the OID path that has complete data.
const string REQUIRED_AE_FIELD_OID = "AESYMP";
const string AE_FORM_OID = "AE";
const string AE_FOLDER_OID = "AE";
const string QUERY_TEXT = @"AE Y/N is answered 'YES', but no AE log symptoms have been completed.";

const int MARKING_GROUP_ID = 1;
const bool ANSWER_ON_CHANGE = false;
const bool CLOSE_ON_CHANGE = false;
const string YES = "1";

//Get the AEYN DataPoint passed in from Rave
//this comes from the check action data point in rave
ActionFunctionParams afp = (ActionFunctionParams) ThisObject;
DataPoint dpAEYN = afp.ActionDataPoint;

bool openQuery = false;

if (dpAEUM.Data == YES)
{
    openQuery = true;

    Subject current_Subject = dpAEYN.Record.Subject;
    DataPoints dpsAESYMP = CustomFunction.FetchAllDataPointsForOIDPath(REQUIRED_AE_FIELD_OID, AE_FORM_OID, AE_FOLDER_OID, current_Subject);

    //Look for one completed AE symptom and stop looking when found
    for (int i = 0; i<dpsAESYMP.Count; i++)
    {
        //Inactivated log lines shouldn't count as valid AEs
        if (dpsAESYMP[i].Active && dpsAESYMP[i].EntryStatus == EntryStatusEnum.EnteredComplete)
        {
            openQuery = false;
            break;
        }
    }
}
//Open or close query as necessary
CustomFunction.PerformQueryAction(QUERY_TEXT, MARKING_GROUP_ID, ANSWER_ON_CHANGE, CLOSE_ON_CHANGE, dpAEYN, openQuery, afp.CheckID, afp.CheckHash);
return null;