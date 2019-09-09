   object[] parameters = (object[]) ThisObject;

        if (parameters.Length == 3 && parameters[0] is Subject && parameters[1] is Instance)
        {
            Subject currentSubject = (Subject) parameters[0];
            Instance proInstance = (Instance) parameters[1];
            int targetWeeks = (int) parameters[2];
            

            DateTime calculationDate = DateTime.Now;
            object registrationDate = CustomFunction.PerformCustomFunction("GetREGDT_ByStep", currentSubject.CRFVersionID, new object[] { currentSubject, "2" } );
            if (registrationDate != null && registrationDate is DateTime)
            {
                calculationDate = (DateTime) registrationDate;
            }

            if (targetWeeks != -1)
            {
                proInstance.Target = calculationDate.AddDays(targetWeeks * 7);
            }
            else
            {
                proInstance.Target = calculationDate;
            }

            proInstance.Overdue = proInstance.Target.AddDays(15);
            currentSubject.SetSubjectDirty();
        }

        return null;