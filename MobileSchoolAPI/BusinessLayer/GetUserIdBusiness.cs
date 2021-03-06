﻿using MobileSchoolAPI.BUSINESSLAYER;
using MobileSchoolAPI.Models;
using MobileSchoolAPI.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileSchoolAPI.BusinessLayer
{
    public class GetUserIdBusiness
    {
       

        public object getUserInfo(GetUserId UserId)
        {
            try
            {
                SchoolMainContext db = new ConcreateContext().GetContext(UserId.UserId, UserId.PASSWORD);
                if (db == null)
                {
                    return new Results() { IsSuccess = false, Message  = "Invalid User" } ;
                }
                object result = "";
                var getUserType = db.VW_GET_USER_TYPE.Where(r => r.UserId == UserId.UserId).FirstOrDefault();

                if (getUserType != null)
                {

                    if (getUserType.UserType == "STUDENT")
                    {
                        STUDENTINFO_BUSINESS GetStudobj = new STUDENTINFO_BUSINESS();
                        result = GetStudobj.getStudInfo(int.Parse(getUserType.EmpCode), UserId.UserId,UserId.PASSWORD);
                    }
                    else if (getUserType.UserType == "Alumini")
                    {

                        return new Results
                        {
                            IsSuccess = true,
                            Message = new InvalidUser() { IsSuccess = true, Result = "Alumini User" }
                        };

                      
                    }
                    else
                    {
                        GetTeacherInfoBusiness GetTeacherobj = new GetTeacherInfoBusiness();
                        result = GetTeacherobj.getTeacherInfo(int.Parse(getUserType.EmpCode), UserId.UserId, UserId.PASSWORD);
                    }
                    return result;
                }

                return new Results
                {
                    IsSuccess = false,
                    Message = new InvalidUser() { IsSuccess = false, Result = "User Not Found" } 
                };


            

            }
            catch (Exception ex)
            {


                return new Results
                {
                    IsSuccess = false,
                    Message =   ex.Message  
                };
               
            }

        }
    }
}