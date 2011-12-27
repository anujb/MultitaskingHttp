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
		CustomerViewController _RootViewController;
		
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{	
			_Window = new UIWindow(UIScreen.MainScreen.Bounds);
			_RootViewController = new CustomerViewController();
			
			_Window.RootViewController = _RootViewController;
			_Window.MakeKeyAndVisible();
			
			HttpServer.Initialize();
			
			return true;
		}
		
		public override void DidEnterBackground(UIApplication application)
		{
			Console.WriteLine("Entered Background State...");
			
			var taskId = UIApplication.SharedApplication.BeginBackgroundTask(() => { });
			
			Thread task = new Thread(new ThreadStart(() => {
			  	RegisterBackgroundHttpServer(application, taskId);
			}));
			
			task.Start();
		}
		
		public override void WillEnterForeground(UIApplication application)
		{
			var lastbackgrounded = NSUserDefaults.StandardUserDefaults.StringForKey("last_backgrounded");
			var lastrequest = NSUserDefaults.StandardUserDefaults.StringForKey("last_request");
			Console.WriteLine("Resuming state from: {0}", lastbackgrounded);
			Console.WriteLine("Last Request from: {0}", lastrequest);
			
			//So now that we're back we probably don't want anyone sending us data!
			//So lets stop the service!
			
			HttpServer.Stop();
		}
		
		public void RegisterBackgroundHttpServer(UIApplication application, int taskId)
		{
			if(HttpServer.IsInitialized == false) {
				HttpServer.Initialize();
			}
			
			HttpServer.Start();
		
			NSUserDefaults.StandardUserDefaults.SetString(DateTime.Now.ToString(), "last_backgrounded");
			NSUserDefaults.StandardUserDefaults.Synchronize();
			
			// As per Apple docs ew can go up to 600 seconds here, but this is not guaranteed.
			Thread.Sleep(TimeSpan.FromSeconds(300));
		
			UIApplication.SharedApplication.EndBackgroundTask(taskId);
		}
	}
}

