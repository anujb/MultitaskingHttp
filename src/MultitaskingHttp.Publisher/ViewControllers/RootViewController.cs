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

namespace MultitaskingHttp.Publisher
{
	public class RootViewController : UINavigationController
	{
		public UILabel _ResponseLabel { get; set; }
		
		public RootViewController()
			: base()
		{
		}
		
		public override void LoadView()
		{
			base.LoadView();
			
			_ResponseLabel = new UILabel();
			_ResponseLabel.Text = "Click to send Request";
			_ResponseLabel.Frame = new System.Drawing.RectangleF(25, 300, View.Frame.Width - 50, 55);
			_ResponseLabel.TextColor = UIColor.Black;
			
			this.View.AddSubview(_ResponseLabel);
			
			var button = UIButton.FromType(UIButtonType.RoundedRect);
			button.Frame = new System.Drawing.RectangleF(25, 150, View.Frame.Width - 50, 55);
			button.SetTitle("Send Request!", UIControlState.Normal);
			
			var x = new RestRequest("");
			x.Timeout = 5;
			
			button.TouchUpInside += delegate {
				
				try {
				
					var client = new RestClient(@"http://127.0.0.1:30001/");
					var req = new RestRequest("");
					
					
					var response = client.Execute(req);
					
					Console.WriteLine(response.Content);
					
					_ResponseLabel.Text = string.IsNullOrWhiteSpace(response.Content) ? "No Content..." : response.Content;
					
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

