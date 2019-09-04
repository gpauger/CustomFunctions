const string MATRIX_OID = "NEXT_RES_UTIL";
        const string RU_PARENT_FOLDER_OID = "RESUTIL";
       // const string RU_CHILD_FOLDER_OID = "RESUTILTP";
        const string RU_FORM_OID = "S1806_RESOURCES";
        const string RU_FIELD_OID = "TIMEPNT";
        const string DICTIONARY_NAME = "TIMEPNT";

        DataPoint dataPoint = ((ActionFunctionParams) ThisObject).ActionDataPoint;
        Subject currentSubject = dataPoint.Record.Subject;

        if (dataPoint != null)
        {   //adjusting targets?
            DataDictionary timepointValues = DataDictionary.FetchByName(DICTIONARY_NAME, "eng", dataPoint.Record.Subject.CRFVersionID);
            int entryIndex = -1, targetWeeks = -1, targetMonths = -1;
            string folderNamePrefix = "";
            //determine which matrix to add and days to calculate target date based on the submitted form's timepoint
             DataPoint timePoint = dataPoint.Record.DataPage.MasterRecord.DataPoints.FindByFieldOID("TIMEPNT");
            switch (dataPoint.CodedValue().ToString())
            {
                case "WK18":
                entryIndex = 1;
                targetWeeks = 18;
                folderNamePrefix = "";
                break;
                case "WK54":
                entryIndex = 2;
                targetWeeks = 54;
                folderNamePrefix = "";
                break;
                case "WK78":
                entryIndex = 3;
                targetWeeks = 78;
                folderNamePrefix = "";
                break;
                case "WK104":
                entryIndex = 4;
                targetWeeks = 104;
                folderNamePrefix = "";
                break;
                case "YR2.5":
                entryIndex = 5;
                targetWeeks = 130;
                folderNamePrefix = "";
                break;
                case "YR3":
                entryIndex = 6;
                targetWeeks = 156;
                folderNamePrefix = "";
                break;
                case "YR3.5":
                entryIndex = 7;
                targetWeeks = 182;
                folderNamePrefix = "";
                break;
                case "YR4":
                entryIndex = 8;
                targetWeeks = 208;
                folderNamePrefix = "";
                break;
                case "YR4.5":
                entryIndex = 9;
                targetWeeks = 234;
                folderNamePrefix = "";
                break;
                case "YR5":
                entryIndex = 10;
                targetWeeks = 260;
                folderNamePrefix = "";
                break;
                default:
                break;
            }

            //check if there is a TimePNT enterend on form in folder, if yes -> already added
            if (dataPoint.CodedValue().ToString() != "YR5")
            {
                Instance ruParent = null, ruInstance = null;
                //prefill RU timepoint
                ruParent = currentSubject.Instances.FindByFolderOID(RU_PARENT_FOLDER_OID);
                Instance ruChild = currentSubject.Instances.FindByFolderOID(RU_CHILD_FOLDER_OID);
                if (ruParent != null)
                {
                    bool alreadyAdded = false;
                    foreach (Instance subFolder in ruParent.Instances)
                    {
                        DataPage ruForm = subFolder.DataPages.FindByFormOID(RU_FORM_OID);
                        if (ruForm != null)
                        {
                            DataPoint timepoint = ruForm.MasterRecord.DataPoints.FindByFieldOID(RU_FIELD_OID);
                        
                            if (timepoint != null && timepoint.Data == timepointValues.Entries[entryIndex].CodedData)
                            {
                                alreadyAdded = true;
                                break;
                            }
                        }
                    }
                    //add second check to see if folder name already exists (GA) this actually added new folder "RU RU TPNT"
                   foreach (Instance check in ruParent.Instances)
                   {
                       string willBeAdded = "Resource Utilization " + timepointValues.Entries[entryIndex].UserDataString;
                       
                       if (check.Name == willBeAdded)
                       { 
                            alreadyAdded = true;
                            break;
                       } 
                    }

                    //add the matrix (if not already added) and prefill the timepoint for the new folder
                    Matrix next_RU_matrix = Matrix.FetchByOID(MATRIX_OID, currentSubject.CRFVersionID);
                    if (next_RU_matrix != null && !alreadyAdded)
                    {
                        currentSubject.AddMatrix(next_RU_matrix);

                        ruInstance = ruParent.Instances[ruParent.Instances.Count - 1];
                        if (entryIndex != -1 && timepointValues.Entries[entryIndex] != null)
                        {
                            ruInstance.SetInstanceName(folderNamePrefix + timepointValues.Entries[entryIndex].UserDataString);
                       //     CustomFunction.PerformCustomFunction("PrefillField", dataPoint.Record.Subject.CRFVersionID,
//new object[] { ruInstance.DataPages.FindByFormOID(RU_FORM_OID), RU_FIELD_OID, timepointValues.Entries[entryIndex].CodedData, true } );
                        }
//change after adding calendar cfs
                        //CustomFunction.PerformCustomFunction("CALENDARING_SetTargetDate_Resources", dataPoint.Record.Subject.CRFVersionID,
//new object[] { dataPoint.Record.Subject, ruInstance, targetWeeks} );

                        //CustomFunction.PerformCustomFunction("CTSUX_CM_doHandleCMForm", currentSubject.CRFVersionID, ruInstance);
                    }
                }
            }
        }

        return 0;