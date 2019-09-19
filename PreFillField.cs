  object[] parameters = (object[]) ThisObject;

        if (parameters.Length == 4 && parameters[0] is DataPage)
        {
            bool isFrozen = (bool) parameters[3];
            DataPoint dataPoint = ((DataPage) parameters[0]).MasterRecord.DataPoints.FindByFieldOID(parameters[1].ToString());

            if (isFrozen) dataPoint.UnFreeze();
            dataPoint.Enter(parameters[2].ToString(), null, 0);
            if (isFrozen) dataPoint.Freeze();
        }
        else if (parameters.Length == 3 && parameters[0] is DataPage) //leaving this here for CFs using the original version...don't copy this case into Rave
        {
            DataPoint dataPoint = ((DataPage) parameters[0]).MasterRecord.DataPoints.FindByFieldOID(parameters[1].ToString());

            dataPoint.UnFreeze();
            dataPoint.Enter(parameters[2].ToString(), null, 0);
            dataPoint.Freeze();
        }

        return 0;