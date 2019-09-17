
     ActionFunctionParams afp = (ActionFunctionParams) ThisObject;
     DataPoint dataPoint = afp.ActionDataPoint;
     Subject currentSubject = dataPoint.Record.Subject;
     const string RU_PARENT_FOLDER_OID = "RESUTIL";
     const string RU_SUB_FOLDER_OID = "RESUTILTP";

         Instance resutilsFolder = currentSubject.Instances.FindByFolderOID(RU_PARENT_FOLDER_OID);
            if (resutilsFolder != null)
            {
                Instance rusubFolder = resutilsFolder.Instances.FindByFolderOID(RU_SUB_FOLDER_OID);
                if (rusubFolder != null)
                {
                    rusubFolder.Target = regdt.AddDays(18 * 7);
                    rusubFolder.Overdue = rusubFolder.Target.AddDays(15);
                    //currentSubject.SetSubjectDirty();
                }
            }

        return null;