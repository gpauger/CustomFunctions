 const string MATRIX_OID = "NEXT_PRO";
        const string PRO_PARENT_FOLDER_OID = "PRO";
        const string PRO_FORM_OID = "QLCS1806";
        const string PRO_FIELD_OID = "S1806CSTP";
        const string DICTIONARY_NAME = "PRO Timepoint";

        DataPoint dataPoint = ((ActionFunctionParams) ThisObject).ActionDataPoint;
        Subject currentSubject = dataPoint.Record.Subject;

        if (dataPoint != null)
        {
            DataDictionary timepointValues = DataDictionary.FetchByName(DICTIONARY_NAME, "eng", dataPoint.Record.Subject.CRFVersionID);
            int entryIndex = -1, targetWeeks = -1, targetMonths = -1;
            string folderNamePrefix = "";
            //determine which matrix to add and days to calculate target date based on the submitted form's timepoint
            switch (dataPoint.CodedValue().ToString())
            {
                case "RAND":
                entryIndex = 1;
                targetWeeks = 12;
                folderNamePrefix = " ";
                break;
                case "W12":
                entryIndex = 2;
                targetWeeks = 18;
                folderNamePrefix = " ";
                break;
                case "W18":
                entryIndex = 3;
                targetMonths = 12;
                break;
                case "M12":
                entryIndex = 4;
                targetMonths = 24;
                break;
                case "M24":
                entryIndex = 5;
                targetMonths = 36;
                break;
                default:
                break;
            }

            //check if there is a blank PRO sub-folder already
            if (dataPoint.CodedValue().ToString() != "M36")
            {
                Instance proParent = null, proInstance = null;
                //prefill PRO timepoint
                proParent = currentSubject.Instances.FindByFolderOID(PRO_PARENT_FOLDER_OID);
                if (proParent != null)
                {
                    bool alreadyAdded = false;
                    foreach (Instance subFolder in proParent.Instances)
                    {
                        DataPage coverSheet = subFolder.DataPages.FindByFormOID(PRO_FORM_OID);
                        if (coverSheet != null)
                        {
                            DataPoint timepoint = coverSheet.MasterRecord.DataPoints.FindByFieldOID(PRO_FIELD_OID);
                            if (timepoint != null && timepoint.Data == timepointValues.Entries[entryIndex].CodedData)
                            {
                                alreadyAdded = true;
                                break;
                            }
                        }
                    }

                    //add the matrix (if not already added) and prefill the timepoint for the new folder/form
                    Matrix next_PRO_matrix = Matrix.FetchByOID(MATRIX_OID, currentSubject.CRFVersionID);
                    if (next_PRO_matrix != null && !alreadyAdded)
                    {
                        currentSubject.AddMatrix(next_PRO_matrix);

                        proInstance = proParent.Instances[proParent.Instances.Count - 1];
                        if (entryIndex != -1 && timepointValues.Entries[entryIndex] != null)
                        {
                            proInstance.SetInstanceName(folderNamePrefix + timepointValues.Entries[entryIndex].UserDataString);
                            CustomFunction.PerformCustomFunction("PrefillField", dataPoint.Record.Subject.CRFVersionID,
new object[] { proInstance.DataPages.FindByFormOID(PRO_FORM_OID), PRO_FIELD_OID, timepointValues.Entries[entryIndex].CodedData, true } );
                        }

                        CustomFunction.PerformCustomFunction("CALENDARING_SetTargetDate_PRO", dataPoint.Record.Subject.CRFVersionID,
new object[] { dataPoint.Record.Subject, proInstance, targetWeeks, targetMonths } );

                        CustomFunction.PerformCustomFunction("CTSUX_CM_doHandleCMForm", currentSubject.CRFVersionID, proInstance);
                    }
                }
            }
        }

        return 0;