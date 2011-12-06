// 
//  Copyright 2011  abhatia
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Net;
using MonoTouch.Foundation;

namespace MultitaskingHttp.Subscriber
{
	public class HttpService
	{
		public static HttpListener Listener;
		public static bool IsInitialized { get; private set; }
		public static bool IsListening { get { return Listener.IsListening; } }

		public static void Initialize()
		{
			Listener  = new HttpListener();
			Listener.Prefixes.Add("http://*:30001/");
			
			IsInitialized = true;
		}
		
		public static void Start()
		{
			Listener.Start();
			Listener.BeginGetContext(new AsyncCallback(HandleHttpRequest), Listener);
		}
		
		public static void Stop()
		{
			Listener.Stop();
		}
		
		public static void HandleHttpRequest(IAsyncResult result)
		{
			var context = Listener.EndGetContext(result);
			
			DeserializeRequest(context);
			
			Listener.BeginGetContext(new AsyncCallback(HandleHttpRequest), Listener);
			
			string response = "foo";
    		byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(response);
		
		    context.Response.ContentType = "text/json";
		    context.Response.StatusCode = (int)HttpStatusCode.OK;
		    context.Response.ContentLength64 = responseBytes.Length;
		    context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
		    context.Response.OutputStream.Close();
		}
		
		public static void DeserializeRequest(HttpListenerContext context)
		{
			if(context == null) {
				return;
			}
			
			NSUserDefaults.StandardUserDefaults.SetString(DateTime.Now.ToString(), "last_request");
			NSUserDefaults.StandardUserDefaults.Synchronize();
			
//			if(context.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase)) {
//				Console.WriteLine("Get request from publisher");
//			}
		}
		
		public static void AddCustomersRequest()
		{
			
		}
		
	}
}

