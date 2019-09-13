//This is the OID path for the enrollment date
const string FIELD_OID = "PARTIC_ENROL_DT";
const string FORM_OID = "SUBJECT_ENROLLMENT";
const string FOLDER_OID = "SCREEN";
const string QUERY_TEXT = @"Diagnosis date must be before enrollment date but not more than 180 days prior.";

const int MARKING_GROUP_ID = 1;
const bool ANSWER_ON_CHANGE = false;
const bool CLOSE_ON_CHANGE = false;

//Get the NEGDT field passed in from RAVE
ActionFunctionParams afp =(ActionFunctionParams) ThisObject;
DataPoint dpDiagnosis = afp.ActionDataPoint;

//Initially we dont plan to open a query
bool openQuery = false;

//only obtain the enrollment date if the diagnosis is passed in is a valid date
if (dpDiagnosis.StandardValue() is DateTime)
{
    //Get a reference to the current subject
    Subject current_Subject = dpDiagnosis.Record.Subject;
    //Fetch enrollment date
    DataPoints dpsENRLDT = CustomFunction.FetchAllDataPointsForOIDPath(
        FIELD_OID, FORM_OID, FOLDER_OID, current_Subject
    );
    //Dont continue unless the enrollment is valid  (Rave didn't like this if statement???)
    //if (dpsENRLDT.Count > 0 dpsENRLDT[0].StandardValue() is DateTime)
   // {
        //Perform casts directly from StandardValue() instead of using Convert.ToDateTime
        DateTime enrollment_DT = (DateTime)dpsENRLDT[0].StandardValue();
        DateTime diagnosis_DT = (DateTime)dpDiagnosis.StandardValue();
        //Set openQuery to true if the dates are outside timespan
        openQuery = !(diagnosis_DT < enrollment_DT && 
                      diagnosis_DT >= enrollment_DT.Subtract( 
                          new TimeSpan(180, 0, 0, 0, 0)));
   // }
}

//Open or close query as necessary
CustomFunction.PerformQueryAction(
    QUERY_TEXT, MARKING_GROUP_ID, ANSWER_ON_CHANGE, CLOSE_ON_CHANGE,
    dpDiagnosis, openQuery, afp.CheckID, afp.CheckHash
);
return null;