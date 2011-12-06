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
using System;
using MonoTouch.Foundation;

namespace MultitaskingHttp.Subscriber
{
	public partial class MainViewController : UIViewController
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		UIPopoverController flipsidePopoverController;
		
		public MainViewController()
			: base (UserInterfaceIdiomIsPhone ? "MainViewController_iPhone" : "MainViewController_iPad" , null)
		{
			// Custom initialization
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			//any additional setup after loading the view, typically from a nib.
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			if(UserInterfaceIdiomIsPhone) {
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
		
		partial void showInfo(NSObject sender)
		{
			if(UserInterfaceIdiomIsPhone) {
				var controller = new FlipsideViewController() {
					ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal,
				};
				controller.Done += delegate {
					this.DismissModalViewControllerAnimated(true);
				};
				this.PresentModalViewController(controller, true);
			} else {
				if(flipsidePopoverController == null) {
					var controller = new FlipsideViewController();
					flipsidePopoverController = new UIPopoverController(controller);
					controller.Done += delegate {
						flipsidePopoverController.Dismiss(true);
					};
				}
				if(flipsidePopoverController.PopoverVisible) {
					flipsidePopoverController.Dismiss(true);
				} else {
					flipsidePopoverController.PresentFromBarButtonItem((UIBarButtonItem)sender, UIPopoverArrowDirection.Any, true);
				}
			}
		}
	}
}
