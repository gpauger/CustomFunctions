DataPoint dataPoint = ((ActionFunctionParams)ThisObject).ActionDataPoint;
                DataPoint timepointDatapoint = dataPoint.Record.DataPage.MasterRecord.DataPoints.FindByFieldOID("TIMEPNT");
                Instance instance = dataPoint.Record.DataPage.Instance;
                foreach (DataPage dataPage in instance.DataPages)
                {
                    if (dataPage.Active && dataPage.Form.OID != dataPoint.Record.DataPage.Form.OID)
                    {
                        DataPoint currentTimepoint = dataPage.MasterRecord.DataPoints.FindByFieldOID("TIMEPNT");
                        if (currentTimepoint != null)
                        {
                            currentTimepoint.Enter(timepointDatapoint.Data, null, 0);
                        }
                    }
                }
                InactivateForms(dataPoint, ((ActionFunctionParams)ThisObject).ActionResult);
                return null;
            }

            private void InactivateForms(DataPoint dataPoint, bool actionResult)
            {
                if (actionResult)
                {
                    return;
                }

                if (dataPoint == null || dataPoint.Data.Contains("Y"))
                {
                    return;
                }

                string[] formOIDS = new string[] { };
                string fieldOID = dataPoint.Field.OID;
                if (fieldOID == "MEDPROC")
                {
                    formOIDS = new string[] { "S1806_RESOURCES_MP" };
                }
                else if (fieldOID == "REQHOSP")
                {
                    formOIDS = new string[] { "S1806_RESOURCES_HO" };
                }
                else if (fieldOID == "RUOFF")
                {
                    formOIDS = new string[] { "S1806_RESOURCES_OV" };
                }
                else if (fieldOID == "IMGPERF")
                {
                    formOIDS = new string[] { "S1806_RESOURCES_IM" };
                }
                else if (fieldOID == "CMYN")
                {
                    formOIDS = new string[] { "S1806_RESOURCES_ME" };
                }
                //string formOID = formOIDS[Array.IndexOf(fieldOIDS, fieldOID)];
                Instance instance = dataPoint.Record.DataPage.Instance;
                foreach (string formOID in formOIDS)
                {
                    DataPage dataPage = instance.DataPages.FindByFormOID(formOID);
                    if (dataPage != null && dataPage.Active)
                    {
                        DataPoint cmtDataPoint = dataPage.MasterRecord.DataPoints.FindByFieldOID("CMT");
                        if (cmtDataPoint != null && cmtDataPoint.EntryStatus == EntryStatusEnum.NoData)
                        {
                            // inactivate form
                            dataPage.Active = false;
                        }
                    }
                }