using DataTemplateSelector;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MainPage), typeof(DataTemplateSelector.iOS.IosPageRenderer))]

namespace DataTemplateSelector.iOS
{
	public class IosPageRenderer : PageRenderer
	{
		NSObject observerHideKeyboard;
		NSObject observerShowKeyboard;

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			observerHideKeyboard = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
			observerShowKeyboard = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			NSNotificationCenter.DefaultCenter.RemoveObserver(observerHideKeyboard);
			NSNotificationCenter.DefaultCenter.RemoveObserver(observerShowKeyboard);
		}

		void OnKeyboardNotification(NSNotification notification)
		{
			if (!IsViewLoaded) return;

			var frameBegin = UIKeyboard.FrameBeginFromNotification(notification);
			var frameEnd = UIKeyboard.FrameEndFromNotification(notification);
			var bounds = Element.Bounds;
			var newBounds = new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height - frameBegin.Top + frameEnd.Top);
			Element.Layout(newBounds);
		}
	}
}