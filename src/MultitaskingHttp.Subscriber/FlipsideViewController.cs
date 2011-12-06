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
using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;

namespace MultitaskingHttp.Subscriber
{
	public partial class FlipsideViewController : UIViewController
	{
		public FlipsideViewController() : base ("FlipsideViewController", null)
		{
			this.ContentSizeForViewInPopover = new SizeF(320f, 480f);
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			//any additional setup after loading the view, typically from a nib.
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			if(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
			} else {
				return true;
			}
		}
		
		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidUnload()
		{
			base.ViewDidUnload();
			
			// Release any retained subviews of the main view.
			// e.g. this.myOutlet = null;
		}
		
		partial void done(UIBarButtonItem sender)
		{
			if(Done != null)
				Done(this, EventArgs.Empty);
		}
		
		public event EventHandler Done;
	}
}
