﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace MobileSchoolAPI.Models
{
	public class FCMPushNotification
	{
		public FCMPushNotification()
		{
			// TODO: Add constructor logic here
		}

		public bool Successful
		{
			get;
			set;
		}

		public string Response
		{
			get;
			set;
		}
		public Exception Error
		{
			get;
			set;
		}



		public FCMPushNotification SendNotification(string _title, string _message, string _topic)
		{
			FCMPushNotification result = new FCMPushNotification();
			try
			{
				result.Successful = true;
				result.Error = null;
				// var value = message;
				var requestUri = "https://fcm.googleapis.com/fcm/send";

				WebRequest webRequest = WebRequest.Create(requestUri);
				webRequest.Method = "POST";
				webRequest.Headers.Add(string.Format("Authorization: key={0}", ConfigurationManager.AppSettings["SERVER_API_KEY"] ));
				webRequest.Headers.Add(string.Format("Sender: id={0}", ConfigurationManager.AppSettings["FCM_SENDER_ID"]));
				webRequest.ContentType = "application/json";

				var data = new
				{
					to = _topic, // Uncoment this if you want to test for single device
					//to = "/topics/" + _topic, // this is for topic 
					priority = "high",
					content_available = true,
					notification = new
					{
						title = _title,
						body = _message,
						//icon="myicon"
					}
				};
				var serializer = new JavaScriptSerializer();
				var json = serializer.Serialize(data);

				Byte[] byteArray = Encoding.UTF8.GetBytes(json);

				webRequest.ContentLength = byteArray.Length;
				using (Stream dataStream = webRequest.GetRequestStream())
				{
					dataStream.Write(byteArray, 0, byteArray.Length);

					using (WebResponse webResponse = webRequest.GetResponse())
					{
						using (Stream dataStreamResponse = webResponse.GetResponseStream())
						{
							using (StreamReader tReader = new StreamReader(dataStreamResponse))
							{
								String sResponseFromServer = tReader.ReadToEnd();
								result.Response = sResponseFromServer;
							}
						}
					}
				}

			}
			catch (Exception ex)
			{
				result.Successful = false;
				result.Response = null;
				result.Error = ex;
			}
			return result;
		}
	}
}