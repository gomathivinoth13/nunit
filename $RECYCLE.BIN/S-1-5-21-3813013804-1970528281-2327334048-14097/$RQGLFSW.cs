﻿using Microsoft.Extensions.Logging;
using SEG.EagleEyeLibrary.Models;
using SEG.SalesForce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimePointsProcessFunctionApp.Interface
{
    public interface ISfmcServices1
    {
        public Task<DataExtentionsResponse> CreateInputItem(int Current_point_balance, string member_id, int Expiring_points, DateTime Next_exp_date);
        public Task<DataExtentionsResponse> InsertToSfmc(DataExtentionsRequest dataExtentionRequest);
        public Task<DataExtentionsResponse> RealTimeDataProcess(Account account);


    }
}
