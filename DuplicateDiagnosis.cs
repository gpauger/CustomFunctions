//Initialize parameters
const string QUERY_TEXT = "Duplicate diagnosis.  Please correct.";
const int MARKING_GROUP_ID = 1;
const bool ANSWER_ON_CHANGE = false;
const bool CLOSE_ON_CHANGE = false;

//get the cmts field passed in from rave
ActionFunctionParams afp = (ActionFunctionParams) ThisObject;
DataPoint dpCMT = afp.ActionDataPoint;

//get a reference to all records on the CORETB_BTA page
Records allRecords = dpCMT.IDataPage.Records;

//Order the records by thier record position
//allRecords = GetSortedRecords(allRecords);
bool openQuery = false;
DataPoint i_dp = null; //for outer loop
DataPoint j_dp = null;  //for inner loop
// i>1 because allrecords also contains master record
for (int i = allRecords.Count-1; i>1; i--)
{
    if (allRecords[i].Active) //skip inactive records
    {
        i_dp = allRecords[i].DataPoints.FindByFieldOID(fieldOID);
        for (int j = i -1; j>0; j--)
        {
            if (allRecords[j].Active)
            {
                j_dp = allRecords[j].DataPoints.FindByFieldOID(fieldOID);
                if (string.Compare(i_dp.Data, j_dp.Data, true) == 0)
                {
                    openQuery = true;
                    break;
                }
            }
        }
        //open or close query as necessary
        CustomFunction.PerformQueryAction(QUERY_TEXT, MARKING_GROUP_ID,
            ANSWER_ON_CHANGE, CLOSE_ON_CHANGE, i_dp, openQuery, afp.CheckID, afp.CheckHash);
            openQuery = false;
    }
}
return null;



