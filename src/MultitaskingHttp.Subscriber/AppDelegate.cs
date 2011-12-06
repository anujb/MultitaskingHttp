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
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Net;
using System.Threading;

namespace MultitaskingHttp.Subscriber
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow _Window;
		RootViewController _RootViewController;
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{	
			_Window = new UIWindow(UIScreen.MainScreen.Bounds);
			_RootViewController = new RootViewController();
			
			_Window.RootViewController = new UIViewController();
			_Window.MakeKeyAndVisible();
			
			HttpService.Initialize();
			
			return true;
		}
		
		public override void DidEnterBackground(UIApplication application)
		{
			Console.WriteLine("Entering Background State.");
			
			var taskId = UIApplication.SharedApplication.BeginBackgroundTask(() => { });
			
			Thread task = new Thread(new ThreadStart(() => {
			  	RegisterBackgroundHttpServer(application, taskId);
			}));
			
			task.Start();
		}
		
		public override void WillEnterForeground(UIApplication application)
		{
			Console.WriteLine("Entering Active State.");
			
			var result = NSUserDefaults.StandardUserDefaults.StringForKey("last_backgrounded");
			Console.WriteLine("Last Backgrounded: {0}", result);
			
			var request = NSUserDefaults.StandardUserDefaults.StringForKey("last_request");
			Console.WriteLine("Last Request: {0}", request);
			
			HttpService.Stop();
		}
		
		public void RegisterBackgroundHttpServer(UIApplication application, int taskId)
		{
			if(HttpService.IsInitialized == false) {
				HttpService.Initialize();
			}
			
			HttpService.Start();
		
			NSUserDefaults.StandardUserDefaults.SetString(DateTime.Now.ToString(), "last_backgrounded");
			NSUserDefaults.StandardUserDefaults.Synchronize();
			
			Thread.Sleep(TimeSpan.FromMinutes(3));
		
			UIApplication.SharedApplication.EndBackgroundTask(taskId);
		}
	}
}

