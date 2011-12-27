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
using MonoTouch.UIKit;
using RestSharp;
using ServiceStack.Text;
using MonoTouch.Foundation;

namespace MultitaskingHttp.Publisher
{
	public class RootViewController : UIViewController
	{
		private static Random _Random;
		private static RestClient _Client = new RestClient("http://127.0.0.1:30001/");
		
		public string[] Names = { "Bob", "Mary", "Steve", "Bill", "Chad", "BABA ANUJ", "Thad", "Miguel", "Scott" };
		public UILabel _ResponseLabel { get; set; }
		
		
		
		public RootViewController()
			: base()
		{
			_Random = new Random(233092);
		}
		
		public override void LoadView()
		{
			base.LoadView();
			
			this.View.BackgroundColor = UIColor.White;
			
			_ResponseLabel = new UILabel();
			_ResponseLabel.Text = "Click to send Request";
			_ResponseLabel.Frame = new System.Drawing.RectangleF(25, 300, View.Frame.Width - 50, 55);
			_ResponseLabel.TextColor = UIColor.Black;
			
			this.View.AddSubview(_ResponseLabel);
			
			var button = UIButton.FromType(UIButtonType.RoundedRect);
			button.Frame = new System.Drawing.RectangleF(25, 150, View.Frame.Width - 50, 55);
			button.SetTitle("Add Customer!", UIControlState.Normal);
			
			
			button.TouchUpInside += delegate {
				
				try {
					var req = new RestRequest("", Method.PUT);
					req.Timeout = (int)TimeSpan.FromSeconds(5).TotalMilliseconds;
					
					var customer = new Customer {
						Id = _Random.Next(1, 100),
						Name = Names[_Random.Next(0, 8)],
						Address = string.Format(@"{0} FARFLUFFLE on {1} STREET", _Random.Next(1, 1000), _Random.Next(1, 100)),
					};
					var body = JsonSerializer.SerializeToString<Customer>(customer);
					Console.WriteLine(body);
					req.AddParameter("body", body, ParameterType.RequestBody);
					
					var response = _Client.Execute(req);
					Console.WriteLine("Response -- Code: {0} -- Content: {1} -- Error: {2}", response.StatusCode, response.Content, response.ErrorMessage);
					
					
					using(var pool = new NSAutoreleasePool()) {
						pool.BeginInvokeOnMainThread(()=>{
							_ResponseLabel.Text = response.StatusCode == System.Net.HttpStatusCode.OK ? response.Content : "No Response from Server...";
						});
					}
					
					
					
				} catch (Exception ex) {
					Console.WriteLine(ex);
				}
			};
			
			this.View.AddSubview(button);
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}
		
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
}

