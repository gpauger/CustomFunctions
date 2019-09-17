   ActionFunctionParams afp = (ActionFunctionParams) ThisObject;
     DataPoint dataPoint = afp.ActionDataPoint;
     Subject currentSubject = dataPoint.Record.Subject;
     DataPage currentDP = dataPoint.Record.DataPage;
     Instance currentInst = currentDP.Instance;
     DateTime F1Date = DateTime.Parse(dataPoint.Data);
                if (dataPoint != null)
                {
                    currentInst.Target = F1Date.AddDays(18 * 7);
                    currentInst.Overdue = currentInst.Target.AddDays(15);
                    //currentSubject.SetSubjectDirty();
                }
            

        return null;




     ActionFunctionParams afp = (ActionFunctionParams) ThisObject;
     DataPoint dataPoint = afp.ActionDataPoint;
     Subject currentSubject = dataPoint.Record.Subject;
     DataPage currentDP = dataPoint.Record.DataPage;
     Instance currentInst = currentDP.Instance;
     DateTime F2Date = DateTime.Parse(dataPoint.Data);
                if (dataPoint != null)
                {
                    currentInst.SetDate(F2Date);
                
                }
            

        return null;



