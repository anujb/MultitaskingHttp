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
using System.Linq;
using System.Net;
using MonoTouch.Foundation;
using System.IO;
using ServiceStack.Text;
using System.Collections.Generic;

namespace MultitaskingHttp.Subscriber
{
	public class HttpServer
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
			try {
				
				Listener.BeginGetContext(new AsyncCallback(HandleHttpRequest), Listener);
				
			} catch (HttpListenerException ex) {
				Console.WriteLine("Bummer, I was trying to rock that request, dawg! -- {0}", ex.Message);
				return;
			}
		}
		
		public static void Stop()
		{
			Listener.Stop();
		}
		
		public static void HandleHttpRequest(IAsyncResult result)
		{
			var context = Listener.EndGetContext(result);
			
			if(context.Request.RawUrl.ToLower().Contains("customer")) {
				HandleCustomersRequest(context);
			}
			
			Listener.BeginGetContext(new AsyncCallback(HandleHttpRequest), Listener);
		}
		
		public static void HandleCustomersRequest(HttpListenerContext context)
		{
			if(context == null) {
				return;
			}
			
			var customers = new List<Customer> { };
			var json = NSUserDefaults.StandardUserDefaults.StringForKey("customers");
			
			if(string.IsNullOrWhiteSpace(json) == false) {
				var result = JsonSerializer.DeserializeFromString<IEnumerable<Customer>>(json);
				customers.AddRange(result);
			}
			
			if(context.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase)) {
				SendResponse(context, HttpStatusCode.OK, json);
				return;
			}
			else if(context.Request.HttpMethod.Equals("PUT", StringComparison.OrdinalIgnoreCase)) {
				var customer = JsonSerializer.DeserializeFromStream<Customer>(context.Request.InputStream);
				
				var result = SaveCustomer(customers, customer);
				SendResponse(context, result ? HttpStatusCode.OK : HttpStatusCode.InternalServerError, string.Empty);
				return;
			}
			
			SendResponse(context, HttpStatusCode.BadRequest, string.Empty);
		}
					
		private static bool SaveCustomer(List<Customer> customers, Customer newCustomer)
		{
			if(newCustomer == null && newCustomer == default(Customer)) {
				return false;
			}
			
			customers.Add(newCustomer);
			
			var json = JsonSerializer.SerializeToString<IEnumerable<Customer>>(customers);
			
			NSUserDefaults.StandardUserDefaults.SetString(json, "customers");
			
			return true;
		}
		
		
		private static void SendResponse(HttpListenerContext context, HttpStatusCode statusCode, string content)
		{
    		byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(content);
		
		    context.Response.ContentType = "text/json";
		    context.Response.StatusCode = (int)statusCode;
		    context.Response.ContentLength64 = responseBytes.Length;
		    context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
		    context.Response.OutputStream.Close();
		}
	}
}

